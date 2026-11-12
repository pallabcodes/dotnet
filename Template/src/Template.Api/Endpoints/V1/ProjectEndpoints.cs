using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Template.Api.Models;
using Template.Api.Observability;
using Template.Api.Models.V1;
using Template.Application.Commands.Projects.CreateProject;
using Template.Application.Queries.Projects.GetProject;

namespace Template.Api.Endpoints.V1;

public static class ProjectEndpoints
{
    public static void MapProjectEndpoints(this WebApplication app)
    {
        app.MapPost("/api/v1/projects", CreateProject)
            .WithTags("Projects")
            .Produces<ProjectCreatedResponse>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create a project",
                Description = "Creates a new project."
            });

        app.MapGet("/api/v1/projects/{projectId:guid}", GetProject)
            .WithTags("Projects")
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get a project",
                Description = "Returns a project by id."
            });
    }

    private static async Task<IResult> CreateProject(
        CreateProjectRequest request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new CreateProjectCommand(request.Name, request.Description);
        var projectId = await mediator.Send(command, cancellationToken);

        TemplateMetrics.Requests.Add(1, new KeyValuePair<string, object?>("route", "projects.create"));

        return Results.Created($"/api/v1/projects/{projectId}", new ProjectCreatedResponse
        {
            ProjectId = projectId,
            Name = request.Name
        });
    }

    private static async Task<IResult> GetProject(
        Guid projectId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetProjectQuery(projectId), cancellationToken);

        TemplateMetrics.Requests.Add(1, new KeyValuePair<string, object?>("route", "projects.get"));

        return response is null
            ? Results.NotFound(new ErrorResponse
            {
                Type = ErrorTypes.NotFound,
                Message = "Project not found"
            })
            : Results.Ok(response);
    }
}
