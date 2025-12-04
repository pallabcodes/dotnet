using EventDrivenEcommerce.Domain.Common;
using EventDrivenEcommerce.Domain.ValueObjects;

namespace EventDrivenEcommerce.Domain.Events;

/// <summary>
/// Domain event raised when an order is shipped.
/// </summary>
public sealed class OrderShippedEvent : DomainEventBase
{
    public OrderId OrderId { get; }
    public CustomerId CustomerId { get; }
    public string TrackingNumber { get; }

    public OrderShippedEvent(OrderId orderId, CustomerId customerId, string trackingNumber)
    {
        OrderId = orderId;
        CustomerId = customerId;
        TrackingNumber = trackingNumber;
    }
}

