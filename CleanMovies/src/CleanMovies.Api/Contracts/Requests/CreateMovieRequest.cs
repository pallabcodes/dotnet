namespace CleanMovies.Api.Contracts.Requests;

public class CreateMovieRequest
{
    public string Title { get; set; } = string.Empty;
    public int YearOfRelease { get; set; }
    public string? Description { get; set; }
    public IReadOnlyCollection<string> Genres { get; set; } = Array.Empty<string>();
}