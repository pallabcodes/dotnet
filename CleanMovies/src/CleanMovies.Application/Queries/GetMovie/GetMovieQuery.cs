using CleanMovies.Application.Common;
using MediatR;

namespace CleanMovies.Application.Queries.GetMovie;

public sealed record GetMovieQuery(string IdOrSlug) : IRequest<Result<MovieResponse>>;

public sealed record MovieResponse(Guid Id, string Title, string Slug, int YearOfRelease, string? Description, double Rating, IReadOnlyCollection<string> Genres);
