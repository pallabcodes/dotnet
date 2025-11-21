using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Api.Configuration;
using Movies.Application.Services;

namespace Movies.Api.Endpoints.Movies;

public static class DeleteMovieEndpoint
{
    public const string Name = "DeleteMovie";

    public static IEndpointRouteBuilder MapDeleteMovie(this IEndpointRouteBuilder app)
    {
        app.MapDelete(
                ApiEndpoints.Movies.Delete,
                async (Guid id, IMovieService movieService, HttpContext context,
                    IOutputCacheStore outputCacheStore, CancellationToken token) =>
                {
                    var deleted = await movieService.DeleteAsync(id, token);
                    if (!deleted)
                    {
                        return Results.NotFound();
                    }

                    await outputCacheStore.EvictByTagAsync("movies", token);
                    return TypedResults.Ok();
                })
            .WithName(Name)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(AuthConstants.AdminUserPolicyName)
            .RequireRateLimiting(RateLimitingConfiguration.AdminPolicy);

        return app;
    }
}
