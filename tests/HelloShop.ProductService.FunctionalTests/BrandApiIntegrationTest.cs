// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ProductService.FunctionalTests.Utilities;
using HelloShop.ProductService.Models.Products;
using System.Net;
using System.Net.Http.Json;

namespace HelloShop.ProductService.FunctionalTests
{
    public class BrandApiIntegrationTest(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GetBrandByIdReturnsUnauthorizedStatusCode()
        {
            // Arrange
            HttpClient client = factory.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync("api/Brands/1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task GetBrandByIdReturnsBrandDetailsResponse()
        {
            // Arrange
            HttpClient client = factory.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync("api/Brands/1");
            BrandDetailsResponse? brandDetails = await response.Content.ReadFromJsonAsync<BrandDetailsResponse>();

            // Assert
            Assert.NotNull(brandDetails);
            Assert.Equal(1, brandDetails.Id);
        }
    }
}