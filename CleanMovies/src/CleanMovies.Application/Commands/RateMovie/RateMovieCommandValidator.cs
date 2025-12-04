using FluentValidation;

namespace CleanMovies.Application.Commands.RateMovie;

public sealed class RateMovieCommandValidator : AbstractValidator<RateMovieCommand>
{
    public RateMovieCommandValidator()
    {
        RuleFor(x => x.MovieId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Rating).InclusiveBetween(1, 10);
    }
}
