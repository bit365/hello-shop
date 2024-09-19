// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Dapr.Client;
using HelloShop.ServiceDefaults.DistributedEvents.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HelloShop.ServiceDefaults.DistributedEvents.DaprBuildingBlocks
{
    public class DaprDistributedEventBus(DaprClient daprClient, IOptions<DaprDistributedEventBusOptions> options, ILogger<DaprDistributedEventBus> logger) : IDistributedEventBus
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
