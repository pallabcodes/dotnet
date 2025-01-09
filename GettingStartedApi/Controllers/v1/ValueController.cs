using Microsoft.AspNetCore.Mvc;

namespace GettingStartedApiApp.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0", Deprecated = true)]
public class ValueController : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return ["value1", "value2"];
    }
}