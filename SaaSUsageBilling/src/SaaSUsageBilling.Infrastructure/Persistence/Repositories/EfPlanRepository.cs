using SaaSUsageBilling.Application.Abstractions;
using SaaSUsageBilling.Domain.Entities;

namespace SaaSUsageBilling.Infrastructure.Persistence.Repositories;

public class EfPlanRepository : IPlanRepository
{
    private readonly BillingDbContext _context;

    public EfPlanRepository(BillingDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Plan plan, CancellationToken ct)
    {
        await _context.Plans.AddAsync(plan, ct);
        // Note: SaveChanges is handled by UnitOfWork
    }

    public async Task<Plan?> GetAsync(Guid id, CancellationToken ct)
    {
        return await _context.Plans.FindAsync(new object[] { id }, ct);
    }
}