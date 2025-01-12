using System.Text.RegularExpressions;

namespace Movies.Application.Models;

public class Movie
{
    public required Guid Id { get; init; }
    public required string Title { get; set; }

    public string Slug => GenerateSlug();

    public required int YearOfRelease { get; set; }
    public required List<string> Genres { get; init; } = new();

    private string GenerateSlug()
    {
        var sluggedTitle = SlugRegex().Replace(Title, "").Replace(" ", "-");
        return $"{sluggedTitle}-{YearOfRelease}";
    }

    // Use a static method to create the regex
    private static Regex SlugRegex()
    {
        return new Regex("[^0-9A-Za-z _-]", RegexOptions.Compiled | RegexOptions.NonBacktracking);
    }
}