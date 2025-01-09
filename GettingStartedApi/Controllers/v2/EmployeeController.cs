using Microsoft.AspNetCore.Mvc;

namespace GettingStartedApiApp.Controllers.v2;

[Route("api/v{version: apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
public class EmployeeController : ControllerBase
{
    // GET: api/<EmployeeController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return ["value1", "value2"];
    }
}