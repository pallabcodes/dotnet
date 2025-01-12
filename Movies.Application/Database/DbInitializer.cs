using Dapper;

namespace Movies.Application.Database;

public class DbInitializer(IDbConnectionFactory dbConnectionFactory)
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync("""
                                      CREATE TABLE IF NOT EXISTS movies (
                                      id uuid PRIMARY KEY,
                                      slug TEXT NOT NULL,
                                      title TEXT NOT NULL,
                                      yearOfRelease INTEGER NOT NULL);
                                      """);
        await connection.ExecuteAsync("""
                                      CREATE UNIQUE INDEX CONCURRENTLY IF NOT EXISTS movies_slug_idx ON movies using btree(slug);
                                      """);
    }
}