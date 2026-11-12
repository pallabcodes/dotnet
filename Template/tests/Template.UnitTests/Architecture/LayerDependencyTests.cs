using FluentAssertions;
using NetArchTest.Rules;
using Shared.Application.Behaviors;
using Template.Domain.Projects;
using Template.Infrastructure.Persistence;
using Xunit;

namespace Template.UnitTests.Architecture;

public class LayerDependencyTests
{
    [Fact]
    public void Domain_MustNotDependOn_OuterLayers()
    {
        var result = Types.InAssembly(typeof(Project).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Template.Application",
                "Template.Infrastructure",
                "Template.Api")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Domain layer leaked to: {string.Join(", ", result.FailingTypeNames ?? Array.Empty<string>())}");
    }

    [Fact]
    public void Application_MustNotDependOn_InfrastructureOrApi()
    {
        var result = Types.InAssembly(typeof(ValidationBehavior<,>).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Template.Infrastructure",
                "Template.Api")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Application layer leaked to: {string.Join(", ", result.FailingTypeNames ?? Array.Empty<string>())}");
    }

    [Fact]
    public void Infrastructure_MustNotDependOn_Api()
    {
        var result = Types.InAssembly(typeof(TemplateDbContext).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny("Template.Api")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Infrastructure layer leaked to: {string.Join(", ", result.FailingTypeNames ?? Array.Empty<string>())}");
    }
}
