using EventDrivenEcommerce.Api;
using EventDrivenEcommerce.Application.Common;
using EventDrivenEcommerce.Application.IntegrationEvents;
using EventDrivenEcommerce.Domain.Entities;
using EventDrivenEcommerce.Domain.ValueObjects;
using EventDrivenEcommerce.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text.Json;

namespace EventDrivenEcommerce.IntegrationTests;

// DTOs matching the API
public record PlaceOrderRequest(
    Guid CustomerId,
    AddressDto ShippingAddress,
    IReadOnlyCollection<OrderItemDto> Items);

public record AddressDto(
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country);

public record OrderItemDto(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity);

public class OrderProcessingTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public OrderProcessingTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace database with in-memory for testing
                services.AddDbContext<EcommerceDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });
            builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Testing");
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task PlaceOrder_ShouldCreateOrderAndPublishEvents()
    {
        // Arrange
        var request = new PlaceOrderRequest(
            CustomerId: Guid.NewGuid(),
            ShippingAddress: new AddressDto(
                Street: "123 Test St",
                City: "Test City",
                State: "TC",
                ZipCode: "12345",
                Country: "USA"),
            Items: new[]
            {
                new OrderItemDto(
                    ProductId: Guid.NewGuid(),
                    ProductName: "Test Product",
                    UnitPrice: 10.99m,
                    Quantity: 2)
            });

        // Act
        var response = await _client.PostAsJsonAsync("/orders", request);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
        responseData.Should().ContainKey("orderId");

        // Verify order was created in database
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EcommerceDbContext>();

        var orders = await dbContext.Orders
            .Include(o => o.Items)
            .ToListAsync();

        orders.Should().HaveCount(1);
        var order = orders.Single();

        order.Should().NotBeNull();
        order.Status.Should().Be(OrderStatus.Pending);
        order.Items.Should().HaveCount(1);
        order.TotalAmount.Amount.Should().Be(21.98m); // 10.99 * 2

        // Verify outbox message was created for event publishing
        var outboxMessages = await dbContext.OutboxMessages.ToListAsync();
        outboxMessages.Should().ContainSingle();
        var outboxMessage = outboxMessages.Single();
        outboxMessage.Type.Should().Be("EventDrivenEcommerce.Application.IntegrationEvents.OrderPlacedIntegrationEvent");
        outboxMessage.ProcessedOn.Should().BeNull(); // Not yet processed by background service
    }

    [Fact]
    public async Task PlaceOrder_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var request = new PlaceOrderRequest(
            CustomerId: Guid.Empty, // Invalid
            ShippingAddress: new AddressDto(
                Street: "",
                City: "",
                State: "",
                ZipCode: "",
                Country: ""),
            Items: Array.Empty<OrderItemDto>()); // Empty items

        // Act
        var response = await _client.PostAsJsonAsync("/orders", request);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}

