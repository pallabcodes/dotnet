using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[ApiController]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }


    [Authorize]
    [HttpPut(ApiEndpoints.Movies.Rate)]
    public async Task<IActionResult> RateMovie([FromRoute] Guid id, [FromBody] RateMovieRequest request,
        CancellationToken token = default)
    {
        // TODO: when accessing userId through HttpContext.GetUserId() it is null so for now used `WORKAROUND` 
        // var userId = HttpContext.GetUserId();
        // var result = await _ratingService.RateMovieAsync(id, request.Rating, userId!.Value, token);
        // return result ? Ok() : NotFound();

        // WORKAROUND: Hardcode an arbitrary userId for now as below
        var userId = Guid.Parse("d8566de3-b1a6-4a9b-b842-8e3887a82e41");

        // Proceed with the hardcoded userId
        var result = await _ratingService.RateMovieAsync(id, request.Rating, userId, token);
        return result ? Ok() : NotFound();
    }
}