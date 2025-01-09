using Microsoft.Extensions.Diagnostics.HealthChecks;

public class RandomHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        int responseTimeInMs = Random.Shared.Next(300);
        if (responseTimeInMs < 100)
            return Task.FromResult(HealthCheckResult.Healthy($"Response time is excellent: {responseTimeInMs}ms"));
        if (responseTimeInMs < 200)
            return Task.FromResult(HealthCheckResult.Degraded($"Response time is acceptable: {responseTimeInMs}ms"));
        return Task.FromResult(HealthCheckResult.Unhealthy($"Response time is too high: {responseTimeInMs}ms"));
    }
}