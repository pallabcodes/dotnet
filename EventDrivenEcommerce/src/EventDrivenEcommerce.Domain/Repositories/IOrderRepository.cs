using EventDrivenEcommerce.Domain.Entities;
using EventDrivenEcommerce.Domain.ValueObjects;

namespace EventDrivenEcommerce.Domain.Repositories;

/// <summary>
/// Repository interface for Order aggregate operations.
/// </summary>
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(OrderId orderId, CancellationToken cancellationToken = default);
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByCustomerIdAsync(CustomerId customerId, CancellationToken cancellationToken = default);
}

