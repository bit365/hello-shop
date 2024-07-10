// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using FluentValidation;
using HelloShop.BasketService.Protos;

namespace HelloShop.BasketService.Validations.Greeters
{
    public class HelloRequestValidator : AbstractValidator<HelloRequest>
    {
        public HelloRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().Length(3, 20);
        }
    }
}
