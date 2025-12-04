using CleanMovies.Api.Contracts.Requests;
using CleanMovies.Api.Contracts.Responses;
using CleanMovies.Application.Commands.CreateMovie;
using CleanMovies.Application.Commands.RateMovie;
using CleanMovies.Application.Queries.GetMovie;
using CleanMovies.Application.Queries.ListMovies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanMovies.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class MoviesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MoviesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Policy = "Editor")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateMovie([FromBody] CreateMovieRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateMovieCommand(
            request.Title,
            request.YearOfRelease,
            request.Description,
            request.Genres);

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(new { error = result.Error });
        }

        return CreatedAtAction(nameof(GetMovie), new { idOrSlug = result.Value }, new { id = result.Value });
    }

    [HttpGet("{idOrSlug}")]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMovie(string idOrSlug, CancellationToken cancellationToken)
    {
        var query = new GetMovieQuery(idOrSlug);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.Succeeded)
        {
            return NotFound(new { error = result.Error });
        }

        var movie = result.Value;
        var response = new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            YearOfRelease = movie.YearOfRelease,
            Description = movie.Description,
            Slug = movie.Slug.Value,
            Genres = movie.Genres.Select(g => g.Name).ToList(),
            AverageRating = movie.AverageRating,
            RatingCount = movie.Ratings.Count
        };

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(MoviesListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListMovies(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? title = null,
        [FromQuery] int? year = null,
        CancellationToken cancellationToken = default)
    {
        if (page < 1)
        {
            return BadRequest(new { error = "Page must be greater than 0" });
        }

        if (pageSize < 1 || pageSize > 100)
        {
            return BadRequest(new { error = "Page size must be between 1 and 100" });
        }

        var query = new ListMoviesQuery(page, pageSize, title, year);
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(new { error = result.Error });
        }

        var moviesResponse = result.Value.Items.Select(movie => new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            YearOfRelease = movie.YearOfRelease,
            Description = movie.Description,
            Slug = movie.Slug.Value,
            Genres = movie.Genres.Select(g => g.Name).ToList(),
            AverageRating = movie.AverageRating,
            RatingCount = movie.Ratings.Count
        }).ToList();

        var response = new MoviesListResponse
        {
            Movies = moviesResponse,
            Page = result.Value.Page,
            PageSize = result.Value.PageSize,
            TotalCount = result.Value.TotalCount
        };

        return Ok(response);
    }

    [HttpPost("{id}/ratings")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RateMovie(
        Guid id,
        [FromBody] RateMovieRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RateMovieCommand(id, request.UserId, request.Rating);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(new { error = result.Error });
        }

        return NoContent();
    }
}