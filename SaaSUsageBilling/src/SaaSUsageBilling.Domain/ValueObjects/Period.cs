namespace SaaSUsageBilling.Domain.ValueObjects;

/// <summary>
/// Billing period represented by inclusive start and exclusive end.
/// </summary>
public sealed record Period(DateTimeOffset From, DateTimeOffset To)
{
    private Period() : this(DateTimeOffset.MinValue, DateTimeOffset.MaxValue) { } // For EF Core

    public static Period Create(DateTimeOffset from, DateTimeOffset to)
    {
        if (from >= to) throw new ArgumentException("From must be before To", nameof(from));
        return new Period(from, to);
    }

    public static Period CurrentMonthUtc()
    {
        var now = DateTimeOffset.UtcNow;
        var start = new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, TimeSpan.Zero);
        var end = start.AddMonths(1);
        return new Period(start, end);
    }

    public bool Contains(DateTimeOffset timestamp) => timestamp >= From && timestamp < To;

    public TimeSpan Duration => To - From;

    public int Days => (int)Duration.TotalDays;

    public Period Next() => new(To, To.AddDays(Days));

    public bool OverlapsWith(Period other) =>
        From < other.To && To > other.From;
}
