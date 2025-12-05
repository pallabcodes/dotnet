using SaaSUsageBilling.Domain.Common;

namespace SaaSUsageBilling.Domain.ValueObjects;

/// <summary>
/// Represents a discount that can be applied to pricing.
/// </summary>
public abstract class Discount : ValueObject
{
    public string Description { get; }
    public DateTimeOffset? ValidUntil { get; }

    protected Discount(string description, DateTimeOffset? validUntil = null)
    {
        Description = description ?? throw new ArgumentNullException(nameof(description));
        ValidUntil = validUntil;
    }

    public bool IsValid(DateTimeOffset asOf) => !ValidUntil.HasValue || ValidUntil.Value >= asOf;

    public abstract Money Apply(Money amount, DateTimeOffset asOf);
}

/// <summary>
/// Fixed amount discount.
/// </summary>
public sealed class FixedAmountDiscount : Discount
{
    public Money Amount { get; }

    public FixedAmountDiscount(Money amount, string description, DateTimeOffset? validUntil = null)
        : base(description, validUntil)
    {
        Amount = amount;
    }

    public override Money Apply(Money amount, DateTimeOffset asOf)
    {
        if (!IsValid(asOf)) return amount;
        return amount.Subtract(Math.Min(amount.Amount, Amount.Amount));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Description;
        yield return ValidUntil ?? DateTimeOffset.MinValue;
    }
}

/// <summary>
/// Percentage-based discount.
/// </summary>
public sealed class PercentageDiscount : Discount
{
    public decimal Percentage { get; }

    public PercentageDiscount(decimal percentage, string description, DateTimeOffset? validUntil = null)
        : base(description, validUntil)
    {
        if (percentage < 0 || percentage > 100) throw new ArgumentOutOfRangeException(nameof(percentage));
        Percentage = percentage;
    }

    public override Money Apply(Money amount, DateTimeOffset asOf)
    {
        if (!IsValid(asOf)) return amount;
        var discountAmount = amount.Amount * (Percentage / 100m);
        return new Money(amount.Amount - discountAmount, amount.Currency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Percentage;
        yield return Description;
        yield return ValidUntil ?? DateTimeOffset.MinValue;
    }
}

