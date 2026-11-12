using MediatR;

namespace Template.Application.Commands.Projects.CreateProject;

public record CreateProjectCommand(string Name, string Description) : IRequest<Guid>;
