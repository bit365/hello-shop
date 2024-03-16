// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelloShop.IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        [HttpGet(nameof(Foo))]
        [Authorize(IdentityPermissions.Users.Update)]
        public IActionResult Foo()
        {
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
