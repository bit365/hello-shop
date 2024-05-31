// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.ProductService.Models.Products
{
    public class ProductCreateRequest
    {
        public required string Name { get; init; }

        public string? Description { get; init; }

        public decimal Price { get; init; }

        public int BrandId { get; init; }

        public string? ImageUrl { get; init; }
    }
}