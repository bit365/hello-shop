// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.EntityFrameworks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PermissionsController(IdentityServiceDbContext dbContext) : ControllerBase
{
    [HttpHead]
    public async Task<IActionResult> CheckPermission(int roleId, string permissionName, string? resourceType = null, string? resourceId = null)
    {

        if (await dbContext.Set<PermissionGranted>().AnyAsync(x => x.RoleId == roleId && x.PermissionName == permissionName && x.ResourceType == resourceType && x.ResourceId == resourceId))
        {
            return Ok();
        }

        return Forbid();
    }
}
