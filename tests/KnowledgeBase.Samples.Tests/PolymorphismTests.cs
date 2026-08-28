using KnowledgeBase.Samples.Polymorphism;

namespace KnowledgeBase.Samples.Tests;

public sealed class PolymorphismTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Circle_rejects_non_positive_radius(double radius)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Circle(radius));
    }

    [Fact]
    public void Circle_area_is_pi_times_radius_squared()
    {
        var circle = new Circle(2);
        Assert.Equal(Math.PI * 4, circle.Area(), precision: 10);
    }

    [Fact]
    public void Rectangle_area_is_length_times_width()
    {
        var rectangle = new Rectangle(4, 6);
        Assert.Equal(24, rectangle.Area());
    }

    [Fact]
    public void Runtime_type_drives_area_dispatch()
    {
        Shape[] shapes = [new Circle(5), new Rectangle(4, 6)];

        var total = shapes.Sum(shape => shape.Area());

        Assert.Equal(Math.PI * 25 + 24, total, precision: 10);
    }

    [Fact]
    public void Circle_description_delegates_to_base_then_specializes()
    {
        var circle = new Circle(5);
        var description = circle.Describe();

        Assert.StartsWith("This is a Circle", description);
        Assert.Contains("radius 5.00", description);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Rectangle_rejects_non_positive_dimensions(double dimension)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Rectangle(dimension, 4));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Rectangle(4, dimension));
    }
}