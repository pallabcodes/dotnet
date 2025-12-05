using Microsoft.EntityFrameworkCore;
using SaaSUsageBilling.Application.Abstractions;
using SaaSUsageBilling.Domain.Entities;

namespace SaaSUsageBilling.Infrastructure.Persistence.Repositories;

public class EfUsageEventRepository : IUsageEventRepository
{
    private readonly BillingDbContext _context;

    public EfUsageEventRepository(BillingDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UsageEvent usageEvent, CancellationToken ct)
    {
        await _context.UsageEvents.AddAsync(usageEvent, ct);
        // Note: SaveChanges is handled by UnitOfWork
    }

    public async Task<IReadOnlyCollection<UsageEvent>> GetForPeriodAsync(
        Guid subscriptionId,
        DateTimeOffset from,
        DateTimeOffset to,
        CancellationToken ct)
    {
        return await _context.UsageEvents
            .Where(u => u.SubscriptionId == subscriptionId &&
                       u.OccurredAt >= from &&
                       u.OccurredAt < to)
            .ToListAsync(ct);
    }
}