using EventDrivenEcommerce.Domain.Entities;
using EventDrivenEcommerce.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventDrivenEcommerce.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Order aggregate.
/// </summary>
public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.OrderId)
            .HasConversion(orderId => orderId.Value, value => new OrderId(value))
            .IsRequired();

        builder.Property(o => o.CustomerId)
            .HasConversion(customerId => customerId.Value, value => new CustomerId(value))
            .IsRequired();

        builder.OwnsOne(o => o.ShippingAddress, address =>
        {
            address.Property(a => a.Street).HasMaxLength(200).IsRequired();
            address.Property(a => a.City).HasMaxLength(100).IsRequired();
            address.Property(a => a.State).HasMaxLength(50).IsRequired();
            address.Property(a => a.ZipCode).HasMaxLength(20).IsRequired();
            address.Property(a => a.Country).HasMaxLength(50).IsRequired();
        });

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.OwnsOne(o => o.TotalAmount, money =>
        {
            money.Property(m => m.Amount).HasColumnName("TotalAmount").IsRequired();
            money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
        });

        builder.OwnsMany(o => o.Items, item =>
        {
            item.ToTable("OrderItems");
            item.WithOwner().HasForeignKey("OrderId");
            item.Property<int>("Id");
            item.HasKey("Id");

            item.Property(i => i.ProductId)
                .HasConversion(productId => productId.Value, value => new ProductId(value))
                .IsRequired();

            item.Property(i => i.ProductName).HasMaxLength(200).IsRequired();

            item.OwnsOne(i => i.UnitPrice, price =>
            {
                price.Property(p => p.Amount).HasColumnName("UnitPrice").IsRequired();
                price.Property(p => p.Currency).HasColumnName("UnitPriceCurrency").HasMaxLength(3).IsRequired();
            });

            item.Property(i => i.Quantity).IsRequired();
        });

        builder.HasIndex(o => o.OrderId).IsUnique();
        builder.HasIndex(o => o.CustomerId);
        builder.HasIndex(o => o.Status);
    }
}

