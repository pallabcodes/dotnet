using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaaSUsageBilling.Domain.Entities;

namespace SaaSUsageBilling.Infrastructure.Persistence.Configurations;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsOne(p => p.MonthlyBase, money =>
        {
            money.Property(m => m.Amount).HasColumnName("MonthlyBaseAmount").IsRequired();
            money.Property(m => m.Currency).HasColumnName("MonthlyBaseCurrency").HasMaxLength(3).IsRequired();
        });

        builder.Property(p => p.IsActive)
            .IsRequired();

        builder.HasIndex(p => p.IsActive);

        builder.HasIndex(p => p.Name)
            .IsUnique();

        builder.Property(p => p.RowVersion)
            .IsRowVersion();

        builder.OwnsMany(p => p.PricingTiers, tier =>
        {
            tier.WithOwner().HasForeignKey("PlanId");
            tier.HasKey("PlanId", "MinUnits");

            tier.Property(t => t.MinUnits).HasColumnName("TierMinUnits");
            tier.Property(t => t.MaxUnits).HasColumnName("TierMaxUnits");

            tier.OwnsOne(t => t.PricePerUnit, money =>
            {
                money.Property(m => m.Amount).HasColumnName("TierPriceAmount").IsRequired();
                money.Property(m => m.Currency).HasColumnName("TierPriceCurrency").HasMaxLength(3).IsRequired();
            });
        });

        builder.OwnsMany(p => p.Discounts, discount =>
        {
            discount.WithOwner().HasForeignKey("PlanId");
            discount.HasKey("PlanId", "Description");

            discount.Property(d => d.Description).HasMaxLength(500).IsRequired();
            discount.Property(d => d.ValidUntil).HasColumnName("DiscountValidUntil");
        });

        builder.OwnsMany(p => p.ApplicableTaxes, tax =>
        {
            tax.WithOwner().HasForeignKey("PlanId");
            tax.HasKey("PlanId", "Name");

            tax.Property(t => t.Name).HasMaxLength(100).IsRequired();
            tax.Property(t => t.Rate).IsRequired();
            tax.Property(t => t.Region).HasMaxLength(100);
        });
    }
}

