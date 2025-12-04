using CleanMovies.Domain.Entities;
using CleanMovies.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanMovies.Infrastructure.Persistence.Configurations;

public sealed class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.Description)
            .HasMaxLength(2000);

        builder.Property(m => m.YearOfRelease)
            .IsRequired();

        builder.Property(m => m.Slug)
            .HasConversion(s => s.Value, v => Slug.FromExisting(v))
            .IsRequired()
            .HasMaxLength(250);

        builder.HasIndex(m => m.Slug).IsUnique();
        builder.HasIndex(m => m.Title);
        builder.HasIndex(m => m.YearOfRelease);
        builder.HasIndex(m => new { m.Title, m.YearOfRelease }).IsUnique();

        builder.OwnsMany(m => m.Genres, g =>
        {
            g.ToTable("Genres");
            g.WithOwner().HasForeignKey("MovieId");
            g.Property<Guid>("MovieId");
            g.Property<int>("Id");
            g.HasKey("Id");
            g.Property(p => p.Name).HasMaxLength(100).IsRequired();
            g.HasIndex("MovieId", nameof(Genre.Name)).IsUnique();
        });

        builder.OwnsMany(m => m.Ratings, r =>
        {
            r.ToTable("Ratings");
            r.WithOwner().HasForeignKey("MovieId");
            r.Ignore(x => x.Id);
            r.HasKey(x => new { x.MovieId, x.UserId });
            r.Property(x => x.MovieId).IsRequired();
            r.Property(x => x.UserId)
                .HasConversion(u => u.Value, v => new UserId(v))
                .IsRequired();
            r.Property(x => x.Value).IsRequired();
        });
    }
}
