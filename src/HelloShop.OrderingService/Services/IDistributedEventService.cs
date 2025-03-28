// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;

namespace HelloShop.OrderingService.Services
{
    public interface IDistributedEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId, CancellationToken cancellationToken = default);

        Task AddAndSaveEventAsync(DistributedEvent @event, CancellationToken cancellationToken = default);
    }
}
