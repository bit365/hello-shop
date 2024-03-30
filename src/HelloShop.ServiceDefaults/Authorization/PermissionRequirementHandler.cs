using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace HelloShop.ServiceDefaults.Authorization;

public class PermissionRequirementHandler(IPermissionChecker permissionChecker) : AuthorizationHandler<OperationAuthorizationRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
    {
        if (await permissionChecker.IsGrantedAsync(context.User, requirement.Name))
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}

public class ResourcePermissionRequirementHandler(IPermissionChecker permissionChecker) : AuthorizationHandler<OperationAuthorizationRequirement, IAuthorizationResource>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, IAuthorizationResource resource)
    {
        if (await permissionChecker.IsGrantedAsync(context.User, requirement.Name, resource.ResourceType, resource.ResourceId))
        {
            context.Succeed(requirement);
        }
    }
}
