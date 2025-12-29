using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopifyHub.Domain.Entities;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

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

        builder.HasIndex(o => new { o.StoreId, o.ShopifyOrderId }, "IX_Orders_StoreId_ShopifyOrderId");
        builder.HasIndex(o => o.OrderNumber, "IX_Orders_OrderNumber");
        builder.HasIndex(o => o.Email, "IX_Orders_Email");
        builder.HasIndex(o => o.FinancialStatus, "IX_Orders_FinancialStatus");
        builder.HasIndex(o => o.CreatedAt, "IX_Orders_CreatedAt");

        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}