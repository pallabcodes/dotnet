using EventDrivenEcommerce.Domain.ValueObjects;

namespace EventDrivenEcommerce.Application.IntegrationEvents;

/// <summary>
/// Integration event published when an order is placed.
/// Used to trigger downstream processes like payment and inventory.
/// </summary>
public sealed class OrderPlacedIntegrationEvent : IntegrationEventBase
{
    public OrderId OrderId { get; }
    public CustomerId CustomerId { get; }
    public Money TotalAmount { get; }
    public IReadOnlyCollection<OrderItem> Items { get; }

    public OrderPlacedIntegrationEvent(OrderId orderId, CustomerId customerId, Money totalAmount, IReadOnlyCollection<OrderItem> items)
    {
        OrderId = orderId;
        CustomerId = customerId;
        TotalAmount = totalAmount;
        Items = items;
    }
}

