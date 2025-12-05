using System.Collections.Concurrent;
using SaaSUsageBilling.Application.Abstractions;
using SaaSUsageBilling.Domain.Entities;

namespace SaaSUsageBilling.Infrastructure.Persistence.InMemory;

public sealed class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly ConcurrentDictionary<Guid, Customer> _store = new();

    public Task AddAsync(Customer customer, CancellationToken ct)
    {
        _store[customer.Id] = customer;
        return Task.CompletedTask;
    }

    public Task<Customer?> GetAsync(Guid id, CancellationToken ct)
    {
        _store.TryGetValue(id, out var value);
        return Task.FromResult<Customer?>(value);
    }
}

public sealed class InMemoryPlanRepository : IPlanRepository
{
    private readonly ConcurrentDictionary<Guid, Plan> _store = new();

    public Task AddAsync(Plan plan, CancellationToken ct)
    {
        _store[plan.Id] = plan;
        return Task.CompletedTask;
    }

    public Task<Plan?> GetAsync(Guid id, CancellationToken ct)
    {
        _store.TryGetValue(id, out var value);
        return Task.FromResult<Plan?>(value);
    }
}

public sealed class InMemorySubscriptionRepository : ISubscriptionRepository
{
    private readonly ConcurrentDictionary<Guid, Subscription> _store = new();

    public Task AddAsync(Subscription subscription, CancellationToken ct)
    {
        _store[subscription.Id] = subscription;
        return Task.CompletedTask;
    }

    public Task<Subscription?> GetAsync(Guid id, CancellationToken ct)
    {
        _store.TryGetValue(id, out var value);
        return Task.FromResult<Subscription?>(value);
    }

    public Task UpdateAsync(Subscription subscription, CancellationToken ct)
    {
        _store[subscription.Id] = subscription;
        return Task.CompletedTask;
    }
}

public sealed class InMemoryUsageEventRepository : IUsageEventRepository
{
    private readonly ConcurrentDictionary<Guid, List<UsageEvent>> _store = new();

    public Task AddAsync(UsageEvent usageEvent, CancellationToken ct)
    {
        var list = _store.GetOrAdd(usageEvent.SubscriptionId, _ => new List<UsageEvent>());
        lock (list)
        {
            list.Add(usageEvent);
        }
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<UsageEvent>> GetForPeriodAsync(Guid subscriptionId, DateTimeOffset from, DateTimeOffset to, CancellationToken ct)
    {
        if (_store.TryGetValue(subscriptionId, out var list))
        {
            lock (list)
            {
                return Task.FromResult<IReadOnlyCollection<UsageEvent>>(list
                    .Where(e => e.OccurredAt >= from && e.OccurredAt < to)
                    .ToList());
            }
        }

        return Task.FromResult<IReadOnlyCollection<UsageEvent>>(Array.Empty<UsageEvent>());
    }
}

public sealed class InMemoryInvoiceRepository : IInvoiceRepository
{
    private readonly ConcurrentDictionary<Guid, List<Invoice>> _store = new();

    public Task AddAsync(Invoice invoice, CancellationToken ct)
    {
        var list = _store.GetOrAdd(invoice.SubscriptionId, _ => new List<Invoice>());
        lock (list)
        {
            list.Add(invoice);
        }
        return Task.CompletedTask;
    }

    public Task<Invoice?> GetLatestAsync(Guid subscriptionId, CancellationToken ct)
    {
        if (_store.TryGetValue(subscriptionId, out var list))
        {
            lock (list)
            {
                return Task.FromResult<Invoice?>(list.OrderByDescending(i => i.Period.To).FirstOrDefault());
            }
        }

        return Task.FromResult<Invoice?>(null);
    }
}
