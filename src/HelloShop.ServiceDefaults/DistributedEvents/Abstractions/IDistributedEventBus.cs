// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.ServiceDefaults.DistributedEvents.Abstractions
{
    public interface IDistributedEventBus
    {
        Task PublishAsync(DistributedEvent @event, CancellationToken cancellationToken = default);
    }
}
