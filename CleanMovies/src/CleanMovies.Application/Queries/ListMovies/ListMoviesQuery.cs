using CleanMovies.Application.Common;
using MediatR;

namespace CleanMovies.Application.Queries.ListMovies;

public sealed record ListMoviesQuery(int Page = 1, int PageSize = 20, string? Title = null, int? Year = null) : IRequest<Result<MoviesPage>>;

public sealed record MoviesPage(int Page, int PageSize, int TotalCount, IReadOnlyCollection<MovieListItem> Items);
public sealed record MovieListItem(Guid Id, string Title, string Slug, int YearOfRelease, double Rating, IReadOnlyCollection<string> Genres);
