using Template.Domain.Projects;

namespace Template.Application.Abstractions;

public interface IProjectRepository
{
    Task AddAsync(Project project, CancellationToken ct);
    Task<Project?> GetAsync(Guid id, CancellationToken ct);
}
