namespace HelloShop.ProductService.Models.Products;
public class ProductDetailsResponse
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public string? Description { get; init; }

    public decimal Price { get; init; }

    public required BrandDetailsResponse Brand { get; init; }

    public string? ImageUrl { get; init; }

    public DateTimeOffset CreationTime { get; init; }
}
