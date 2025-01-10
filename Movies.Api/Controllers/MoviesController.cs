using Microsoft.AspNetCore.Mvc;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[ApiController]
[Route("api")]
public class MoviesController(IMovieRepository movieRepository) : ControllerBase
{
    private readonly IMovieRepository _movieRepository = movieRepository;

    [HttpPost("movies")]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        return Ok(request);
    }
}