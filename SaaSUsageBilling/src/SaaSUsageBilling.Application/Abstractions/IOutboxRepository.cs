using SaaSUsageBilling.Domain.Entities;

namespace SaaSUsageBilling.Application.Abstractions;

public interface IOutboxRepository
{
    Task AddAsync(OutboxMessage message, CancellationToken ct);
    Task<IReadOnlyCollection<OutboxMessage>> GetPendingAsync(int batchSize, CancellationToken ct);
    Task UpdateAsync(OutboxMessage message, CancellationToken ct);
    Task MarkAsProcessedAsync(Guid id, CancellationToken ct);
    Task MarkAsFailedAsync(Guid id, string error, CancellationToken ct);
}

