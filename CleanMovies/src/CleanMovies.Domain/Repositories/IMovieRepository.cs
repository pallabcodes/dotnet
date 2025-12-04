using CleanMovies.Domain.Entities;
using CleanMovies.Domain.ValueObjects;

namespace CleanMovies.Domain.Repositories;

public interface IMovieRepository
{
    Task<Movie?> GetByIdAsync(MovieId id, CancellationToken cancellationToken = default);
    Task<Movie?> GetBySlugAsync(Slug slug, CancellationToken cancellationToken = default);
    Task AddAsync(Movie movie, CancellationToken cancellationToken = default);
    Task UpdateAsync(Movie movie, CancellationToken cancellationToken = default);
    Task DeleteAsync(Movie movie, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Movie>> ListAsync(int page, int pageSize, string? title, int? year, CancellationToken cancellationToken = default);
    Task<int> CountAsync(string? title, int? year, CancellationToken cancellationToken = default);
}
