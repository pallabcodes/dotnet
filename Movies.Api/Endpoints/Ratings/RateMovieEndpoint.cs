using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Endpoints.Ratings;

public static class RateMovieEndpoint
{
    public const string Name = "RateMovie";

    public static IEndpointRouteBuilder MapRateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.Movies.Rate, async (Guid id, RateMovieRequest request,
                HttpContext context, IRatingService ratingService,
                CancellationToken token = default) =>
            {
                // N.B: This is not working so used the `WORKAROUND` below
                // var userId = context.GetUserId();

                // WORKAROUND: Hardcode an arbitrary userId for now as below
                var userId = Guid.Parse("d8566de3-b1a6-4a9b-b842-8e3887a82e41");

                // Proceed with the hardcoded userId
                var result = await ratingService.RateMovieAsync(id, request.Rating, userId, token);
                return result ? TypedResults.Ok() : Results.NotFound();
            }).WithName(Name)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();


        return app;
    }
}