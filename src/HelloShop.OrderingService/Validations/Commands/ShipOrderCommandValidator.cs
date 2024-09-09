// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using FluentValidation;
using HelloShop.OrderingService.Commands.Orders;

namespace HelloShop.OrderingService.Validations.Commands
{
    public class ShipOrderCommandValidator : AbstractValidator<ShipOrderCommand>
    {
        public ShipOrderCommandValidator()
        {
            RuleFor(order => order.OrderNumber).GreaterThan(0);
        }
    }
}
