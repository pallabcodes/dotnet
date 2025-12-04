namespace CleanMovies.Api.Contracts.Responses;

public class ErrorResponse
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = string.Empty;
    public string Instance { get; set; } = string.Empty;
    public Dictionary<string, string[]>? Errors { get; set; }

    public static ErrorResponse Create(
        string type,
        string title,
        int status,
        string detail,
        string instance,
        Dictionary<string, string[]>? errors = null)
    {
        return new ErrorResponse
        {
            Type = type,
            Title = title,
            Status = status,
            Detail = detail,
            Instance = instance,
            Errors = errors
        };
    }
}

