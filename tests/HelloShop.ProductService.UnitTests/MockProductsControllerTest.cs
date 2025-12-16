// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using AutoMapper;
using HelloShop.ProductService.Controllers;
using HelloShop.ProductService.Entities.Products;
using HelloShop.ProductService.Models.Products;
using HelloShop.ProductService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace HelloShop.ProductService.UnitTests
{
    public class MockProductsControllerTest
    {
        [Fact]
        public async Task GetProductReturnsNotFoundIfNotExists()
        {
            // Arrange
            Mock<IProductService> mock = new();

            mock.Setup(m => m.FindAsync(It.Is<int>(id => id == 1), It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

            // Act
            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductDetailsResponse>(), new NullLoggerFactory()));
            MockProductsController mockProductsController = new(mock.Object, mapper);
            ActionResult<ProductDetailsResponse> result = await mockProductsController.GetProduct(1);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAllReturnsPorductListItems()
        {
            // Arrange
            Mock<IProductService> mock = new();

            var mockProducts = new List<Product> {
                new() { Id = 1, Name = "Product 1", Description = "Product 1", ImageUrl = "1.jpg", Price = 10 },
                new() { Id = 2, Name = "Product 2",Description="Product 2", ImageUrl="2.jpg", Price = 20 }
            };

            mock.Setup(m => m.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mockProducts);

            // Act
            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductListItem>(), new NullLoggerFactory()));
            MockProductsController mockProductsController = new(mock.Object, mapper);
            ActionResult<IEnumerable<ProductListItem>> result = await mockProductsController.GetProducts();

            //Assert
            Assert.NotNull(result.Value);

            Assert.Collection(result.Value, productListItem1 =>
            {
                Assert.Equal(1, productListItem1.Id);
                Assert.Equal("Product 1", productListItem1.Name);
                Assert.Equal(10, productListItem1.Price);
            }, productListItem2 =>
            {
                Assert.Equal(2, productListItem2.Id);
                Assert.Equal("Product 2", productListItem2.Name);
                Assert.Equal(20, productListItem2.Price);
            });
        }
    }
}