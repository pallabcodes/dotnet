using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Responses;

namespace Movies.Api.Endpoints.Ratings;

public static class GetUserRatingsEndpoint
{
    public const string Name = "GetUserRatings";

    public static IEndpointRouteBuilder MapGetUserRatings(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Ratings.GetUserRatings,
                async (HttpContext context, IRatingService ratingService, CancellationToken token) =>
                {
                    var userId = Guid.Parse("d8566de3-b1a6-4a9b-b842-8e3887a82e41");
                    var ratings = await ratingService.GetRatingsForUserAsync(userId, token);
                    var ratingsResponse = ratings.MapToResponse();
                    return TypedResults.Ok(ratingsResponse);
                }).WithName(Name)
            .Produces<MovieRatingResponse>()
            .RequireAuthorization();

        return app;
    }
}