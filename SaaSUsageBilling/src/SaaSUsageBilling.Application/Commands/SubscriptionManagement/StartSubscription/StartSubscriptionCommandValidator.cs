using FluentValidation;

namespace SaaSUsageBilling.Application.Commands.SubscriptionManagement.StartSubscription;

public class StartSubscriptionCommandValidator : AbstractValidator<StartSubscriptionCommand>
{
    public StartSubscriptionCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.PlanId).NotEmpty();
    }
}

