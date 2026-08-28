namespace KnowledgeBase.Samples.Polymorphism;

/// <summary>A concrete shape; immutable, validated, and fully testable.</summary>
public sealed class Circle : Shape
{
    public Circle(double radius)
        : base("Circle")
    {
        if (radius <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(radius), "Radius must be positive.");
        }

        Radius = radius;
    }

    public double Radius { get; }

    public override double Area() => Math.PI * Radius * Radius;

    public override string Describe() => $"{base.Describe()} with radius {Radius:F2}";
}