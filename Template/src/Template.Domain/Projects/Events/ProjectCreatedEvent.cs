using Shared.Domain.Common;

namespace Template.Domain.Projects.Events;

public sealed record ProjectCreatedEvent(Guid ProjectId, string Name) : DomainEventBase;
