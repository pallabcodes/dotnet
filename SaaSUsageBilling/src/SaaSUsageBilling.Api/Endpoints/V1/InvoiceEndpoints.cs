using Asp.Versioning;
using MediatR;
using SaaSUsageBilling.Api.Models.V1;
using SaaSUsageBilling.Api.Models;
using SaaSUsageBilling.Api.Observability;
using SaaSUsageBilling.Application.Commands.Billing.GenerateInvoice;

namespace SaaSUsageBilling.Api.Endpoints.V1;

/// <summary>
/// Invoice management endpoints (v1.0)
/// </summary>
public static class InvoiceEndpoints
{
    /// <summary>
    /// Maps invoice endpoints to the application
    /// </summary>
    public static void MapInvoiceEndpoints(this WebApplication app)
    {
        app.MapPost("/api/v1/invoices/generate", GenerateInvoice)
            .WithTags("Invoices")
            .Produces<GenerateInvoiceResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Generate an invoice",
                Description = "Generates a new invoice for the current billing period of a subscription."
            });
    }

    private static async Task<IResult> GenerateInvoice(
        GenerateInvoiceRequest request,
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new GenerateInvoiceCommand(
            request.SubscriptionId,
            DateTimeOffset.UtcNow,
            request.IdempotencyKey);

        var invoice = await mediator.Send(command, cancellationToken);

        BillingMetrics.InvoicesGenerated.Add(1, new KeyValuePair<string, object?>("route", "invoices.generate"));

        var response = new GenerateInvoiceResponse
        {
            InvoiceId = invoice.Id,
            SubscriptionId = invoice.SubscriptionId,
            Period = new BillingPeriod
            {
                From = invoice.Period.From,
                To = invoice.Period.To
            },
            Status = invoice.Status.ToString(),
            Total = new Money
            {
                Amount = invoice.Total.Amount,
                Currency = invoice.Total.Currency
            },
            Lines = invoice.Lines.Select(l => new InvoiceLineItem
            {
                Description = l.Description,
                Amount = new Money
                {
                    Amount = l.Amount.Amount,
                    Currency = l.Amount.Currency
                },
                Category = l.Category
            }).ToList(),
            IssuedOn = invoice.IssuedOn,
            DueOn = invoice.DueOn
        };

        return Results.Ok(response);
    }
}
