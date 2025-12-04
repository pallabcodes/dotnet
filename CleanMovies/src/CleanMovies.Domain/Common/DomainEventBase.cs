namespace CleanMovies.Domain.Common;

public abstract class DomainEventBase : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
