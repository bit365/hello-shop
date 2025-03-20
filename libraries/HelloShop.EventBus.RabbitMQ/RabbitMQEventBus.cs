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
        private IModel? _consumerChannel;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    _rabbitMQConnection = serviceProvider.GetRequiredService<IConnection>();
                    _consumerChannel = _rabbitMQConnection.CreateModel();
                    _consumerChannel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct);
                    _consumerChannel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
                    consumer.Received += OnMessageReceivedAsync;
                    _consumerChannel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

                    foreach (var (eventName, _) in _eventBusOptions.EventTypes)
                    {
                        _consumerChannel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: eventName);
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

        public void Dispose() => _consumerChannel?.Dispose();

        public Task PublishAsync(DistributedEvent @event, CancellationToken cancellationToken = default)
        {
            string routingKey = @event.GetType().Name;

            using var channel = _rabbitMQConnection?.CreateModel() ?? throw new InvalidOperationException("RabbitMQ connection is not available.");

            channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct);

            var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), _eventBusOptions.JsonSerializerOptions);

            return _pipeline.Execute(() =>
            {
                IBasicProperties properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2;
                channel.BasicPublish(exchange: _exchangeName, routingKey: routingKey, mandatory: true, basicProperties: properties, body: body);
                return Task.CompletedTask;
            });
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

            _consumerChannel?.BasicAck(eventArgs.DeliveryTag, multiple: false);
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