// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;

namespace HelloShop.ProductService.Services
{
    public interface IDistributedEventService
    {
        Task SaveEventAndDbContextChangesAsync(DistributedEvent @event);

        Task PublishThroughEventBusAsync(DistributedEvent @event);
    }
}
