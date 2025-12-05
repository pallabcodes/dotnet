using MediatR;
using SaaSUsageBilling.Application.Abstractions;
using SaaSUsageBilling.Domain.Entities;
using SaaSUsageBilling.Domain.ValueObjects;

namespace SaaSUsageBilling.Application.Commands.SubscriptionManagement.StartSubscription;

public class StartSubscriptionCommandHandler : IRequestHandler<StartSubscriptionCommand, Guid>
{
    private readonly ICustomerRepository _customers;
    private readonly IPlanRepository _plans;
    private readonly ISubscriptionRepository _subscriptions;
    private readonly IUnitOfWork _unitOfWork;

    public StartSubscriptionCommandHandler(
        ICustomerRepository customers,
        IPlanRepository plans,
        ISubscriptionRepository subscriptions,
        IUnitOfWork unitOfWork)
    {
        _customers = customers;
        _plans = plans;
        _subscriptions = subscriptions;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(StartSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customers.GetAsync(request.CustomerId, cancellationToken)
            ?? throw new InvalidOperationException("Customer not found.");
        var plan = await _plans.GetAsync(request.PlanId, cancellationToken)
            ?? throw new InvalidOperationException("Plan not found.");

        var period = Period.CurrentMonthUtc();
        var subscription = new Subscription(customer.Id, plan.Id, DateTimeOffset.UtcNow, period);
        await _subscriptions.AddAsync(subscription, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return subscription.Id;
    }
}

