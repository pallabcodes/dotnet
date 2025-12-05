using SaaSUsageBilling.Domain.Common;

namespace SaaSUsageBilling.Domain.Entities;

/// <summary>
/// Entity for tracking processed idempotency keys.
/// </summary>
public sealed class IdempotencyKey : Entity
{
    public string Key { get; private set; } = string.Empty;
    public DateTimeOffset ProcessedOn { get; private set; }
    public DateTimeOffset ExpiresOn { get; private set; }
    public string? Response { get; private set; }

    private IdempotencyKey() { }

    public IdempotencyKey(string key, TimeSpan ttl = default)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
        ProcessedOn = DateTimeOffset.UtcNow;
        ExpiresOn = ttl == default ? DateTimeOffset.MaxValue : ProcessedOn.Add(ttl);
    }

    public void SetResponse(string response)
    {
        Response = response;
    }

    public bool IsExpired() => DateTimeOffset.UtcNow > ExpiresOn;
}

