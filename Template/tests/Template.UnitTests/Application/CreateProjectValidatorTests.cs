using FluentValidation.TestHelper;
using Template.Application.Commands.Projects.CreateProject;
using Xunit;

namespace Template.UnitTests.Application;

public class CreateProjectValidatorTests
{
    private readonly CreateProjectCommandValidator _validator = new();

    [Fact]
    public void EmptyName_Fails()
    {
        var result = _validator.TestValidate(new CreateProjectCommand("", ""));
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void NameOverMaxLength_Fails()
    {
        var result = _validator.TestValidate(new CreateProjectCommand(new string('x', 201), ""));
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ValidCommand_Passes()
    {
        var result = _validator.TestValidate(new CreateProjectCommand("Apollo", "Internal tooling"));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
