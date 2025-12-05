using Asp.Versioning;
using MediatR;
using SaaSUsageBilling.Api.Models.V1;
using SaaSUsageBilling.Api.Models;
using SaaSUsageBilling.Api.Observability;
using SaaSUsageBilling.Application.Commands.SubscriptionManagement.StartSubscription;
using SaaSUsageBilling.Application.Commands.SubscriptionManagement.RecordUsage;

namespace SaaSUsageBilling.Api.Endpoints.V1;

/// <summary>
/// Subscription management endpoints (v1.0)
/// </summary>
public static class SubscriptionEndpoints
{
    /// <summary>
    /// Maps subscription endpoints to the application
    /// </summary>
    public static void MapSubscriptionEndpoints(this WebApplication app)
    {
        app.MapPost("/api/v1/subscriptions", StartSubscription)
            .WithTags("Subscriptions")
            .Produces<StartSubscriptionResponse>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Start a new subscription",
                Description = "Creates a new subscription for a customer to a pricing plan."
            });

        app.MapPost("/api/v1/subscriptions/{subscriptionId}/usage", RecordUsage)
            .WithTags("Subscriptions")
            .Produces<RecordUsageResponse>(StatusCodes.Status202Accepted)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Record usage for a subscription",
                Description = "Records usage events for billing purposes. Supports idempotent operations."
            });
    }

    private static async Task<IResult> StartSubscription(
        StartSubscriptionRequest request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new StartSubscriptionCommand(request.CustomerId, request.PlanId);
        var subscriptionId = await mediator.Send(command, cancellationToken);

        BillingMetrics.Requests.Add(1, new KeyValuePair<string, object?>("route", "subscriptions.start"));

        var response = new StartSubscriptionResponse
        {
            SubscriptionId = subscriptionId,
            CustomerId = request.CustomerId,
            PlanId = request.PlanId,
            Status = "Active",
            StartedOn = DateTimeOffset.UtcNow
        };

        return Results.Created($"/api/v1/subscriptions/{subscriptionId}", response);
    }

    private static async Task<IResult> RecordUsage(
        Guid subscriptionId,
        RecordUsageRequest request,
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        // Validate subscription ID matches
        if (subscriptionId != request.SubscriptionId)
        {
            return Results.BadRequest(new ErrorResponse
            {
                Type = ErrorTypes.ValidationError,
                Message = "Subscription ID in path does not match request body"
            });
        }

        var command = new RecordUsageCommand(
            request.SubscriptionId,
            request.Quantity,
            request.OccurredAt,
            request.IdempotencyKey);

        await mediator.Send(command, cancellationToken);

        BillingMetrics.UsageRecorded.Add(1, new KeyValuePair<string, object?>("route", "subscriptions.usage"));

        var response = new RecordUsageResponse
        {
            Success = true,
            CurrentPeriodUnits = 0, // Would need to query this from the subscription
            RecordedAt = DateTimeOffset.UtcNow
        };

        return Results.Accepted(string.Empty, response);
    }
}
