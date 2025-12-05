using System.ComponentModel.DataAnnotations;

namespace SaaSUsageBilling.Api.Models.V1;

/// <summary>
/// Request to start a new subscription
/// </summary>
public class StartSubscriptionRequest
{
    /// <summary>
    /// ID of the customer
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440000</example>
    [Required]
    public Guid CustomerId { get; set; }

    /// <summary>
    /// ID of the plan to subscribe to
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440001</example>
    [Required]
    public Guid PlanId { get; set; }
}

/// <summary>
/// Response after starting a subscription
/// </summary>
public class StartSubscriptionResponse
{
    /// <summary>
    /// The unique identifier of the created subscription
    /// </summary>
    public Guid SubscriptionId { get; set; }

    /// <summary>
    /// Customer ID
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Plan ID
    /// </summary>
    public Guid PlanId { get; set; }

    /// <summary>
    /// Subscription status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// When the subscription started
    /// </summary>
    public DateTimeOffset StartedOn { get; set; }
}

/// <summary>
/// Request to record usage
/// </summary>
public class RecordUsageRequest
{
    /// <summary>
    /// ID of the subscription
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440002</example>
    [Required]
    public Guid SubscriptionId { get; set; }

    /// <summary>
    /// Number of units used
    /// </summary>
    /// <example>150</example>
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    /// <summary>
    /// When the usage occurred
    /// </summary>
    /// <example>2024-01-15T10:30:00Z</example>
    [Required]
    public DateTimeOffset OccurredAt { get; set; }

    /// <summary>
    /// Idempotency key for safe retries
    /// </summary>
    /// <example>usage-2024-01-15-001</example>
    [Required]
    [StringLength(100)]
    public string IdempotencyKey { get; set; } = string.Empty;
}

/// <summary>
/// Response after recording usage
/// </summary>
public class RecordUsageResponse
{
    /// <summary>
    /// Whether the usage was recorded successfully
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Current total units for the billing period
    /// </summary>
    public int CurrentPeriodUnits { get; set; }

    /// <summary>
    /// When the usage was recorded
    /// </summary>
    public DateTimeOffset RecordedAt { get; set; }
}
