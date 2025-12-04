namespace CleanMovies.Api.Contracts.Responses;

public class MovieResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int YearOfRelease { get; set; }
    public string? Description { get; set; }
    public string Slug { get; set; } = string.Empty;
    public IReadOnlyCollection<string> Genres { get; set; } = Array.Empty<string>();
    public double AverageRating { get; set; }
    public int RatingCount { get; set; }
}

