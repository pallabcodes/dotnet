namespace EventDrivenEcommerce.Domain.ValueObjects;

/// <summary>
/// Strongly-typed identifier for customers.
/// </summary>
public readonly record struct CustomerId(Guid Value)
{
    public static CustomerId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}

