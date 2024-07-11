// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using HelloShop.BasketService.Protos;

namespace HelloShop.BasketService.FunctionalTests
{
    public class BasketServiceIntegrationTest
    {
        private readonly Basket.BasketClient _client;

        public BasketServiceIntegrationTest()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(GrpcConstants.GrpcAddress);
            CallInvoker invoker = channel.Intercept(new AuthenticatedInterceptor());
            _client = new Basket.BasketClient(invoker);
        }

        [Fact]
        public async Task GetBasketReturnsBasket()
        {
            // Arrange
            var request = new Empty();

            // Act
            var reply = await _client.GetBasketAsync(request);

            // Assert
            Assert.NotNull(reply);
        }

        [Fact]
        public async Task UpdateBasketReturnsBasketResponse()
        {
            // Arrange
            var request = new UpdateBasketRequest
            {
                Items =
                {
                    new BasketListItem { ProductId = 1, Quantity = 2 },
                    new BasketListItem { ProductId = 2, Quantity = 3 }
                }
            };

            // Act
            var reply = await _client.UpdateBasketAsync(request);

            // Assert
            Assert.Equal(2, reply.Items.Count);
        }

        [Fact]
        public async Task DeleteBasketReturnsEmpty()
        {
            // Arrange
            var request = new Empty();

            // Act
            var reply = await _client.DeleteBasketAsync(request);

            // Assert
            Assert.NotNull(reply);
        }
    }
}