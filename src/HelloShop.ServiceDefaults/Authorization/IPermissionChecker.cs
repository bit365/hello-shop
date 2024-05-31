// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using System.Security.Claims;

namespace HelloShop.ServiceDefaults.Authorization;

public interface IPermissionChecker
{
    Task<bool> IsGrantedAsync(string permissionName, string? resourceType = null, string? resourceId = null);

    Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string permissionName, string? resourceType = null, string? resourceId = null);
}
