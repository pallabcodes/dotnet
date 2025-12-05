using System.Diagnostics;
using Microsoft.Extensions.Primitives;

namespace SaaSUsageBilling.Api.Middleware;

/// <summary>
/// Adds correlation IDs and logs basic request/response telemetry.
/// </summary>
public class RequestLoggingMiddleware
{
    private const string CorrelationHeader = "X-Correlation-ID";
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = EnsureCorrelationId(context);
        using var scope = _logger.BeginScope(new Dictionary<string, object?>
        {
            ["CorrelationId"] = correlationId,
            ["Path"] = context.Request.Path,
            ["Method"] = context.Request.Method
        });

        var stopwatch = Stopwatch.StartNew();
        try
        {
            await _next(context);
            stopwatch.Stop();

            _logger.LogInformation("Request completed {StatusCode} in {ElapsedMs}ms",
                context.Response.StatusCode,
                stopwatch.Elapsed.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Request failed {StatusCode} in {ElapsedMs}ms",
                context.Response.StatusCode,
                stopwatch.Elapsed.TotalMilliseconds);
            throw;
        }
    }

    private static string EnsureCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(CorrelationHeader, out StringValues headerValues) &&
            !StringValues.IsNullOrEmpty(headerValues))
        {
            context.Response.Headers[CorrelationHeader] = headerValues;
            return headerValues.First()!;
        }

        var generated = Guid.NewGuid().ToString();
        context.Request.Headers[CorrelationHeader] = generated;
        context.Response.Headers[CorrelationHeader] = generated;
        return generated;
    }
}

public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app) =>
        app.UseMiddleware<RequestLoggingMiddleware>();
}

