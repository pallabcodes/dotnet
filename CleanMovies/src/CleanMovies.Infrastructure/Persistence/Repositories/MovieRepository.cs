using CleanMovies.Domain.Entities;
using CleanMovies.Domain.Repositories;
using CleanMovies.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CleanMovies.Infrastructure.Persistence.Repositories;

public sealed class MovieRepository : IMovieRepository
{
    private readonly MovieDbContext _db;

    public MovieRepository(MovieDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        await _db.Movies.AddAsync(movie, cancellationToken);
    }

    public async Task<int> CountAsync(string? title, int? year, CancellationToken cancellationToken = default)
    {
        return await ApplyFilters(title, year).CountAsync(cancellationToken);
    }

    public async Task DeleteAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        _db.Movies.Remove(movie);
        await Task.CompletedTask;
    }

    public async Task<Movie?> GetByIdAsync(MovieId id, CancellationToken cancellationToken = default)
    {
        return await _db.Movies
            .Include(m => m.Genres)
            .Include(m => m.Ratings)
            .FirstOrDefaultAsync(m => m.Id == id.Value, cancellationToken);
    }

    public async Task<Movie?> GetBySlugAsync(Slug slug, CancellationToken cancellationToken = default)
    {
        return await _db.Movies
            .Include(m => m.Genres)
            .Include(m => m.Ratings)
            .FirstOrDefaultAsync(m => m.Slug.Value == slug.Value, cancellationToken);
    }

    public async Task<IReadOnlyList<Movie>> ListAsync(int page, int pageSize, string? title, int? year, CancellationToken cancellationToken = default)
    {
        var skip = (page - 1) * pageSize;
        return await ApplyFilters(title, year)
            .Include(m => m.Genres)
            .Include(m => m.Ratings)
            .OrderBy(m => m.Title)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        _db.Movies.Update(movie);
        await Task.CompletedTask;
    }

    private IQueryable<Movie> ApplyFilters(string? title, int? year)
    {
        var query = _db.Movies.AsQueryable();
        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(m => m.Title.Contains(title));
        }

        if (year.HasValue)
        {
            query = query.Where(m => m.YearOfRelease == year.Value);
        }

        return query;
    }
}
