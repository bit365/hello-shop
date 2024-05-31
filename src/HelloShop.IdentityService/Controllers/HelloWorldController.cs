// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace HelloShop.IdentityService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HelloWorldController() : ControllerBase
{
    [HttpGet(nameof(Localizer1))]
    public IActionResult Localizer1([FromServices] IStringLocalizerFactory factory)
    {
        var location = Assembly.GetExecutingAssembly().GetName().Name;

        ArgumentException.ThrowIfNullOrWhiteSpace(location);

        var localizer = factory.Create(typeof(Welcome));

        string message = localizer["HelloWorld"];

        return Ok(message);
    }

    [HttpGet(nameof(Localizer2))]
    public IActionResult Localizer2([FromServices] IStringLocalizer<Welcome> localizer)
    {
        string message = localizer["HelloWorld"];

        return Ok(message);
    }

    [HttpGet(nameof(Localizer3))]
    public IActionResult Localizer3()
    {
        object result = new
        {
            Time = TimeProvider.System.GetUtcNow().ToString(),
            Amount = Random.Shared.Next(1, 1000).ToString("C")
        };

        return Ok(result);
    }
}
