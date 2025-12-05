using SaaSUsageBilling.Domain.Common;

namespace SaaSUsageBilling.Domain.ValueObjects;

/// <summary>
/// Represents a pricing tier with volume ranges and pricing.
/// </summary>
public sealed class PricingTier : ValueObject
{
    public int MinUnits { get; }
    public int? MaxUnits { get; }
    public Money PricePerUnit { get; }

    public PricingTier(int minUnits, int? maxUnits, Money pricePerUnit)
    {
        if (minUnits < 0) throw new ArgumentOutOfRangeException(nameof(minUnits));
        if (maxUnits.HasValue && maxUnits.Value <= minUnits) throw new ArgumentOutOfRangeException(nameof(maxUnits));

        MinUnits = minUnits;
        MaxUnits = maxUnits;
        PricePerUnit = pricePerUnit;
    }

    public bool Contains(int units) => units >= MinUnits && (!MaxUnits.HasValue || units <= MaxUnits);

    public int GetBillableUnits(int totalUnits)
    {
        if (totalUnits <= MinUnits) return 0;
        var maxInTier = MaxUnits ?? int.MaxValue;
        return Math.Min(totalUnits, maxInTier) - MinUnits;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return MinUnits;
        yield return MaxUnits ?? int.MaxValue;
        yield return PricePerUnit;
    }
}