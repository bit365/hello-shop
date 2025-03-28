// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace HelloShop.EventBus.Logging
{
    public class DistributedEventLogService<TContext>(TContext dbContext) : IDistributedEventLogService where TContext : DbContext
    {
        public async Task<IEnumerable<DistributedEventLog>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Set<DistributedEventLog>().Where(e => e.TransactionId == transactionId && e.Status == DistributedEventStatus.NotPublished).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<DistributedEventLog>> RetrieveEventLogsFailedToPublishAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.Set<DistributedEventLog>().Where(e => e.Status == DistributedEventStatus.PublishedFailed).ToListAsync(cancellationToken);
        }

        public async Task SaveEventAsync(DistributedEvent @event, IDbContextTransaction transaction, CancellationToken cancellationToken = default)
        {
            var eventLog = new DistributedEventLog(@event, transaction.TransactionId);
            dbContext.Database.UseTransaction(transaction.GetDbTransaction());
            await dbContext.Set<DistributedEventLog>().AddAsync(eventLog, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

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

        public Task MarkEventAsPublishedAsync(Guid eventId, CancellationToken cancellationToken = default) => UpdateEventStatusAsync(eventId, DistributedEventStatus.Published, cancellationToken);

        public Task MarkEventAsInProgressAsync(Guid eventId, CancellationToken cancellationToken = default) => UpdateEventStatusAsync(eventId, DistributedEventStatus.InProgress, cancellationToken);

        public Task MarkEventAsFailedAsync(Guid eventId, CancellationToken cancellationToken = default) => UpdateEventStatusAsync(eventId, DistributedEventStatus.PublishedFailed, cancellationToken);
    }
}
