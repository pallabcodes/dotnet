using MediatR;

namespace SaaSUsageBilling.Application.Commands.SubscriptionManagement.StartSubscription;

public record StartSubscriptionCommand(Guid CustomerId, Guid PlanId) : IRequest<Guid>;