// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using MediatR;

namespace HelloShop.OrderingService.Commands.Orders
{
    public record CancelOrderCommand(int OrderNumber) : IRequest<bool>;
}