using FluentValidation;
using Microsoft.Extensions.Logging;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IValidator<Movie> _movieValidator;
    private readonly IValidator<GetAllMoviesOptions> _optionsValidator;
    private readonly IRatingRepository _ratingRepository;
    private readonly ILogger<MovieService> _logger;

    public MovieService(
        IMovieRepository movieRepository,
        IValidator<Movie> movieValidator,
        IRatingRepository ratingRepository,
        IValidator<GetAllMoviesOptions> optionsValidator,
        ILogger<MovieService> logger)
    {
        _movieRepository = movieRepository;
        _movieValidator = movieValidator;
        _ratingRepository = ratingRepository;
        _optionsValidator = optionsValidator;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
    {
        if (movie is null)
        {
            _logger.LogWarning("Attempted to create null movie");
            return false;
        }

        await _movieValidator.ValidateAndThrowAsync(movie, token);
        return await _movieRepository.CreateAsync(movie, token);
    }

    public Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
    {
        return _movieRepository.GetByIdAsync(id, userId, token);
    }

    public Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            _logger.LogWarning("Attempted to get movie with null or empty slug");
            return Task.FromResult<Movie?>(null);
        }

        return _movieRepository.GetBySlugAsync(slug, userId, token);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options, CancellationToken token = default)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        await _optionsValidator.ValidateAndThrowAsync(options, token);
        return await _movieRepository.GetAllAsync(options, token);
    }

    public async Task<Movie?> UpdateAsync(Movie movie, Guid? userId = default, CancellationToken token = default)
    {
        if (movie is null)
        {
            _logger.LogWarning("Attempted to update null movie");
            return null;
        }

        await _movieValidator.ValidateAndThrowAsync(movie, token);
        var movieExists = await _movieRepository.ExistsByIdAsync(movie.Id, token);
        
        if (!movieExists)
        {
            _logger.LogWarning("Movie not found for update: {MovieId}", movie.Id);
            return null;
        }

        await _movieRepository.UpdateAsync(movie, token);

        if (!userId.HasValue)
        {
            var rating = await _ratingRepository.GetRatingAsync(movie.Id, token);
            movie.Rating = rating;
            return movie;
        }

        var ratings = await _ratingRepository.GetUserRatingAsync(movie.Id, userId.Value, token);
        movie.Rating = ratings.Rating;
        movie.UserRating = ratings.UserRating;
        return movie;
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
    {
        return _movieRepository.DeleteAsync(id, token);
    }

    public Task<int> GetCountAsync(string? title, int? yearOfRelease, CancellationToken token = default)
    {
        return _movieRepository.GetCountAsync(title, yearOfRelease, token);
    }
}
