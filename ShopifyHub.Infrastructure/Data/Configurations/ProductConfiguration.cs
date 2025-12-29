using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopifyHub.Domain.Entities;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .UseIdentityColumn();

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.Description)
            .HasMaxLength(5000);

        builder.Property(p => p.Vendor)
            .HasMaxLength(255);

        builder.Property(p => p.ProductType)
            .HasMaxLength(255);

        builder.Property(p => p.Handle)
            .HasMaxLength(255);

        builder.Property(p => p.Status)
            .HasMaxLength(50)
            .HasDefaultValue("active");

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(p => new { p.StoreId, p.ShopifyProductId }, "IX_Products_StoreId_ShopifyProductId");
        builder.HasIndex(p => p.Status, "IX_Products_Status");

        builder.HasMany(p => p.Variants)
            .WithOne(v => v.Product)
            .HasForeignKey(v => v.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}