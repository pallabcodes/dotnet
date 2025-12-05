using System.ComponentModel.DataAnnotations;

namespace SaaSUsageBilling.Api.Models.V1;

/// <summary>
/// Request to create a new pricing plan
/// </summary>
public class CreatePlanRequest
{
    /// <summary>
    /// Plan name
    /// </summary>
    /// <example>Professional Plan</example>
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public required string Name { get; set; }

    /// <summary>
    /// Plan description
    /// </summary>
    /// <example>Perfect for growing businesses</example>
    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Monthly base price
    /// </summary>
    /// <example>99.99</example>
    [Required]
    [Range(0.01, 100000)]
    public decimal MonthlyBase { get; set; }

    /// <summary>
    /// Currency for pricing (ISO 4217)
    /// </summary>
    /// <example>USD</example>
    [Required]
    [StringLength(3, MinimumLength = 3)]
    [RegularExpression(@"^[A-Z]{3}$")]
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Included units before additional charges
    /// </summary>
    /// <example>1000</example>
    [Range(0, int.MaxValue)]
    public int IncludedUnits { get; set; }

    /// <summary>
    /// Price per additional unit
    /// </summary>
    /// <example>0.10</example>
    [Range(0, 10000)]
    public decimal PricePerUnit { get; set; }
}

/// <summary>
/// Response after creating a plan
/// </summary>
public class CreatePlanResponse
{
    /// <summary>
    /// The unique identifier of the created plan
    /// </summary>
    public Guid PlanId { get; set; }

    /// <summary>
    /// Plan name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Monthly base price
    /// </summary>
    public decimal MonthlyBase { get; set; }

    /// <summary>
    /// Currency
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// When the plan was created
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
}
