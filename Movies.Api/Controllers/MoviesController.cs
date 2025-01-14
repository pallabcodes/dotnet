using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController(IMovieService movieService) : ControllerBase
{
    private readonly IMovieService _movieService = movieService;

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPost(ApiEndpoints.Movies.Create)]
    // Similar to Next.js @body() parametrized decorator, [FromBody] does exactly the same
    // This means that when a POST request is made to the "/api/movies" route, ASP.NET Core will deserialize the body of the request into an instance of CreateMovieRequest automatically.
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request, CancellationToken token)
    {
        // IActionResult is a general return type used by ASP.NET Core to represent the result of an action method in a controller. It allows for flexible return types like:
        // Ok(), NotFound(), BadRequest(), Created(), etc.
        // In your case, Created() is used to return a 201 Created HTTP status code along with the location of the newly created resource (the movie).
        // Why IActionResult?: This makes it easy to return different types of HTTP responses, depending on the result of the action (for example, success or failure).

        // Now, I know there is an extension method i.e., added on CreateMovieRequest's instance i.e., request; so that method will be available on the instance i.e., request off course, which is what happens below  
        var movie = request.MapToMovie();

        await _movieService.CreateAsync(movie, token);

        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);

        // TODO: below return `movie` which is a mistake and it will be fixed later
        // return Created($"{ApiEndpoints.Movies.Create}/{movie.Id}", movie);
    }

    [AllowAnonymous]
    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug, [FromServices] LinkGenerator linkGenerator, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _movieService.GetByIdAsync(id, userId, token)
            : await _movieService.GetBySlugAsync(idOrSlug, userId, token); // this will return an instance of Movie

        if (movie is null) return NotFound();

        // TODO: rather than returning `movie` directly, use contracts to return
        var response = movie.MapToResponse();
        
        var movieObj = new { id = movie.Id };
        
        response.Links.Add(new Link
        {
            Href = linkGenerator.GetPathByAction(HttpContext, nameof(Get), values: new {idOrSlug = movie.Id })!,
            Rel = "self",
            Type = "GET"
        });
        
        response.Links.Add(new Link
        {
            Href = linkGenerator.GetPathByAction(HttpContext, nameof(Update), values: movieObj)!,
            Rel = "self",
            Type = "PUT"
        });
        
        response.Links.Add(new Link
        {
            Href = linkGenerator.GetPathByAction(HttpContext, nameof(Update), values: movieObj)!,
            Rel = "self",
            Type = "DELETE"
        });
        
        return Ok(response);
    }

    // [Authorize] -> This means anyone (or any role) can access this controller
    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllMoviesRequest request, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var options = request.MapToOptions().WithUser(userId);
        var movies = await _movieService.GetAllAsync(options, token);
        var movieCount = await _movieService.GetCountAsync(options.Title, options.YearOfRelease, token);
        // check: When click on `MapToResponse` it may show multiple options, which is the one ? look at the type of movies by ctrl + q or ide highlights automatically
        var moviesResponse = movies.MapToResponse(request.Page, request.PageSize, movieCount);
        return Ok(moviesResponse);
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request,
        CancellationToken token)
    {
        var movie = request.MapToMovie(id);
        var userId = HttpContext.GetUserId();
        var updatedMovie = await _movieService.UpdateAsync(movie, userId, token);
        if (updatedMovie is null) return NotFound();
        var response = movie.MapToResponse();
        return Ok(response);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var deleted = await _movieService.DeleteAsync(id, token);
        if (!deleted) return NotFound();
        return Ok();
    }
}