using CleanMovies.Application.Common;
using MediatR;

namespace CleanMovies.Application.Commands.CreateMovie;

public sealed record CreateMovieCommand(string Title, int YearOfRelease, string? Description, IReadOnlyCollection<string> Genres) : IRequest<Result<Guid>>;
