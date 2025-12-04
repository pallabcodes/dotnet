using CleanMovies.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanMovies.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Movie> Movies { get; }
    DbSet<Rating> Ratings { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
