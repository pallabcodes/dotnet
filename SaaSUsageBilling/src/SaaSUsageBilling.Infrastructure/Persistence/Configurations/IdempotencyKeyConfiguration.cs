using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaaSUsageBilling.Domain.Entities;

namespace SaaSUsageBilling.Infrastructure.Persistence.Configurations;

public class IdempotencyKeyConfiguration : IEntityTypeConfiguration<IdempotencyKey>
{
    public void Configure(EntityTypeBuilder<IdempotencyKey> builder)
    {
        builder.HasKey(k => k.Id);

        builder.Property(k => k.Key)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(k => k.ProcessedOn)
            .IsRequired();

        builder.Property(k => k.ExpiresOn)
            .IsRequired();

        builder.HasIndex(k => k.Key)
            .IsUnique();

        builder.HasIndex(k => k.ExpiresOn);

        builder.Property(k => k.RowVersion)
            .IsRowVersion();
    }
}

