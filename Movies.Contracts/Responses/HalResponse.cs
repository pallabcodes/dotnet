using System.Text.Json.Serialization;

namespace Movies.Contracts.Responses;

// HAL = HyperMedia API Language
public abstract class HalResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Link> Links { get; set; } = new(); // Todo: what does this new() do here?
}

public class Link
{
    public required string Href { get; init; }
    
    public required string Rel { get; init; }
    
    public required string Type { get; init; }
}