using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Middleware;

namespace Movies.Api.Configuration;

public static class SecurityHeadersConfiguration
{
    public static IServiceCollection AddSecurityHeaders(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SecurityHeadersOptions>(configuration.GetSection("SecurityHeaders"));
        return services;
    }
}

