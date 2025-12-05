using EventDrivenEcommerce.Application.Common;
using EventDrivenEcommerce.Application.IntegrationEvents;
using Microsoft.Extensions.Logging;

namespace EventDrivenEcommerce.Infrastructure.Messaging;

/// <summary>
/// Test-friendly publisher that swallows events instead of touching external brokers.
/// </summary>
public sealed class NoOpEventPublisher : IEventPublisher
{
    private readonly ILogger<NoOpEventPublisher> _logger;

    public NoOpEventPublisher(ILogger<NoOpEventPublisher> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("NoOpEventPublisher received event {EventType}; skipping external publish.", integrationEvent.GetType().Name);
        return Task.CompletedTask;
    }
}
