using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace GettingStartedApiApp.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new[] { "Version 2 Value 1", "Version 2 Value 2" };
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        try
        {
            if (id < 0 || id > 100) throw new ArgumentOutOfRangeException(nameof(id));
            _logger.LogInformation("The api/Users/{id} was called", id);
            return Ok($"Value{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while getting the user");
            return BadRequest("The Index is out of range");
        }
    }

    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    [HttpPatch("{id}")]
    public void Patch(int id, [FromBody] string email)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}