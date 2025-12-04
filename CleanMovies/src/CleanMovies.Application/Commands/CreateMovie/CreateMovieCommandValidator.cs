using FluentValidation;

namespace CleanMovies.Application.Commands.CreateMovie;

public sealed class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    public CreateMovieCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.YearOfRelease).InclusiveBetween(1900, DateTime.UtcNow.Year + 1);
        RuleFor(x => x.Genres).NotNull();
    }
}
