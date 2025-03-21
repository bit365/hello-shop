﻿// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.Infrastructure;
using HelloShop.ServiceDefaults.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace HelloShop.IdentityService.Authorization;
public class LocalPermissionChecker(IHttpContextAccessor httpContextAccessor, IdentityServiceDbContext dbContext, IDistributedCache distributedCache, TimeProvider timeProvider) : PermissionChecker(httpContextAccessor, distributedCache, timeProvider)
{
    public override async Task<bool> IsGrantedAsync(int roleId, string name, string? resourceType = null, string? resourceId = null)
    {
        return await dbContext.Set<PermissionGranted>().AsNoTracking().AnyAsync(x => x.RoleId == roleId && x.PermissionName == name && x.ResourceType == resourceType && x.ResourceId == resourceId);
    }
}