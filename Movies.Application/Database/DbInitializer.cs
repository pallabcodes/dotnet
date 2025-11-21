using Dapper;
using Microsoft.Extensions.Logging;

namespace Movies.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(IDbConnectionFactory dbConnectionFactory, ILogger<DbInitializer> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();

            await CreateMoviesTableAsync(connection);
            await CreateGenresTableAsync(connection);
            await CreateRatingsTableAsync(connection);

            _logger.LogInformation("Database initialization completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing database");
            throw;
        }
    }

    private async Task CreateMoviesTableAsync(IDbConnection connection)
    {
        await connection.ExecuteAsync("""
            CREATE TABLE IF NOT EXISTS movies (
                id uuid PRIMARY KEY,
                slug TEXT NOT NULL,
                title TEXT NOT NULL,
                yearOfRelease INTEGER NOT NULL
            )
            """);

        await connection.ExecuteAsync("""
            CREATE UNIQUE INDEX IF NOT EXISTS movies_slug_idx 
            ON movies USING btree(slug)
            """);
    }

    private async Task CreateGenresTableAsync(IDbConnection connection)
    {
        await connection.ExecuteAsync("""
            CREATE TABLE IF NOT EXISTS genres (
                movieId uuid REFERENCES movies(id),
                name TEXT NOT NULL
            )
            """);
    }

    private async Task CreateRatingsTableAsync(IDbConnection connection)
    {
        await connection.ExecuteAsync("""
            CREATE TABLE IF NOT EXISTS ratings (
                userid uuid,
                movieid uuid REFERENCES movies(id),
                rating INTEGER NOT NULL,
                PRIMARY KEY(userid, movieid)
            )
            """);
    }
}
