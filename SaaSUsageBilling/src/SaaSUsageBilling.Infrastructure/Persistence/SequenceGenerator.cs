using SaaSUsageBilling.Application.Abstractions;
using System.Collections.Concurrent;

namespace SaaSUsageBilling.Infrastructure.Persistence;

public class SequenceGenerator : ISequenceGenerator
{
    private readonly ConcurrentDictionary<string, long> _sequences = new();

    public Task<long> NextAsync(string sequenceName, CancellationToken ct = default)
    {
        var nextValue = _sequences.AddOrUpdate(sequenceName, 1, (_, current) => current + 1);
        return Task.FromResult(nextValue);
    }
}
