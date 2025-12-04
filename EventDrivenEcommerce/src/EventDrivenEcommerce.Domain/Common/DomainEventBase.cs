namespace EventDrivenEcommerce.Domain.Common;

/// <summary>
/// Base class for domain events providing common properties.
/// </summary>
public abstract class DomainEventBase : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

