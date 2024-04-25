using HelloShop.ApiService.Infrastructure;
using HelloShop.ServiceDefaults.Permissions;

namespace HelloShop.ApiService.Services;

public class PermissionService(HttpClient httpClient, IConfiguredServiceEndPointResolver serviceEndPointResolver) : IPermissionService
{
    public async Task<IReadOnlyList<PermissionGroupDefinitionResponse>> GetAllPermissionGorupDefinitionsAsync(CancellationToken cancellationToken = default)
    {
        List<PermissionGroupDefinitionResponse> result = [];

        httpClient.Timeout = TimeSpan.FromSeconds(10);

        IReadOnlyCollection<ConfiguredServiceEndPoint> serviceEndPoints = await serviceEndPointResolver.GetConfiguredServiceEndpointsAsync(cancellationToken);

        await Parallel.ForEachAsync(serviceEndPoints, new ParallelOptions{ CancellationToken= cancellationToken}, async (serviceEndPoint, cancelToken) =>
        {
            UriBuilder uriBuilder = new(serviceEndPoint.ServiceName) { Path = "api/Permissions/PermissionDefinitions" };

            HttpResponseMessage response = await httpClient.GetAsync(uriBuilder.Uri, cancelToken);

            if (response.IsSuccessStatusCode)
            {
                List<PermissionGroupDefinitionResponse>? permissionGroupDefinition = await response.Content.ReadFromJsonAsync<List<PermissionGroupDefinitionResponse>>(cancelToken);

                if (permissionGroupDefinition != null)
                {
                    result.AddRange(permissionGroupDefinition);
                }
            }
        });

        return result.DistinctBy(x => x.GroupName).ToList();
    }
}
