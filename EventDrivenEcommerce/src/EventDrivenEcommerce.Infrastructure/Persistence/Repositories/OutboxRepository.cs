using EventDrivenEcommerce.Application.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EventDrivenEcommerce.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of IOutboxRepository.
/// </summary>
public sealed class OutboxRepository : IOutboxRepository
{
    private readonly EcommerceDbContext _context;

    public OutboxRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default)
    {
        await _context.OutboxMessages.AddAsync(message, cancellationToken);
    }

    public async Task<IReadOnlyCollection<OutboxMessage>> GetUnprocessedMessagesAsync(int batchSize = 50, CancellationToken cancellationToken = default)
    {
        return await _context.OutboxMessages
            .Where(m => m.ProcessedOn == null)
            .OrderBy(m => m.OccurredOn)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAsProcessedAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        var message = await _context.OutboxMessages.FindAsync(new object[] { messageId }, cancellationToken);
        if (message != null)
        {
            message.MarkAsProcessed();
            _context.OutboxMessages.Update(message);
        }
    }

    public async Task MarkAsFailedAsync(Guid messageId, string error, CancellationToken cancellationToken = default)
    {
        var message = await _context.OutboxMessages.FindAsync(new object[] { messageId }, cancellationToken);
        if (message != null)
        {
            message.MarkAsFailed(error);
            _context.OutboxMessages.Update(message);
        }
    }
}

