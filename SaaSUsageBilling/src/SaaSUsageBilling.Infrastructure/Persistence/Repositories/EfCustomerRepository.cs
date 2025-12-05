using SaaSUsageBilling.Application.Abstractions;
using SaaSUsageBilling.Domain.Entities;

namespace SaaSUsageBilling.Infrastructure.Persistence.Repositories;

public class EfCustomerRepository : ICustomerRepository
{
    private readonly BillingDbContext _context;

    public EfCustomerRepository(BillingDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Customer customer, CancellationToken ct)
    {
        await _context.Customers.AddAsync(customer, ct);
        // Note: SaveChanges is handled by UnitOfWork
    }

    public async Task<Customer?> GetAsync(Guid id, CancellationToken ct)
    {
        return await _context.Customers.FindAsync(new object[] { id }, ct);
    }
}

