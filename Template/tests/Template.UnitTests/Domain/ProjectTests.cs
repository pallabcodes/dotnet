using FluentAssertions;
using Template.Domain.Projects;
using Template.Domain.Projects.Events;
using Template.Domain.Projects.ValueObjects;
using Xunit;

namespace Template.UnitTests.Domain;

public class ProjectTests
{
    [Fact]
    public void Constructor_WithValidName_InitializesActiveProjectAndRaisesEvent()
    {
        var project = new Project(ProjectName.Create("Apollo"), "Internal tooling", DateTimeOffset.UtcNow);

        project.Status.Should().Be(ProjectStatus.Active);
        project.Name.Value.Should().Be("Apollo");
        project.DomainEvents.Should().ContainSingle(e => e is ProjectCreatedEvent);
    }

    [Fact]
    public void Rename_AfterArchiving_Throws()
    {
        var project = new Project(ProjectName.Create("Apollo"), "", DateTimeOffset.UtcNow);
        project.Archive();

        var act = () => project.Rename(ProjectName.Create("NewName"));

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Archive_Twice_IsIdempotent()
    {
        var project = new Project(ProjectName.Create("Apollo"), "", DateTimeOffset.UtcNow);

        project.Archive();
        project.Archive();

        project.Status.Should().Be(ProjectStatus.Archived);
    }

    [Fact]
    public void ProjectName_WhenEmpty_Throws()
    {
        var act = () => ProjectName.Create("   ");

        act.Should().Throw<ArgumentException>();
    }
}
