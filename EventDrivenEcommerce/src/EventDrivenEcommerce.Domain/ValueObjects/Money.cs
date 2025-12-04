namespace EventDrivenEcommerce.Domain.ValueObjects;

/// <summary>
/// Value object representing monetary amounts with currency.
/// </summary>
public sealed class Money
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
}

