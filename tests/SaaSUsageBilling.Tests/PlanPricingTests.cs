using System;
using Xunit;
using FluentAssertions;
using SaaSUsageBilling.Domain.Entities;
using SaaSUsageBilling.Domain.ValueObjects;

namespace SaaSUsageBilling.Tests;

public class PlanPricingTests
{
    [Fact]
    public void CalculateUsageCost_ShouldRespectTiersAndDiscounts()
    {
        var plan = new Plan("Pro", "Pro plan", new Money(100, "USD"));
        plan.AddPricingTier(new PricingTier(0, 999, Money.Zero("USD")));
        plan.AddPricingTier(new PricingTier(1000, 5000, new Money(0.10m, "USD")));
        plan.AddDiscount(new PercentageDiscount(10, "Promo", DateTimeOffset.UtcNow.AddDays(1)));

        var cost = plan.CalculateUsageCost(2000, DateTimeOffset.UtcNow);

        cost.Currency.Should().Be("USD");
        cost.Amount.Should().Be(90m); // 1000 units at $0.10 with 10% off
    }
}

