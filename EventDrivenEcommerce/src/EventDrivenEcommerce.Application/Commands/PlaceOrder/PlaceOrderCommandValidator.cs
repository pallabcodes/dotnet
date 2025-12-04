using FluentValidation;

namespace EventDrivenEcommerce.Application.Commands.PlaceOrder;

/// <summary>
/// Validator for place order commands.
/// </summary>
public sealed class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required");

        RuleFor(x => x.ShippingAddress)
            .NotNull().WithMessage("Shipping address is required")
            .ChildRules(address =>
            {
                address.RuleFor(a => a.Street)
                    .NotEmpty().WithMessage("Street address is required")
                    .MaximumLength(200).WithMessage("Street address must not exceed 200 characters");

                address.RuleFor(a => a.City)
                    .NotEmpty().WithMessage("City is required")
                    .MaximumLength(100).WithMessage("City must not exceed 100 characters");

                address.RuleFor(a => a.State)
                    .NotEmpty().WithMessage("State is required")
                    .MaximumLength(50).WithMessage("State must not exceed 50 characters");

                address.RuleFor(a => a.ZipCode)
                    .NotEmpty().WithMessage("Zip code is required")
                    .MaximumLength(20).WithMessage("Zip code must not exceed 20 characters");

                address.RuleFor(a => a.Country)
                    .NotEmpty().WithMessage("Country is required")
                    .MaximumLength(50).WithMessage("Country must not exceed 50 characters");
            });

        RuleFor(x => x.Items)
            .NotNull().WithMessage("Order items are required")
            .NotEmpty().WithMessage("Order must contain at least one item");

        RuleForEach(x => x.Items)
            .ChildRules(items =>
            {
                items.RuleFor(i => i.ProductId).NotEmpty().WithMessage("Product ID is required");
                items.RuleFor(i => i.ProductName).NotEmpty().WithMessage("Product name is required");
                items.RuleFor(i => i.UnitPrice).NotNull().WithMessage("Unit price is required");
                items.RuleFor(i => i.UnitPrice.Amount).GreaterThan(0).WithMessage("Unit price must be positive");
                items.RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("Quantity must be positive");
            });
    }
}

