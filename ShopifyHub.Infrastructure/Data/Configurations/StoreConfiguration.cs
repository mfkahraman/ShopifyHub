using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopifyHub.Domain.Entities;

namespace ShopifyHub.Infrastructure.Data.Configurations;

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.ToTable("stores");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .UseIdentityColumn();

        builder.Property(s => s.ShopifyDomain)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(s => s.AccessToken)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(s => s.StoreName)
            .HasMaxLength(255);

        builder.Property(s => s.Email)
            .HasMaxLength(255);

        builder.Property(s => s.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(s => s.ShopifyDomain, "ix_stores_shopify_domain")
            .IsUnique();

        builder.HasMany(s => s.Products)
            .WithOne(p => p.Store)
            .HasForeignKey(p => p.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Orders)
            .WithOne(o => o.Store)
            .HasForeignKey(o => o.StoreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}