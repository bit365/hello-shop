// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.BasketService.DistributedEvents.Events;
using HelloShop.BasketService.Repositories;
using HelloShop.EventBus.Abstractions;

namespace HelloShop.BasketService.DistributedEvents.EventHandling
{
    public class OrderStartedDistributedEventHandler(IBasketRepository repository) : IDistributedEventHandler<OrderStartedDistributedEvent>
    {
        public async Task HandleAsync(OrderStartedDistributedEvent @event)
        {
            await repository.DeleteBasketAsync(@event.UserId);
        }
    }
}