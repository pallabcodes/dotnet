using Microsoft.Extensions.DependencyInjection;

namespace Movies.Api.Configuration;

public static class CacheConfiguration
{
    public static IServiceCollection AddOutputCaching(this IServiceCollection services)
    {
        services.AddOutputCache(x =>
        {
            x.AddBasePolicy(c => c.Cache());
            x.AddPolicy("MovieCache", c =>
                c.Cache()
                    .Expire(TimeSpan.FromMinutes(1))
                    .SetVaryByQuery(new[] { "title", "year", "sortBy", "page", "pageSize" })
                    .Tag("movies"));
        });

        return services;
    }
}


