namespace EventDrivenEcommerce.Domain.ValueObjects;

/// <summary>
/// Value object representing a shipping/billing address.
/// </summary>
public sealed record Address(
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country)
{
    public string FullAddress => $"{Street}, {City}, {State} {ZipCode}, {Country}";
}

