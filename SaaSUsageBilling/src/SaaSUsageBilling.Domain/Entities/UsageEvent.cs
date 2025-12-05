using SaaSUsageBilling.Domain.Common;

namespace SaaSUsageBilling.Domain.Entities;

/// <summary>
/// Usage record posted by a client system.
/// </summary>
public sealed class UsageEvent : Entity
{
    public Guid SubscriptionId { get; private set; }
    public int Quantity { get; private set; }
    public DateTimeOffset OccurredAt { get; private set; }

    private UsageEvent() { }

    public UsageEvent(Guid subscriptionId, int quantity, DateTimeOffset occurredAt)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        SubscriptionId = subscriptionId;
        Quantity = quantity;
        OccurredAt = occurredAt;
    }
}
