using KnowledgeBase.Samples.Concurrency;

namespace KnowledgeBase.Samples.Tests;

public sealed class ConcurrencyTests
{
    private static readonly WorkItem[] Items =
    [
        new WorkItem("fetch user", TimeSpan.FromMilliseconds(10)),
        new WorkItem("fetch orders", TimeSpan.FromMilliseconds(10)),
        new WorkItem("fetch invoices", TimeSpan.FromMilliseconds(10))
    ];

    [Fact]
    public async Task RunAllAsync_completes_every_item()
    {
        var results = await WorkloadRunner.RunAllAsync(Items, CancellationToken.None);

        Assert.Equal(new[] { "fetch user", "fetch orders", "fetch invoices" }, results);
    }

    [Fact]
    public async Task RunAllAsync_respects_cancellation()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => WorkloadRunner.RunAllAsync(Items, cts.Token));
    }

    [Fact]
    public async Task RunBoundedAsync_completes_every_item_even_with_degree_one()
    {
        var results = await WorkloadRunner.RunBoundedAsync(Items, 1, CancellationToken.None);

        Assert.Equal(new[] { "fetch user", "fetch orders", "fetch invoices" }, results);
    }

    [Fact]
    public async Task RunBoundedAsync_respects_cancellation_mid_fanout()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => WorkloadRunner.RunBoundedAsync(Items, 2, cts.Token));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task RunBoundedAsync_rejects_invalid_parallelism(int degree)
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => WorkloadRunner.RunBoundedAsync(Items, degree, CancellationToken.None));
    }

    [Fact]
    public async Task RunBoundedAsync_respects_degree_of_parallelism()
    {
        var concurrency = 0;
        var peak = 0;
        var gate = new object();

        async Task<string> Marker(WorkItem item)
        {
            lock (gate)
            {
                concurrency++;
                peak = Math.Max(peak, concurrency);
            }

            await Task.Yield();
            Thread.Sleep(20);

            lock (gate)
            {
                concurrency--;
            }

            return item.Name;
        }

        // Drive each work item through the bounded runner with degree 2.
        var results = await RunWithDegreeAsync(Items, 2, Marker);

        Assert.Equal(3, results.Length);
        Assert.True(peak <= 2, $"Expected peak concurrency <= 2, was {peak}");
    }

    private static async Task<string[]> RunWithDegreeAsync(
        IEnumerable<WorkItem> items,
        int degree,
        Func<WorkItem, Task<string>> transform)
    {
        using var gate = new SemaphoreSlim(degree);

        var tasks = items.Select(
            async item =>
            {
                await gate.WaitAsync().ConfigureAwait(false);
                try
                {
                    return await transform(item).ConfigureAwait(false);
                }
                finally
                {
                    gate.Release();
                }
            });

        return await Task.WhenAll(tasks).ConfigureAwait(false);
    }
}