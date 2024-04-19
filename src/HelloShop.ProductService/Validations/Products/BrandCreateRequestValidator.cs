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