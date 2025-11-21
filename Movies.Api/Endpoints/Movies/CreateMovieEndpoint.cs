using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Api.Configuration;
using Movies.Api.Mapping;
using Movies.Api.Telemetry;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Endpoints.Movies;

public static class CreateMovieEndpoint
{
    public const string Name = "CreateMovie";

    public static IEndpointRouteBuilder MapCreateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Movies.Create, async (CreateMovieRequest request, IMovieService movieService,
                IOutputCacheStore outputCacheStore, CancellationToken token) =>
            {
                using var activity = ApplicationActivitySource.StartActivity("CreateMovie", System.Diagnostics.ActivityKind.Server);
                activity?.SetTag("movie.title", request.Title);
                activity?.SetTag("movie.year", request.YearOfRelease);

                var movie = request.MapToMovie();

                await movieService.CreateAsync(movie, token);

                await outputCacheStore.EvictByTagAsync("movies", token);

                var response = movie.MapToResponse();
                activity?.SetTag("movie.id", movie.Id.ToString());

                return TypedResults.CreatedAtRoute(response, GetMovieEndpoint.Name, new { idOrSlug = movie.Id });
            }).WithName(Name)
            .Produces<MovieResponse>(StatusCodes.Status201Created)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthConstants.TrustedMemberPolicyName)
            .RequireRateLimiting(RateLimitingConfiguration.AuthenticatedPolicy);

        return app;
    }
}