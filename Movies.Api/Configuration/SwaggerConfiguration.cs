using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Movies.Api.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(x => x.OperationFilter<SwaggerDefaultValues>());

        return services;
    }
}


