using Dapper;
using Microsoft.Extensions.Logging;
using Movies.Application.Database;
using Movies.Application.Models;
using Movies.Application.Repositories.SqlQueries;

namespace Movies.Application.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<RatingRepository> _logger;

    public RatingRepository(IDbConnectionFactory dbConnectionFactory, ILogger<RatingRepository> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<bool> RateMovieAsync(Guid movieId, int rating, Guid userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        
        try
        {
            var result = await connection.ExecuteAsync(
                new CommandDefinition(
                    RatingSqlQueries.RateMovie,
                    new { userId, movieId, rating },
                    cancellationToken: token));

            if (result > 0)
            {
                _logger.LogInformation("Successfully rated movie: {MovieId} by user: {UserId} with rating: {Rating}", 
                    movieId, userId, rating);
            }

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rating movie: {MovieId} by user: {UserId}", movieId, userId);
            throw;
        }
    }

    public async Task<float?> GetRatingAsync(Guid movieId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.QuerySingleOrDefaultAsync<float?>(
            new CommandDefinition(RatingSqlQueries.GetRating, new { movieId }, cancellationToken: token));
    }

    public async Task<(float? Rating, int? UserRating)> GetUserRatingAsync(
        Guid movieId, 
        Guid userId,
        CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.QuerySingleOrDefaultAsync<(float?, int?)>(
            new CommandDefinition(
                RatingSqlQueries.GetUserRating,
                new { movieId, userId },
                cancellationToken: token));
    }

    public async Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        try
        {
            var result = await connection.ExecuteAsync(
                new CommandDefinition(
                    RatingSqlQueries.DeleteRating,
                    new { userId, movieId },
                    cancellationToken: token));

            if (result > 0)
            {
                _logger.LogInformation("Successfully deleted rating for movie: {MovieId} by user: {UserId}", 
                    movieId, userId);
            }
            else
            {
                _logger.LogWarning("Rating not found for deletion: MovieId={MovieId}, UserId={UserId}", 
                    movieId, userId);
            }

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting rating: MovieId={MovieId}, UserId={UserId}", movieId, userId);
            throw;
        }
    }

    public async Task<IEnumerable<MovieRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.QueryAsync<MovieRating>(
            new CommandDefinition(
                RatingSqlQueries.GetRatingsForUser,
                new { userId },
                cancellationToken: token));
    }
}
