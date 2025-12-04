using EventDrivenEcommerce.Domain.Common;
using EventDrivenEcommerce.Domain.ValueObjects;

namespace EventDrivenEcommerce.Domain.Events;

/// <summary>
/// Domain event raised when an order is confirmed (payment successful).
/// </summary>
public sealed class OrderConfirmedEvent : DomainEventBase
{
    public OrderId OrderId { get; }
    public CustomerId CustomerId { get; }
    public Money TotalAmount { get; }

    public OrderConfirmedEvent(OrderId orderId, CustomerId customerId, Money totalAmount)
    {
        OrderId = orderId;
        CustomerId = customerId;
        TotalAmount = totalAmount;
    }
}

