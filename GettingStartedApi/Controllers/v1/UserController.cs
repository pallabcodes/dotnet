using Microsoft.AspNetCore.Mvc;

namespace GettingStartedApiApp.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0", Deprecated = true)]
public class UserController : ControllerBase
{
    // GET api/v1/Users
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return ["Version 1 Value 1", "Version 1 Value 2"];
    }
}