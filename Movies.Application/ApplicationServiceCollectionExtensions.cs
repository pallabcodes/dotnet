using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Movies.Application.Database;
using Movies.Application.Database.Migrations;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Npgsql;

namespace Movies.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRatingRepository, RatingRepository>();
        services.AddScoped<IRatingService, RatingService>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Scoped);

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string? connectionString, IConfiguration? configuration = null)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Database connection string cannot be null or empty", nameof(connectionString));
        }

        var connectionStringBuilder = new Npgsql.NpgsqlConnectionStringBuilder(connectionString)
        {
            Pooling = true,
            MinPoolSize = 0,
            MaxPoolSize = 100,
            ConnectionIdleLifetime = 300,
            CommandTimeout = configuration?.GetValue<int>($"{MigrationOptions.SectionName}:TimeoutSeconds") ?? 300
        };

        services.AddSingleton<IDbConnectionFactory>(sp => 
            new NpgSqlConnectionFactory(connectionStringBuilder.ConnectionString, sp.GetRequiredService<ILogger<NpgSqlConnectionFactory>>()));
        services.AddSingleton<DbInitializer>();

        var migrationOptions = new MigrationOptions();
        if (configuration != null)
        {
            configuration.GetSection(MigrationOptions.SectionName).Bind(migrationOptions);
        }
        services.AddSingleton(migrationOptions);
        
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionStringBuilder.ConnectionString)
                .WithVersionTable(new FluentMigrator.Runner.Initialization.VersionTableMetaData
                {
                    SchemaName = migrationOptions.VersionTableSchema,
                    TableName = migrationOptions.VersionTableName
                })
                .ScanIn(typeof(InitialSchema).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole()
                .SetMinimumLevel(LogLevel.Information));
        
        services.AddSingleton<IMigrationRunner, FluentMigrationRunner>();
        
        return services;
    }
}
