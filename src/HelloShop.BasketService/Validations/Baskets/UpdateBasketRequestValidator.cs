// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using FluentValidation;
using HelloShop.BasketService.Protos;

namespace HelloShop.BasketService.Validations.Baskets
{
    public class UpdateBasketRequestValidator : AbstractValidator<UpdateBasketRequest>
    {
        public UpdateBasketRequestValidator()
        {
            RuleFor(x => x.Items).NotNull();
            RuleForEach(x => x.Items).SetValidator(new BasketListItemValidator());
        }
    }
}
