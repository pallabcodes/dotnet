using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Mapping;

// why: since, this class used `static` so it can't be instantiated at all (whereas abstract class could still be instanced by itself or by its derived classes)
// purpose: serve as a utility class for mapping and transformation
public static class ContractMapping
{
    /**
     * The method `MapToMovie` is an extension method for the CreateMovieRequest class,
     * meaning it can be called on any instance of CreateMovieRequest just like it is a method of the class itself.
     * ------------------------------------------------------------------------------------------------------------
     * This CreateMovieRequest request is the syntax that allows the method to be called on an instance of CreateMovieRequest.
     * ------------------------------------------------------------------------------------------------------------
     * In C#, the extension method syntax this CreateMovieRequest request is what allows the method to be called directly on an instance of the CreateMovieRequest class. This is part of how extension methods are implemented in C#.
     * ------------------------------------------------------------------------------------------------------------
     * When defining an extension method
     * ------------------------------------------------------------------------------------------------------------
     * The first parameter must be `this` keyword, followed by type of Object i.e. extending which being CreateMovieRequest & then instance variable i.e. instance
     * Now, `MapToMovie` directly on any instance of CreateMovieRequest as if it were a method that belongs to CreateMovieRequest itself.
     */
    public static Movie MapToMovie(this CreateMovieRequest request)
    {
        try
        {
            // 1. Here, the only thing that happens is that data transfomration. So, access whatever available from request
            // 2. Then, transform the data through below Movie by mapping the correct value to correct fields of Movie class
            return new Movie
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,
                Genres = request.Genres
                    .ToList() // Genres is a collection, and request.Genres.ToList() is used to convert it to a list.
            };
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error mapping to Movie", ex);
        }
    }

    public static Movie MapToMovie(this UpdateMovieRequest request, Guid id)
    {
        try
        {
            return new Movie
            {
                Id = id,
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,
                Genres = request.Genres
                    .ToList() // Genres is a collection, and request.Genres.ToList() is used to convert it to a list.
            };
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error mapping to Movie", ex);
        }
    }

    public static MovieResponse MapToResponse(this Movie movie)
    {
        return new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            Slug = movie.Slug,
            Rating = movie.Rating,
            UserRating = movie.UserRating,
            YearOfRelease = movie.YearOfRelease,
            Genres = movie.Genres
        };
    }

    public static MoviesResponse MapToResponse(this IEnumerable<Movie> movies)
    {
        return new MoviesResponse
        {
            Items = movies.Select(MapToResponse)
        };
    }
}