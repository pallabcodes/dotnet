using MediatR;
using SaaSUsageBilling.Application.Abstractions;
using SaaSUsageBilling.Domain.Entities;
using System.Text.Json;

namespace SaaSUsageBilling.Application.Commands.SubscriptionManagement.RecordUsage;

public class RecordUsageCommandHandler : IRequestHandler<RecordUsageCommand, Unit>
{
    private readonly ISubscriptionRepository _subscriptions;
    private readonly IUsageEventRepository _usageEvents;
    private readonly IOutboxRepository _outbox;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISequenceGenerator _sequenceGenerator;

    public RecordUsageCommandHandler(
        ISubscriptionRepository subscriptions,
        IUsageEventRepository usageEvents,
        IOutboxRepository outbox,
        IUnitOfWork unitOfWork,
        ISequenceGenerator sequenceGenerator)
    {
        _subscriptions = subscriptions;
        _usageEvents = usageEvents;
        _outbox = outbox;
        _unitOfWork = unitOfWork;
        _sequenceGenerator = sequenceGenerator;
    }

    public async Task<Unit> Handle(RecordUsageCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var subscription = await _subscriptions.GetAsync(request.SubscriptionId, cancellationToken)
                ?? throw new InvalidOperationException("Subscription not found.");

            subscription.RecordUsage(request.Quantity, request.OccurredAt);
            await _subscriptions.UpdateAsync(subscription, cancellationToken);

            var usageEvent = new UsageEvent(subscription.Id, request.Quantity, request.OccurredAt);
            await _usageEvents.AddAsync(usageEvent, cancellationToken);

            // Add to outbox for reliable event publishing
            var sequenceNumber = await _sequenceGenerator.NextAsync("UsageEvents", cancellationToken);
            var eventData = new UsageEventData(subscription.Id, request.Quantity, request.OccurredAt);
            var message = new OutboxMessage("UsageRecorded", JsonSerializer.Serialize(eventData), sequenceNumber);
            await _outbox.AddAsync(message, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return Unit.Value;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    private record UsageEventData(Guid SubscriptionId, int Quantity, DateTimeOffset OccurredAt);
}