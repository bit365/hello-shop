// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Distributed;
using System.Net.Http.Headers;

namespace HelloShop.ServiceDefaults.Authorization;

public class RemotePermissionChecker(IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache, IHttpClientFactory httpClientFactory) : PermissionChecker(httpContextAccessor, distributedCache)
{
    public override async Task<bool> IsGrantedAsync(int roleId, string permissionName, string? resourceType = null, string? resourceId = null)
    {
        string? accessToken = await HttpContext.GetTokenAsync("access_token");

        HttpClient httpClient = httpClientFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        httpClient.BaseAddress = new Uri("https+http://identityservice");

        Dictionary<string, string?> parameters = new()
        {
            { nameof(roleId), roleId.ToString() },
            { nameof(permissionName), permissionName },
            { nameof(resourceType) , resourceType },
            { nameof(resourceId), resourceId }
        };

        string queryString = QueryHelpers.AddQueryString(string.Empty, parameters);

        HttpRequestMessage request = new(HttpMethod.Head, queryString);

        HttpResponseMessage response = await httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }
}
