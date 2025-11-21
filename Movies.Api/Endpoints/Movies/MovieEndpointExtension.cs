namespace Movies.Api.Endpoints.Movies;

public static class MovieEndpointExtension
{
    public static IEndpointRouteBuilder MapMovieEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetMovie();
        app.MapCreateMovie();
        app.MapGetAllMovies();
        app.MapUpdateMovie();
        app.MapDeleteMovie();

        return app;
    }
}

/**
 *namespace Movies.Api.Endpoints.Movies;

public static class MovieEndpointExtension
{
    public static IEndpointRouteBuilder MapMovieEndpoints(this IEndpointRouteBuilder app)
    {
        // N.B: This will only needed to do when need to add a prefix
        // without the below line "/api/movies", with the below line -> "movies/api/movies"
        var group = app.MapGroup("movies");

        group.MapGetMovie();
        group.MapCreateMovie();
        group.MapGetAllMovies();
        group.MapUpdateMovie();
        group.MapDeleteMovie();

        return app;
    }
}
 *
 *
 */