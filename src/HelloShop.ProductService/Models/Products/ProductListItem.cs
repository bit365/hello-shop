namespace HelloShop.ProductService.Models.Products;

public class ProductListItem
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public decimal Price { get; init; }

    public required string BrandName { get; set; }

    public DateTimeOffset CreationTime { get; init; }
}
