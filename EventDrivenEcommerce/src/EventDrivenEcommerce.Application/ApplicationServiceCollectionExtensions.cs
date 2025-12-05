using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventDrivenEcommerce.Application;

/// <summary>
/// Dependency injection extensions for application layer.
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining(typeof(ApplicationServiceCollectionExtensions));
        });

        services.AddValidatorsFromAssemblyContaining(typeof(ApplicationServiceCollectionExtensions));

        return services;
    }
}
