namespace EventDrivenEcommerce.Domain.ValueObjects;

/// <summary>
/// Strongly-typed identifier for orders.
/// </summary>
public readonly record struct OrderId(Guid Value)
{
    public static OrderId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}

