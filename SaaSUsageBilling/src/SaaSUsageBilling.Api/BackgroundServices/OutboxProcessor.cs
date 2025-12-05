using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SaaSUsageBilling.Application.Abstractions;
using SaaSUsageBilling.Domain.Entities;
using SaaSUsageBilling.Infrastructure.Persistence;
using System.Text.Json;

namespace SaaSUsageBilling.Api.BackgroundServices;

/// <summary>
/// Background service that processes outbox messages for reliable event publishing.
/// </summary>
public class OutboxProcessor : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxProcessor> _logger;
    private readonly TimeSpan _processingInterval = TimeSpan.FromSeconds(10);

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
                await Task.Delay(_processingInterval, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing outbox messages");
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); // Back off on errors
            }
        }

        _logger.LogInformation("Outbox processor stopped");
    }

    private async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

        // Also clean up expired idempotency keys periodically
        await CleanupExpiredIdempotencyKeysAsync(scope.ServiceProvider, cancellationToken);

        var messages = await outboxRepository.GetPendingAsync(batchSize: 10, cancellationToken);

        // Process messages in sequence order to maintain event ordering
        foreach (var message in messages.OrderBy(m => m.SequenceNumber))
        {
            try
            {
                // Check if message is ready for retry (based on exponential backoff)
                if (message.Status == OutboxMessageStatus.Failed &&
                    !IsReadyForRetry(message))
                {
                    continue; // Skip this message for now
                }

                message.MarkAsProcessing();
                await outboxRepository.UpdateAsync(message, cancellationToken);

                await ProcessMessageAsync(message, scope.ServiceProvider, cancellationToken);

                message.MarkAsProcessed();
                await outboxRepository.UpdateAsync(message, cancellationToken);

                _logger.LogInformation("Processed outbox message {MessageId} of type {MessageType} (seq: {Sequence})",
                    message.Id, message.Type, message.SequenceNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process outbox message {MessageId}", message.Id);
                message.MarkAsFailed(ex.Message);
                await outboxRepository.UpdateAsync(message, cancellationToken);
            }
        }
    }

    private bool IsReadyForRetry(OutboxMessage message)
    {
        if (!message.CanRetry()) return false;

        var retryDelay = message.GetNextRetryDelay();
        var timeSinceLastFailure = DateTimeOffset.UtcNow - (message.ProcessedOn ?? message.CreatedOn);
        return timeSinceLastFailure >= retryDelay;
    }

    private async Task CleanupExpiredIdempotencyKeysAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        try
        {
            var dbContext = serviceProvider.GetRequiredService<BillingDbContext>();
            var expiredKeys = dbContext.IdempotencyKeys.Where(k => k.ExpiresOn < DateTimeOffset.UtcNow);
            dbContext.IdempotencyKeys.RemoveRange(expiredKeys);
            await dbContext.SaveChangesAsync(cancellationToken);

            if (expiredKeys.Any())
            {
                _logger.LogInformation("Cleaned up {Count} expired idempotency keys", expiredKeys.Count());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cleanup expired idempotency keys");
        }
    }

    private async Task ProcessMessageAsync(OutboxMessage message, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        // For now, just log the message. In a real implementation, this would publish to a message queue
        // or trigger domain events based on the message type.

        switch (message.Type)
        {
            case "UsageRecorded":
                await ProcessUsageRecordedEventAsync(message.Content, serviceProvider, cancellationToken);
                break;
            default:
                _logger.LogWarning("Unknown message type: {MessageType}", message.Type);
                break;
        }
    }

    private Task ProcessUsageRecordedEventAsync(string content, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        // Parse the usage event and potentially trigger additional processing
        var usageEvent = JsonSerializer.Deserialize<UsageEventData>(content);
        if (usageEvent != null)
        {
            _logger.LogInformation("Processing usage event for subscription {SubscriptionId}: {Quantity} units at {OccurredAt}",
                usageEvent.SubscriptionId, usageEvent.Quantity, usageEvent.OccurredAt);
        }

        return Task.CompletedTask;
    }

    private record UsageEventData(Guid SubscriptionId, int Quantity, DateTimeOffset OccurredAt);
}