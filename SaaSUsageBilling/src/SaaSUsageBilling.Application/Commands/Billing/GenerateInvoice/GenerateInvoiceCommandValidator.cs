using FluentValidation;

namespace SaaSUsageBilling.Application.Commands.Billing.GenerateInvoice;

public class GenerateInvoiceCommandValidator : AbstractValidator<GenerateInvoiceCommand>
{
    public GenerateInvoiceCommandValidator()
    {
        RuleFor(x => x.SubscriptionId).NotEmpty();
        RuleFor(x => x.NowUtc).NotEqual(default(DateTimeOffset));
        RuleFor(x => x.IdempotencyKey).NotEmpty().MaximumLength(100);
    }
}

