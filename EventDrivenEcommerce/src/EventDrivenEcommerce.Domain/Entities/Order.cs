using EventDrivenEcommerce.Domain.Common;
using EventDrivenEcommerce.Domain.Events;
using EventDrivenEcommerce.Domain.ValueObjects;

namespace EventDrivenEcommerce.Domain.Entities;

/// <summary>
/// Aggregate root representing an order in the e-commerce system.
/// Manages the complete order lifecycle through domain events.
/// </summary>
public sealed class Order : AggregateRoot
{
    private readonly List<OrderItem> _items = new();

    public OrderId OrderId { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public Address ShippingAddress { get; private set; } = null!;
    public OrderStatus Status { get; private set; }
    public Money TotalAmount { get; private set; } = null!;
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    // For EF Core
    private Order() { }

    private Order(OrderId orderId, CustomerId customerId, Address shippingAddress, IEnumerable<OrderItem> items)
    {
        OrderId = orderId;
        CustomerId = customerId;
        ShippingAddress = shippingAddress;
        Status = OrderStatus.Pending;

        _items.AddRange(items);
        RecalculateTotal();

        AddDomainEvent(new OrderPlacedEvent(orderId, customerId, TotalAmount, Items));
    }

    public static Order Create(CustomerId customerId, Address shippingAddress, IEnumerable<OrderItem> items)
    {
        if (!items.Any())
            throw new ArgumentException("Order must contain at least one item", nameof(items));

        var orderId = OrderId.New();
        return new Order(orderId, customerId, shippingAddress, items);
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"Cannot confirm order in status {Status}");

        Status = OrderStatus.Confirmed;
        AddDomainEvent(new OrderConfirmedEvent(OrderId, CustomerId, TotalAmount));
    }

    public void Ship(string trackingNumber)
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException($"Cannot ship order in status {Status}");

        Status = OrderStatus.Shipped;
        AddDomainEvent(new OrderShippedEvent(OrderId, CustomerId, trackingNumber));
    }

    public void Cancel(string reason)
    {
        if (Status is OrderStatus.Shipped or OrderStatus.Delivered)
            throw new InvalidOperationException($"Cannot cancel order in status {Status}");

        Status = OrderStatus.Cancelled;
        AddDomainEvent(new OrderCancelledEvent(OrderId, CustomerId, reason));
    }

    private void RecalculateTotal()
    {
        TotalAmount = _items.Aggregate(Money.Zero("USD"), (total, item) => total.Add(item.TotalPrice));
    }
}

