
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HelloShop.ServiceDefaults.Authorization;

public class RemotePermissionChecker(IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache, IHttpClientFactory httpClientFactory, IOptions<RemotePermissionCheckerOptions> options) : PermissionChecker(httpContextAccessor, distributedCache)
{
    private readonly RemotePermissionCheckerOptions _remotePermissionCheckerOptions = options.Value;

    public override async Task<bool> IsGrantedAsync(int roleId, string name, string? resourceType = null, string? resourceId = null)
    {
        string? accessToken = await HttpContext.GetTokenAsync("access_token");

        HttpClient httpClient = httpClientFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        httpClient.BaseAddress = new Uri(_remotePermissionCheckerOptions.ApiEndpoint);

        Dictionary<string, string?> parameters = new()
        {
            { nameof(roleId), roleId.ToString() },
            { nameof(name), name },
            { nameof(resourceType) , resourceType },
            { nameof(resourceId), resourceId }
        };

        string queryString = QueryHelpers.AddQueryString(string.Empty, parameters);

        var permissionGrants = httpClient.GetFromJsonAsAsyncEnumerable<PermissionGrantedResponse>(queryString);

        await foreach (var permissionGrant in permissionGrants)
        {
            if (permissionGrant != null && permissionGrant.IsGranted)
            {
                return true;
            }
        }

        return false;
    }
}
