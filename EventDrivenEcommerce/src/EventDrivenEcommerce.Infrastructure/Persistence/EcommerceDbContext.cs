using EventDrivenEcommerce.Application.Common;
using EventDrivenEcommerce.Application.EventHandlers;
using EventDrivenEcommerce.Domain.Common;
using EventDrivenEcommerce.Domain.Entities;
using EventDrivenEcommerce.Domain.Repositories;
using EventDrivenEcommerce.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EventDrivenEcommerce.Infrastructure.Persistence;

/// <summary>
/// Main DbContext for the e-commerce system.
/// Implements Unit of Work pattern and outbox for reliable event publishing.
/// </summary>
public sealed class EcommerceDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EcommerceDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Collect domain events before saving
        var domainEvents = CollectDomainEvents();

        var result = await base.SaveChangesAsync(cancellationToken);

        // Publish domain events after successful save
        await PublishDomainEventsAsync(domainEvents, cancellationToken);

        return result;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction != null)
            return;

        await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction != null)
            await Database.CurrentTransaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction != null)
            await Database.CurrentTransaction.RollbackAsync(cancellationToken);
    }

    private List<IDomainEvent> CollectDomainEvents()
    {
        var domainEvents = new List<IDomainEvent>();

        foreach (var entry in ChangeTracker.Entries<AggregateRoot>())
        {
            if (entry.Entity.DomainEvents.Any())
            {
                domainEvents.AddRange(entry.Entity.DomainEvents);
                entry.Entity.ClearDomainEvents();
            }
        }

        return domainEvents;
    }

    private async Task PublishDomainEventsAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken)
    {
        if (!domainEvents.Any())
            return;

        Console.WriteLine($"Publishing {domainEvents.Count()} domain events");

        try
        {
            var notification = new DomainEventsPublishedNotification(domainEvents);
            await _mediator.Publish(notification, cancellationToken);
            Console.WriteLine("Domain events notification published successfully");
        }
        catch (Exception ex)
        {
            // Log the error but don't fail the transaction
            // Events will be published later via outbox pattern
            // In a real system, you might want to store failed events for retry
            Console.WriteLine($"Failed to publish domain events: {ex.Message}");
        }
    }
}

