namespace KnowledgeBase.Samples.Polymorphism;

/// <summary>A concrete shape; the runtime type drives the polymorphic behaviour.</summary>
public sealed class Rectangle : Shape
{
    public Rectangle(double length, double width)
        : base("Rectangle")
    {
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be positive.");
        }

        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be positive.");
        }

        Length = length;
        Width = width;
    }

    public double Length { get; }

    public double Width { get; }

    public override double Area() => Length * Width;

    public override string Describe() => $"{base.Describe()} with length {Length:F2} and width {Width:F2}";
}