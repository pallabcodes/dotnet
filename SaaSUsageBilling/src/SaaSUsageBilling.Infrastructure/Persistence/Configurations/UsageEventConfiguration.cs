using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaaSUsageBilling.Domain.Entities;

namespace SaaSUsageBilling.Infrastructure.Persistence.Configurations;

public class UsageEventConfiguration : IEntityTypeConfiguration<UsageEvent>
{
    public void Configure(EntityTypeBuilder<UsageEvent> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.SubscriptionId)
            .IsRequired();

        builder.Property(u => u.Quantity)
            .IsRequired();

        builder.Property(u => u.OccurredAt)
            .IsRequired();

        builder.HasOne<Subscription>()
            .WithMany()
            .HasForeignKey(u => u.SubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(u => new { u.SubscriptionId, u.OccurredAt });
        builder.HasIndex(u => u.OccurredAt);

        builder.Property(u => u.RowVersion)
            .IsRowVersion();
    }
}

