using KnowledgeBase.Samples.Operators;

namespace KnowledgeBase.Samples.Tests;

public sealed class BoxTests
{
    [Fact]
    public void Addition_combines_each_dimension()
    {
        var result = new Box(1, 2, 3) + new Box(4, 5, 6);
        Assert.Equal(new Box(5, 7, 9), result);
    }

    [Fact]
    public void Subtraction_differences_each_dimension()
    {
        var result = new Box(5, 7, 9) - new Box(1, 2, 3);
        Assert.Equal(new Box(4, 5, 6), result);
    }

    [Fact]
    public void Subtraction_cannot_produce_a_folded_box()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Box(1, 2, 3) - new Box(4, 5, 6));
    }

    [Fact]
    public void Equality_is_value_based()
    {
        var a = new Box(2, 3, 4);
        var b = new Box(2, 3, 4);
        var c = new Box(2, 3, 5);

        Assert.True(a == b);
        Assert.False(a == c);
        Assert.True(a != c);
        Assert.True(a.Equals(b));
        Assert.True(a.Equals((object)b));
        Assert.False(a.Equals(null));
    }

    [Fact]
    public void Equal_boxes_have_equal_hash_codes()
    {
        var a = new Box(2, 3, 4);
        var b = new Box(2, 3, 4);

        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void An_int_implicitly_becomes_a_cube_and_a_box_collapses_to_its_mean_side()
    {
        Box cube = 5;
        var backToInt = (int)cube;

        Assert.Equal(new Box(5, 5, 5), cube);
        Assert.Equal(5, backToInt);
    }

    [Fact]
    public void Non_positive_dimensions_are_rejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Box(0, 1, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Box(1, -1, 1));
    }

    [Fact]
    public void ToString_reports_all_dimensions()
    {
        Assert.Equal("Box(2.00 x 3.00 x 4.00)", new Box(2, 3, 4).ToString());
    }
}