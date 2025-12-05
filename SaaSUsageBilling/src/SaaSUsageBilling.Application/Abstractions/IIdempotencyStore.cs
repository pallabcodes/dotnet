namespace SaaSUsageBilling.Application.Abstractions;

/// <summary>
/// Persists idempotency keys to prevent duplicate side effects.
/// </summary>
public interface IIdempotencyStore
{
    Task<bool> ExistsAsync(string key, CancellationToken ct);
    Task RecordAsync(string key, CancellationToken ct);
    Task<string?> GetResponseAsync(string key, CancellationToken ct);
    Task StoreResponseAsync(string key, string response, CancellationToken ct);
}
