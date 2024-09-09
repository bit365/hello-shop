// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using FluentValidation;
using HelloShop.OrderingService.Commands.Orders;

namespace HelloShop.OrderingService.Validations.Commands
{
    public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator()
        {
            RuleFor(x => x.OrderNumber).GreaterThan(0);
        }
    }
}
