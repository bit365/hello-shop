// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Aspire.Hosting;
using Microsoft.AspNetCore.Authentication.BearerToken;
using System.Dynamic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace HelloShop.FunctionalTests
{
    public class FirstWebApiIntegrationTest
    {
        [Fact]
        public async Task WebAppRootReturnsOkStatusCode()
        {
            // Arrange
            IDistributedApplicationTestingBuilder appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.HelloShop_AppHost>();
            await using DistributedApplication app = await appHost.BuildAsync();
            await app.StartAsync();

            // Act
            HttpClient httpClient = app.CreateHttpClient("webapp");
            HttpResponseMessage response = await httpClient.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task IdetityServiceAccountLoginReturnsSuccessStatusCode()
        {
            // Arrange
            IDistributedApplicationTestingBuilder appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.HelloShop_AppHost>();
            await using DistributedApplication app = await appHost.BuildAsync();
            await app.StartAsync();

            // Act
            HttpClient httpClient = app.CreateHttpClient("identityservice");
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/Account/Login", new
            {
                UserName = "guest",
                Password = "guest"
            });

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task IdetityServiceAccountLoginReturnsAccessToken()
        {
            // Arrange
            IDistributedApplicationTestingBuilder appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.HelloShop_AppHost>();
            await using DistributedApplication app = await appHost.BuildAsync();
            await app.StartAsync();

            // Act
            HttpClient httpClient = app.CreateHttpClient("identityservice");
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/Account/Login", new
            {
                UserName = "guest",
                Password = "guest"
            });
            dynamic? result = await response.Content.ReadFromJsonAsync<ExpandoObject>();

            // Assert
            Assert.NotNull(result?.accessToken);
        }

        [Fact]
        public async Task IdetityServiceAccountLoginReturnsTokenExpiresInSeconds()
        {
            // Arrange
            IDistributedApplicationTestingBuilder appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.HelloShop_AppHost>();
            await using DistributedApplication app = await appHost.BuildAsync();

            await app.StartAsync();

            // Act
            HttpClient httpClient = app.CreateHttpClient("identityservice");
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/Account/Login", new
            {
                UserName = "guest",
                Password = "guest"
            });
            JsonNode? result = await response.Content.ReadFromJsonAsync<JsonNode>();
            int expiresInSeconds = result?["expiresIn"]?.GetValue<int>() ?? default;

            // Assert
            Assert.Equal(3600, expiresInSeconds);
        }

        [Fact]
        public async Task ProductServiceGetProductReturnsProductDetails()
        {
            // Arrange
            IDistributedApplicationTestingBuilder appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.HelloShop_AppHost>();

            await using DistributedApplication app = await appHost.BuildAsync();
            await app.StartAsync();

            // Act
            HttpClient identityServiceHttpClient = app.CreateHttpClient("identityservice");
            HttpResponseMessage loginResponse = await identityServiceHttpClient.PostAsJsonAsync("api/Account/Login", new
            {
                UserName = "admin",
                Password = "admin"
            });

            AccessTokenResponse? accessTokenResponse = await loginResponse.Content.ReadFromJsonAsync<AccessTokenResponse>();

            HttpClient productServiceHttpClient = app.CreateHttpClient("productservice");
            productServiceHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenResponse?.AccessToken);

            HttpResponseMessage productDetailsResponse = await productServiceHttpClient.GetAsync("api/Products/1");

            JsonNode? result = await productDetailsResponse.Content.ReadFromJsonAsync<JsonNode>();

            string? productName = result?["Name"]?.GetValue<string>();

            // Assert
            Assert.NotNull(productName);
            Assert.Equal("Product 1", productName);
        }
    }
}