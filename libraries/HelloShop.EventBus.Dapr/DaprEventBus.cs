// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Dapr.Client;
using HelloShop.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HelloShop.EventBus.Dapr
{
    public class DaprEventBus(DaprClient daprClient, IOptions<DaprEventBusOptions> options, ILogger<DaprEventBus> logger) : IEventBus
    {
        public async Task PublishAsync(DistributedEvent @event, CancellationToken cancellationToken = default)
        {
            string pubSubName = options.Value.PubSubName;
            string topicName = @event.GetType().Name;

            logger.LogInformation("Publishing event {@Event} to {PubsubName}.{TopicName}", @event, pubSubName, topicName);

            object? data = Convert.ChangeType(@event, @event.GetType());

            await daprClient.PublishEventAsync(pubSubName, topicName, data, cancellationToken);
        }
    }
}