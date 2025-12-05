using System.ComponentModel.DataAnnotations;

namespace SaaSUsageBilling.Api.Models.V1;

/// <summary>
/// Request to register a new customer
/// </summary>
public class RegisterCustomerRequest
{
    /// <summary>
    /// Customer's full name
    /// </summary>
    /// <example>John Doe</example>
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public required string Name { get; set; }

    /// <summary>
    /// Customer's email address
    /// </summary>
    /// <example>john.doe@example.com</example>
    [Required]
    [EmailAddress]
    [StringLength(200)]
    public required string Email { get; set; }
}

/// <summary>
/// Response after registering a customer
/// </summary>
public class RegisterCustomerResponse
{
    /// <summary>
    /// The unique identifier of the created customer
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440000</example>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// The customer's name
    /// </summary>
    /// <example>John Doe</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The customer's email
    /// </summary>
    /// <example>john.doe@example.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// When the customer was created
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
}
