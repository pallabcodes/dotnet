using System;
using CleanMovies.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CleanMovies.UnitTests.Domain;

public class MovieTests
{
    [Fact]
    public void Create_ShouldGenerateSlugAndGenres()
    {
        var movie = Movie.Create("The Matrix", 1999, new[] { "Action", "Sci-Fi" }, "Neo learns the truth");

        movie.Slug.Value.Should().Be("the-matrix-1999");
        movie.Genres.Should().HaveCount(2);
        movie.AverageRating.Should().Be(0);
    }

    [Fact]
    public void AddOrUpdateRating_ShouldComputeAverage()
    {
        var movie = Movie.Create("Dune", 2021, Array.Empty<string>());
        movie.AddOrUpdateRating(new(Guid.NewGuid()), 8);
        movie.AddOrUpdateRating(new(Guid.NewGuid()), 10);

        movie.AverageRating.Should().Be(9.0);
    }
}
