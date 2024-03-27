using System.Security.Claims;

namespace HelloShop.ServiceDefaults.Authorization;

public interface IPermissionChecker
{
    Task<bool> IsGrantedAsync(string name, string? resourceType = null, string? resourceId = null);

    Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name, string? resourceType = null, string? resourceId = null);
}
