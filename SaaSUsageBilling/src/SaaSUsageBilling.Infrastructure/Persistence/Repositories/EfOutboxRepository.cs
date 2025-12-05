using Microsoft.EntityFrameworkCore;
using SaaSUsageBilling.Application.Abstractions;
using SaaSUsageBilling.Domain.Entities;

namespace SaaSUsageBilling.Infrastructure.Persistence.Repositories;

public class EfOutboxRepository : IOutboxRepository
{
    private readonly BillingDbContext _context;

    public EfOutboxRepository(BillingDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(OutboxMessage message, CancellationToken ct)
    {
        await _context.OutboxMessages.AddAsync(message, ct);
        // Note: SaveChanges is handled by UnitOfWork
    }

    public async Task<IReadOnlyCollection<OutboxMessage>> GetPendingAsync(int batchSize, CancellationToken ct)
    {
        return await _context.OutboxMessages
            .Where(m => m.Status == OutboxMessageStatus.Pending ||
                       (m.Status == OutboxMessageStatus.Failed && m.CanRetry()))
            .OrderBy(m => m.CreatedOn)
            .Take(batchSize)
            .ToListAsync(ct);
    }

    public async Task UpdateAsync(OutboxMessage message, CancellationToken ct)
    {
        _context.OutboxMessages.Update(message);
        // Note: SaveChanges is handled by UnitOfWork
    }

    public async Task MarkAsProcessedAsync(Guid id, CancellationToken ct)
    {
        var message = await _context.OutboxMessages.FindAsync(new object[] { id }, ct);
        if (message != null)
        {
            message.MarkAsProcessed();
            _context.OutboxMessages.Update(message);
            // Note: SaveChanges is handled by UnitOfWork
        }
    }

    public async Task MarkAsFailedAsync(Guid id, string error, CancellationToken ct)
    {
        var message = await _context.OutboxMessages.FindAsync(new object[] { id }, ct);
        if (message != null)
        {
            message.MarkAsFailed(error);
            _context.OutboxMessages.Update(message);
            // Note: SaveChanges is handled by UnitOfWork
        }
    }
}

