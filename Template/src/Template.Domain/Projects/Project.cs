using Shared.Domain.Common;
using Template.Domain.Projects.Events;
using Template.Domain.Projects.ValueObjects;

namespace Template.Domain.Projects;

public sealed class Project : Entity
{
    public ProjectName Name { get; private set; } = null!;
    public string Description { get; private set; } = string.Empty;
    public ProjectStatus Status { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Project() { }

    public Project(ProjectName name, string description, DateTimeOffset createdOn)
    {
        Name = name;
        Description = description ?? string.Empty;
        Status = ProjectStatus.Active;
        CreatedOn = createdOn;

        _domainEvents.Add(new ProjectCreatedEvent(Id, name.Value));
    }

    public void Rename(ProjectName newName)
    {
        if (Status == ProjectStatus.Archived)
            throw new InvalidOperationException("Cannot rename an archived project.");

        Name = newName;
    }

    public void UpdateDescription(string description)
    {
        Description = description ?? string.Empty;
    }

    public void Archive()
    {
        if (Status == ProjectStatus.Archived)
            return;

        Status = ProjectStatus.Archived;
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
