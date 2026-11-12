using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.Projects;
using Template.Domain.Projects.ValueObjects;

namespace Template.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.ComplexProperty(p => p.Name, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("Name")
                .HasMaxLength(ProjectName.MaxLength)
                .IsRequired();
        });

        builder.Property(p => p.Description)
            .HasMaxLength(2000);

        builder.Property(p => p.Status)
            .HasConversion(
                status => status.ToString(),
                value => Enum.Parse<ProjectStatus>(value))
            .HasMaxLength(20);

        builder.Property(p => p.RowVersion)
            .HasColumnName("RowVersion");
    }
}
