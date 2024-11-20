﻿// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ServiceDefaults.DistributedEvents.Abstractions;

namespace HelloShop.OrderingService.DistributedEvents.Events
{
    public record OrderPaymentFailedDistributedEvent : DistributedEvent
    {
        public int OrderId { get; }

        public OrderPaymentFailedDistributedEvent(int orderId) => OrderId = orderId;
    }
}