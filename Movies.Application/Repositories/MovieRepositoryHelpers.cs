using Dapper;
using Movies.Application.Models;
using Movies.Application.Repositories.SqlQueries;

namespace Movies.Application.Repositories;

internal static class MovieRepositoryHelpers
{
    public static async Task LoadGenresAsync(IDbConnection connection, Movie movie, CancellationToken token)
    {
        var genres = await connection.QueryAsync<string>(
            new CommandDefinition(
                MovieSqlQueries.GetGenresByMovieId,
                new { id = movie.Id },
                cancellationToken: token));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }
    }

    public static Movie MapToMovie(dynamic x)
    {
        return new Movie
        {
            Id = x.id,
            Title = x.title,
            YearOfRelease = x.yearofrelease,
            Rating = (float?)x.rating,
            UserRating = (int?)x.userrating,
            Genres = x.genres?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>()
        };
    }
}

