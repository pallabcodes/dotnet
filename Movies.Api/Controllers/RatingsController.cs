using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }


    [Authorize]
    [HttpPut(ApiEndpoints.Movies.Rate)]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    [Authorize]
    [HttpDelete(ApiEndpoints.Movies.DeleteRating)]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRating([FromRoute] Guid id, CancellationToken token = default)
    {
        // N.B: The id i.e., provided must be a movieId (that is mapped to id) that has a valid rating (not null)
        // N.B: After deletion means (rating of the given movieId will be reset to null) but off course won't remove the movie itself 
        var userId = Guid.Parse("d8566de3-b1a6-4a9b-b842-8e3887a82e41");
        var result = await _ratingService.DeleteRatingAsync(id, userId, token);
        return result ? Ok() : NotFound();
    }

    [Authorize]
    [HttpGet(ApiEndpoints.Ratings.GetUserRatings)]
    [ProducesResponseType(typeof(IEnumerable<MovieRatingResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserRatings(CancellationToken token = default)
    {
        var userId = Guid.Parse("d8566de3-b1a6-4a9b-b842-8e3887a82e41");
        var ratings = await _ratingService.GetRatingsForUserAsync(userId, token);
        // TODO: understand what happened here ? and how MapToResponse() is available
        var ratingsResponse = ratings.MapToResponse();
        return Ok(ratingsResponse);
    }
}

