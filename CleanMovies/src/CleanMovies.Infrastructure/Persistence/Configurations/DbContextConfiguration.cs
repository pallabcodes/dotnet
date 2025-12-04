using CleanMovies.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanMovies.Infrastructure.Persistence;

public static class DbContextConfiguration
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.MovieConfiguration());
    }
}
