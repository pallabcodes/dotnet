using EventDrivenEcommerce.Application.Common;
using EventDrivenEcommerce.Application.IntegrationEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EventDrivenEcommerce.Infrastructure.Messaging;

/// <summary>
/// Background service that processes outbox messages and publishes them via RabbitMQ.
/// Implements the outbox pattern for reliable event publishing.
/// </summary>
public sealed class OutboxProcessor : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxProcessor> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(10);

    public OutboxProcessor(IServiceProvider serviceProvider, ILogger<OutboxProcessor> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox processor started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxMessagesAsync(stoppingToken);
                await Task.Delay(_interval, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing outbox messages");
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); // Back off on error
            }
        }

        _logger.LogInformation("Outbox processor stopped");
    }

    private async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
        var eventPublisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();

        var messages = await outboxRepository.GetUnprocessedMessagesAsync(batchSize: 50, cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                var integrationEvent = DeserializeEvent(message);
                if (integrationEvent != null)
                {
                    await eventPublisher.PublishAsync(integrationEvent, cancellationToken);
                    await outboxRepository.MarkAsProcessedAsync(message.Id, cancellationToken);
                    _logger.LogInformation("Successfully processed outbox message {MessageId}", message.Id);
                }
                else
                {
                    await outboxRepository.MarkAsFailedAsync(message.Id, "Failed to deserialize event", cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process outbox message {MessageId}", message.Id);
                await outboxRepository.MarkAsFailedAsync(message.Id, ex.Message, cancellationToken);
            }
        }
    }

    private static IIntegrationEvent? DeserializeEvent(OutboxMessage message)
    {
        var type = Type.GetType(message.Type);
        if (type == null)
            return null;

        return JsonSerializer.Deserialize(message.Content, type) as IIntegrationEvent;
    }
}

