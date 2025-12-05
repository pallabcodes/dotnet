using MediatR;
using SaaSUsageBilling.Domain.Entities;

namespace SaaSUsageBilling.Application.Commands.Billing.GenerateInvoice;

public record GenerateInvoiceCommand(
    Guid SubscriptionId,
    DateTimeOffset NowUtc,
    string IdempotencyKey) : IRequest<Invoice>;