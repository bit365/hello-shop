// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;

namespace HelloShop.OrderingService.DistributedEvents.Events
{
    public record OrderAwaitingValidationDistributedEvent(int OrderId, IEnumerable<OrderStockItem> OrderStockItems) : DistributedEvent;

    public record OrderStockItem(int ProductId, int Units);
}
