using EventDrivenEcommerce.Application.IntegrationEvents;
using EventDrivenEcommerce.Domain.Common;
using EventDrivenEcommerce.Domain.Events;

namespace EventDrivenEcommerce.Application.EventHandlers;

/// <summary>
/// Maps domain events to integration events for publishing.
/// </summary>
public static class DomainEventToIntegrationEventMapper
{
    public static IIntegrationEvent? MapToIntegrationEvent(IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            OrderPlacedEvent e => new OrderPlacedIntegrationEvent(e.OrderId, e.CustomerId, e.TotalAmount, e.Items),
            OrderConfirmedEvent e => new OrderConfirmedIntegrationEvent(e.OrderId, e.CustomerId, e.TotalAmount),
            _ => null // Not all domain events need to be published externally
        };
    }
}

