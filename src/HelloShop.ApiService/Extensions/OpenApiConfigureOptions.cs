using HelloShop.ApiService.Infrastructure;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace HelloShop.ApiService.Extensions;

public class OpenApiConfigureOptions(IConfiguredServiceEndPointResolver serviceResolver, HttpClient httpClient) : IConfigureOptions<SwaggerUIOptions>
{
    public void Configure(SwaggerUIOptions options)
    {
        List<UrlDescriptor> urlDescriptors = [];

        foreach (var serviceEndpoint in serviceResolver.GetConfiguredServiceEndpointsAsync().GetAwaiter().GetResult())
        {
            foreach (var endPoint in serviceEndpoint.Endpoints ?? [])
            {
                UriBuilder uriBuilder = new(endPoint) { Path = "swagger/v1/swagger.json" };

                HttpResponseMessage response = httpClient.GetAsync(uriBuilder.Uri).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    urlDescriptors.Add(new UrlDescriptor
                    {
                        Url = uriBuilder.Uri.ToString(),
                        Name = serviceEndpoint.ServiceName
                    });
                    break;
                }
            }
        }
        options.ConfigObject.Urls = urlDescriptors;
        
        options.SwaggerEndpoint("v1/swagger.json", "apiservice");
    }
}
