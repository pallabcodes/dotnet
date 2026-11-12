using MediatR;
using Template.Application.Abstractions;
using Template.Domain.Projects;

namespace Template.Application.Queries.Projects.GetProject;

public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, ProjectResponse?>
{
    private readonly IProjectRepository _projects;

    public GetProjectQueryHandler(IProjectRepository projects)
    {
        _projects = projects;
    }

    public async Task<ProjectResponse?> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        var project = await _projects.GetAsync(request.ProjectId, cancellationToken);

        if (project is null)
        {
            return null;
        }

        return new ProjectResponse(
            project.Id,
            project.Name.Value,
            project.Description,
            project.Status.ToString(),
            project.CreatedOn);
    }
}
