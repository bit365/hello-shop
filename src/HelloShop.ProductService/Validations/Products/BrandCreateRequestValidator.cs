// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using FluentValidation;
using HelloShop.ProductService.Models.Products;

namespace HelloShop.ProductService.Validations.Products
{
    public class BrandCreateRequestValidator : AbstractValidator<BrandCreateRequest>
    {
        public BrandCreateRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().Length(8, 32);
        }
    }
}