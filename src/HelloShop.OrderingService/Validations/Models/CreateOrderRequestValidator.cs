// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using FluentValidation;
using HelloShop.OrderingService.Models.Orders;

namespace HelloShop.OrderingService.Validations.Models
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.Items).NotNull().NotEmpty();
            RuleForEach(x => x.Items).SetValidator(new BasketListItemValidator());
            RuleFor(x => x.Country).NotNull().NotEmpty().Length(1, 16);
            RuleFor(x => x.State).NotNull().NotEmpty().Length(1, 16);
            RuleFor(x => x.City).NotNull().NotEmpty().Length(1, 16);
            RuleFor(x => x.Street).NotNull().NotEmpty().MaximumLength(32);
            RuleFor(x => x.ZipCode).NotNull().NotEmpty().Length(6).Must(x => x.All(char.IsNumber));
            RuleFor(x => x.CardAlias).NotNull().NotEmpty().Length(1, 16);
            RuleFor(x => x.CardNumber).NotNull().NotEmpty().Length(16).Must(x => x.All(char.IsNumber));
            RuleFor(x => x.CardHolderName).NotNull().NotEmpty().Length(1, 8);
            RuleFor(x => x.CardSecurityNumber).NotNull().NotEmpty().Length(6).Must(x => x.All(char.IsNumber));
            RuleFor(x => x.CardExpiration).Must(x => x.HasValue && x.Value > DateTimeOffset.Now);
        }

        public class BasketListItemValidator : AbstractValidator<BasketItem>
        {
            public BasketListItemValidator()
            {
                RuleFor(x => x.ProductId).GreaterThan(0);
                RuleFor(x => x.Quantity).GreaterThan(0);
                RuleFor(x => x.ProductName).Length(2, 16);
                RuleFor(x => x.PictureUrl).MaximumLength(256);
                RuleFor(x => x.UnitPrice).GreaterThan(0);
            }
        }
    }
}
