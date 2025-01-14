using FluentValidation.Results;
using Movies.Application.Repositories;
using ValidationException = FluentValidation.ValidationException;

namespace Movies.Application.Services;

public class RatingService : IRatingService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IRatingRepository _ratingRepository;

    public RatingService(IRatingRepository ratingRepository, IMovieRepository movieRepository)
    {
        _ratingRepository = ratingRepository;
        _movieRepository = movieRepository;
    }

    public async Task<bool> RateMovieAsync(Guid movieId, int rating, Guid userId, CancellationToken token = default)
    {
        if (rating is <= 0 or > 5)
            throw new ValidationException([
                new ValidationFailure
                {
                    PropertyName = "Rating",
                    ErrorMessage = "Rating must be between 0 and 5"
                }
            ]);

        var movieExists = await _movieRepository.ExistsByIdAsync(movieId, token);
        if (!movieExists) return false;

        return await _ratingRepository.RateMovieAsync(movieId, rating, userId, token);
    }
}