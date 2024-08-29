// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ServiceDefaults.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace HelloShop.IdentityService.Authentication;

public class CustomUserClaimsPrincipalFactory<TUser, TRole>(UserManager<TUser> userManager, RoleManager<TRole> roleManager, IOptions<IdentityOptions> options) : UserClaimsPrincipalFactory<TUser, TRole>(userManager, roleManager, options) where TUser : IdentityUser<int> where TRole : IdentityRole<int>
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
    {
        var claimsIdentity = await base.GenerateClaimsAsync(user).ConfigureAwait(false);

        if (UserManager.SupportsUserRole)
        {
            var roleNames = await UserManager.GetRolesAsync(user).ConfigureAwait(false);

            var roles = RoleManager.Roles.Where(r => r.Name != null && roleNames.Contains(r.Name));

            foreach (var role in roles)
            {
                claimsIdentity.AddClaim(new Claim(CustomClaimTypes.RoleIdentifier, role.Id.ToString()));
            }
        }

        return claimsIdentity;
    }
}
