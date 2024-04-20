using Microsoft.AspNetCore.Mvc;

namespace HelloShop.ApiService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [HttpGet]
    public string GetHelloWorld()
    {
        return "Hello World!";
    }
}
