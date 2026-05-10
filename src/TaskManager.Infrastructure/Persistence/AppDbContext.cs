using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence;

/// <summary>
/// Single DbContext for the application. On Day 1 we configure the two
/// core entities and verify the DB can be created. Subsequent days will
/// add global query filters, owned types, and more configuration here.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Load all IEntityTypeConfiguration<T> classes in this assembly
        // so each entity keeps its own configuration file (added Day 2+).
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        ConfigureCategory(modelBuilder);
        ConfigureTaskItem(modelBuilder);
    }

    private static void ConfigureCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(c => c.Description)
                  .HasMaxLength(500);

            // Unique index: no two categories can share a name.
            entity.HasIndex(c => c.Name).IsUnique();
        });
    }

    private static void ConfigureTaskItem(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.ToTable("Tasks");
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(t => t.Description)
                  .HasMaxLength(2000);

            // Store the enum as a string so the DB is readable without
            // looking up integer mappings. Slight storage cost, big DX gain.
            entity.Property(t => t.Status)
                  .HasConversion<string>()
                  .HasMaxLength(20);

            // Index DueDate so filtering/sorting by it stays fast.
            entity.HasIndex(t => t.DueDate);

            // Relationship stub — fully configured on Day 5.
            entity.HasOne(t => t.Category)
                  .WithMany(c => c.Tasks)
                  .HasForeignKey(t => t.CategoryId)
                  .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
