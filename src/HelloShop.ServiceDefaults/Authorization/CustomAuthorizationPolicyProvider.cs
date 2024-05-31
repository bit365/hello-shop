// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ServiceDefaults.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;

namespace HelloShop.ServiceDefaults.Authorization;

public class CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IPermissionDefinitionManager permissionDefinitionManager) : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);

        if (policy != null)
        {
            return policy;
        }

        var permissionDefinition = permissionDefinitionManager.GetOrNullAsync(policyName);

        if (permissionDefinition != null)
        {
            var policyBuilder = new AuthorizationPolicyBuilder();

            policyBuilder.Requirements.Add(new OperationAuthorizationRequirement { Name = policyName });

            return policyBuilder.Build();
        }

        return null;
    }
}
