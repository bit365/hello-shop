// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;

namespace HelloShop.OrderingService.DistributedEvents.Events
{
    public record OrderPaymentSucceededDistributedEvent(int OrderId) : DistributedEvent;
}
