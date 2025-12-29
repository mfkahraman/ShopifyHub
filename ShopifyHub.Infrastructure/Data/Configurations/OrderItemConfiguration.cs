using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopifyHub.Domain.Entities;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .UseIdentityColumn();

        builder.Property(i => i.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(i => i.Quantity)
            .IsRequired();

        builder.Property(i => i.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.SKU)
            .HasMaxLength(255);

        builder.Property(i => i.RequiresShipping)
            .HasDefaultValue(true);

        builder.HasIndex(i => i.OrderId, "IX_OrderItems_OrderId");
        builder.HasIndex(i => i.VariantId, "IX_OrderItems_VariantId");

        builder.HasOne(i => i.Variant)
            .WithMany(v => v.OrderItems)
            .HasForeignKey(i => i.VariantId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}