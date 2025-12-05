using EventDrivenEcommerce.Application.Common;
using EventDrivenEcommerce.Application.EventHandlers;
using EventDrivenEcommerce.Domain.Common;
using EventDrivenEcommerce.Domain.Entities;
using EventDrivenEcommerce.Domain.Repositories;
using EventDrivenEcommerce.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventDrivenEcommerce.Infrastructure.Persistence;

/// <summary>
/// Main DbContext for the e-commerce system.
/// Implements Unit of Work pattern and outbox for reliable event publishing.
/// </summary>
public sealed class EcommerceDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;
    private readonly ILogger<EcommerceDbContext> _logger;

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public EcommerceDbContext(
        DbContextOptions<EcommerceDbContext> options,
        IMediator mediator,
        ILogger<EcommerceDbContext> logger)
        : base(options)
    {
        _mediator = mediator;
        _logger = logger;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EcommerceDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = CollectDomainEvents();

        if (domainEvents.Any())
        {
            await PublishDomainEventsAsync(domainEvents, cancellationToken);
        }

        return await base.SaveChangesAsync(cancellationToken);
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
        try
        {
            var notification = new DomainEventsPublishedNotification(domainEvents);
            await _mediator.Publish(notification, cancellationToken);
        }
        catch (Exception ex)
        {
            // We intentionally swallow exceptions to avoid breaking the transaction
            // but we log so the outbox processor can be retried or investigated.
            _logger.LogError(ex, "Failed to publish domain events");
        }
    }
}
