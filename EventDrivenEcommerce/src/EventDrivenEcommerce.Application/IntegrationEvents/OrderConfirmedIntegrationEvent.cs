using EventDrivenEcommerce.Domain.ValueObjects;

namespace EventDrivenEcommerce.Application.IntegrationEvents;

/// <summary>
/// Integration event published when an order is confirmed (payment successful).
/// Used to trigger shipping and inventory updates.
/// </summary>
public sealed class OrderConfirmedIntegrationEvent : IntegrationEventBase
{
    public OrderId OrderId { get; }
    public CustomerId CustomerId { get; }
    public Money TotalAmount { get; }

    public OrderConfirmedIntegrationEvent(OrderId orderId, CustomerId customerId, Money totalAmount)
    {
        OrderId = orderId;
        CustomerId = customerId;
        TotalAmount = totalAmount;
    }
}

