using SaaSUsageBilling.Domain.Common;

namespace SaaSUsageBilling.Domain.Entities;

public enum OutboxMessageStatus { Pending, Processing, Processed, Failed }

/// <summary>
/// Outbox message for reliable event publishing.
/// </summary>
public sealed class OutboxMessage : Entity
{
    public string Type { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public OutboxMessageStatus Status { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset? ProcessedOn { get; private set; }
    public string? Error { get; private set; }
    public int RetryCount { get; private set; }
    public long SequenceNumber { get; private set; } // For ordering

    private OutboxMessage() { }

    public OutboxMessage(string type, string content, long sequenceNumber = 0)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Content = content ?? throw new ArgumentNullException(nameof(content));
        Status = OutboxMessageStatus.Pending;
        CreatedOn = DateTimeOffset.UtcNow;
        RetryCount = 0;
        SequenceNumber = sequenceNumber;
    }

    public void MarkAsProcessing()
    {
        if (Status != OutboxMessageStatus.Pending && Status != OutboxMessageStatus.Failed)
            throw new InvalidOperationException("Can only mark pending or failed messages as processing");

        Status = OutboxMessageStatus.Processing;
    }

    public void MarkAsProcessed()
    {
        if (Status != OutboxMessageStatus.Processing)
            throw new InvalidOperationException("Can only mark processing messages as processed");

        Status = OutboxMessageStatus.Processed;
        ProcessedOn = DateTimeOffset.UtcNow;
    }

    public void MarkAsFailed(string error)
    {
        Status = OutboxMessageStatus.Failed;
        Error = error;
        RetryCount++;
    }

    public bool CanRetry() => RetryCount < 5; // Allow up to 5 retries

    public TimeSpan GetNextRetryDelay()
    {
        // Exponential backoff: 10s, 30s, 1.5m, 4.5m, 13.5m
        var delayMinutes = Math.Pow(3, RetryCount) * 10.0 / 60.0;
        return TimeSpan.FromMinutes(delayMinutes);
    }
}

