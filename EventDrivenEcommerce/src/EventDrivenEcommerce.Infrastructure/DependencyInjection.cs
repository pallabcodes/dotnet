using EventDrivenEcommerce.Application.Common;
using EventDrivenEcommerce.Domain.Repositories;
using EventDrivenEcommerce.Infrastructure.Messaging;
using EventDrivenEcommerce.Infrastructure.Persistence;
using EventDrivenEcommerce.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventDrivenEcommerce.Infrastructure;

/// <summary>
/// Dependency injection extensions for infrastructure layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment? environment = null)
    {
        var isTesting = string.Equals(environment?.EnvironmentName, "Testing", StringComparison.OrdinalIgnoreCase);

        // Database
        if (isTesting)
        {
            services.AddDbContext<EcommerceDbContext>(options =>
                options.UseInMemoryDatabase("EventDrivenEcommerce"));
        }
        else
        {
            services.AddDbContext<EcommerceDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default")));
        }

        // Repositories & UoW
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<EcommerceDbContext>());
        services.AddScoped<IOutboxRepository, OutboxRepository>();

        // Messaging
        if (isTesting)
        {
            services.AddSingleton<IEventPublisher, NoOpEventPublisher>();
        }
        else
        {
            var rabbitMqSettings = configuration.GetSection("RabbitMQ").Get<RabbitMqSettings>() ?? new RabbitMqSettings();
            services.AddSingleton(rabbitMqSettings);
            services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
            services.AddHostedService<OutboxProcessor>();
        }

        return services;
    }
}
