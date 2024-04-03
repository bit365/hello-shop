using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.EntityFrameworks;
using HelloShop.ServiceDefaults.Authorization;
using HelloShop.ServiceDefaults.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PermissionsController(IPermissionChecker permissionChecker) : ControllerBase
{
    [HttpHead]
    public async Task<IActionResult> CheckPermission(string permissionName, string? resourceType = null, string? resourceId = null)
    {
        if(await permissionChecker.IsGrantedAsync(permissionName, resourceType, resourceId))
        {
            return Ok();
        }

        return Forbid();
    }
}
