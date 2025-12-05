using Asp.Versioning;
using MediatR;
using SaaSUsageBilling.Api.Models.V1;
using SaaSUsageBilling.Api.Models;
using SaaSUsageBilling.Api.Observability;
using SaaSUsageBilling.Application.Commands.CustomerManagement.RegisterCustomer;

namespace SaaSUsageBilling.Api.Endpoints.V1;

/// <summary>
/// Customer management endpoints (v1.0)
/// </summary>
public static class CustomerEndpoints
{
    /// <summary>
    /// Maps customer endpoints to the application
    /// </summary>
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        app.MapPost("/api/v1/customers", RegisterCustomer)
            .WithTags("Customers")
            .Produces<RegisterCustomerResponse>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status409Conflict)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Register a new customer",
                Description = "Creates a new customer account in the billing system."
            });
    }

    private static async Task<IResult> RegisterCustomer(
        RegisterCustomerRequest request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCustomerCommand(request.Name, request.Email);
        var customerId = await mediator.Send(command, cancellationToken);

        BillingMetrics.Requests.Add(1, new KeyValuePair<string, object?>("route", "customers.register"));

        var response = new RegisterCustomerResponse
        {
            CustomerId = customerId,
            Name = request.Name,
            Email = request.Email,
            CreatedAt = DateTimeOffset.UtcNow
        };

        return Results.Created($"/api/v1/customers/{customerId}", response);
    }
}
