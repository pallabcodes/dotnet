using SaaSUsageBilling.Domain.Common;

namespace SaaSUsageBilling.Domain.Entities;

/// <summary>
/// Customer owning subscriptions.
/// </summary>
public sealed class Customer : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    private Customer() { }

    public Customer(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
