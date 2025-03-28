// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;
using HelloShop.EventBus.Logging;
using HelloShop.ProductService.Infrastructure;

namespace HelloShop.ProductService.Services
{
    public class DistributedEventService(ILogger<DistributedEventService> logger, IEventBus eventBus, ProductServiceDbContext dbContext, IDistributedEventLogService eventLogService) : IDistributedEventService
    {
        public async Task PublishThroughEventBusAsync(DistributedEvent @event)
        {
            try
            {
                await eventLogService.MarkEventAsInProgressAsync(@event.Id);
                await eventBus.PublishAsync(@event);
                await eventLogService.MarkEventAsPublishedAsync(@event.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Publish through event bus failed for {EventId}", @event.Id);
                await eventLogService.MarkEventAsFailedAsync(@event.Id);
            }
        }

        public async Task SaveEventAndDbContextChangesAsync(DistributedEvent @event)
        {
            await ResilientTransaction.New(dbContext).ExecuteAsync(async () =>
            {
                var transaction = dbContext.Database.CurrentTransaction ?? await dbContext.Database.BeginTransactionAsync();
                await dbContext.SaveChangesAsync();
                await eventLogService.SaveEventAsync(@event, transaction);
            });
        }
    }
}
