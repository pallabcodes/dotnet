using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Endpoints.Movies;

public static class UpdateMovieEndpoint
{
    public const string Name = "UpdateMovie";

    public static IEndpointRouteBuilder MapUpdateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.Movies.Update, async (Guid id, UpdateMovieRequest request, IMovieService movieService,
                HttpContext context,
                IOutputCacheStore outputCacheStore, CancellationToken token) =>
            {
                var movie = request.MapToMovie(id);
                var userId = context.GetUserId();

                var updatedMovie = await movieService.UpdateAsync(movie, userId, token);
                if (updatedMovie is null) return Results.NotFound();
                var response = movie.MapToResponse();
                await outputCacheStore.EvictByTagAsync("movies", token);
                return TypedResults.Ok(response);
            }).WithName(Name)
            .Produces<MovieResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthConstants.TrustedMemberPolicyName);

        // N.B: it makes for `RequireAuthorization` to be at bottom since most likely it follows bottom -> top


        return app;
    }
}