using EventDrivenEcommerce.Domain.Common;
using EventDrivenEcommerce.Domain.ValueObjects;

namespace EventDrivenEcommerce.Domain.Events;

/// <summary>
/// Domain event raised when an order is placed.
/// </summary>
public sealed class OrderPlacedEvent : DomainEventBase
{
    public OrderId OrderId { get; }
    public CustomerId CustomerId { get; }
    public Money TotalAmount { get; }
    public IReadOnlyCollection<OrderItem> Items { get; }

    public OrderPlacedEvent(OrderId orderId, CustomerId customerId, Money totalAmount, IReadOnlyCollection<OrderItem> items)
    {
        OrderId = orderId;
        CustomerId = customerId;
        TotalAmount = totalAmount;
        Items = items;
    }
}

