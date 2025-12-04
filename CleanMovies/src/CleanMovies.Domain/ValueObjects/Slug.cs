using System.Text.RegularExpressions;

namespace CleanMovies.Domain.ValueObjects;

public sealed class Slug : IEquatable<Slug>
{
    private static readonly Regex InvalidChars = new("[^a-z0-9-]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private Slug(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Slug From(string title, int year)
    {
        var normalized = title.Trim().ToLowerInvariant().Replace(' ', '-');
        normalized = InvalidChars.Replace(normalized, string.Empty);
        return new Slug($"{normalized}-{year}");
    }

    public static Slug FromExisting(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        return new Slug(value);
    }

    public bool Equals(Slug? other) => other is not null && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    public override string ToString() => Value;
}
