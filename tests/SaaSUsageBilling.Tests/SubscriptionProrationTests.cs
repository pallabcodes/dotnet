using System;
using Xunit;
using FluentAssertions;
using SaaSUsageBilling.Domain.Entities;
using SaaSUsageBilling.Domain.ValueObjects;

namespace SaaSUsageBilling.Tests;

public class SubscriptionProrationTests
{
    [Fact]
    public void CalculateProrationFactor_ShouldAccountForMidPeriodStart()
    {
        var now = new DateTimeOffset(2024, 01, 15, 0, 0, 0, TimeSpan.Zero);
        var period = new Period(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2024, 02, 01, 0, 0, 0, TimeSpan.Zero));

        var subscription = new Subscription(Guid.NewGuid(), Guid.NewGuid(), now, period);

        var factor = subscription.CalculateProrationFactor(period.From, period.To);

        factor.Should().BeApproximately(0.5m, 0.1m);
    }
}

