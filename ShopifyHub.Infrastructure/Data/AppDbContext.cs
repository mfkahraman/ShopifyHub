using Microsoft.EntityFrameworkCore;
using ShopifyHub.Domain.Entities;

namespace ShopifyHub.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<InventoryHistory> InventoryHistories => Set<InventoryHistory>();
    public DbSet<WebhookLog> WebhookLogs => Set<WebhookLog>();
    public DbSet<SyncJob> SyncJobs => Set<SyncJob>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // PostgreSQL specific: use lowercase table and column names convention
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Table names to snake_case (optional but common in PostgreSQL)
            // entity.SetTableName(entity.GetTableName()?.ToSnakeCase());

            // For now, keep PascalCase but you can enable snake_case if you prefer
        }

        // Apply all configurations from current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // PostgreSQL specific optimizations
        if (optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSnakeCaseNamingConvention(); // Optional: converts to snake_case
        }
    }
}