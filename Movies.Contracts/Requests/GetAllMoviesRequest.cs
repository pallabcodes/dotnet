namespace Movies.Contracts.Requests;

public class GetAllMoviesRequest : PaginatedRequest
{
    // N.B: if used require then the below attributes must be provided even with null
    public string? Title { get; init; }
    public int? Year { get; init; }

    public string? SortBy { get; init; }
}