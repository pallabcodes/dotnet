using Microsoft.EntityFrameworkCore;
using Template.Domain.Projects;

namespace Template.Infrastructure.Persistence;

public class TemplateDbContext : DbContext
{
    public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options) { }

    public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TemplateDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
