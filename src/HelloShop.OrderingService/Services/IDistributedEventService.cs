// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Entities.EventLogs;
using HelloShop.ServiceDefaults.DistributedEvents.Abstractions;

namespace HelloShop.OrderingService.Services
{
    public interface IDistributedEventService
    {
        Task<IEnumerable<DistributedEventLog>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId, CancellationToken cancellationToken = default);

        Task AddAndSaveEventAsync(DistributedEvent @event, CancellationToken cancellationToken = default);

        Task UpdateEventStatusAsync(Guid eventId, DistributedEventStatus status, CancellationToken cancellationToken = default);

        Task PublishEventsThroughEventBusAsync(Guid transactionId, CancellationToken cancellationToken = default);
    }
}
