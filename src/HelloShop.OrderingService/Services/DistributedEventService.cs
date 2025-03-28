// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;
using HelloShop.EventBus.Logging;
using HelloShop.OrderingService.Infrastructure;

namespace HelloShop.OrderingService.Services
{
    public class DistributedEventService(ILogger<DistributedEventService> logger, IEventBus eventBus, OrderingServiceDbContext dbContext, IDistributedEventLogService eventLogService) : IDistributedEventService
    {
        public async Task PublishEventsThroughEventBusAsync(Guid transactionId, CancellationToken cancellationToken = default)
        {
            var pendingLogEvents = await eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId, cancellationToken);

            foreach (var eventLog in pendingLogEvents)
            {
                logger.LogInformation("Publishing distributed event: {EventId} from {EventType}.", eventLog.EventId, eventLog.EventTypeName);

                try
                {
                    await eventLogService.MarkEventAsInProgressAsync(eventLog.EventId, cancellationToken);
                    await eventBus.PublishAsync(eventLog.DistributedEvent, cancellationToken);
                    await eventLogService.MarkEventAsPublishedAsync(eventLog.EventId, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error publishing distributed event: {EventId}.", eventLog.EventId);
                    await eventLogService.MarkEventAsFailedAsync(eventLog.EventId, cancellationToken);
                }
            }
        }

        public async Task AddAndSaveEventAsync(DistributedEvent @event, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Creating and saving distributed event: {EventId} from {EventType}.", @event.Id, @event.GetType().Name);

            var transaction = dbContext.Database.CurrentTransaction ?? await dbContext.Database.BeginTransactionAsync(cancellationToken);

            await eventLogService.SaveEventAsync(@event, transaction, cancellationToken);
        }
    }
}
