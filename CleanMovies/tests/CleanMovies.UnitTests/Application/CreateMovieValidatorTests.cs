using System;
using CleanMovies.Application.Commands.CreateMovie;
using FluentAssertions;
using Xunit;

namespace CleanMovies.UnitTests.Application;

public class CreateMovieValidatorTests
{
    private readonly CreateMovieCommandValidator _validator = new();

    [Fact]
    public void Should_Fail_When_TitleMissing()
    {
        var result = _validator.Validate(new CreateMovieCommand("", 2024, "", Array.Empty<string>()));
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateMovieCommand.Title));
    }

    [Fact]
    public void Should_Pass_For_Valid_Command()
    {
        var result = _validator.Validate(new CreateMovieCommand("Interstellar", 2014, "", new[] { "Sci-Fi" }));
        result.IsValid.Should().BeTrue();
    }
}
