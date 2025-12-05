using SaaSUsageBilling.Domain.Entities;

namespace SaaSUsageBilling.Application.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);
}

public interface ICustomerRepository
{
    Task AddAsync(Customer customer, CancellationToken ct);
    Task<Customer?> GetAsync(Guid id, CancellationToken ct);
}

public interface IPlanRepository
{
    Task AddAsync(Plan plan, CancellationToken ct);
    Task<Plan?> GetAsync(Guid id, CancellationToken ct);
}

public interface ISubscriptionRepository
{
    Task AddAsync(Subscription subscription, CancellationToken ct);
    Task<Subscription?> GetAsync(Guid id, CancellationToken ct);
    Task UpdateAsync(Subscription subscription, CancellationToken ct);
}

public interface IUsageEventRepository
{
    Task AddAsync(UsageEvent usageEvent, CancellationToken ct);
    Task<IReadOnlyCollection<UsageEvent>> GetForPeriodAsync(Guid subscriptionId, DateTimeOffset from, DateTimeOffset to, CancellationToken ct);
}

public interface IInvoiceRepository
{
    Task AddAsync(Invoice invoice, CancellationToken ct);
    Task<Invoice?> GetLatestAsync(Guid subscriptionId, CancellationToken ct);
}
