using System.Numerics;

namespace KnowledgeBase.Samples.Generics;

/// <summary>
/// Generic math without boxing. The INumber&lt;T&gt; static-abstract interface
/// gives the compiler a checked contract ('T has arithmetic operators') instead
/// of funneling every value through object/Convert.ToDouble at runtime.
/// </summary>
public static class Numeric
{
    public static T Add<T>(T left, T right)
        where T : INumber<T>
        => left + right;

    public static T Sum<T>(IEnumerable<T> values)
        where T : INumber<T>
        => values.Aggregate(T.Zero, static (acc, value) => acc + value);
}