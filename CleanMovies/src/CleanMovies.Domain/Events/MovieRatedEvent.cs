using CleanMovies.Domain.Common;

namespace CleanMovies.Domain.Events;

public sealed class MovieRatedEvent : DomainEventBase
{
    public MovieRatedEvent(Guid movieId, Guid userId, int value)
    {
        MovieId = movieId;
        UserId = userId;
        Value = value;
    }

    public Guid MovieId { get; }
    public Guid UserId { get; }
    public int Value { get; }
}
