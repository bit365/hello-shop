// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using FluentValidation;
using HelloShop.OrderingService.Commands;
using HelloShop.OrderingService.Commands.Orders;

namespace HelloShop.OrderingService.Validations.Commands
{
    public class CreateOrderIdentifiedCommandValidator : AbstractValidator<IdentifiedCommand<CreateOrderCommand, bool>>
    {
        public CreateOrderIdentifiedCommandValidator() => RuleFor(command => command.Id).Must(id => id != Guid.Empty).WithMessage("Invalid x-request-id in the request header.");
    }
}
