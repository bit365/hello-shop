// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;

namespace HelloShop.ProductService.DistributedEvents.Events
{
    public record OrderPaidDistributedEvent(int OrderId, IEnumerable<OrderStockItem> OrderStockItems) : DistributedEvent;
}
