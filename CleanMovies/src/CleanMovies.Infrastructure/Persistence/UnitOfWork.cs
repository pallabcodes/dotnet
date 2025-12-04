using CleanMovies.Domain.Repositories;

namespace CleanMovies.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly MovieDbContext _db;

    public UnitOfWork(MovieDbContext db)
    {
        _db = db;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _db.SaveChangesAsync(cancellationToken);
    }
}
