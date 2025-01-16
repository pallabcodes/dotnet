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
                var userId = Guid.Parse("d8566de3-b1a6-4a9b-b842-8e3887a82e41");
                var result = await ratingService.DeleteRatingAsync(id, userId, token);
                return result ? TypedResults.Ok() : Results.NotFound();
            });

        return app;
    }
}