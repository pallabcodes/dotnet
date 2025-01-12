using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.Application.Repositories;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController(IMovieRepository movieRepository) : ControllerBase
{
    private readonly IMovieRepository _movieRepository = movieRepository;

    [HttpPost(ApiEndpoints.Movies.Create)]
    // Similar to Next.js @body() parametrized decorator, [FromBody] does exactly the same
    // This means that when a POST request is made to the "/api/movies" route, ASP.NET Core will deserialize the body of the request into an instance of CreateMovieRequest automatically.
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        // IActionResult is a general return type used by ASP.NET Core to represent the result of an action method in a controller. It allows for flexible return types like:
        // Ok(), NotFound(), BadRequest(), Created(), etc.
        // In your case, Created() is used to return a 201 Created HTTP status code along with the location of the newly created resource (the movie).
        // Why IActionResult?: This makes it easy to return different types of HTTP responses, depending on the result of the action (for example, success or failure).

        // Now, I know there is an extension method i.e., added on CreateMovieRequest's instance i.e., request; so that method will be available on the instance i.e., request off course, which is what happens below  
        var movie = request.MapToMovie();

        await _movieRepository.CreateAsync(movie);

        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);

        // TODO: below return `movie` which is a mistake and it will be fixed later
        // return Created($"{ApiEndpoints.Movies.Create}/{movie.Id}", movie);
    }

    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug)
    {
        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _movieRepository.GetByIdAsync(id)
            : await _movieRepository.GetBySlugAsync(idOrSlug); // this will return an instance of Movie

        if (movie is null) return NotFound();

        // TODO: rather than returning `movie` directly, use contracts to return

        var response = movie.MapToResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _movieRepository.GetAllAsync(); // this return
        var response = movies.MapToResponse();

        return Ok(response);
    }

    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request)
    {
        var movie = request.MapToMovie(id);
        var updated = await _movieRepository.UpdateAsync(movie);
        if (!updated) return NotFound();
        var response = movie.MapToResponse();
        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _movieRepository.DeleteAsync(id);
        if (!deleted) return NotFound();
        return Ok();
    }
}