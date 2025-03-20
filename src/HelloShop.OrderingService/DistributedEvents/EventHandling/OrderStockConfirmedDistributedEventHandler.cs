// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;
using HelloShop.OrderingService.DistributedEvents.Events;
using HelloShop.OrderingService.Entities.Orders;
using HelloShop.OrderingService.Infrastructure;

namespace HelloShop.OrderingService.DistributedEvents.EventHandling
{
    public class OrderStockConfirmedDistributedEventHandler(OrderingServiceDbContext dbContext) : IDistributedEventHandler<OrderStockConfirmedDistributedEvent>
    {
        public async Task HandleAsync(OrderStockConfirmedDistributedEvent @event)
        {
            Order order = await dbContext.Set<Order>().FindAsync(@event.OrderId) ?? throw new Exception($"Order with id {@event.OrderId} not found");

            order.OrderStatus = OrderStatus.StockConfirmed;

            await dbContext.SaveChangesAsync();
        }
    }
}
