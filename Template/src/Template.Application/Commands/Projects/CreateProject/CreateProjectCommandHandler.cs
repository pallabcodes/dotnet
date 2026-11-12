using MediatR;
using Shared.Application.Abstractions;
using Template.Application.Abstractions;
using Template.Domain.Projects;
using Template.Domain.Projects.ValueObjects;

namespace Template.Application.Commands.Projects.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _projects;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProjectCommandHandler(IProjectRepository projects, IUnitOfWork unitOfWork)
    {
        _projects = projects;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project(
            ProjectName.Create(request.Name),
            request.Description,
            DateTimeOffset.UtcNow);

        await _projects.AddAsync(project, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        project.ClearDomainEvents();

        return project.Id;
    }
}
