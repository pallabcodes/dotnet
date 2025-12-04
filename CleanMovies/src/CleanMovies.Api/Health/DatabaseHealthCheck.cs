using Microsoft.Extensions.Diagnostics.HealthChecks;
using CleanMovies.Application.Abstractions;

namespace CleanMovies.Api.Health;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<DatabaseHealthCheck> _logger;

    public DatabaseHealthCheck(IApplicationDbContext dbContext, ILogger<DatabaseHealthCheck> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // Simple database connectivity check
            var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);

            if (canConnect)
            {
                _logger.LogDebug("Database health check passed");
                return HealthCheckResult.Healthy("Database is healthy");
            }
            else
            {
                _logger.LogWarning("Database health check failed: Cannot connect to database");
                return HealthCheckResult.Unhealthy("Cannot connect to database");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed with exception");
            return HealthCheckResult.Unhealthy("Database check failed", ex);
        }
    }
}

