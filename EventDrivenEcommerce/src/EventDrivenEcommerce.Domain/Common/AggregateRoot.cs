namespace EventDrivenEcommerce.Domain.Common;

/// <summary>
/// Base class for all aggregate roots.
/// Provides domain event collection and management.
/// </summary>
public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void MarkAsModified()
    {
        base.MarkAsModified();
    }
}

