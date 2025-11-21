using Dapper;
using Microsoft.Extensions.Logging;
using Movies.Application.Database;
using Movies.Application.Models;
using Movies.Application.Repositories.SqlQueries;

namespace Movies.Application.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<MovieRepository> _logger;

    public MovieRepository(IDbConnectionFactory dbConnectionFactory, ILogger<MovieRepository> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
    {
        if (movie is null)
        {
            _logger.LogWarning("Attempted to create null movie");
            return false;
        }

        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        try
        {
            var result = await connection.ExecuteAsync(
                new CommandDefinition(MovieSqlQueries.CreateMovie, movie, transaction, cancellationToken: token));

            if (result <= 0)
            {
                transaction.Rollback();
                _logger.LogWarning("Failed to create movie: {MovieId}", movie.Id);
                return false;
            }

            await CreateGenresAsync(connection, transaction, movie, token);
            transaction.Commit();
            _logger.LogInformation("Successfully created movie: {MovieId}", movie.Id);
            return true;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error creating movie: {MovieId}", movie.Id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(
                new CommandDefinition(MovieSqlQueries.DeleteGenresByMovieId, new { id }, transaction, cancellationToken: token));

            var result = await connection.ExecuteAsync(
                new CommandDefinition(MovieSqlQueries.DeleteMovieById, new { id }, transaction, cancellationToken: token));

            transaction.Commit();
            
            if (result > 0)
            {
                _logger.LogInformation("Successfully deleted movie: {MovieId}", id);
            }
            else
            {
                _logger.LogWarning("Movie not found for deletion: {MovieId}", id);
            }

            return result > 0;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error deleting movie: {MovieId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(MovieSqlQueries.ExistsById, new { id }, cancellationToken: token));
    }

    public async Task<int> GetCountAsync(string? title, int? yearOfRelease, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.QuerySingleAsync<int>(
            new CommandDefinition(MovieSqlQueries.GetCount, new { title, yearOfRelease }, cancellationToken: token));
    }

    public async Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition(MovieSqlQueries.GetById, new { id, userId }, cancellationToken: token));

        if (movie is null)
        {
            _logger.LogDebug("Movie not found: {MovieId}", id);
            return null;
        }

        await MovieRepositoryHelpers.LoadGenresAsync(connection, movie, token);
        return movie;
    }

    public async Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            _logger.LogWarning("Attempted to get movie with null or empty slug");
            return null;
        }

        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition(MovieSqlQueries.GetBySlug, new { slug, userId }, cancellationToken: token));

        if (movie is null)
        {
            _logger.LogDebug("Movie not found by slug: {Slug}", slug);
            return null;
        }

        await MovieRepositoryHelpers.LoadGenresAsync(connection, movie, token);
        return movie;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options, CancellationToken token = default)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        var sortField = options.SortField;
        var isAscending = options.SortOrder == SortOrder.Ascending;
        var query = MovieSqlQueries.BuildGetAllQuery(sortField, isAscending);

        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var result = await connection.QueryAsync(
            new CommandDefinition(
                query,
                new
                {
                    userId = options.UserId,
                    title = options.Title,
                    yearofrelease = options.YearOfRelease,
                    pageSize = options.PageSize,
                    pageOffset = (options.Page - 1) * options.PageSize
                },
                cancellationToken: token));

        return result.Select(MovieRepositoryHelpers.MapToMovie);
    }

    public async Task<bool> UpdateAsync(Movie movie, CancellationToken token = default)
    {
        if (movie is null)
        {
            _logger.LogWarning("Attempted to update null movie");
            return false;
        }

        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(
                new CommandDefinition(MovieSqlQueries.DeleteGenresByMovieId, new { id = movie.Id }, transaction, cancellationToken: token));

            await CreateGenresAsync(connection, transaction, movie, token);

            var result = await connection.ExecuteAsync(
                new CommandDefinition(MovieSqlQueries.UpdateMovie, movie, transaction, cancellationToken: token));

            transaction.Commit();
            
            if (result > 0)
            {
                _logger.LogInformation("Successfully updated movie: {MovieId}", movie.Id);
            }
            else
            {
                _logger.LogWarning("Movie not found for update: {MovieId}", movie.Id);
            }

            return result > 0;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error updating movie: {MovieId}", movie.Id);
            throw;
        }
    }

    private async Task CreateGenresAsync(IDbConnection connection, IDbTransaction transaction, Movie movie, CancellationToken token)
    {
        foreach (var genre in movie.Genres)
        {
            await connection.ExecuteAsync(
                new CommandDefinition(
                    MovieSqlQueries.CreateGenre,
                    new { MovieId = movie.Id, Name = genre },
                    transaction,
                    cancellationToken: token));
        }
    }
}
