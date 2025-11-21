using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Movies.Api.Configuration;

public static class ApiVersioningConfiguration
{
    public static IServiceCollection AddApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(x =>
        {
            x.DefaultApiVersion = new ApiVersion(1.0);
            x.AssumeDefaultVersionWhenUnspecified = true;
            x.ReportApiVersions = true;
            x.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
        }).AddApiExplorer();

        services.AddEndpointsApiExplorer();

        return services;
    }
}


