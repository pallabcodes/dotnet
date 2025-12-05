namespace EventDrivenEcommerce.Domain.ValueObjects;

/// <summary>
/// Value object representing monetary amounts with currency.
/// Implements value-based equality semantics for predictable comparisons.
/// </summary>
public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = string.Empty;

    // Parameterless constructor for EF Core
    private Money() { }

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Zero(string currency = "USD") => new(0, currency);

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot add money with different currencies");

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Multiply(int quantity)
    {
        return new Money(Amount * quantity, Currency);
    }

    public bool IsPositive => Amount > 0;
    public bool IsNegative => Amount < 0;
    public bool IsZero => Amount == 0;

    public bool Equals(Money? other) =>
        other is not null &&
        Amount == other.Amount &&
        string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => Equals(obj as Money);

    public override int GetHashCode() => HashCode.Combine(Amount, Currency.ToUpperInvariant());

    public static bool operator ==(Money? left, Money? right) =>
        EqualityComparer<Money>.Default.Equals(left, right);

    public static bool operator !=(Money? left, Money? right) => !(left == right);
}
