using FluentValidation;

namespace SaaSUsageBilling.Application.Commands.SubscriptionManagement.RecordUsage;

public class RecordUsageCommandValidator : AbstractValidator<RecordUsageCommand>
{
    public RecordUsageCommandValidator()
    {
        RuleFor(x => x.SubscriptionId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0).LessThanOrEqualTo(1_000_000);
        RuleFor(x => x.OccurredAt).LessThanOrEqualTo(DateTimeOffset.UtcNow.AddMinutes(5));
        RuleFor(x => x.IdempotencyKey).NotEmpty().MaximumLength(100);
    }
}

