// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.ServiceDefaults.DistributedEvents.Abstractions
{
    public interface IDistributedEventHandler
    {
        Task HandleAsync(DistributedEvent @event);
    }

    public interface IDistributedEventHandler<in TDistributedEvent> : IDistributedEventHandler where TDistributedEvent : DistributedEvent
    {
        Task HandleAsync(TDistributedEvent @event);

        Task IDistributedEventHandler.HandleAsync(DistributedEvent @event) => HandleAsync((TDistributedEvent)@event);
    }
}
