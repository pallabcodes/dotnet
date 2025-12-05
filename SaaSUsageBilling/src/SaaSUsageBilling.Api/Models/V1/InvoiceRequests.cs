using System.ComponentModel.DataAnnotations;

namespace SaaSUsageBilling.Api.Models.V1;

/// <summary>
/// Request to generate an invoice
/// </summary>
public class GenerateInvoiceRequest
{
    /// <summary>
    /// ID of the subscription to invoice
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440002</example>
    [Required]
    public Guid SubscriptionId { get; set; }

    /// <summary>
    /// Idempotency key for safe retries
    /// </summary>
    /// <example>invoice-2024-01-001</example>
    [Required]
    [StringLength(100)]
    public string IdempotencyKey { get; set; } = string.Empty;
}

/// <summary>
/// Response after generating an invoice
/// </summary>
public class GenerateInvoiceResponse
{
    /// <summary>
    /// The unique identifier of the generated invoice
    /// </summary>
    public Guid InvoiceId { get; set; }

    /// <summary>
    /// Subscription ID
    /// </summary>
    public Guid SubscriptionId { get; set; }

    /// <summary>
    /// Billing period
    /// </summary>
    public BillingPeriod Period { get; set; } = new();

    /// <summary>
    /// Invoice status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Total amount due
    /// </summary>
    public Money Total { get; set; } = new();

    /// <summary>
    /// Invoice line items
    /// </summary>
    public List<InvoiceLineItem> Lines { get; set; } = new();

    /// <summary>
    /// When the invoice was issued
    /// </summary>
    public DateTimeOffset IssuedOn { get; set; }

    /// <summary>
    /// When payment is due
    /// </summary>
    public DateTimeOffset DueOn { get; set; }
}

/// <summary>
/// Billing period information
/// </summary>
public class BillingPeriod
{
    /// <summary>
    /// Period start date
    /// </summary>
    public DateTimeOffset From { get; set; }

    /// <summary>
    /// Period end date
    /// </summary>
    public DateTimeOffset To { get; set; }
}

/// <summary>
/// Money amount with currency
/// </summary>
public class Money
{
    /// <summary>
    /// Amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Currency code (ISO 4217)
    /// </summary>
    public string Currency { get; set; } = "USD";
}

/// <summary>
/// Invoice line item
/// </summary>
public class InvoiceLineItem
{
    /// <summary>
    /// Line description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Line amount
    /// </summary>
    public Money Amount { get; set; } = new();

    /// <summary>
    /// Line category
    /// </summary>
    public string? Category { get; set; }
}
