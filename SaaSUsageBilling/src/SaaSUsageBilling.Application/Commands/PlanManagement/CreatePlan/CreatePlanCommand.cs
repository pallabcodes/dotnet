using MediatR;

namespace SaaSUsageBilling.Application.Commands.PlanManagement.CreatePlan;

public record CreatePlanCommand(string Name, string? Description, decimal MonthlyBase, int IncludedUnits, decimal PricePerUnit) : IRequest<Guid>;