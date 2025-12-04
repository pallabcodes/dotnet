namespace EventDrivenEcommerce.Application.IntegrationEvents;

/// <summary>
/// Marker interface for integration events.
/// Integration events are used for cross-service communication.
/// </summary>
public interface IIntegrationEvent
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

