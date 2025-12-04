namespace CleanMovies.Api.Contracts.Requests;

public class RateMovieRequest
{
    public Guid UserId { get; set; }
    public int Rating { get; set; }
}

