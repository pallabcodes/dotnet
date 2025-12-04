using CleanMovies.Domain.Common;
using CleanMovies.Domain.ValueObjects;

namespace CleanMovies.Domain.Entities;

public sealed class Rating : Entity
{
    private Rating(Guid movieId, UserId userId, int value)
    {
        Id = Guid.NewGuid();
        MovieId = movieId;
        UserId = userId;
        Value = value;
    }

    private Rating() { }

    public Guid MovieId { get; private set; }
    public UserId UserId { get; private set; }
    public int Value { get; private set; }

    public static Rating Create(Guid movieId, UserId userId, int value)
    {
        Validate(value);
        return new Rating(movieId, userId, value);
    }

    public void Update(int value)
    {
        Validate(value);
        Value = value;
    }

    private static void Validate(int value)
    {
        if (value is < 1 or > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Rating must be between 1 and 10");
        }
    }
}
