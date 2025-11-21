using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Movies.Application.Database;

public interface IMigrationRunner
{
    Task MigrateAsync(CancellationToken cancellationToken = default);
    Task<MigrationInfo> GetMigrationInfoAsync(CancellationToken cancellationToken = default);
    Task<bool> HasPendingMigrationsAsync(CancellationToken cancellationToken = default);
}

public class FluentMigrationRunner : IMigrationRunner
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<FluentMigrationRunner> _logger;

    public FluentMigrationRunner(IServiceProvider serviceProvider, ILogger<FluentMigrationRunner> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<FluentMigrator.Runner.IMigrationRunner>();
            var versionLoader = scope.ServiceProvider.GetRequiredService<IVersionLoader>();
            
            var currentVersion = versionLoader.VersionInfo.Latest();
            _logger.LogInformation("Current database version: {Version}", currentVersion);

            var pendingMigrations = runner.MigrationLoader.LoadMigrations()
                .Where(m => m.Key > currentVersion)
                .ToList();

            if (pendingMigrations.Any())
            {
                _logger.LogInformation("Found {Count} pending migration(s)", pendingMigrations.Count);
                foreach (var migration in pendingMigrations)
                {
                    _logger.LogInformation("  - Migration {Version}: {Description}", 
                        migration.Key, migration.Value.Description);
                }
            }
            else
            {
                _logger.LogInformation("No pending migrations. Database is up to date.");
                return Task.CompletedTask;
            }

            _logger.LogInformation("Applying database migrations...");
            runner.MigrateUp();
            
            var newVersion = versionLoader.VersionInfo.Latest();
            _logger.LogInformation("Database migrations completed successfully. New version: {Version}", newVersion);
            
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running database migrations");
            throw new MigrationException("Failed to apply database migrations", ex);
        }
    }

    public Task<MigrationInfo> GetMigrationInfoAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<FluentMigrator.Runner.IMigrationRunner>();
            var versionLoader = scope.ServiceProvider.GetRequiredService<IVersionLoader>();
            
            var currentVersion = versionLoader.VersionInfo.Latest();
            var allMigrations = runner.MigrationLoader.LoadMigrations();
            var appliedMigrations = versionLoader.VersionInfo.AppliedMigrations();
            var pendingMigrations = allMigrations
                .Where(m => m.Key > currentVersion)
                .Select(m => new MigrationDetails
                {
                    Version = m.Key,
                    Description = m.Value.Description
                })
                .ToList();

            var info = new MigrationInfo
            {
                CurrentVersion = currentVersion,
                TotalMigrations = allMigrations.Count,
                AppliedMigrations = appliedMigrations.Count(),
                PendingMigrations = pendingMigrations.Count,
                PendingMigrationDetails = pendingMigrations
            };

            return Task.FromResult(info);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting migration info");
            throw;
        }
    }

    public Task<bool> HasPendingMigrationsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<FluentMigrator.Runner.IMigrationRunner>();
            var versionLoader = scope.ServiceProvider.GetRequiredService<IVersionLoader>();
            
            var currentVersion = versionLoader.VersionInfo.Latest();
            var pendingMigrations = runner.MigrationLoader.LoadMigrations()
                .Any(m => m.Key > currentVersion);

            return Task.FromResult(pendingMigrations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking for pending migrations");
            throw;
        }
    }
}

public class MigrationException : Exception
{
    public MigrationException(string message) : base(message) { }
    public MigrationException(string message, Exception innerException) : base(message, innerException) { }
}

public class MigrationInfo
{
    public long CurrentVersion { get; set; }
    public int TotalMigrations { get; set; }
    public int AppliedMigrations { get; set; }
    public int PendingMigrations { get; set; }
    public List<MigrationDetails> PendingMigrationDetails { get; set; } = new();
}

public class MigrationDetails
{
    public long Version { get; set; }
    public string Description { get; set; } = string.Empty;
}

