using GettingStartedApiApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace GettingStartedApiApp.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class UserController(ILogger<UserController> logger) : ControllerBase
{
    [HttpGet]
    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
    public IEnumerable<string> Get()
    {
        return [Random.Shared.Next(1, 101).ToString()];
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60 * 60 * 24, Location = ResponseCacheLocation.Any, NoStore = false)]
    public string Get(int id)
    {
        // try
        // {
        //     if (id < 0 || id > 100) throw new ArgumentOutOfRangeException(nameof(id));
        //     logger.LogInformation("The api/Users/{id} was called", id);
        //     return Ok($"Value{id}");
        // }
        // catch (Exception ex)
        // {
        //     logger.LogError(ex, "An exception occurred while getting the user");
        //     return BadRequest("The Index is out of range");
        // }

        return $"Random Number: {Random.Shared.Next(1, 101)} for id {id}";
    }

    [HttpPost]
    public IActionResult Post([FromBody] UserModel user)
    {
        if (ModelState.IsValid) return Ok("The model is valid");

        return BadRequest(ModelState);
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