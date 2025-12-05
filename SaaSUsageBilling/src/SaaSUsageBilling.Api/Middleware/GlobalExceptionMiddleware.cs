using Microsoft.AspNetCore.Mvc;
using SaaSUsageBilling.Api.Models;
using System.Net;
using System.Text.Json;
using FluentValidation;

namespace SaaSUsageBilling.Api.Middleware;

/// <summary>
/// Global exception handling middleware
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var errorId = Guid.NewGuid().ToString();
        var errorResponse = new ErrorResponse
        {
            ErrorId = errorId,
            Path = context.Request.Path,
            Timestamp = DateTimeOffset.UtcNow
        };

        (HttpStatusCode statusCode, string type, string message) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                ErrorTypes.ValidationError,
                "The request contains invalid data"
            ),
            ArgumentException => (
                HttpStatusCode.BadRequest,
                ErrorTypes.ValidationError,
                exception.Message
            ),
            InvalidOperationException => (
                HttpStatusCode.Conflict,
                ErrorTypes.Conflict,
                exception.Message
            ),
            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                ErrorTypes.NotFound,
                "The requested resource was not found"
            ),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                ErrorTypes.Unauthorized,
                "Authentication required"
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                ErrorTypes.InternalError,
                "An unexpected error occurred"
            )
        };

        errorResponse.Type = type;
        errorResponse.Message = message;

        // Add validation details for validation exceptions
        if (exception is ValidationException vex)
        {
            errorResponse.Details = vex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        }

        // Log the full exception details
        _logger.LogError(exception,
            "Error {ErrorId}: {Message} at {Path}",
            errorId, exception.Message, context.Request.Path);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}

/// <summary>
/// Extension methods for global exception middleware
/// </summary>
public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
