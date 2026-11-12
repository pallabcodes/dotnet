using FluentValidation;

namespace Template.Application.Commands.Projects.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(Domain.Projects.ValueObjects.ProjectName.MaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(2000);
    }
}
