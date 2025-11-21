using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Movies.Application.Models;
using Movies.Application.Repositories;
using ValidationException = FluentValidation.ValidationException;

namespace Movies.Application.Services;

public class RatingService : IRatingService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IRatingRepository _ratingRepository;
    private readonly ILogger<RatingService> _logger;

    private const int MinRating = 1;
    private const int MaxRating = 5;

    public RatingService(
        IRatingRepository ratingRepository,
        IMovieRepository movieRepository,
        ILogger<RatingService> logger)
    {
        _ratingRepository = ratingRepository;
        _movieRepository = movieRepository;
        _logger = logger;
    }

    public async Task<bool> RateMovieAsync(Guid movieId, int rating, Guid userId, CancellationToken token = default)
    {
        if (rating < MinRating || rating > MaxRating)
        {
            _logger.LogWarning(
                "Invalid rating value: {Rating} for movie: {MovieId} by user: {UserId}. Rating must be between {MinRating} and {MaxRating}",
                rating, movieId, userId, MinRating, MaxRating);

            throw new ValidationException([
                new ValidationFailure
                {
                    PropertyName = "Rating",
                    ErrorMessage = $"Rating must be between {MinRating} and {MaxRating}"
                }
            ]);
        }

        var movieExists = await _movieRepository.ExistsByIdAsync(movieId, token);
        if (!movieExists)
        {
            _logger.LogWarning("Movie not found for rating: {MovieId}", movieId);
            return false;
        }

        return await _ratingRepository.RateMovieAsync(movieId, rating, userId, token);
    }

    public Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken token = default)
    {
        return _ratingRepository.DeleteRatingAsync(movieId, userId, token);
    }

    public Task<IEnumerable<MovieRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default)
    {
        return _ratingRepository.GetRatingsForUserAsync(userId, token);
    }
}
