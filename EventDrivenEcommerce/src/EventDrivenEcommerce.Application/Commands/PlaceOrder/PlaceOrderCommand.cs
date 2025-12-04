using EventDrivenEcommerce.Application.Common;
using EventDrivenEcommerce.Domain.ValueObjects;
using MediatR;

namespace EventDrivenEcommerce.Application.Commands.PlaceOrder;

/// <summary>
/// Command to place a new order.
/// </summary>
public sealed record PlaceOrderCommand(
    CustomerId CustomerId,
    Address ShippingAddress,
    IReadOnlyCollection<OrderItem> Items) : IRequest<Result<OrderId>>;

