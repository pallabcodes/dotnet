using MediatR;

namespace SaaSUsageBilling.Application.Commands.SubscriptionManagement.RecordUsage;

public record RecordUsageCommand(
    Guid SubscriptionId,
    int Quantity,
    DateTimeOffset OccurredAt,
    string IdempotencyKey) : IRequest<Unit>;