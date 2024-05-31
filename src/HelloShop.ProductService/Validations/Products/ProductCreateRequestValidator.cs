// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using FluentValidation;
using HelloShop.ProductService.Models.Products;

namespace HelloShop.ProductService.Validations.Products
{
    public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
    {
        public ProductCreateRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().Length(8, 32);
            RuleFor(x => x.Description).Length(8, 256);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.BrandId).GreaterThan(0);
            RuleFor(x => x.ImageUrl).Length(8, 256);
        }
    }
}
