// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.EventBus.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync(DistributedEvent @event, CancellationToken cancellationToken = default);
    }
}
