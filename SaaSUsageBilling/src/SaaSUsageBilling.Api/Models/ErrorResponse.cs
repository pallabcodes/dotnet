using System.Text.Json.Serialization;

namespace SaaSUsageBilling.Api.Models;

/// <summary>
/// Standardized error response
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Error type/category
    /// </summary>
    /// <example>validation_error</example>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable error message
    /// </summary>
    /// <example>The request contains invalid data</example>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Detailed error information
    /// </summary>
    public Dictionary<string, string[]>? Details { get; set; }

    /// <summary>
    /// Unique error identifier for tracking
    /// </summary>
    /// <example>err_550e8400-e29b-41d4-a716-446655440000</example>
    public string? ErrorId { get; set; }

    /// <summary>
    /// Request path that caused the error
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// When the error occurred
    /// </summary>
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Validation error details
/// </summary>
public class ValidationErrorDetails
{
    /// <summary>
    /// Field that failed validation
    /// </summary>
    public string Field { get; set; } = string.Empty;

    /// <summary>
    /// Validation error messages
    /// </summary>
    public string[] Errors { get; set; } = Array.Empty<string>();
}

/// <summary>
/// Common error types
/// </summary>
public static class ErrorTypes
{
    public const string ValidationError = "validation_error";
    public const string NotFound = "not_found";
    public const string Conflict = "conflict";
    public const string Unauthorized = "unauthorized";
    public const string Forbidden = "forbidden";
    public const string RateLimited = "rate_limited";
    public const string InternalError = "internal_error";
    public const string ServiceUnavailable = "service_unavailable";
}
