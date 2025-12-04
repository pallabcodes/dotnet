namespace EventDrivenEcommerce.Domain.Entities;

/// <summary>
/// Enumeration of possible order statuses.
/// </summary>
public enum OrderStatus
{
    Pending,
    Confirmed,
    Shipped,
    Delivered,
    Cancelled
}

