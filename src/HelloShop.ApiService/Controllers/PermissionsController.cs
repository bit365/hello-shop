using HelloShop.ApiService.Services;
using HelloShop.ServiceDefaults.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace HelloShop.ApiService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PermissionsController(IPermissionService permissionService) : ControllerBase
{
    [HttpGet("Definitions")]
    public async Task<ActionResult<IReadOnlyList<PermissionGroupDefinitionResponse>>> GetAllPermissionDefinitions()
    {
        IReadOnlyList<PermissionGroupDefinitionResponse> permissionGroupDefinitions = await permissionService.GetAllPermissionGorupDefinitionsAsync(HttpContext.RequestAborted);

        return permissionGroupDefinitions.Any() ? Ok(permissionGroupDefinitions) : NoContent();
    }
}
