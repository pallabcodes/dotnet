using CleanMovies.Application.Abstractions;
using CleanMovies.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanMovies.Infrastructure.Persistence;

public sealed class MovieDbContext : DbContext, IApplicationDbContext
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Rating> Ratings => Set<Rating>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DbContextConfiguration.Configure(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}
