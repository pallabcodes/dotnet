using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaaSUsageBilling.Application.Abstractions;
using SaaSUsageBilling.Infrastructure.Persistence;
using SaaSUsageBilling.Infrastructure.Persistence.InMemory;
using SaaSUsageBilling.Infrastructure.Persistence.Repositories;

namespace SaaSUsageBilling.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase", false);

        if (useInMemory)
        {
            // Use in-memory stores for testing
            services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();
            services.AddSingleton<IPlanRepository, InMemoryPlanRepository>();
            services.AddSingleton<ISubscriptionRepository, InMemorySubscriptionRepository>();
            services.AddSingleton<IUsageEventRepository, InMemoryUsageEventRepository>();
            services.AddSingleton<IInvoiceRepository, InMemoryInvoiceRepository>();
            services.AddSingleton<IIdempotencyStore, InMemoryIdempotencyStore>();
        }
        else
        {
            // Use EF Core for production
            services.AddDbContext<BillingDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("BillingDatabase")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<ISequenceGenerator, SequenceGenerator>();
            services.AddScoped<ICustomerRepository, EfCustomerRepository>();
            services.AddScoped<IPlanRepository, EfPlanRepository>();
            services.AddScoped<ISubscriptionRepository, EfSubscriptionRepository>();
            services.AddScoped<IUsageEventRepository, EfUsageEventRepository>();
            services.AddScoped<IInvoiceRepository, EfInvoiceRepository>();
            services.AddScoped<IOutboxRepository, EfOutboxRepository>();
            services.AddScoped<IIdempotencyStore, EfIdempotencyStore>();
        }

        return services;
    }
}
