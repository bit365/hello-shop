using FluentValidation;
using HelloShop.ProductService.Models.Products;

namespace HelloShop.ProductService.Validations.Products
{
    public class BrandUpdateRequestValidator : AbstractValidator<BrandUpdateRequest>
    {
        public BrandUpdateRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotNull().NotEmpty().Length(8, 32);
        }
    }
}