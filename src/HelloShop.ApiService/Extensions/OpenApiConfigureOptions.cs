// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ApiService.Infrastructure;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace HelloShop.ApiService.Extensions;

public class OpenApiConfigureOptions(IConfiguredServiceEndPointResolver serviceResolver, HttpClient httpClient, ILogger<OpenApiConfigureOptions> logger) : IConfigureOptions<SwaggerUIOptions>
{
    public void Configure(SwaggerUIOptions options)
    {
        List<UrlDescriptor> urlDescriptors = [];

        foreach (var serviceEndpoint in serviceResolver.GetConfiguredServiceEndpointsAsync().GetAwaiter().GetResult())
        {
            foreach (var endPoint in serviceEndpoint.Endpoints ?? [])
            {
                UriBuilder uriBuilder = new(endPoint) { Path = "/openapi/v1.json" };

                try
                {   
                    HttpRequestMessage request = new(HttpMethod.Get, uriBuilder.Uri) { Version = new Version(2, 0) };

                    using CancellationTokenSource cts = new(TimeSpan.FromSeconds(5));
                    HttpResponseMessage response = httpClient.SendAsync(request, cts.Token).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        urlDescriptors.Add(new UrlDescriptor
                        {
                            Url = uriBuilder.Uri.ToString(),
                            Name = serviceEndpoint.ServiceName
                        });
                        break;
                    }
                    else
                    {
                        if (logger.IsEnabled(LogLevel.Warning))
                        {
                            logger.LogWarning("Failed to get swagger endpoint for {ServiceName} at {Endpoint}, Status: {StatusCode}",
                                serviceEndpoint.ServiceName, uriBuilder.Uri, response.StatusCode);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    if (logger.IsEnabled(LogLevel.Warning))
                    {
                        logger.LogWarning("Timeout getting swagger endpoint for {ServiceName} at {Endpoint}",
                            serviceEndpoint.ServiceName, uriBuilder.Uri);
                    }
                }
                catch (Exception ex)
                {
                    if (logger.IsEnabled(LogLevel.Warning))
                    {
                        logger.LogWarning(ex, "Failed to get swagger endpoint for {ServiceName} at {Endpoint}",
                            serviceEndpoint.ServiceName, uriBuilder.Uri);
                    }
                }
            }
        }

        options.ConfigObject.Urls = urlDescriptors;
        options.SwaggerEndpoint("/openapi/v1.json", "apiservice");
    }
}
