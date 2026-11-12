using Shared.Domain.Common;

namespace Template.Domain.Projects.ValueObjects;

public sealed class ProjectName : ValueObject
{
    public const int MaxLength = 200;

    public string Value { get; }

    private ProjectName(string value)
    {
        Value = value;
    }

    public static ProjectName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Project name is required.", nameof(value));

        var trimmed = value.Trim();

        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"Project name must be at most {MaxLength} characters.", nameof(value));

        return new ProjectName(trimmed);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
