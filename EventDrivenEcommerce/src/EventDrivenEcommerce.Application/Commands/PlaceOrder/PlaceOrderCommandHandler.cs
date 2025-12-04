using EventDrivenEcommerce.Application.Common;
using EventDrivenEcommerce.Domain.Entities;
using EventDrivenEcommerce.Domain.Repositories;
using EventDrivenEcommerce.Domain.ValueObjects;
using MediatR;

namespace EventDrivenEcommerce.Application.Commands.PlaceOrder;

/// <summary>
/// Handler for processing place order commands.
/// </summary>
public sealed class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Result<OrderId>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PlaceOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<OrderId>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = Order.Create(request.CustomerId, request.ShippingAddress, request.Items);

            await _orderRepository.AddAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Note: Domain events will be published by the infrastructure layer
            // through the outbox pattern for reliable event publishing

            return Result<OrderId>.Success(order.OrderId);
        }
        catch (Exception ex)
        {
            return Result<OrderId>.Failure($"Failed to place order: {ex.Message}");
        }
    }
}

