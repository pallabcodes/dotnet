using Microsoft.EntityFrameworkCore;
using Template.Application.Abstractions;
using Template.Domain.Projects;

namespace Template.Infrastructure.Persistence.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly TemplateDbContext _dbContext;

    public ProjectRepository(TemplateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Project project, CancellationToken ct)
    {
        await _dbContext.Projects.AddAsync(project, ct);
    }

    public Task<Project?> GetAsync(Guid id, CancellationToken ct)
    {
        return _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id, ct);
    }
}
