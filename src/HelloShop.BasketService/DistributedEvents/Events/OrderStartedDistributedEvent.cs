// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;

namespace HelloShop.BasketService.DistributedEvents.Events
{
    public record OrderStartedDistributedEvent(int UserId) : DistributedEvent;
}
