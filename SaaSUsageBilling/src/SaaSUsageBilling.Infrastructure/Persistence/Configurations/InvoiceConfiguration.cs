using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaaSUsageBilling.Domain.Entities;

namespace SaaSUsageBilling.Infrastructure.Persistence.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.SubscriptionId)
            .IsRequired();

        builder.OwnsOne(i => i.Period, period =>
        {
            period.Property(p => p.From).HasColumnName("PeriodFrom").IsRequired();
            period.Property(p => p.To).HasColumnName("PeriodTo").IsRequired();
        });

        builder.OwnsOne(i => i.Total, money =>
        {
            money.Property(m => m.Amount).HasColumnName("TotalAmount").IsRequired();
            money.Property(m => m.Currency).HasColumnName("TotalCurrency").HasMaxLength(3).IsRequired();
        });

        builder.OwnsMany(i => i.Lines, line =>
        {
            line.WithOwner().HasForeignKey("InvoiceId");
            line.HasKey("InvoiceId", "Description");

            line.Property(l => l.Description)
                .HasMaxLength(500)
                .IsRequired();

            line.OwnsOne(l => l.Amount, money =>
            {
                money.Property(m => m.Amount).HasColumnName("LineAmount").IsRequired();
                money.Property(m => m.Currency).HasColumnName("LineCurrency").HasMaxLength(3).IsRequired();
            });
        });

        builder.HasOne<Subscription>()
            .WithMany()
            .HasForeignKey(i => i.SubscriptionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(i => new { i.SubscriptionId, i.Period });

        builder.Property(i => i.RowVersion)
            .IsRowVersion();
    }
}

