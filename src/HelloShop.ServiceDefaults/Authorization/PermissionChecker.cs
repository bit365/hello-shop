using HelloShop.ServiceDefaults.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;

namespace HelloShop.ServiceDefaults.Authorization;

public abstract class PermissionChecker(IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache) : IPermissionChecker
{
    protected HttpContext HttpContext { get; init; } = httpContextAccessor.HttpContext ?? throw new InvalidOperationException();

    public async Task<bool> IsGrantedAsync(string name, string? resourceType = null, string? resourceId = null) => await IsGrantedAsync(HttpContext.User, name, resourceType, resourceId);

    public async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name, string? resourceType = null, string? resourceId = null)
    {
        var roleIds = claimsPrincipal.FindAll(CustomClaimTypes.RoleIdentifier).Select(c => Convert.ToInt32(c.Value)).ToArray();

        foreach (var roleId in roleIds)
        {
            var cacheKey = PermissionGrantCacheItem.CreateCacheKey(roleId, name, resourceType, resourceId);

            if (distributedCache.TryGetValue(cacheKey, out PermissionGrantCacheItem? cacheItem) && cacheItem != null)
            {
                if (cacheItem.IsGranted)
                {
                    return true;
                }

                continue;
            }

            bool isGranted = await IsGrantedAsync(roleId, name, resourceType, resourceId);

            await distributedCache.SetObjectAsync(cacheKey, new PermissionGrantCacheItem(isGranted), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(1)
            });

            if (isGranted)
            {
                return true;
            }
        }

        return false;
    }

    public abstract Task<bool> IsGrantedAsync(int roleId, string name, string? resourceType = null, string? resourceId = null);
}
