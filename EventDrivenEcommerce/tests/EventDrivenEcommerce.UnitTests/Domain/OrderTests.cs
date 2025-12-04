using EventDrivenEcommerce.Domain.Entities;
using EventDrivenEcommerce.Domain.Events;
using EventDrivenEcommerce.Domain.ValueObjects;
using FluentAssertions;

namespace EventDrivenEcommerce.UnitTests.Domain;

public class OrderTests
{
    [Fact]
    public void Create_ShouldCreateOrderWithCorrectProperties()
    {
        // Arrange
        var customerId = CustomerId.New();
        var address = new Address("123 Main St", "Anytown", "CA", "12345", "USA");
        var items = new[]
        {
            new OrderItem(ProductId.New(), "Widget A", new Money(10.99m, "USD"), 2),
            new OrderItem(ProductId.New(), "Widget B", new Money(5.50m, "USD"), 1)
        };

        // Act
        var order = Order.Create(customerId, address, items);

        // Assert
        order.CustomerId.Should().Be(customerId);
        order.ShippingAddress.Should().Be(address);
        order.Status.Should().Be(OrderStatus.Pending);
        order.TotalAmount.Should().Be(new Money(27.48m, "USD"));
        order.Items.Should().HaveCount(2);
        order.DomainEvents.Should().ContainSingle(e => e is OrderPlacedEvent);
    }

    [Fact]
    public void Create_ShouldThrow_WhenNoItemsProvided()
    {
        // Arrange
        var customerId = CustomerId.New();
        var address = new Address("123 Main St", "Anytown", "CA", "12345", "USA");

        // Act & Assert
        var action = () => Order.Create(customerId, address, Array.Empty<OrderItem>());
        action.Should().Throw<ArgumentException>()
             .WithMessage("Order must contain at least one item*");
    }

    [Fact]
    public void Confirm_ShouldChangeStatusAndRaiseEvent()
    {
        // Arrange
        var order = CreateTestOrder();

        // Act
        order.Confirm();

        // Assert
        order.Status.Should().Be(OrderStatus.Confirmed);
        order.DomainEvents.Should().Contain(e => e is OrderConfirmedEvent);
    }

    [Fact]
    public void Confirm_ShouldThrow_WhenOrderNotPending()
    {
        // Arrange
        var order = CreateTestOrder();
        order.Confirm(); // Already confirmed

        // Act & Assert
        var action = () => order.Confirm();
        action.Should().Throw<InvalidOperationException>()
             .WithMessage("*Confirmed*");
    }

    [Fact]
    public void Ship_ShouldChangeStatusAndRaiseEvent()
    {
        // Arrange
        var order = CreateTestOrder();
        order.Confirm();
        var trackingNumber = "TRK123456";

        // Act
        order.Ship(trackingNumber);

        // Assert
        order.Status.Should().Be(OrderStatus.Shipped);
        order.DomainEvents.Should().Contain(e => e is OrderShippedEvent);
    }

    [Fact]
    public void Ship_ShouldThrow_WhenOrderNotConfirmed()
    {
        // Arrange
        var order = CreateTestOrder();

        // Act & Assert
        var action = () => order.Ship("TRK123");
        action.Should().Throw<InvalidOperationException>()
             .WithMessage("*status*");
    }

    [Fact]
    public void Cancel_ShouldChangeStatusAndRaiseEvent()
    {
        // Arrange
        var order = CreateTestOrder();
        var reason = "Customer request";

        // Act
        order.Cancel(reason);

        // Assert
        order.Status.Should().Be(OrderStatus.Cancelled);
        order.DomainEvents.Should().Contain(e => e is OrderCancelledEvent);
    }

    [Fact]
    public void Cancel_ShouldThrow_WhenOrderShipped()
    {
        // Arrange
        var order = CreateTestOrder();
        order.Confirm();
        order.Ship("TRK123");

        // Act & Assert
        var action = () => order.Cancel("Test");
        action.Should().Throw<InvalidOperationException>()
             .WithMessage("*Shipped*");
    }

    private static Order CreateTestOrder()
    {
        var customerId = CustomerId.New();
        var address = new Address("123 Main St", "Anytown", "CA", "12345", "USA");
        var items = new[]
        {
            new OrderItem(ProductId.New(), "Test Product", new Money(10.00m, "USD"), 1)
        };

        return Order.Create(customerId, address, items);
    }
}

