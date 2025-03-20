// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;
using HelloShop.OrderingService.DistributedEvents.Events;
using HelloShop.OrderingService.Entities.Orders;
using HelloShop.OrderingService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HelloShop.OrderingService.Workers
{
    public class PaymentWorker(IServiceScopeFactory serviceScopeFactory, ILogger<PaymentWorker> logger) : BackgroundService
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
                var distributedEventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

                bool paymentSucceeded = Random.Shared.NextDouble() > 0.3;

                try
                {
                    var orders = await dbContext.Set<Order>().Include(o => o.OrderItems).Where(o => o.OrderStatus == OrderStatus.StockConfirmed).ToListAsync(stoppingToken);

                    foreach (var order in orders)
                    {
                        DistributedEvent @event = paymentSucceeded ? new OrderPaymentSucceededDistributedEvent(order.Id) : new OrderPaymentFailedDistributedEvent(order.Id);

                        await distributedEventBus.PublishAsync(@event, stoppingToken);
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
