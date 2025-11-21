using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Auth;

namespace Movies.Api.Configuration;

public static class AuthorizationConfiguration
{
    public static IServiceCollection AddCustomAuthorization(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var apiKey = configuration["ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("API Key is not configured");
        }

        services.AddAuthorization(x =>
        {
            x.AddPolicy(
                AuthConstants.AdminUserPolicyName,
                p => p.AddRequirements(new AdminAuthRequirement(apiKey)));

            x.AddPolicy(
                AuthConstants.TrustedMemberPolicyName,
                p => p.RequireAssertion(c =>
                    c.User.HasClaim(m => m is { Type: AuthConstants.AdminUserClaimName, Value: "true" }) ||
                    c.User.HasClaim(m => m is { Type: AuthConstants.TrustedMemberClaimName, Value: "true" })));
        });

        services.AddScoped<ApiKeyAuthFilter>();

        return services;
    }
}


