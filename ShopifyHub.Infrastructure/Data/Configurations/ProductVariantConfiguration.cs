using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopifyHub.Domain.Entities;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("ProductVariants");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .UseIdentityColumn();

        builder.Property(v => v.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(v => v.SKU)
            .HasMaxLength(255);

        builder.Property(v => v.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(v => v.CompareAtPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(v => v.Barcode)
            .HasMaxLength(255);

        builder.Property(v => v.Weight)
            .HasColumnType("decimal(10,2)");

        builder.Property(v => v.WeightUnit)
            .HasMaxLength(50);

        builder.Property(v => v.RequiresShipping)
            .HasDefaultValue(true);

        builder.Property(v => v.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(v => new { v.ProductId, v.ShopifyVariantId }, "IX_ProductVariants_ProductId_ShopifyVariantId");
        builder.HasIndex(v => v.SKU, "IX_ProductVariants_SKU");
        builder.HasIndex(v => v.InventoryQuantity, "IX_ProductVariants_InventoryQuantity");
    }
}
