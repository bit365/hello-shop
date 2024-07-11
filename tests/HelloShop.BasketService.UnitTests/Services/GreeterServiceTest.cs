// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.BasketService.Protos;
using HelloShop.BasketService.Services;
using HelloShop.BasketService.UnitTests.Helpers;

namespace HelloShop.BasketService.UnitTests.Services
{
    public class GreeterServiceTest
    {
        [Fact]
        public async Task SayHelloReturnsCorrectMessage()
        {
            // Arrange
            var service = new GreeterService();
            var request = new HelloRequest { Name = "World" };

            // Act
            var response = await service.SayHello(request, TestServerCallContext.Create());

            // Assert
            Assert.Equal("Hello World", response.Message);
        }
    }
}
