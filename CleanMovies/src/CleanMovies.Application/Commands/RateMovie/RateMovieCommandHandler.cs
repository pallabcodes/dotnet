using CleanMovies.Application.Common;
using CleanMovies.Application.Abstractions;
using CleanMovies.Domain.Repositories;
using CleanMovies.Domain.ValueObjects;
using MediatR;

namespace CleanMovies.Application.Commands.RateMovie;

public sealed class RateMovieCommandHandler : IRequestHandler<RateMovieCommand, Result>
{
    private readonly IMovieRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;

    public RateMovieCommandHandler(IMovieRepository repository, IUnitOfWork unitOfWork, ICacheService cache)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<Result> Handle(RateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _repository.GetByIdAsync(new MovieId(request.MovieId), cancellationToken);
        if (movie is null)
        {
            return Result.Failure("Movie not found");
        }

        movie.AddOrUpdateRating(new UserId(request.UserId), request.Rating);
        await _repository.UpdateAsync(movie, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync($"movie:{request.MovieId}", cancellationToken);
        await _cache.RemoveAsync($"movie:{movie.Slug.Value}", cancellationToken);
        return Result.Success();
    }
}
