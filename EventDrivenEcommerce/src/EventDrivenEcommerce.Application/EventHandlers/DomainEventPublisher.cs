using EventDrivenEcommerce.Application.Common;
using EventDrivenEcommerce.Application.IntegrationEvents;
using EventDrivenEcommerce.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventDrivenEcommerce.Application.EventHandlers;

/// <summary>
/// Handler that processes domain events and stores them as outbox messages for reliable publishing.
/// </summary>
public sealed class DomainEventPublisher : INotificationHandler<DomainEventsPublishedNotification>
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly ILogger<DomainEventPublisher> _logger;

    public DomainEventPublisher(IOutboxRepository outboxRepository, ILogger<DomainEventPublisher> logger)
    {
        _outboxRepository = outboxRepository;
        _logger = logger;
    }

    public async Task Handle(DomainEventsPublishedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"DomainEventPublisher handling {notification.DomainEvents.Count()} events");

        foreach (var domainEvent in notification.DomainEvents)
        {
            Console.WriteLine($"Processing domain event: {domainEvent.GetType().Name}");

            var integrationEvent = DomainEventToIntegrationEventMapper.MapToIntegrationEvent(domainEvent);
            if (integrationEvent != null)
            {
                Console.WriteLine($"Creating outbox message for integration event: {integrationEvent.GetType().Name}");

                var outboxMessage = new OutboxMessage(integrationEvent);
                await _outboxRepository.AddAsync(outboxMessage, cancellationToken);

                _logger.LogInformation("Stored integration event {EventType} in outbox for domain event {DomainEventType}",
                    integrationEvent.GetType().Name, domainEvent.GetType().Name);
            }
            else
            {
                Console.WriteLine($"No integration event mapped for domain event: {domainEvent.GetType().Name}");
            }
        }
    }
}

/// <summary>
/// Notification raised when domain events are published from an aggregate.
/// </summary>
public sealed record DomainEventsPublishedNotification(IEnumerable<IDomainEvent> DomainEvents) : INotification;

