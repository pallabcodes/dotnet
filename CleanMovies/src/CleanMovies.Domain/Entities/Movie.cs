using CleanMovies.Domain.Common;
using CleanMovies.Domain.Events;
using CleanMovies.Domain.ValueObjects;

namespace CleanMovies.Domain.Entities;

public sealed class Movie : AggregateRoot
{
    private readonly List<Genre> _genres = new();
    private readonly List<Rating> _ratings = new();

    private Movie(MovieId id, Slug slug, string title, int year, string? description)
    {
        Id = id.Value;
        Slug = slug;
        Title = title;
        YearOfRelease = year;
        Description = description;
    }

    // For EF Core
    private Movie() : this(MovieId.New(), Slug.From("placeholder", 1900), "placeholder", 1900, null) { }

    public Slug Slug { get; private set; }
    public string Title { get; private set; }
    public int YearOfRelease { get; private set; }
    public string? Description { get; private set; }
    public IReadOnlyCollection<Genre> Genres => _genres.AsReadOnly();
    public IReadOnlyCollection<Rating> Ratings => _ratings.AsReadOnly();

    public static Movie Create(string title, int year, IEnumerable<string> genres, string? description = null)
    {
        Validate(title, year);

        var slug = Slug.From(title, year);
        var movie = new Movie(MovieId.New(), slug, title.Trim(), year, description);
        foreach (var g in genres ?? Enumerable.Empty<string>())
        {
            movie.AddGenre(g);
        }
        movie.AddDomainEvent(new MovieCreatedEvent(movie.Id, movie.Slug.Value));
        return movie;
    }

    public void UpdateDetails(string title, int year, string? description, IEnumerable<string> genres)
    {
        Validate(title, year);

        Title = title.Trim();
        YearOfRelease = year;
        Description = description;
        Slug = Slug.From(title, year);
        SyncGenres(genres);
        AddDomainEvent(new MovieUpdatedEvent(Id, Slug.Value));
    }

    private static void Validate(string title, int year)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        if (title.Length > 200) throw new ArgumentOutOfRangeException(nameof(title), "Title too long");
        if (year is < 1888 or > 3000) throw new ArgumentOutOfRangeException(nameof(year));
    }

    public Rating AddOrUpdateRating(UserId userId, int value)
    {
        var existing = _ratings.FirstOrDefault(r => r.UserId == userId);
        if (existing is null)
        {
        var rating = Rating.Create(Id, userId, value);
        _ratings.Add(rating);
            AddDomainEvent(new MovieRatedEvent(Id, userId.Value, value));
            return rating;
        }

        existing.Update(value);
        AddDomainEvent(new MovieRatedEvent(Id, userId.Value, value));
        return existing;
    }

    public double AverageRating => _ratings.Count == 0 ? 0 : Math.Round(_ratings.Average(r => r.Value), 1);

    private void AddGenre(string genre)
    {
        var g = Genre.From(genre);
        if (_genres.All(x => !x.Name.Equals(g.Name, StringComparison.OrdinalIgnoreCase)))
        {
            _genres.Add(g);
        }
    }

    private void SyncGenres(IEnumerable<string> genres)
    {
        var incoming = genres.Select(Genre.From).ToList();
        _genres.Clear();
        _genres.AddRange(incoming);
    }
}
