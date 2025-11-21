using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace Movies.Api.Configuration;

public static class RateLimitingConfiguration
{
    public const string GlobalPolicy = "GlobalPolicy";
    public const string AuthenticatedPolicy = "AuthenticatedPolicy";
    public const string AdminPolicy = "AdminPolicy";

    public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        var rateLimitOptions = new RateLimitOptions();
        configuration.GetSection("RateLimiting").Bind(rateLimitOptions);

        services.AddRateLimiter(options =>
        {
            options.AddPolicy(GlobalPolicy, context =>
            {
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = rateLimitOptions.GlobalPermitLimit,
                        Window = TimeSpan.FromSeconds(rateLimitOptions.GlobalWindowSeconds),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = rateLimitOptions.GlobalQueueLimit
                    });
            });

            options.AddPolicy(AuthenticatedPolicy, context =>
            {
                var userId = GetUserIdFromClaims(context) 
                    ?? context.Connection.RemoteIpAddress?.ToString() 
                    ?? "unknown";
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: userId,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = rateLimitOptions.AuthenticatedPermitLimit,
                        Window = TimeSpan.FromSeconds(rateLimitOptions.AuthenticatedWindowSeconds),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = rateLimitOptions.AuthenticatedQueueLimit
                    });
            });

            options.AddPolicy(AdminPolicy, context =>
            {
                var userId = GetUserIdFromClaims(context) 
                    ?? context.Connection.RemoteIpAddress?.ToString() 
                    ?? "unknown";
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: userId,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = rateLimitOptions.AdminPermitLimit,
                        Window = TimeSpan.FromSeconds(rateLimitOptions.AdminWindowSeconds),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = rateLimitOptions.AdminQueueLimit
                    });
            });

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsJsonAsync(new
                {
                    error = "Rate limit exceeded",
                    message = "Too many requests. Please try again later.",
                    retryAfter = context.RetryAfter?.TotalSeconds
                }, cancellationToken: token);
            };
        });

        return services;
    }

    private static string? GetUserIdFromClaims(HttpContext context)
    {
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        return context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? context.User.FindFirst("sub")?.Value
            ?? context.User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
    }
}

public class RateLimitOptions
{
    public int GlobalPermitLimit { get; set; } = 100;
    public int GlobalWindowSeconds { get; set; } = 60;
    public int GlobalQueueLimit { get; set; } = 10;

    public int AuthenticatedPermitLimit { get; set; } = 200;
    public int AuthenticatedWindowSeconds { get; set; } = 60;
    public int AuthenticatedQueueLimit { get; set; } = 20;

    public int AdminPermitLimit { get; set; } = 500;
    public int AdminWindowSeconds { get; set; } = 60;
    public int AdminQueueLimit { get; set; } = 50;
}

