using Microsoft.EntityFrameworkCore;
using SaaSUsageBilling.Application.Abstractions;
using SaaSUsageBilling.Domain.Entities;

namespace SaaSUsageBilling.Infrastructure.Persistence.Repositories;

public class EfInvoiceRepository : IInvoiceRepository
{
    private readonly BillingDbContext _context;

    public EfInvoiceRepository(BillingDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Invoice invoice, CancellationToken ct)
    {
        await _context.Invoices.AddAsync(invoice, ct);
        // Note: SaveChanges is handled by UnitOfWork
    }

    public async Task<Invoice?> GetLatestAsync(Guid subscriptionId, CancellationToken ct)
    {
        return await _context.Invoices
            .Where(i => i.SubscriptionId == subscriptionId)
            .OrderByDescending(i => i.Period.To)
            .FirstOrDefaultAsync(ct);
    }
}