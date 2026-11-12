using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Abstractions;
using Template.Application.Abstractions;
using Template.Infrastructure.Persistence;
using Template.Infrastructure.Persistence.Repositories;

namespace Template.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TemplateDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("TemplateDatabase")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProjectRepository, ProjectRepository>();

        return services;
    }
}
