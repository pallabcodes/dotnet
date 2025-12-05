using Asp.Versioning;
using MediatR;
using SaaSUsageBilling.Api.Models.V1;
using SaaSUsageBilling.Api.Models;
using SaaSUsageBilling.Api.Observability;
using SaaSUsageBilling.Application.Commands.PlanManagement.CreatePlan;
using SaaSUsageBilling.Domain.ValueObjects;

namespace SaaSUsageBilling.Api.Endpoints.V1;

/// <summary>
/// Plan management endpoints (v1.0)
/// </summary>
public static class PlanEndpoints
{
    /// <summary>
    /// Maps plan endpoints to the application
    /// </summary>
    public static void MapPlanEndpoints(this WebApplication app)
    {
        app.MapPost("/api/v1/plans", CreatePlan)
            .WithTags("Plans")
            .Produces<CreatePlanResponse>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create a new pricing plan",
                Description = "Creates a new pricing plan with tiered pricing, discounts, and taxes."
            });
    }

    private static async Task<IResult> CreatePlan(
        CreatePlanRequest request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new CreatePlanCommand(
            request.Name,
            request.Description ?? $"{request.Name} plan",
            request.MonthlyBase,
            request.IncludedUnits,
            request.PricePerUnit);

        var planId = await mediator.Send(command, cancellationToken);

        BillingMetrics.Requests.Add(1, new KeyValuePair<string, object?>("route", "plans.create"));

        var response = new CreatePlanResponse
        {
            PlanId = planId,
            Name = request.Name,
            MonthlyBase = request.MonthlyBase,
            Currency = request.Currency,
            CreatedAt = DateTimeOffset.UtcNow
        };

        return Results.Created($"/api/v1/plans/{planId}", response);
    }
}
