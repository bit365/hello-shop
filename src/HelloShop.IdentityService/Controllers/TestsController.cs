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
        [Authorize(Roles = "AdminRole")]
        public IActionResult Foo()
        {
            return Ok("Hello, World!");
        }

        [HttpGet(nameof(Bar))]
        [Authorize(Roles = "GuestRole")]
        public IActionResult Bar()
        {
            return Ok("Hello, World!");
        }

        [HttpGet(nameof(Baz))]
        public IActionResult Baz()
        {
            return Ok("Hello, World!");
        }
    }
}
