using EventDrivenEcommerce.Application.IntegrationEvents;

namespace EventDrivenEcommerce.Application.Common;

/// <summary>
/// Repository for managing outbox messages.
/// </summary>
public interface IOutboxRepository
{
    Task AddAsync(OutboxMessage message, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<OutboxMessage>> GetUnprocessedMessagesAsync(int batchSize = 50, CancellationToken cancellationToken = default);
    Task MarkAsProcessedAsync(Guid messageId, CancellationToken cancellationToken = default);
    Task MarkAsFailedAsync(Guid messageId, string error, CancellationToken cancellationToken = default);
}

/// <summary>
/// Entity representing a message in the outbox for reliable event publishing.
/// </summary>
public sealed class OutboxMessage
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Type { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;
    public DateTime? ProcessedOn { get; private set; }
    public string? Error { get; private set; }

    private OutboxMessage() { } // For EF Core

    public OutboxMessage(IIntegrationEvent integrationEvent)
    {
        Type = integrationEvent.GetType().FullName ?? integrationEvent.GetType().Name;
        Content = System.Text.Json.JsonSerializer.Serialize(integrationEvent);
        OccurredOn = integrationEvent.OccurredOn;
    }

    public void MarkAsProcessed()
    {
        ProcessedOn = DateTime.UtcNow;
    }

    public void MarkAsFailed(string error)
    {
        Error = error;
    }
}

