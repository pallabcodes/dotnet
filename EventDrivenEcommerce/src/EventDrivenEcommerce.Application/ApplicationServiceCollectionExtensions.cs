using EventDrivenEcommerce.Application.EventHandlers;
using FluentValidation;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace EventDrivenEcommerce.Application;

/// <summary>
/// Dependency injection extensions for application layer.
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining(typeof(ApplicationServiceCollectionExtensions));
        });

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining(typeof(ApplicationServiceCollectionExtensions));

        // Event Handlers
        services.AddTransient<DomainEventPublisher>();
        services.AddTransient<INotificationHandler<DomainEventsPublishedNotification>, DomainEventPublisher>();

        return services;
    }
}

