using System.Text.Json.Serialization;

namespace Template.Api.Models;

public class ErrorResponse
{
    public string Type { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public Dictionary<string, string[]>? Details { get; set; }

    public string? ErrorId { get; set; }

    public string? Path { get; set; }

    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
}

public class ValidationErrorDetails
{
    public string Field { get; set; } = string.Empty;

    public string[] Errors { get; set; } = Array.Empty<string>();
}

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
