using CleanMovies.Application.Common;
using CleanMovies.Application.Abstractions;
using CleanMovies.Domain.Repositories;
using MediatR;

namespace CleanMovies.Application.Queries.ListMovies;

public sealed class ListMoviesQueryHandler : IRequestHandler<ListMoviesQuery, Result<MoviesPage>>
{
    private readonly IMovieRepository _repository;
    private readonly ICacheService _cache;

    public ListMoviesQueryHandler(IMovieRepository repository, ICacheService cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<Result<MoviesPage>> Handle(ListMoviesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"movies:{request.Page}:{request.PageSize}:{request.Title}:{request.Year}";
        var cached = await _cache.GetAsync<MoviesPage>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return Result<MoviesPage>.Success(cached);
        }

        var movies = await _repository.ListAsync(request.Page, request.PageSize, request.Title, request.Year, cancellationToken);
        var count = await _repository.CountAsync(request.Title, request.Year, cancellationToken);

        var items = movies.Select(m => new MovieListItem(
            m.Id,
            m.Title,
            m.Slug.Value,
            m.YearOfRelease,
            m.AverageRating,
            m.Genres.Select(g => g.Name).ToList())).ToList();

        var page = new MoviesPage(request.Page, request.PageSize, count, items);
        await _cache.SetAsync(cacheKey, page, TimeSpan.FromMinutes(2), cancellationToken);
        return Result<MoviesPage>.Success(page);
    }
}
