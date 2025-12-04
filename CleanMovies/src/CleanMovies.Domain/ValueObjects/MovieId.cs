namespace CleanMovies.Domain.ValueObjects;

public readonly record struct MovieId(Guid Value)
{
    public static MovieId New() => new(Guid.NewGuid());
}
