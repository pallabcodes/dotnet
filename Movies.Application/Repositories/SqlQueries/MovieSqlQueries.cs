namespace Movies.Application.Repositories.SqlQueries;

internal static class MovieSqlQueries
{
    public const string CreateMovie = """
        INSERT INTO movies (id, slug, title, yearofrelease) 
        VALUES (@Id, @Slug, @Title, @YearOfRelease)
        """;

    public const string CreateGenre = """
        INSERT INTO genres (movieId, name) 
        VALUES (@MovieId, @Name)
        """;

    public const string DeleteGenresByMovieId = """
        DELETE FROM genres WHERE movieid = @id
        """;

    public const string DeleteMovieById = """
        DELETE FROM movies WHERE id = @id
        """;

    public const string ExistsById = """
        SELECT COUNT(1) FROM movies WHERE id = @id
        """;

    public const string GetCount = """
        SELECT COUNT(id) FROM movies 
        WHERE (@title IS NULL OR title LIKE ('%' || @title || '%')) 
        AND (@yearOfRelease IS NULL OR yearofrelease = @yearOfRelease)
        """;

    public const string GetById = """
        SELECT m.*, 
               ROUND(AVG(r.rating), 1) AS rating, 
               myr.rating AS userrating 
        FROM movies m 
        LEFT JOIN ratings r ON m.id = r.movieid 
        LEFT JOIN ratings myr ON m.id = myr.movieid AND myr.userid = @userId 
        WHERE m.id = @id 
        GROUP BY m.id, userrating
        """;

    public const string GetBySlug = """
        SELECT m.*, 
               ROUND(AVG(r.rating), 1) AS rating, 
               myr.rating AS userrating 
        FROM movies m 
        LEFT JOIN ratings r ON m.id = r.movieid 
        LEFT JOIN ratings myr ON m.id = myr.movieid AND myr.userid = @userId 
        WHERE m.slug = @slug 
        GROUP BY m.id, userrating
        """;

    public const string GetGenresByMovieId = """
        SELECT name FROM genres WHERE movieid = @id
        """;

    public const string GetAllBase = """
        SELECT m.*, 
               STRING_AGG(DISTINCT g.name, ',') AS genres, 
               ROUND(AVG(r.rating), 1) AS rating, 
               myr.rating AS userrating 
        FROM movies m 
        LEFT JOIN genres g ON m.id = g.movieid 
        LEFT JOIN ratings r ON m.id = r.movieid 
        LEFT JOIN ratings myr ON m.id = myr.movieid AND myr.userid = @userId 
        WHERE (@title IS NULL OR m.title LIKE ('%' || @title || '%')) 
        AND (@yearofrelease IS NULL OR m.yearofrelease = @yearofrelease) 
        GROUP BY m.id, userrating
        """;

    public static string BuildGetAllQuery(string? sortField, bool isAscending)
    {
        var query = GetAllBase;
        
        if (string.IsNullOrWhiteSpace(sortField))
        {
            return $"{query} LIMIT @pageSize OFFSET @pageOffset";
        }

        var allowedFields = new[] { "title", "yearofrelease" };
        var normalizedField = sortField.ToLowerInvariant();
        
        if (!allowedFields.Contains(normalizedField))
        {
            throw new ArgumentException($"Invalid sort field: {sortField}", nameof(sortField));
        }

        var direction = isAscending ? "ASC" : "DESC";
        var sortColumn = normalizedField switch
        {
            "title" => "m.title",
            "yearofrelease" => "m.yearofrelease",
            _ => throw new ArgumentException($"Invalid sort field: {sortField}", nameof(sortField))
        };
        
        return $"{query} ORDER BY {sortColumn} {direction} LIMIT @pageSize OFFSET @pageOffset";
    }

    public const string UpdateMovie = """
        UPDATE movies 
        SET slug = @Slug, title = @Title, yearofrelease = @YearOfRelease 
        WHERE id = @Id
        """;
}


