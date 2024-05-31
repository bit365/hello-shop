// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace HelloShop.ServiceDefaults.Authorization;

public class PermissionRequirementHandler(IPermissionChecker permissionChecker) : AuthorizationHandler<OperationAuthorizationRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
    {
        if (context.Resource is IAuthorizationResource resource)
        {
            if (await permissionChecker.IsGrantedAsync(context.User, requirement.Name, resource.ResourceType, resource.ResourceId))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return;
        }

        if (await permissionChecker.IsGrantedAsync(context.User, requirement.Name))
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}
