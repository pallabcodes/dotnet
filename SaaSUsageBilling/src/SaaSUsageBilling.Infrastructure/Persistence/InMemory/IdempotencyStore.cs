using System.Collections.Concurrent;
using SaaSUsageBilling.Application.Abstractions;

namespace SaaSUsageBilling.Infrastructure.Persistence.InMemory;

public sealed class InMemoryIdempotencyStore : IIdempotencyStore
{
    private readonly ConcurrentDictionary<string, string> _responses = new();

    public Task<bool> ExistsAsync(string key, CancellationToken ct)
    {
        return Task.FromResult(_responses.ContainsKey(key));
    }

    public Task RecordAsync(string key, CancellationToken ct)
    {
        _responses.TryAdd(key, string.Empty);
        return Task.CompletedTask;
    }

    public Task<string?> GetResponseAsync(string key, CancellationToken ct)
    {
        _responses.TryGetValue(key, out var response);
        return Task.FromResult(response == string.Empty ? null : response);
    }

    public Task StoreResponseAsync(string key, string response, CancellationToken ct)
    {
        _responses[key] = response;
        return Task.CompletedTask;
    }
}
