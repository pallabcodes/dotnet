using Microsoft.EntityFrameworkCore;
using SaaSUsageBilling.Application.Abstractions;
using SaaSUsageBilling.Domain.Entities;

namespace SaaSUsageBilling.Infrastructure.Persistence.Repositories;

public class EfIdempotencyStore : IIdempotencyStore
{
    private readonly BillingDbContext _context;
    private readonly TimeSpan _defaultTtl = TimeSpan.FromDays(7); // Keep keys for 7 days

    public EfIdempotencyStore(BillingDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken ct)
    {
        // Clean up expired keys periodically (simple approach - could be done in background)
        await CleanupExpiredKeysAsync(ct);

        var existingKey = await _context.IdempotencyKeys
            .FirstOrDefaultAsync(k => k.Key == key && !k.IsExpired(), ct);

        return existingKey != null;
    }

    public async Task RecordAsync(string key, CancellationToken ct)
    {
        var idempotencyKey = new IdempotencyKey(key, _defaultTtl);
        await _context.IdempotencyKeys.AddAsync(idempotencyKey, ct);
        // Note: SaveChanges is handled by UnitOfWork
    }

    public async Task<string?> GetResponseAsync(string key, CancellationToken ct)
    {
        var existingKey = await _context.IdempotencyKeys
            .FirstOrDefaultAsync(k => k.Key == key && !k.IsExpired(), ct);

        return existingKey?.Response;
    }

    public async Task StoreResponseAsync(string key, string response, CancellationToken ct)
    {
        var existingKey = await _context.IdempotencyKeys
            .FirstOrDefaultAsync(k => k.Key == key && !k.IsExpired(), ct);

        if (existingKey != null)
        {
            existingKey.SetResponse(response);
            // Note: SaveChanges is handled by UnitOfWork
        }
    }

    private async Task CleanupExpiredKeysAsync(CancellationToken ct)
    {
        // Simple cleanup - remove expired keys
        var expiredKeys = _context.IdempotencyKeys.Where(k => k.IsExpired());
        _context.IdempotencyKeys.RemoveRange(expiredKeys);
        await _context.SaveChangesAsync(ct);
    }
}

