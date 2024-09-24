// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using AutoMapper;
using HelloShop.ProductService.AutoMapper;
using HelloShop.ProductService.Controllers;
using HelloShop.ProductService.Entities.Products;
using HelloShop.ProductService.Infrastructure;
using HelloShop.ProductService.Models.Products;
using HelloShop.ProductService.UnitTests.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HelloShop.ProductService.UnitTests
{
    public class ProductsControllerTest
    {
        [Fact]
        public async Task GetProductByIdReturnsProductDetailsResponse()
        {
            // Arrange
            await using ProductServiceDbContext dbContext = new FakeDbContextFactory().CreateDbContext();

            await dbContext.AddAsync(new Product { Id = 1, Name = "Product 1", Price = 10, Description = "Product 1", ImageUrl = "1.jpg" });

            await dbContext.SaveChangesAsync();

            IMapper mapper = new MapperConfiguration(configure => configure.CreateMap<Product, ProductDetailsResponse>()).CreateMapper();

            ProductsController productsController = new(dbContext, mapper);

            // Act
            ActionResult<ProductDetailsResponse> result = await productsController.GetProduct(1);

            //Assert
            Assert.Equal(10, result.Value?.Price);
        }

        [Theory]
        [InlineData("Product 1", 10, "Product 1", 1, "1.jpg", 100)]
        [InlineData("Product 2", 20, "Product 2", 2, "2.jpg", 200)]
        [InlineData("Product 3", 30, "Product 3", 3, "3.jpg", 300)]
        public async Task PostProductReturnsProductDetailsResponse(string productName, decimal price, string description, int brandId, string imageUrl, int availableStock)
        {
            // Arrange
            await using ProductServiceDbContext dbContext = new FakeDbContextFactory().CreateDbContext();

            IMapper mapper = new MapperConfiguration(configure => configure.AddProfile<ProductsMapConfiguration>()).CreateMapper();

            ProductsController productsController = new(dbContext, mapper);

            // Act
            ProductCreateRequest productCreateRequest = new() { Name = productName, Price = price, Description = description, BrandId = brandId, ImageUrl = imageUrl, AvailableStock = availableStock };
            ActionResult<ProductDetailsResponse> createdAtActionResult = await productsController.PostProduct(productCreateRequest);
            ProductDetailsResponse? result = (createdAtActionResult?.Result as CreatedAtActionResult)?.Value as ProductDetailsResponse;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(price, result?.Price);
                Assert.Equal(productName, result?.Name);
            });
        }
    }
}