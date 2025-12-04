using EventDrivenEcommerce.Application.Commands.PlaceOrder;
using EventDrivenEcommerce.Domain.ValueObjects;
using FluentAssertions;

namespace EventDrivenEcommerce.UnitTests.Application;

public class PlaceOrderValidatorTests
{
    private readonly PlaceOrderCommandValidator _validator = new();

    [Fact]
    public void Should_Pass_WhenAllFieldsValid()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_WhenCustomerIdEmpty()
    {
        // Arrange
        var command = CreateValidCommand() with { CustomerId = new CustomerId(Guid.Empty) };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(PlaceOrderCommand.CustomerId));
    }

    [Fact]
    public void Should_Fail_WhenShippingAddressNull()
    {
        // Arrange
        var command = CreateValidCommand() with { ShippingAddress = null! };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(PlaceOrderCommand.ShippingAddress));
    }

    [Fact]
    public void Should_Fail_WhenNoItems()
    {
        // Arrange
        var command = CreateValidCommand() with { Items = Array.Empty<OrderItem>() };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(PlaceOrderCommand.Items));
    }

    [Fact]
    public void Should_Fail_WhenItemPriceNegative()
    {
        // Arrange
        var items = new[]
        {
            new OrderItem(ProductId.New(), "Test", new Money(-10.00m, "USD"), 1)
        };
        var command = CreateValidCommand() with { Items = items };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("UnitPrice"));
    }

    [Fact]
    public void Should_Fail_WhenItemQuantityZero()
    {
        // Arrange
        var items = new[]
        {
            new OrderItem(ProductId.New(), "Test", new Money(10.00m, "USD"), 0)
        };
        var command = CreateValidCommand() with { Items = items };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Quantity"));
    }

    private static PlaceOrderCommand CreateValidCommand()
    {
        var customerId = CustomerId.New();
        var address = new Address("123 Test St", "Test City", "TC", "12345", "USA");
        var items = new[]
        {
            new OrderItem(ProductId.New(), "Test Product", new Money(10.00m, "USD"), 1)
        };

        return new PlaceOrderCommand(customerId, address, items);
    }
}

