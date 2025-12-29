using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopifyHub.Domain.Entities;

public class InventoryHistoryConfiguration : IEntityTypeConfiguration<InventoryHistory>
{
    public void Configure(EntityTypeBuilder<InventoryHistory> builder)
    {
        builder.ToTable("inventory_histories");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id)
            .UseIdentityColumn();

        builder.Property(h => h.NewQuantity)
            .IsRequired();

        builder.Property(h => h.QuantityChange)
            .IsRequired();

        builder.Property(h => h.ChangeType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(h => h.ChangedBy)
            .HasMaxLength(255);

        builder.Property(h => h.Reason)
            .HasMaxLength(1000);

        builder.Property(h => h.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(h => h.VariantId, "ix_inventory_histories_variant_id");

        builder.HasIndex(h => h.CreatedAt, "ix_inventory_histories_created_at");

        builder.HasIndex(h => h.ChangeType, "ix_inventory_histories_change_type");

        builder.HasIndex(h => new { h.VariantId, h.CreatedAt }, "ix_inventory_histories_variant_id_created_at");
    }
}