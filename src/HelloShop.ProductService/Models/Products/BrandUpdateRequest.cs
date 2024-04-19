namespace HelloShop.ProductService.Models.Products;

public class BrandUpdateRequest
{
    public int Id { get; init; }

    public required string Name { get; init; }
}
