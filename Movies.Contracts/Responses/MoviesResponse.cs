namespace Movies.Contracts.Responses;

public class MoviesResponse
{
    public required IEnumerable<MoviesResponse> Iteams { get; init; } = Enumerable.Empty<MoviesResponse>();
}