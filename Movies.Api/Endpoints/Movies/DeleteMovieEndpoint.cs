using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Application.Services;

namespace Movies.Api.Endpoints.Movies;

public static class DeleteMovieEndpoint
{
    public const string Name = "DeleteMovie";

    public static IEndpointRouteBuilder MapDeleteMovie(this IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.Movies.Delete, async (Guid id, IMovieService movieService,
            HttpContext context,
            IOutputCacheStore outputCacheStore, CancellationToken token) =>
        {
            var userId = context.GetUserId();
            var deleted = await movieService.DeleteAsync(id, token);
            if (!deleted) return Results.NotFound();
            return TypedResults.Ok();
        }).WithName(Name);

        return app;
    }
}