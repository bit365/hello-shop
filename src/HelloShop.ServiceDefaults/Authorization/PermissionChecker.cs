// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ServiceDefaults.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;

namespace HelloShop.ServiceDefaults.Authorization;

public abstract class PermissionChecker(IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache) : IPermissionChecker
{
    protected HttpContext HttpContext { get; init; } = httpContextAccessor.HttpContext ?? throw new InvalidOperationException();

    public async Task<bool> IsGrantedAsync(string permissionName, string? resourceType = null, string? resourceId = null) => await IsGrantedAsync(HttpContext.User, permissionName, resourceType, resourceId);

    public async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string permissionName, string? resourceType = null, string? resourceId = null)
    {
        var roleIds = claimsPrincipal.FindAll(CustomClaimTypes.RoleIdentifier).Select(c => Convert.ToInt32(c.Value)).ToArray();

        foreach (var roleId in roleIds)
        {
            var cacheKey = PermissionGrantCacheItem.CreateCacheKey(roleId, permissionName, resourceType, resourceId);

            if (distributedCache.TryGetValue(cacheKey, out PermissionGrantCacheItem? cacheItem) && cacheItem != null)
            {
                if (cacheItem.IsGranted)
                {
                    return true;
                }

                continue;
            }

            bool isGranted = await IsGrantedAsync(roleId, permissionName, resourceType, resourceId);

            await distributedCache.SetObjectAsync(cacheKey, new PermissionGrantCacheItem(isGranted), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now
            });

            if (isGranted)
            {
                return true;
            }
        }

        return false;
    }

    public abstract Task<bool> IsGrantedAsync(int roleId, string permissionName, string? resourceType = null, string? resourceId = null);
}
