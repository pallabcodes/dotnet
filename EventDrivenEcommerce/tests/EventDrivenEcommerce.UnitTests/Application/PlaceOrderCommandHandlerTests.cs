using EventDrivenEcommerce.Application.Commands.PlaceOrder;
using EventDrivenEcommerce.Application.Common;
using EventDrivenEcommerce.Domain.Entities;
using EventDrivenEcommerce.Domain.Repositories;
using EventDrivenEcommerce.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace EventDrivenEcommerce.UnitTests.Application;

public class PlaceOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly PlaceOrderCommandHandler _handler;

    public PlaceOrderCommandHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new PlaceOrderCommandHandler(_orderRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateOrderAndReturnSuccess()
    {
        // Arrange
        var command = CreateTestCommand();
        var capturedOrder = (Order)null!;

        _orderRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((order, _) => capturedOrder = order);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Value.Should().NotBe(OrderId.New()); // Should have a valid ID

        _orderRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        capturedOrder.Should().NotBeNull();
        capturedOrder.CustomerId.Should().Be(command.CustomerId);
        capturedOrder.Items.Should().HaveCount(command.Items.Count);
        capturedOrder.DomainEvents.Should().ContainSingle(); // OrderPlacedEvent
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
    {
        // Arrange
        var command = CreateTestCommand();

        _orderRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Error.Should().Contain("Failed to place order");
    }

    private static PlaceOrderCommand CreateTestCommand()
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

