using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Endpoints.Movies;

public static class GetAllMoviesEndpoint
{
    public const string Name = "GetMovies";

    public static IEndpointRouteBuilder MapGeAllMovies(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Movies.GetAll,
                async ([AsParameters] GetAllMoviesRequest request, IMovieService movieService, HttpContext context,
                    CancellationToken token) =>
                {
                    var userId = context.GetUserId();
                    var options = request.MapToOptions().WithUser(userId);
                    var movies = await movieService.GetAllAsync(options, token);
                    var movieCount = await movieService.GetCountAsync(options.Title, options.YearOfRelease, token);
                    // check: When click on `MapToResponse` it may show multiple options, which is the one ? look at the type of movies by ctrl + q or ide highlights automatically
                    var moviesResponse = movies.MapToResponse(
                        request.Page.GetValueOrDefault(PaginatedRequest.DefaultPage),
                        request.PageSize.GetValueOrDefault(PaginatedRequest.DefaultPageSize), movieCount);
                    return TypedResults.Ok(moviesResponse);
                })
            .WithName($"{Name}V1")
            .Produces<MoviesResponse>()
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0);


        app.MapGet(ApiEndpoints.Movies.GetAll,
                async ([AsParameters] GetAllMoviesRequest request, IMovieService movieService, HttpContext context,
                    CancellationToken token) =>
                {
                    var userId = context.GetUserId();
                    var options = request.MapToOptions().WithUser(userId);
                    var movies = await movieService.GetAllAsync(options, token);
                    var movieCount = await movieService.GetCountAsync(options.Title, options.YearOfRelease, token);
                    // check: When click on `MapToResponse` it may show multiple options, which is the one ? look at the type of movies by ctrl + q or ide highlights automatically
                    var moviesResponse = movies.MapToResponse(
                        request.Page.GetValueOrDefault(PaginatedRequest.DefaultPage),
                        request.PageSize.GetValueOrDefault(PaginatedRequest.DefaultPageSize), movieCount);
                    return TypedResults.Ok(moviesResponse);
                })
            .WithName($"{Name}V2")
            .Produces<MoviesResponse>()
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(2.0)
            .CacheOutput("MovieCache");


        return app;
    }
}