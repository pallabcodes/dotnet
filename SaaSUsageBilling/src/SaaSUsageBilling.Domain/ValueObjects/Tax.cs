using SaaSUsageBilling.Domain.Common;

using SaaSUsageBilling.Domain.Common;

namespace SaaSUsageBilling.Domain.ValueObjects;

/// <summary>
/// Represents tax calculation rules.
/// </summary>
public sealed class Tax : ValueObject
{
    public string Name { get; }
    public decimal Rate { get; } // Percentage (e.g., 8.25 for 8.25%)
    public string? Region { get; } // Optional region-specific tax

    public Tax(string name, decimal rate, string? region = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        if (rate < 0 || rate > 100) throw new ArgumentOutOfRangeException(nameof(rate));
        Rate = rate;
        Region = region;
    }

    public Money CalculateTax(Money amount)
    {
        var taxAmount = amount.Amount * (Rate / 100m);
        return new Money(taxAmount, amount.Currency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Rate;
        yield return Region ?? string.Empty;
    }
}