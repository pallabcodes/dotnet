using EventDrivenEcommerce.Application.IntegrationEvents;

namespace EventDrivenEcommerce.Application.Common;

/// <summary>
/// Interface for publishing integration events.
/// </summary>
public interface IEventPublisher
{
    Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}

