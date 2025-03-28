// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace HelloShop.EventBus.Logging
{
    public interface IDistributedEventLogService
    {
        Task<IEnumerable<DistributedEventLog>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId, CancellationToken cancellationToken = default);

        Task<IEnumerable<DistributedEventLog>> RetrieveEventLogsFailedToPublishAsync(CancellationToken cancellationToken = default);

        Task SaveEventAsync(DistributedEvent @event, IDbContextTransaction transaction, CancellationToken cancellationToken = default);

        Task MarkEventAsPublishedAsync(Guid eventId, CancellationToken cancellationToken = default);

        Task MarkEventAsInProgressAsync(Guid eventId, CancellationToken cancellationToken = default);

        Task MarkEventAsFailedAsync(Guid eventId, CancellationToken cancellationToken = default);
    }
}