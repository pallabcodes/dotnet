namespace CleanMovies.Domain.ValueObjects;

public sealed record Genre(string Name)
{
    public static Genre From(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new Genre(name.Trim());
    }
}
