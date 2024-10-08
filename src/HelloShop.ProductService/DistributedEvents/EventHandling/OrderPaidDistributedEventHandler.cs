// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ProductService.DistributedEvents.Events;
using HelloShop.ProductService.Entities.Products;
using HelloShop.ProductService.Infrastructure;
using HelloShop.ServiceDefaults.DistributedEvents.Abstractions;

namespace HelloShop.ProductService.DistributedEvents.EventHandling
{
    public class OrderPaidDistributedEventHandler(ProductServiceDbContext dbContext) : IDistributedEventHandler<OrderPaidDistributedEvent>
    {
        public async Task HandleAsync(OrderPaidDistributedEvent @event)
        {
            foreach (var orderStockItem in @event.OrderStockItems)
            {
                var product = await dbContext.Set<Product>().FindAsync(orderStockItem.ProductId) ?? throw new Exception($"Product with id {orderStockItem.ProductId} not found");

                product.AvailableStock -= orderStockItem.Units;
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
