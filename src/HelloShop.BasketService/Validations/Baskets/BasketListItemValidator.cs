// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using FluentValidation;
using FluentValidation.Validators;
using HelloShop.BasketService.Protos;

namespace HelloShop.BasketService.Validations.Baskets
{
    public class BasketListItemValidator : AbstractValidator<BasketListItem>
    {
        public BasketListItemValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}