// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;
using HelloShop.OrderingService.Entities.EventLogs;
using Microsoft.EntityFrameworkCore;

namespace HelloShop.OrderingService.Services
{
    public class DistributedEventService<TContext>(TContext dbContext, IEventBus distributedEventBus, ILogger<DistributedEventService<TContext>> logger) : IDistributedEventService, IDisposable where TContext : DbContext
    {
        private volatile bool _disposedValue;

        public async Task UpdateEventStatusAsync(Guid eventId, DistributedEventStatus status, CancellationToken cancellationToken = default)
        {
            var eventLogEntry = dbContext.Set<DistributedEventLog>().Single(ie => ie.EventId == eventId);

            eventLogEntry.Status = status;

            if (status == DistributedEventStatus.InProgress)
            {
                eventLogEntry.TimesSent++;
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<DistributedEventLog>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId, CancellationToken cancellationToken = default)
        {
            var result = await dbContext.Set<DistributedEventLog>().Where(e => e.TransactionId == transactionId && e.Status == DistributedEventStatus.NotPublished).ToListAsync(cancellationToken: cancellationToken);

            return result.Count != 0 ? result.OrderBy(o => o.CreationTime) : [];
        }

        public async Task AddAndSaveEventAsync(DistributedEvent @event, CancellationToken cancellationToken = default)
        {
            var transaction = dbContext.Database.CurrentTransaction ?? throw new InvalidOperationException("This method must be called within a transaction scope.");

            var eventLog = new DistributedEventLog(@event, transaction.TransactionId);

            await dbContext.AddAsync(eventLog, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task PublishEventsThroughEventBusAsync(Guid transactionId, CancellationToken cancellationToken = default)
        {
            var pendingEventLogs = await RetrieveEventLogsPendingToPublishAsync(transactionId, cancellationToken);

            foreach (var eventLog in pendingEventLogs)
            {
                logger.LogInformation("Publishing integration event {EventId} {DistributedEvent}", eventLog.EventId, eventLog.DistributedEvent);

                try
                {
                    await UpdateEventStatusAsync(eventLog.EventId, DistributedEventStatus.InProgress, cancellationToken);
                    await distributedEventBus.PublishAsync(eventLog.DistributedEvent, cancellationToken);
                    await UpdateEventStatusAsync(eventLog.EventId, DistributedEventStatus.Published, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error publishing distributed event {EventId}", eventLog.EventId);

                    await UpdateEventStatusAsync(eventLog.EventId, DistributedEventStatus.PublishedFailed, cancellationToken);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
