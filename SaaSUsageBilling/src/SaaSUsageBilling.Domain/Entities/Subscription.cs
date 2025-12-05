using SaaSUsageBilling.Domain.Common;
using SaaSUsageBilling.Domain.ValueObjects;

namespace SaaSUsageBilling.Domain.Entities;

public enum SubscriptionStatus { Active, Suspended, Cancelled, PastDue }

/// <summary>
/// Subscription of a customer to a plan with proration and advanced state management.
/// </summary>
public sealed class Subscription : Entity
{
    public Guid CustomerId { get; private set; }
    public Guid PlanId { get; private set; }
    public DateTimeOffset StartedOn { get; private set; }
    public DateTimeOffset? CancelledOn { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public Period CurrentPeriod { get; private set; }
    public int CurrentPeriodUnits { get; private set; }
    public DateTimeOffset LastBilledOn { get; private set; }
    public bool AutoRenew { get; private set; } = true;

    // Track usage by day for proration calculations
    private readonly Dictionary<DateTimeOffset, int> _dailyUsage = new();

    private Subscription() { }

    public Subscription(Guid customerId, Guid planId, DateTimeOffset startedOn, Period period)
    {
        CustomerId = customerId;
        PlanId = planId;
        StartedOn = startedOn;
        CurrentPeriod = period;
        Status = SubscriptionStatus.Active;
        CurrentPeriodUnits = 0;
        LastBilledOn = startedOn;
        AutoRenew = true;
    }

    public void RecordUsage(int units, DateTimeOffset occurredAt)
    {
        if (Status != SubscriptionStatus.Active) throw new InvalidOperationException("Subscription inactive.");
        if (units <= 0) throw new ArgumentOutOfRangeException(nameof(units));
        if (!CurrentPeriod.Contains(occurredAt)) throw new ArgumentOutOfRangeException(nameof(occurredAt));

        CurrentPeriodUnits += units;

        // Track daily usage for proration
        var dayKey = occurredAt.Date;
        _dailyUsage[dayKey] = _dailyUsage.GetValueOrDefault(dayKey) + units;
    }

    public void Suspend()
    {
        if (Status != SubscriptionStatus.Active) throw new InvalidOperationException("Can only suspend active subscriptions");
        Status = SubscriptionStatus.Suspended;
    }

    public void Reactivate()
    {
        if (Status != SubscriptionStatus.Suspended) throw new InvalidOperationException("Can only reactivate suspended subscriptions");
        Status = SubscriptionStatus.Active;
    }

    public void Cancel(DateTimeOffset cancelledAt)
    {
        if (Status == SubscriptionStatus.Cancelled) return;
        Status = SubscriptionStatus.Cancelled;
        CancelledOn = cancelledAt;
        AutoRenew = false;
    }

    public void MarkPastDue()
    {
        if (Status == SubscriptionStatus.Active)
        {
            Status = SubscriptionStatus.PastDue;
        }
    }

    public void SetAutoRenew(bool autoRenew)
    {
        AutoRenew = autoRenew;
    }

    public void ResetPeriod(Period nextPeriod, DateTimeOffset billedOn)
    {
        CurrentPeriodUnits = 0;
        CurrentPeriod = nextPeriod;
        LastBilledOn = billedOn;
        _dailyUsage.Clear();
    }

    public decimal CalculateProrationFactor(DateTimeOffset billingStart, DateTimeOffset billingEnd)
    {
        if (billingStart >= billingEnd) return 0;

        // If subscription started after billing period, no proration needed
        if (StartedOn >= billingEnd) return 0;

        // If subscription was cancelled before billing period, no proration needed
        if (CancelledOn.HasValue && CancelledOn.Value <= billingStart) return 0;

        var effectiveStart = StartedOn > billingStart ? StartedOn : billingStart;
        var effectiveEnd = CancelledOn.HasValue && CancelledOn.Value < billingEnd
            ? CancelledOn.Value
            : billingEnd;

        if (effectiveStart >= effectiveEnd) return 0;

        var totalPeriod = billingEnd - billingStart;
        var activePeriod = effectiveEnd - effectiveStart;

        return (decimal)(activePeriod.TotalSeconds / totalPeriod.TotalSeconds);
    }

    private bool IsActiveOnDate(DateTimeOffset date)
    {
        if (Status == SubscriptionStatus.Cancelled && CancelledOn.HasValue)
        {
            return date < CancelledOn.Value.Date;
        }

        return Status == SubscriptionStatus.Active ||
               (Status == SubscriptionStatus.Suspended && date < DateTimeOffset.UtcNow.Date);
    }

    public IReadOnlyDictionary<DateTimeOffset, int> GetDailyUsage() => _dailyUsage;
}
