// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;
using HelloShop.OrderingService.DistributedEvents.Events;
using HelloShop.OrderingService.Entities.Orders;
using HelloShop.OrderingService.Infrastructure;
using HelloShop.OrderingService.Services;
using Microsoft.EntityFrameworkCore;

namespace HelloShop.OrderingService.DistributedEvents.EventHandling
{
    public class OrderPaymentSucceededDistributedEventHandler(OrderingServiceDbContext dbContext, IDistributedEventService distributedEventService) : IDistributedEventHandler<OrderPaymentSucceededDistributedEvent>
    {
        public async Task HandleAsync(OrderPaymentSucceededDistributedEvent @event)
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync();

                DbSet<Order> orders = dbContext.Set<Order>();

                Order order = await orders.FindAsync(@event.OrderId) ?? throw new Exception($"Order with id {@event.OrderId} not found");

                await orders.Entry(order).Collection(i => i.OrderItems).LoadAsync();

                order.OrderStatus = OrderStatus.Paid;

                await dbContext.SaveChangesAsync();

                var orderStockList = order.OrderItems.Select(orderItem => new OrderStockItem(orderItem.ProductId, orderItem.Units));

                var integrationEvent = new OrderPaidDistributedEvent(order.Id, orderStockList);

                await distributedEventService.AddAndSaveEventAsync(integrationEvent);

                await distributedEventService.PublishEventsThroughEventBusAsync(transaction.TransactionId);

                await transaction.CommitAsync();
            });
        }
    }
}
