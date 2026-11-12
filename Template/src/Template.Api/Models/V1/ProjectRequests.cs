namespace Template.Api.Models.V1;

public class CreateProjectRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class ProjectCreatedResponse
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
}
