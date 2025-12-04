using System;
using CleanMovies.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CleanMovies.Infrastructure.Migrations
{
    [DbContext(typeof(MovieDbContext))]
    partial class MovieDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("CleanMovies.Domain.Entities.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("YearOfRelease")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.HasIndex("Title");

                    b.HasIndex("Title", "YearOfRelease")
                        .IsUnique();

                    b.HasIndex("YearOfRelease");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("CleanMovies.Domain.Entities.Movie", b =>
                {
                    b.OwnsMany("CleanMovies.Domain.ValueObjects.Genre", "Genres", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<Guid>("MovieId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.HasKey("Id");

                            b1.HasIndex("MovieId", "Name")
                                .IsUnique();

                            b1.ToTable("Genres");

                            b1.WithOwner()
                                .HasForeignKey("MovieId");
                        });

                    b.OwnsMany("CleanMovies.Domain.Entities.Rating", "Ratings", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<Guid>("MovieId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Value")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("UserId", "MovieId")
                                .IsUnique();

                            b1.HasIndex("MovieId");

                            b1.ToTable("Ratings");

                            b1.WithOwner()
                                .HasForeignKey("MovieId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
