// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ProductService.DistributedEvents.Events;
using HelloShop.ProductService.Entities.Products;
using HelloShop.ProductService.Infrastructure;
using HelloShop.ServiceDefaults.DistributedEvents.Abstractions;
using HelloShop.ServiceDefaults.DistributedLocks;

namespace HelloShop.ProductService.DistributedEvents.EventHandling
{
    public class OrderAwaitingValidationDistributedEventHandler(ProductServiceDbContext dbContext, IDistributedEventBus distributedEventBus, IDistributedLock distributedLock, ILogger<OrderAwaitingValidationDistributedEventHandler> logger) : IDistributedEventHandler<OrderAwaitingValidationDistributedEvent>
    {
        public async Task HandleAsync(OrderAwaitingValidationDistributedEvent @event)
        {
            await using var lockResult = await distributedLock.LockAsync("stock");

            logger.LogInformation("Handling distributed event {EventId} {Event}", @event.Id, @event);

            var confirmedOrderStockItems = new Dictionary<int, bool>();

            foreach (var orderStockItem in @event.OrderStockItems)
            {
                var product = await dbContext.Set<Product>().FindAsync(orderStockItem.ProductId) ?? throw new Exception($"Product with id {orderStockItem.ProductId} not found");

                var hasStock = product.AvailableStock >= orderStockItem.Units;

                confirmedOrderStockItems.Add(product.Id, hasStock);
            }

            DistributedEvent confirmedEvent = confirmedOrderStockItems.All(c => c.Value) ? new OrderStockConfirmedDistributedEvent(@event.OrderId) : new OrderStockRejectedDistributedEvent(@event.OrderId);

            await distributedEventBus.PublishAsync(confirmedEvent);
        }
    }
}
