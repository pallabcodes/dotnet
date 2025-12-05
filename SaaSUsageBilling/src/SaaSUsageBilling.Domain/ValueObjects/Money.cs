namespace SaaSUsageBilling.Domain.ValueObjects;

/// <summary>
/// Represents a currency amount with value-based equality.
/// </summary>
public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "USD")
    {
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency cannot be empty", nameof(currency));
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative");

        Currency = currency.ToUpperInvariant();
        Amount = amount;
    }

    public static Money Zero(string currency = "USD") => new(0, currency);

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(decimal amount)
    {
        return new Money(Math.Max(0, Amount - amount), Currency);
    }

    public Money Multiply(decimal factor) => new(Amount * factor, Currency);

    public bool Equals(Money? other) =>
        other is not null && Amount == other.Amount && Currency == other.Currency;

    public override bool Equals(object? obj) => Equals(obj as Money);
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);

    private void EnsureSameCurrency(Money other)
    {
        if (!string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Currencies must match.");
    }

    public override string ToString() => $"{Amount:0.00} {Currency}";
}
