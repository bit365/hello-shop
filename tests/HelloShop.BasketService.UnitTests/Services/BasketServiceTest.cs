// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Google.Protobuf.WellKnownTypes;
using HelloShop.BasketService.AutoMapper;
using HelloShop.BasketService.Entities;
using HelloShop.BasketService.Protos;
using HelloShop.BasketService.Repositories;
using HelloShop.BasketService.Services;
using HelloShop.BasketService.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Security.Claims;

namespace HelloShop.BasketService.UnitTests.Services
{
    public class BasketServiceTest
    {
        [Fact]
        public async Task GetBasketReturnsEmptyForNoUser()
        {
            // Arrange
            var basketRepositoryMock = new Mock<IBasketRepository>();
            var loggerMock = NullLogger<CustomerBasketService>.Instance;
            var mapperMock = new Mock<IMapper>();
            var validatorMock = new Mock<IValidator<UpdateBasketRequest>>();
            var service = new CustomerBasketService(basketRepositoryMock.Object, loggerMock, mapperMock.Object, validatorMock.Object);

            TestServerCallContext serverCallContext = TestServerCallContext.Create();

            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1")]))
            };

            serverCallContext.UserState["__HttpContext"] = httpContext;

            // Act
            CustomerBasketResponse result = await service.GetBasket(new Empty(), serverCallContext);

            // Assert
            Assert.Empty(result.Items);
        }

        [Fact]
        public async Task GetBasketReturnsItemsForValidUserId()
        {
            // Arrange
            var basketRepositoryMock = new Mock<IBasketRepository>();
            basketRepositoryMock.Setup(x => x.GetBasketAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new CustomerBasket() { BuyerId = 1, Items = [new BasketItem { ProductId = 1, Quantity = 1 }] });

            var validatorMock = new Mock<IValidator<UpdateBasketRequest>>();

            var logger = NullLogger<CustomerBasketService>.Instance;

            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BasketsMapConfiguration>(), new NullLoggerFactory()));

            var service = new CustomerBasketService(basketRepositoryMock.Object, logger, mapper, validatorMock.Object);

            TestServerCallContext serverCallContext = TestServerCallContext.Create();

            HttpContext httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1")]))
            };

            serverCallContext.UserState["__HttpContext"] = httpContext;

            // Act
            CustomerBasketResponse result = await service.GetBasket(new Empty(), serverCallContext);

            // Assert
            Assert.NotEmpty(result.Items);
        }

        [Fact]
        public async Task UpdateBasketReturnsCustomerBasketResponse()
        {
            // Arrange
            var basketRepositoryMock = new Mock<IBasketRepository>();
            basketRepositoryMock.Setup(x => x.UpdateBasketAsync(It.IsAny<CustomerBasket>(), It.IsAny<CancellationToken>())).ReturnsAsync((CustomerBasket basket, CancellationToken token) => basket);

            var logger = NullLogger<CustomerBasketService>.Instance;
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BasketsMapConfiguration>(), new NullLoggerFactory()));

            var validatorMock = new Mock<IValidator<UpdateBasketRequest>>();
            validatorMock.Setup(x => x.ValidateAsync(It.IsAny<UpdateBasketRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            var service = new CustomerBasketService(basketRepositoryMock.Object, logger, mapper, validatorMock.Object);

            TestServerCallContext serverCallContext = TestServerCallContext.Create();

            HttpContext httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1")]))
            };

            serverCallContext.UserState["__HttpContext"] = httpContext;

            // Act
            UpdateBasketRequest updateBasketRequest = new()
            {
                Items = { new BasketListItem { ProductId = 1, Quantity = 1 }, new BasketListItem { ProductId = 2, Quantity = 2 } }
            };

            CustomerBasketResponse result = await service.UpdateBasket(updateBasketRequest, serverCallContext);

            // Assert
            Assert.Collection(result.Items, item => Assert.Equal(1, item.ProductId), item => Assert.Equal(2, item.ProductId));
        }

        [Fact]
        public async Task DeleteBasketReturnsEmpty()
        {
            // Arrange
            var basketRepositoryMock = new Mock<IBasketRepository>();
            var logger = NullLogger<CustomerBasketService>.Instance;
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BasketsMapConfiguration>(), new NullLoggerFactory()));

            var validatorMock = new Mock<IValidator<UpdateBasketRequest>>();

            var service = new CustomerBasketService(basketRepositoryMock.Object, logger, mapper, validatorMock.Object);

            TestServerCallContext serverCallContext = TestServerCallContext.Create();

            HttpContext httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1")]))
            };

            serverCallContext.UserState["__HttpContext"] = httpContext;

            // Act
            Empty result = await service.DeleteBasket(new Empty(), serverCallContext);

            // Assert
            Assert.NotNull(result);
        }
    }
}
