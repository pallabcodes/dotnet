using EventDrivenEcommerce.Application.Common;
using EventDrivenEcommerce.Application.IntegrationEvents;
using EventDrivenEcommerce.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventDrivenEcommerce.Application.EventHandlers;

/// <summary>
/// Handles domain events by persisting corresponding integration events into the outbox.
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
        foreach (var domainEvent in notification.DomainEvents)
        {
            var integrationEvent = DomainEventToIntegrationEventMapper.MapToIntegrationEvent(domainEvent);
            if (integrationEvent is null)
            {
                _logger.LogDebug("Skipping domain event {DomainEventType} because no integration mapping exists", domainEvent.GetType().Name);
                continue;
            }

            var outboxMessage = new OutboxMessage(integrationEvent);
            await _outboxRepository.AddAsync(outboxMessage, cancellationToken);

            _logger.LogInformation("Enqueued integration event {EventType} for domain event {DomainEventType}", integrationEvent.GetType().Name, domainEvent.GetType().Name);
        }
    }
}

/// <summary>
/// Notification raised when domain events are collected for persistence.
/// </summary>
public sealed record DomainEventsPublishedNotification(IEnumerable<IDomainEvent> DomainEvents) : INotification;
