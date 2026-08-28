using KnowledgeBase.Samples.Generics;

namespace KnowledgeBase.Samples.Tests;

public sealed class GenericsTests
{
    [Theory]
    [InlineData(2, 3, 5)]
    [InlineData(-1, 1, 0)]
    [InlineData(int.MaxValue, 0, int.MaxValue)]
    public void Add_works_for_integers(int left, int right, int expected)
    {
        Assert.Equal(expected, Numeric.Add(left, right));
    }

    [Fact]
    public void Add_works_for_doubles()
    {
        Assert.Equal(7.75, Numeric.Add(3.5, 4.25), precision: 10);
    }

    [Fact]
    public void Sum_uses_the_types_zero()
    {
        Assert.Equal(55, Numeric.Sum(Enumerable.Range(1, 10)));
        Assert.Equal(0m, Numeric.Sum(Array.Empty<decimal>()));
    }

    [Fact]
    public void Sum_accumulates_arbitrary_sequences()
    {
        Assert.Equal(6m, Numeric.Sum(new[] { 1m, 2m, 3m }));
    }
}