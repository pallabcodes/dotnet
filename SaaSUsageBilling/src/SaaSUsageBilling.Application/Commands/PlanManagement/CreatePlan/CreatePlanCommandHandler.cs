using MediatR;
using SaaSUsageBilling.Application.Abstractions;
using SaaSUsageBilling.Domain.Entities;
using SaaSUsageBilling.Domain.ValueObjects;

namespace SaaSUsageBilling.Application.Commands.PlanManagement.CreatePlan;

public class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand, Guid>
{
    private readonly IPlanRepository _plans;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePlanCommandHandler(IPlanRepository plans, IUnitOfWork unitOfWork)
    {
        _plans = plans;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = new Plan(
            request.Name,
            request.Description ?? $"{request.Name} plan",
            new Money(request.MonthlyBase));

        // Add basic pricing tier for backwards compatibility
        if (request.IncludedUnits > 0 && request.PricePerUnit > 0)
        {
            plan.AddPricingTier(new PricingTier(0, request.IncludedUnits, Money.Zero()));
            plan.AddPricingTier(new PricingTier(request.IncludedUnits, null, new Money(request.PricePerUnit)));
        }

        await _plans.AddAsync(plan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return plan.Id;
    }
}