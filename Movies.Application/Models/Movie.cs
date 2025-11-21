using System.Text.RegularExpressions;

namespace Movies.Application.Models;

public class Movie
{
    private static readonly Regex SlugRegex = new("[^0-9A-Za-z _-]", RegexOptions.Compiled | RegexOptions.NonBacktracking);

    public required Guid Id { get; init; }
    public required string Title { get; set; }

    public string Slug => GenerateSlug();

    public float? Rating { get; set; }
    public int? UserRating { get; set; }
    public required int YearOfRelease { get; set; }
    public required List<string> Genres { get; init; } = new();

    private string GenerateSlug()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            return $"{YearOfRelease}";
        }

        var sluggedTitle = SlugRegex.Replace(Title, string.Empty).Replace(" ", "-", StringComparison.Ordinal);
        return $"{sluggedTitle}-{YearOfRelease}";
    }
}
