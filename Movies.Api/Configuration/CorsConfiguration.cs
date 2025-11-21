using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Movies.Api.Configuration;

public static class CorsConfiguration
{
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var corsOptions = new CorsOptions();
        configuration.GetSection("Cors").Bind(corsOptions);

        if (!corsOptions.Enabled)
        {
            return services;
        }

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                if (corsOptions.AllowedOrigins?.Length > 0)
                {
                    policy.WithOrigins(corsOptions.AllowedOrigins);
                }
                else
                {
                    policy.AllowAnyOrigin();
                }

                if (corsOptions.AllowedMethods?.Length > 0)
                {
                    policy.WithMethods(corsOptions.AllowedMethods);
                }
                else
                {
                    policy.AllowAnyMethod();
                }

                if (corsOptions.AllowedHeaders?.Length > 0)
                {
                    policy.WithHeaders(corsOptions.AllowedHeaders);
                }
                else
                {
                    policy.AllowAnyHeader();
                }

                if (corsOptions.AllowCredentials)
                {
                    policy.AllowCredentials();
                }

                if (corsOptions.MaxAgeSeconds > 0)
                {
                    policy.SetPreflightMaxAge(TimeSpan.FromSeconds(corsOptions.MaxAgeSeconds));
                }
            });
        });

        return services;
    }
}

public class CorsOptions
{
    public bool Enabled { get; set; } = false;
    public string[]? AllowedOrigins { get; set; }
    public string[]? AllowedMethods { get; set; }
    public string[]? AllowedHeaders { get; set; }
    public bool AllowCredentials { get; set; } = false;
    public int MaxAgeSeconds { get; set; } = 86400;
}

