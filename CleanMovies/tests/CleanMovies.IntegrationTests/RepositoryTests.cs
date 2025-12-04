using System;
using System.Threading.Tasks;
using CleanMovies.Domain.Entities;
using CleanMovies.Infrastructure.Persistence;
using CleanMovies.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CleanMovies.IntegrationTests;

public class RepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly MovieDbContext _db;
    private readonly MovieRepository _repository;

    public RepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new MovieDbContext(options);
        _db.Database.EnsureCreated();

        _repository = new MovieRepository(_db);
    }

    [Fact]
    public async Task AddMovie_ShouldPersistAndPreventDuplicates()
    {
        var movie = Movie.Create("Arrival", 2016, new[] { "Sci-Fi" });
        await _repository.AddAsync(movie);
        await _db.SaveChangesAsync();

        var duplicate = Movie.Create("Arrival", 2016, new[] { "Sci-Fi" });
        await _repository.AddAsync(duplicate);
        await Assert.ThrowsAsync<DbUpdateException>(() => _db.SaveChangesAsync());
    }

    [Fact]
    public async Task RateMovie_ShouldUpdateAverageAndNotDuplicateRatings()
    {
        var movie = Movie.Create("Dune", 2021, Array.Empty<string>());
        await _repository.AddAsync(movie);
        await _db.SaveChangesAsync();

        movie.AddOrUpdateRating(new(Guid.NewGuid()), 8);
        movie.AddOrUpdateRating(new(Guid.NewGuid()), 6);
        movie.AddOrUpdateRating(new(Guid.Parse("11111111-1111-1111-1111-111111111111")), 5);
        movie.AddOrUpdateRating(new(Guid.Parse("11111111-1111-1111-1111-111111111111")), 9);

        await _repository.UpdateAsync(movie);
        await _db.SaveChangesAsync();

        movie.AverageRating.Should().Be(7.7);
        movie.Ratings.Count.Should().Be(3);
    }

    [Fact]
    public async Task ListMovies_ShouldFilterAndPaginate()
    {
        await _repository.AddAsync(Movie.Create("Movie A", 2000, Array.Empty<string>()));
        await _repository.AddAsync(Movie.Create("Movie B", 2001, Array.Empty<string>()));
        await _repository.AddAsync(Movie.Create("Another Movie", 2001, Array.Empty<string>()));
        await _db.SaveChangesAsync();

        var page = await _repository.ListAsync(1, 2, "Movie", null);
        page.Should().HaveCount(2);

        var filtered = await _repository.ListAsync(1, 10, null, 2001);
        filtered.Should().OnlyContain(m => m.YearOfRelease == 2001);
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }
}
