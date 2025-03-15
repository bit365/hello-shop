// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Aspire.Hosting.ApplicationModel;
using HelloShop.FunctionalTests.Helpers;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.DependencyInjection;
using System.Dynamic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace HelloShop.FunctionalTests
{
    public class FirstWebApiIntegrationTest(TestingAspireAppHost app) : IAsyncLifetime, IClassFixture<TestingAspireAppHost>
    {
        public async Task InitializeAsync()
        {
            await app.StartAsync();
        }

        public async Task DisposeAsync() => await Task.CompletedTask;

        [Fact]
        public async Task WebAppRootReturnsOkStatusCode()
        {
            // Arrange
            var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();

            // Act
            HttpClient httpClient = app.CreateHttpClient("webapp");
            await resourceNotificationService.WaitForResourceAsync("webapp", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
            HttpResponseMessage response = await httpClient.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task IdetityServiceAccountLoginReturnsSuccessStatusCode()
        {
            // Arrange
            var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();

            // Act
            HttpClient httpClient = app.CreateHttpClient("identityservice");
            await resourceNotificationService.WaitForResourceHealthyAsync("identityservice").WaitAsync(TimeSpan.FromSeconds(30));
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
            var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();

            // Act
            HttpClient httpClient = app.CreateHttpClient("identityservice");
            await resourceNotificationService.WaitForResourceHealthyAsync("identityservice").WaitAsync(TimeSpan.FromSeconds(30));
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
            var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();

            // Act
            HttpClient httpClient = app.CreateHttpClient("identityservice");
            await resourceNotificationService.WaitForResourceHealthyAsync("identityservice").WaitAsync(TimeSpan.FromSeconds(30));
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
            var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();

            // Act
            HttpClient identityServiceHttpClient = app.CreateHttpClient("identityservice");
            await resourceNotificationService.WaitForResourceHealthyAsync("identityservice").WaitAsync(TimeSpan.FromSeconds(30));
            HttpResponseMessage loginResponse = await identityServiceHttpClient.PostAsJsonAsync("api/Account/Login", new
            {
                UserName = "admin",
                Password = "admin"
            });

            AccessTokenResponse? accessTokenResponse = await loginResponse.Content.ReadFromJsonAsync<AccessTokenResponse>();

            HttpClient productServiceHttpClient = app.CreateHttpClient("productservice");
            productServiceHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenResponse?.AccessToken);
            await resourceNotificationService.WaitForResourceHealthyAsync("productservice").WaitAsync(TimeSpan.FromSeconds(30));

            HttpResponseMessage productDetailsResponse = await productServiceHttpClient.GetAsync("api/Products/1");

            JsonNode? result = await productDetailsResponse.Content.ReadFromJsonAsync<JsonNode>();

            int? productId = result?["Id"]?.GetValue<int?>();

            // Assert
            Assert.NotNull(productId);
            Assert.Equal(1, productId);
        }
    }
}