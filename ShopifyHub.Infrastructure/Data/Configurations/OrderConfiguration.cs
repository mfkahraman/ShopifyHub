using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopifyHub.Domain.Entities;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .UseIdentityColumn();

        builder.Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(o => o.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(o => o.TotalPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(o => o.SubtotalPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(o => o.TotalTax)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(o => o.Currency)
            .HasMaxLength(10)
            .HasDefaultValue("USD");

        builder.Property(o => o.FinancialStatus)
            .HasMaxLength(50);

        builder.Property(o => o.FulfillmentStatus)
            .HasMaxLength(50);

        builder.Property(o => o.CustomerName)
            .HasMaxLength(255);

        builder.Property(o => o.CustomerPhone)
            .HasMaxLength(50);

        builder.Property(o => o.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(o => new { o.StoreId, o.ShopifyOrderId }, "ix_orders_store_id_shopify_order_id")
            .IsUnique();

        builder.HasIndex(o => o.OrderNumber, "ix_orders_order_number");

        builder.HasIndex(o => o.Email, "ix_orders_email");

        builder.HasIndex(o => o.FinancialStatus, "ix_orders_financial_status");

        builder.HasIndex(o => o.CreatedAt, "ix_orders_created_at");

        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}