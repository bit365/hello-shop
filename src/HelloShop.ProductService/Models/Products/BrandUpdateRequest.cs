// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.ProductService.Models.Products;

public class BrandUpdateRequest
{
    public int Id { get; init; }

    public required string Name { get; init; }
}
