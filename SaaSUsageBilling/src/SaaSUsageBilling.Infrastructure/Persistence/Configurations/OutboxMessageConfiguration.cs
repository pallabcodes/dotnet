using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaaSUsageBilling.Domain.Entities;

namespace SaaSUsageBilling.Infrastructure.Persistence.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Type)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(m => m.Content)
            .IsRequired();

        builder.Property(m => m.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(m => m.CreatedOn)
            .IsRequired();

        builder.Property(m => m.ProcessedOn);

        builder.Property(m => m.Error)
            .HasMaxLength(2000);

        builder.Property(m => m.RetryCount)
            .IsRequired();

        builder.HasIndex(m => m.Status);
        builder.HasIndex(m => m.CreatedOn);

        builder.Property(m => m.RowVersion)
            .IsRowVersion();
    }
}

