// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.


using HelloShop.OrderingService.DistributedEvents.Events;
using HelloShop.OrderingService.Entities.Orders;
using HelloShop.OrderingService.Infrastructure;
using HelloShop.ServiceDefaults.DistributedEvents.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace HelloShop.OrderingService.Workers
{
    public class GracePeriodWorker(IServiceScopeFactory serviceScopeFactory, ILogger<GracePeriodWorker> logger,TimeProvider timeProvider) : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("{Worker} background task is doing background work.", GetType().Name);
                }

                using var scope = serviceScopeFactory.CreateAsyncScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderingServiceDbContext>();
                var distributedEventBus = scope.ServiceProvider.GetRequiredService<IDistributedEventBus>();

                DateTimeOffset dateTimeOffset = timeProvider.GetUtcNow().AddMinutes(-1);

                try
                {
                    var orders = await dbContext.Set<Order>().Include(o => o.OrderItems).Where(o => o.OrderStatus == OrderStatus.Submitted && o.OrderDate >= dateTimeOffset).ToListAsync(stoppingToken);

                    foreach (var order in orders)
                    {
                        order.OrderStatus = OrderStatus.AwaitingValidation;

                        var orderStockList = order.OrderItems.Select(orderItem => new OrderStockItem(orderItem.ProductId, orderItem.Units));

                        await distributedEventBus.PublishAsync(new OrderAwaitingValidationDistributedEvent(order.Id, orderStockList), stoppingToken);

                        await dbContext.SaveChangesAsync(stoppingToken);
                    }

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred executing {Worker} background task.", GetType().Name);
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
