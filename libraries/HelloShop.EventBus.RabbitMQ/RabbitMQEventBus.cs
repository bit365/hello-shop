// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace HelloShop.EventBus.RabbitMQ
{
    public sealed class RabbitMQEventBus(ILogger<RabbitMQEventBus> logger, IServiceProvider serviceProvider, IOptions<RabbitMQEventBusOptions> rabbitMQEventBusOptions, IOptions<EventBusOptions> eventBusOptions) : IEventBus, IDisposable, IHostedService
    {
        private readonly ResiliencePipeline _pipeline = CreateResiliencePipeline(rabbitMQEventBusOptions.Value.RetryCount);
        private readonly string _queueName = rabbitMQEventBusOptions.Value.QueueName;
        private readonly string _exchangeName = rabbitMQEventBusOptions.Value.ExchangeName;
        private readonly EventBusOptions _eventBusOptions = eventBusOptions.Value;

        private IConnection? _rabbitMQConnection;
        private IChannel? _consumerChannel;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    _rabbitMQConnection = serviceProvider.GetRequiredService<IConnection>();
                    _consumerChannel = await _rabbitMQConnection.CreateChannelAsync();
                    await _consumerChannel.ExchangeDeclareAsync(exchange: _exchangeName, type: ExchangeType.Direct);
                    await _consumerChannel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
                    consumer.ReceivedAsync += OnMessageReceivedAsync;
                    await _consumerChannel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer);

                    foreach (var (eventName, _) in _eventBusOptions.EventTypes)
                    {
                        await _consumerChannel.QueueBindAsync(queue: _queueName, exchange: _exchangeName, routingKey: eventName);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while starting the RabbitMQ event bus.");
                }
            }, TaskCreationOptions.LongRunning);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public async void Dispose()
        {
            if (_consumerChannel is not null)
            {
                await _consumerChannel.CloseAsync();
                await _consumerChannel.DisposeAsync();
            }
        }

        public async Task PublishAsync(DistributedEvent @event, CancellationToken cancellationToken = default)
        {
            string routingKey = @event.GetType().Name;

            var channel = _rabbitMQConnection is not null ? await _rabbitMQConnection.CreateChannelAsync(cancellationToken: cancellationToken) : throw new InvalidOperationException("RabbitMQ connection is not available.");

            await using (channel)
            {
                await channel.ExchangeDeclareAsync(exchange: _exchangeName, type: ExchangeType.Direct, cancellationToken: cancellationToken);

                var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), _eventBusOptions.JsonSerializerOptions);

                await _pipeline.ExecuteAsync(async (ct) =>
                {
                    var properties = new BasicProperties
                    {
                        DeliveryMode = DeliveryModes.Persistent
                    };
                    await channel.BasicPublishAsync(exchange: _exchangeName, routingKey: routingKey, mandatory: true, basicProperties: properties, body: body, cancellationToken: ct);
                }, cancellationToken);
            }
        }

        private async Task OnMessageReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
        {
            string eventName = eventArgs.RoutingKey;
            string message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            if (!_eventBusOptions.EventTypes.TryGetValue(eventName, out var eventType))
            {
                return;
            }

            await using var scope = serviceProvider.CreateAsyncScope();

            var distributedEvent = JsonSerializer.Deserialize(message, eventType, _eventBusOptions.JsonSerializerOptions) as DistributedEvent;

            foreach (var handler in scope.ServiceProvider.GetKeyedServices<IDistributedEventHandler>(eventType))
            {
                if (distributedEvent is not null)
                {
                    await handler.HandleAsync(distributedEvent);
                }
            }

            if (_consumerChannel is not null)
            {
                await _consumerChannel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
            }
        }

        private static ResiliencePipeline CreateResiliencePipeline(int retryCount)
        {
            var retryOptions = new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<BrokerUnreachableException>().Handle<SocketException>(),
                MaxRetryAttempts = retryCount,
                DelayGenerator = (context) => ValueTask.FromResult(GenerateDelay(context.AttemptNumber))
            };

            return new ResiliencePipelineBuilder().AddRetry(retryOptions).Build();

            static TimeSpan? GenerateDelay(int attempt) => TimeSpan.FromSeconds(Math.Pow(2, attempt));
        }
    }
}