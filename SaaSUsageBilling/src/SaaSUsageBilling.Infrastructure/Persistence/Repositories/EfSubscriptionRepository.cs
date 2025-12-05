using SaaSUsageBilling.Application.Abstractions;
using SaaSUsageBilling.Domain.Entities;

namespace SaaSUsageBilling.Infrastructure.Persistence.Repositories;

public class EfSubscriptionRepository : ISubscriptionRepository
{
    private readonly BillingDbContext _context;

    public EfSubscriptionRepository(BillingDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Subscription subscription, CancellationToken ct)
    {
        await _context.Subscriptions.AddAsync(subscription, ct);
        // Note: SaveChanges is handled by UnitOfWork
    }

    public async Task<Subscription?> GetAsync(Guid id, CancellationToken ct)
    {
        return await _context.Subscriptions.FindAsync(new object[] { id }, ct);
    }

    public async Task UpdateAsync(Subscription subscription, CancellationToken ct)
    {
        _context.Subscriptions.Update(subscription);
        // Note: SaveChanges is handled by UnitOfWork
    }
}

