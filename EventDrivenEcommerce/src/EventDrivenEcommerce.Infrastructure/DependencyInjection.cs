using EventDrivenEcommerce.Application.Common;
using EventDrivenEcommerce.Domain.Repositories;
using EventDrivenEcommerce.Infrastructure.Messaging;
using EventDrivenEcommerce.Infrastructure.Persistence;
using EventDrivenEcommerce.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventDrivenEcommerce.Infrastructure;

/// <summary>
/// Dependency injection extensions for infrastructure layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<EcommerceDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        // Repositories
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<EcommerceDbContext>());
        services.AddScoped<IOutboxRepository, OutboxRepository>();

        // Messaging
        var rabbitMqSettings = configuration.GetSection("RabbitMQ").Get<RabbitMqSettings>() ?? new RabbitMqSettings();
        services.AddSingleton(rabbitMqSettings);
        services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
        services.AddHostedService<OutboxProcessor>();

        return services;
    }
}

