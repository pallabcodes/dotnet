using Movies.Api.Configuration;
using Movies.Application.Services;

namespace Movies.Api.Endpoints.Ratings;

public static class DeleteRatingEndpoint
{
    public const string Name = "DeleteRating";

    public static IEndpointRouteBuilder MapDeleteRating(this IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.Movies.DeleteRating,
                async (Guid id, HttpContext context, IRatingService ratingService, CancellationToken token) =>
                {
                    var userId = context.GetUserId();
                    if (!userId.HasValue)
                    {
                        return Results.Unauthorized();
                    }

                    var result = await ratingService.DeleteRatingAsync(id, userId.Value, token);
                    return result ? TypedResults.Ok() : Results.NotFound();
                })            .WithName(Name)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization()
            .RequireRateLimiting(RateLimitingConfiguration.AuthenticatedPolicy);

        return app;
    }
}