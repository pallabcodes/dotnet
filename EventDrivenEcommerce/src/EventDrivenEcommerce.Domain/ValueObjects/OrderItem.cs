namespace EventDrivenEcommerce.Domain.ValueObjects;

/// <summary>
/// Value object representing an item in an order.
/// </summary>
public sealed class OrderItem
{
    public ProductId ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public Money UnitPrice { get; private set; } = null!;
    public int Quantity { get; private set; }

    // Parameterless constructor for EF Core
    private OrderItem() { }

    public OrderItem(ProductId productId, string productName, Money unitPrice, int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public Money TotalPrice => UnitPrice.Multiply(Quantity);

    public OrderItem WithQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(newQuantity), "Quantity must be positive");

        return new OrderItem(ProductId, ProductName, UnitPrice, newQuantity);
    }
}

