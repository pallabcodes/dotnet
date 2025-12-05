namespace SaaSUsageBilling.Application.Abstractions;

public interface ISequenceGenerator
{
    Task<long> NextAsync(string sequenceName, CancellationToken ct = default);
}
