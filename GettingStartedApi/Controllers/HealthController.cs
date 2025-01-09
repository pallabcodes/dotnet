using Microsoft.AspNetCore.Mvc;

namespace GettingStartedApiApp.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersionNeutral]
public class HealthController : ControllerBase
{
    [HttpGet]
    [Route("ping")]
    public IActionResult Ping()
    {
        return Ok("Working fine");
    }
}