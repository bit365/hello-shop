// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.ProductService.Entities.Products
{
    public class Product
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public decimal Price { get; set; }

        public int BrandId { get; set; }

        public Brand Brand { get; set; } = default!;

        public required string ImageUrl { get; set; }

        public int AvailableStock { get; set; }

        public DateTimeOffset CreationTime { get; init; } = TimeProvider.System.GetUtcNow();
    }
}