using CleanMovies.Application.Common;
using CleanMovies.Domain.Entities;
using CleanMovies.Domain.Repositories;
using MediatR;

namespace CleanMovies.Application.Commands.CreateMovie;

public sealed class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, Result<Guid>>
{
    private readonly IMovieRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMovieCommandHandler(IMovieRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = Movie.Create(request.Title, request.YearOfRelease, request.Genres, request.Description);
        await _repository.AddAsync(movie, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(movie.Id);
    }
}
