using FluentMigrator;

namespace Movies.Application.Database.Migrations;

[Migration(20240101000001, "Initial schema - Creates movies, genres, and ratings tables")]
public class InitialSchema : Migration
{
    public override void Up()
    {
        Create.Table("movies")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("slug").AsString().NotNullable()
            .WithColumn("title").AsString().NotNullable()
            .WithColumn("yearofrelease").AsInt32().NotNullable();

        Create.Index("movies_slug_idx")
            .OnTable("movies")
            .OnColumn("slug")
            .Unique();

        Create.Table("genres")
            .WithColumn("movieid").AsGuid().NotNullable()
            .WithColumn("name").AsString().NotNullable();

        Create.ForeignKey("fk_genres_movies")
            .FromTable("genres").ForeignColumn("movieid")
            .ToTable("movies").PrimaryColumn("id")
            .OnDelete(System.Data.Rule.Cascade);

        Create.Table("ratings")
            .WithColumn("userid").AsGuid().NotNullable()
            .WithColumn("movieid").AsGuid().NotNullable()
            .WithColumn("rating").AsInt32().NotNullable();

        Create.PrimaryKey("pk_ratings")
            .OnTable("ratings")
            .Columns("userid", "movieid");

        Create.ForeignKey("fk_ratings_movies")
            .FromTable("ratings").ForeignColumn("movieid")
            .ToTable("movies").PrimaryColumn("id")
            .OnDelete(System.Data.Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("ratings");
        Delete.Table("genres");
        Delete.Table("movies");
    }
}

