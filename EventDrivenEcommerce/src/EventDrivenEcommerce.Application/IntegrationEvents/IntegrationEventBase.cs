namespace EventDrivenEcommerce.Application.IntegrationEvents;

/// <summary>
/// Base class for integration events providing common properties.
/// </summary>
public abstract class IntegrationEventBase : IIntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

