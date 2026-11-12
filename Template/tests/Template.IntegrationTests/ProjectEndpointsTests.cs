using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Template.Api.Models.V1;
using Template.Application.Queries.Projects.GetProject;
using Xunit;

namespace Template.IntegrationTests;

public class ProjectEndpointsTests : IClassFixture<TemplateApiFactory>
{
    private readonly TemplateApiFactory _factory;

    public ProjectEndpointsTests(TemplateApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateProject_Returns201_AndPersists()
    {
        var client = _factory.CreateClient();

        var create = await client.PostAsJsonAsync("/api/v1/projects",
            new { name = "Apollo", description = "Internal tooling" });

        create.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await create.Content.ReadFromJsonAsync<ProjectCreatedResponse>();
        created.Should().NotBeNull();

        var fetched = await client.GetAsync($"/api/v1/projects/{created!.ProjectId}");
        fetched.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await fetched.Content.ReadFromJsonAsync<ProjectResponse>();
        body!.Name.Should().Be("Apollo");
        body.Status.Should().Be("Active");
    }

    [Fact]
    public async Task CreateProject_WithEmptyName_Returns400()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/v1/projects",
            new { name = "", description = "" });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetUnknownProject_Returns404()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync($"/api/v1/projects/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Health_Returns200()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
