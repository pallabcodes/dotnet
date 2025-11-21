namespace Movies.Application.Repositories.SqlQueries;

internal static class RatingSqlQueries
{
    public const string RateMovie = """
        INSERT INTO ratings(userid, movieid, rating) 
        VALUES (@userId, @movieId, @rating) 
        ON CONFLICT (userid, movieid) 
        DO UPDATE SET rating = @rating
        """;

    public const string GetRating = """
        SELECT ROUND(AVG(r.rating), 1) 
        FROM ratings r 
        WHERE movieid = @movieId
        """;

    public const string GetUserRating = """
        SELECT ROUND(AVG(r.rating), 1) AS rating, 
               (SELECT rating FROM ratings WHERE movieid = @movieid AND userid = @userid LIMIT 1) AS userrating 
        FROM ratings r 
        WHERE movieid = @movieid
        """;

    public const string DeleteRating = """
        DELETE FROM ratings 
        WHERE movieid = @movieid AND userid = @userid
        """;

    public const string GetRatingsForUser = """
        SELECT r.rating, r.movieid, m.slug 
        FROM ratings r 
        INNER JOIN movies m ON r.movieid = m.id 
        WHERE userid = @userid
        """;
}


