namespace EventDrivenEcommerce.Domain.ValueObjects;

/// <summary>
/// Strongly-typed identifier for products.
/// </summary>
public readonly record struct ProductId(Guid Value)
{
    public static ProductId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}

