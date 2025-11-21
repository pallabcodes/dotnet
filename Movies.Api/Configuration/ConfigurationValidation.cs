using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Movies.Api.Configuration;

public class DatabaseOptions
{
    public const string SectionName = "Database";
    public string ConnectionString { get; set; } = string.Empty;
}

public class JwtOptions
{
    public const string SectionName = "Jwt";
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}

public static class ConfigurationValidation
{
    public static IServiceCollection AddValidatedConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.ConnectionString),
                "Database connection string must be configured")
            .ValidateOnStart();

        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.Key),
                "JWT Key must be configured")
            .Validate(options => !string.IsNullOrWhiteSpace(options.Issuer),
                "JWT Issuer must be configured")
            .Validate(options => !string.IsNullOrWhiteSpace(options.Audience),
                "JWT Audience must be configured")
            .ValidateOnStart();

        var apiKey = configuration["ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("API Key must be configured");
        }

        return services;
    }
}

