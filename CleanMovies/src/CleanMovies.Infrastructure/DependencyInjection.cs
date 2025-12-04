using CleanMovies.Application.Abstractions;
using CleanMovies.Domain.Repositories;
using CleanMovies.Infrastructure.Persistence;
using CleanMovies.Infrastructure.Persistence.Repositories;
using CleanMovies.Infrastructure.Caching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMovies.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default") ?? "Server=(localdb)\\mssqllocaldb;Database=CleanMovies;Trusted_Connection=True;";

        services.AddDbContext<MovieDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<MovieDbContext>());
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, MemoryCacheService>();

        return services;
    }
}
