using MediatR;

namespace Template.Application.Queries.Projects.GetProject;

public record GetProjectQuery(Guid ProjectId) : IRequest<ProjectResponse?>;

public record ProjectResponse(Guid Id, string Name, string Description, string Status, DateTimeOffset CreatedOn);
