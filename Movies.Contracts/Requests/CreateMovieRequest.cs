namespace Movies.Contracts.Requests;

// This is a DTO class
public class CreateMovieRequest
{
    // required ensures that this property must be initialized when creating an instance of the CreateMovieRequest class.
    // init indicates that this property can only be set during initialization, making it immutable after the object is created.
    public required string Title { get; init; }
    public required int YearOfRelease { get; init; }

    // IEnumerable is an interface that can be used to represent a collection e.g. new List<string>, string[],HashSet<string> or any collection
    // so, here it could be a collection with element must be string

    // N.B: How to properly initialize an empty collection? 

    // If I don't know which collection to use or don't want to be explicit, then use as below
    public required IEnumerable<string> Genres { get; init; } = Enumerable.Empty<string>();

    // If I know that I will use an Array, then below
    // public required IEnumerable<string> Genres { get; init; } = Array.Empty<string>();
}