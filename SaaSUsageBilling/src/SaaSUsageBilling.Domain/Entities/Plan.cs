using SaaSUsageBilling.Domain.Common;
using SaaSUsageBilling.Domain.ValueObjects;

namespace SaaSUsageBilling.Domain.Entities;

/// <summary>
/// Comprehensive pricing plan with tiered pricing, discounts, and taxes.
/// </summary>
public sealed class Plan : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Money MonthlyBase { get; private set; } = Money.Zero();
    public IReadOnlyCollection<PricingTier> PricingTiers => _pricingTiers.AsReadOnly();
    public IReadOnlyCollection<Discount> Discounts => _discounts.AsReadOnly();
    public IReadOnlyCollection<Tax> ApplicableTaxes => _applicableTaxes.AsReadOnly();
    public bool IsActive { get; private set; } = true;

    private readonly List<PricingTier> _pricingTiers = new();
    private readonly List<Discount> _discounts = new();
    private readonly List<Tax> _applicableTaxes = new();

    private Plan() { }

    public Plan(string name, string description, Money monthlyBase)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
        Description = description;
        MonthlyBase = monthlyBase;
        IsActive = true;
    }

    public void AddPricingTier(PricingTier tier)
    {
        if (tier == null) throw new ArgumentNullException(nameof(tier));

        // Validate tier doesn't overlap with existing tiers
        foreach (var existingTier in _pricingTiers)
        {
            if (tier.MinUnits < (existingTier.MaxUnits ?? int.MaxValue) &&
                (tier.MaxUnits ?? int.MaxValue) > existingTier.MinUnits)
            {
                throw new InvalidOperationException($"Pricing tier {tier.MinUnits}-{tier.MaxUnits ?? int.MaxValue} overlaps with existing tier {existingTier.MinUnits}-{existingTier.MaxUnits ?? int.MaxValue}");
            }
        }

        // Validate tier covers from its MinUnits
        if (_pricingTiers.Any() && tier.MinUnits != (_pricingTiers.Max(t => t.MaxUnits ?? int.MaxValue) + 1))
        {
            throw new InvalidOperationException("Pricing tiers must be contiguous and cover all usage levels");
        }

        _pricingTiers.Add(tier);
        _pricingTiers.Sort((a, b) => a.MinUnits.CompareTo(b.MinUnits));
    }

    public void AddDiscount(Discount discount)
    {
        _discounts.Add(discount);
    }

    public void AddTax(Tax tax)
    {
        _applicableTaxes.Add(tax);
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public Money CalculateUsageCost(int units, DateTimeOffset asOf)
    {
        if (!IsActive) throw new InvalidOperationException("Cannot calculate cost for inactive plan");
        if (units < 0) throw new ArgumentOutOfRangeException(nameof(units));

        var totalCost = Money.Zero(MonthlyBase.Currency);

        // Calculate cost across all applicable tiers
        foreach (var tier in _pricingTiers.OrderBy(t => t.MinUnits))
        {
            var billableInTier = tier.GetBillableUnits(units);
            if (billableInTier > 0)
            {
                totalCost = totalCost.Add(tier.PricePerUnit.Multiply(billableInTier));
            }
        }

        // Apply usage-specific discounts
        foreach (var discount in _discounts.Where(d => d.IsValid(asOf)))
        {
            totalCost = discount.Apply(totalCost, asOf);
        }

        return totalCost;
    }

    public Money CalculateTotalCost(int units, DateTimeOffset asOf)
    {
        var usageCost = CalculateUsageCost(units, asOf);
        var subtotal = MonthlyBase.Add(usageCost);

        // Apply discounts to subtotal
        foreach (var discount in _discounts.Where(d => d.IsValid(asOf)))
        {
            subtotal = discount.Apply(subtotal, asOf);
        }

        return subtotal;
    }

    public Money CalculateTaxes(Money amount)
    {
        var totalTax = Money.Zero(amount.Currency);
        foreach (var tax in _applicableTaxes)
        {
            totalTax = totalTax.Add(tax.CalculateTax(amount));
        }
        return totalTax;
    }
}
