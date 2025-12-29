using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopifyHub.Domain.Entities;

public class WebhookLogConfiguration : IEntityTypeConfiguration<WebhookLog>
{
    public void Configure(EntityTypeBuilder<WebhookLog> builder)
    {
        builder.ToTable("webhook_logs");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .UseIdentityColumn();

        builder.Property(w => w.Topic)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(w => w.Payload)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(w => w.IsProcessed)
            .HasDefaultValue(false);

        builder.Property(w => w.Success)
            .HasDefaultValue(false);

        builder.Property(w => w.ErrorMessage)
            .HasMaxLength(2000);

        builder.Property(w => w.ReceivedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(w => w.Topic, "ix_webhook_logs_topic");

        builder.HasIndex(w => w.IsProcessed, "ix_webhook_logs_is_processed");

        builder.HasIndex(w => w.ReceivedAt, "ix_webhook_logs_received_at");

        builder.HasIndex(w => new { w.Topic, w.IsProcessed }, "ix_webhook_logs_topic_is_processed");

        builder.HasIndex(w => w.Payload, "ix_webhook_logs_payload")
            .HasMethod("gin");
    }
}