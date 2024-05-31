// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using AutoMapper;
using HelloShop.ProductService.Entities.Products;
using HelloShop.ProductService.Models.Products;

namespace HelloShop.ProductService.AutoMapper;

public class ProductsMapConfiguration : Profile
{
    public ProductsMapConfiguration()
    {
        CreateMap<ProductCreateRequest, Product>();
        CreateMap<ProductUpdateRequest, Product>();
        CreateMap<Product, ProductListItem>().AfterMap((src, dest) => dest.BrandName = src.Brand.Name);
        CreateMap<Product, ProductDetailsResponse>();

        CreateMap<BrandCreateRequest, Brand>();
        CreateMap<BrandUpdateRequest, Brand>();
        CreateMap<Brand, BrandDetailsResponse>();
        CreateMap<Brand, BrandListItem>();
    }
}
