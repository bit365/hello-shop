// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using FluentValidation;
using HelloShop.OrderingService.Commands.Orders;

namespace HelloShop.OrderingService.Validations.Commands
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator(TimeProvider timeProvider)
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.UserName).NotNull().NotEmpty().Length(1, 16);
            RuleFor(x => x.OrderItems).NotNull().NotEmpty();
            RuleForEach(x => x.OrderItems).SetValidator(new BasketListItemValidator());
            RuleFor(x => x.Country).NotNull().NotEmpty().Length(1, 16);
            RuleFor(x => x.State).NotNull().NotEmpty().Length(1, 16);
            RuleFor(x => x.City).NotNull().NotEmpty().Length(1, 16);
            RuleFor(x => x.Street).NotNull().NotEmpty().MaximumLength(32);
            RuleFor(x => x.ZipCode).NotNull().NotEmpty().Length(6).Must(x => x.All(char.IsNumber));
            RuleFor(x => x.CardAlias).NotNull().NotEmpty().Length(1, 16);
            RuleFor(x => x.CardNumber).NotNull().NotEmpty().Length(16).Must(x => x.All(char.IsNumber));
            RuleFor(x => x.CardHolderName).NotNull().NotEmpty().Length(1, 8);
            RuleFor(x => x.CardSecurityNumber).NotNull().NotEmpty().Length(6).Must(x => x.All(char.IsNumber));
            RuleFor(x => x.CardExpiration).Must(x => x.HasValue && x.Value > timeProvider.GetUtcNow());
        }

        public class BasketListItemValidator : AbstractValidator<CreateOrderCommand.CreateOrderCommandItem>
        {
            public BasketListItemValidator()
            {
                RuleFor(x => x.ProductId).GreaterThan(0);
                RuleFor(x => x.ProductName).NotNull().NotEmpty().Length(1, 16);
                RuleFor(x => x.PictureUrl).NotNull().NotEmpty().Length(1, 256);
                RuleFor(x => x.ProductId).GreaterThan(0);
                RuleFor(x => x.UnitPrice).GreaterThan(0);
            }
        }
    }
}
