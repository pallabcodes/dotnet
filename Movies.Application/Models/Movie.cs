namespace Movies.Application.Models;

public abstract class Movie
{
    public required Guid Id { get; init; }

    public required string Title { get; init; }

    public required int YearOfRelease { get; init; }

    public required List<string> Genres { get; init; } = new();
}