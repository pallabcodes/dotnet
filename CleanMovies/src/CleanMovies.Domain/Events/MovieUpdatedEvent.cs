using CleanMovies.Domain.Common;

namespace CleanMovies.Domain.Events;

public sealed class MovieUpdatedEvent : DomainEventBase
{
    public MovieUpdatedEvent(Guid movieId, string slug)
    {
        MovieId = movieId;
        Slug = slug;
    }

    public Guid MovieId { get; }
    public string Slug { get; }
}
