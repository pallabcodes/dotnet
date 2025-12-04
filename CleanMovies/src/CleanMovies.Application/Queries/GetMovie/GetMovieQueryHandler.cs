using CleanMovies.Application.Common;
using CleanMovies.Application.Abstractions;
using CleanMovies.Domain.Repositories;
using CleanMovies.Domain.ValueObjects;
using MediatR;

namespace CleanMovies.Application.Queries.GetMovie;

public sealed class GetMovieQueryHandler : IRequestHandler<GetMovieQuery, Result<MovieResponse>>
{
    private readonly IMovieRepository _repository;
    private readonly ICacheService _cache;

    public GetMovieQueryHandler(IMovieRepository repository, ICacheService cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<Result<MovieResponse>> Handle(GetMovieQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"movie:{request.IdOrSlug}";
        var cached = await _cache.GetAsync<MovieResponse>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return Result<MovieResponse>.Success(cached);
        }

        var movie = Guid.TryParse(request.IdOrSlug, out var id)
            ? await _repository.GetByIdAsync(new MovieId(id), cancellationToken)
            : await _repository.GetBySlugAsync(Slug.FromExisting(request.IdOrSlug), cancellationToken);

        if (movie is null)
        {
            return Result<MovieResponse>.Failure("Movie not found");
        }

        var dto = new MovieResponse(
            movie.Id,
            movie.Title,
            movie.Slug.Value,
            movie.YearOfRelease,
            movie.Description,
            movie.AverageRating,
            movie.Genres.Select(g => g.Name).ToList());

        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(2), cancellationToken);
        return Result<MovieResponse>.Success(dto);
    }
}
