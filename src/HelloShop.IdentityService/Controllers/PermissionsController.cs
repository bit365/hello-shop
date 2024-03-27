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
public class PermissionsController(IdentityServiceDbContext dbContext) : ControllerBase
{

    [HttpGet(nameof(PermissionList))]
    public async Task<ActionResult<IEnumerable<PermissionGrantedResponse>>> PermissionList(int? roleId, string? name, string? resourceType = null, string? resourceId = null)
    {
        var roleIds = HttpContext.User.FindAll(CustomClaimTypes.RoleIdentifier).Select(c => Convert.ToInt32(c.Value));

        if (roleId.HasValue && roleIds.Any(x => x == roleId))
        {
            roleIds = [roleId.Value];
        }

        List<PermissionGrantedResponse> result = [];

        IQueryable<PermissionGranted> queryable = dbContext.Set<PermissionGranted>().Where(x => roleIds.Contains(x.RoleId));

        if (!string.IsNullOrWhiteSpace(name))
        {
            queryable = queryable.Where(x => x.PermissionName == name);
        }

        var permissionGrants = await queryable.Where(x => x.ResourceType == resourceType && x.ResourceId == resourceId).Distinct().ToListAsync();

        result.AddRange(permissionGrants.Select(x => new PermissionGrantedResponse
        {
            Name = x.PermissionName,
            ResourceType = x.ResourceType,
            ResourceId = x.ResourceId,
            IsGranted = true
        }));

        return Ok(result);
    }
}
