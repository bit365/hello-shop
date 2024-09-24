// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.ProductService.Models.Products
{
    public class ProductUpdateRequest
    {
        public int Id { get; init; }

        public required string Name { get; init; }

        public required string Description { get; init; }

        public decimal Price { get; init; }

        public int BrandId { get; init; }

        public required string ImageUrl { get; init; }

        public int AvailableStock { get; init; }
    }
}