using CleanMovies.Application.Common;
using MediatR;

namespace CleanMovies.Application.Commands.RateMovie;

public sealed record RateMovieCommand(Guid MovieId, Guid UserId, int Rating) : IRequest<Result>;
