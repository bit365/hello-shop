// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ServiceDefaults.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloShop.IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController(IPermissionChecker permissionChecker, IAuthorizationService authorizationService) : ControllerBase
    {

        [Authorize(IdentityPermissions.Users.Update)]
        public async Task<IActionResult> Foo()
        {
            var result = await permissionChecker.IsGrantedAsync(IdentityPermissions.Users.Update, "Order", "1");

            var result2 = await authorizationService.AuthorizeAsync(User, IdentityPermissions.Users.Update);

            return Ok("Hello, World!");
        }

        [HttpGet(nameof(Bar))]
        [Authorize(IdentityPermissions.Users.Create)]
        public IActionResult Bar()
        {
            return Ok("Hello, World!");
        }

        [Authorize(IdentityPermissions.Users.Delete)]
        [HttpGet(nameof(Baz))]
        public IActionResult Baz()
        {
            return Ok("Hello, World!");
        }
    }
}
