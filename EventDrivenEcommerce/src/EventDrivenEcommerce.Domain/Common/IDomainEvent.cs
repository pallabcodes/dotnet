namespace EventDrivenEcommerce.Domain.Common;

/// <summary>
/// Marker interface for all domain events.
/// Domain events represent significant business events that have occurred.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Unique identifier for the event instance.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Timestamp when the event occurred.
    /// </summary>
    DateTime OccurredOn { get; }
}

