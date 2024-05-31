// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace HelloShop.ServiceDefaults.Authorization
{
    public class FakePermissionChecker(IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache) : PermissionChecker(httpContextAccessor, distributedCache)
    {
        public override Task<bool> IsGrantedAsync(int roleId, string permissionName, string? resourceType = null, string? resourceId = null) => Task.FromResult(true);
    }
}