// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Grpc.Net.Client;
using HelloShop.BasketService.Protos;

namespace HelloShop.BasketService.FunctionalTests
{
    public class GreeterServiceIntegrationTest
    {
        [Fact]
        public async Task SayHelloReturnsHelloMessage()
        {
            // Arrange
            using var channel = GrpcChannel.ForAddress(GrpcConstants.GrpcAddress);
            var client = new Greeter.GreeterClient(channel);

            // Act
            var reply = await client.SayHelloAsync(new HelloRequest { Name = "Greeter" });

            // Assert
            Assert.Equal("Hello Greeter", reply.Message);
        }
    }
}
