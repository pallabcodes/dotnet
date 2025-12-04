using EventDrivenEcommerce.Domain.Common;
using EventDrivenEcommerce.Domain.ValueObjects;

namespace EventDrivenEcommerce.Domain.Events;

/// <summary>
/// Domain event raised when an order is cancelled.
/// </summary>
public sealed class OrderCancelledEvent : DomainEventBase
{
    public OrderId OrderId { get; }
    public CustomerId CustomerId { get; }
    public string Reason { get; }

    public OrderCancelledEvent(OrderId orderId, CustomerId customerId, string reason)
    {
        OrderId = orderId;
        CustomerId = customerId;
        Reason = reason;
    }
}

