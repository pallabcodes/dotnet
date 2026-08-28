namespace KnowledgeBase.Samples.Operators;

/// <summary>
/// Operator overloading done correctly. Each overloaded equality operator is
/// backed by a consistent Equals + GetHashCode pair (the two must always agree).
/// GetHashCode uses HashCode.Combine rather than XOR, which would be order-
/// dependent and heavily collision-prone for this shape.
/// </summary>
public sealed class Box : IEquatable<Box>
{
    public Box(double length, double width, double height)
    {
        if (length <= 0 || width <= 0 || height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "All dimensions must be positive.");
        }

        Length = length;
        Width = width;
        Height = height;
    }

    public double Length { get; }

    public double Width { get; }

    public double Height { get; }

    public static Box operator +(Box left, Box right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return new Box(left.Length + right.Length, left.Width + right.Width, left.Height + right.Height);
    }

    public static Box operator -(Box left, Box right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return new Box(left.Length - right.Length, left.Width - right.Width, left.Height - right.Height);
    }

    /// <summary>A Box collapses to int as its average side length.</summary>
    public static explicit operator int(Box box) => (int)Math.Round((box.Length + box.Width + box.Height) / 3);

    /// <summary>An int expands to a cube of that side length.</summary>
    public static implicit operator Box(int size) => new(size, size, size);

    public static bool operator ==(Box? left, Box? right) => left?.Equals(right) ?? right is null;

    public static bool operator !=(Box? left, Box? right) => !(left == right);

    public bool Equals(Box? other) =>
        other is not null &&
        (Length, Width, Height) == (other.Length, other.Width, other.Height);

    public override bool Equals(object? obj) => Equals(obj as Box);

    public override int GetHashCode() => HashCode.Combine(Length, Width, Height);

    public override string ToString() => $"Box({Length:F2} x {Width:F2} x {Height:F2})";
}