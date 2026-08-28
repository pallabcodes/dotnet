namespace KnowledgeBase.Samples.Concurrency;

/// <summary>A unit of work with a simulated duration.</summary>
public sealed record WorkItem(string Name, TimeSpan Duration);

/// <summary>
/// Task-based Asynchronous Pattern (TAP) fundamentals:
/// - fan-out with Task.WhenAll (parallelism without blocked threads),
/// - cooperative cancellation flowing through a CancellationToken,
/// - bounded fan-out via SemaphoreSlim when unbounded parallelism is unsafe.
/// ConfigureAwait(false) is a library convention: never capture per-context state.
/// </summary>
public static class WorkloadRunner
{
    /// <summary>Runs every item concurrently; completes only when all complete.</summary>
    public static async Task<IReadOnlyList<string>> RunAllAsync(IEnumerable<WorkItem> items, CancellationToken ct)
    {
        var tasks = items.Select(
            async item =>
            {
                await Task.Delay(item.Duration, ct).ConfigureAwait(false);
                return item.Name;
            });

        return await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    /// <summary>Runs items concurrently but never more than <c>degreeOfParallelism</c> at once.</summary>
    public static async Task<IReadOnlyList<string>> RunBoundedAsync(
        IEnumerable<WorkItem> items,
        int degreeOfParallelism,
        CancellationToken ct)
    {
        if (degreeOfParallelism <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(degreeOfParallelism));
        }

        using var gate = new SemaphoreSlim(degreeOfParallelism);

        var tasks = items.Select(
            async item =>
            {
                await gate.WaitAsync(ct).ConfigureAwait(false);
                try
                {
                    await Task.Delay(item.Duration, ct).ConfigureAwait(false);
                    return item.Name;
                }
                finally
                {
                    gate.Release();
                }
            });

        return await Task.WhenAll(tasks).ConfigureAwait(false);
    }
}