// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Entities.Orders;
using MediatR;

namespace HelloShop.OrderingService.LocalEvents
{
    public record class OrderStartedLocalEvent(Order Order) : INotification;
}
