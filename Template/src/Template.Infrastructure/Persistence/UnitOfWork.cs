using Shared.Application.Abstractions;

namespace Template.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly TemplateDbContext _dbContext;

    public UnitOfWork(TemplateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return _dbContext.SaveChangesAsync(ct);
    }
}
