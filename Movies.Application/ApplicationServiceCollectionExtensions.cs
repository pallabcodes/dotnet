using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Database;
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

    public static IServiceCollection AddDatabase(this IServiceCollection services, string? connectionString)
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
            ConnectionIdleLifetime = 300
        };

        services.AddSingleton<IDbConnectionFactory>(sp => 
            new NpgSqlConnectionFactory(connectionStringBuilder.ConnectionString, sp.GetRequiredService<ILogger<NpgSqlConnectionFactory>>()));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}
