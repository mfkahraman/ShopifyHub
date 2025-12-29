using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopifyHub.Domain.Entities;

public class SyncJobConfiguration : IEntityTypeConfiguration<SyncJob>
{
    public void Configure(EntityTypeBuilder<SyncJob> builder)
    {
        builder.ToTable("sync_jobs");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.Id)
            .UseIdentityColumn();

        builder.Property(j => j.EntityType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(j => j.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("pending");

        builder.Property(j => j.RecordsProcessed)
            .HasDefaultValue(0);

        builder.Property(j => j.RecordsFailed)
            .HasDefaultValue(0);

        builder.Property(j => j.ErrorMessage)
            .HasMaxLength(2000);

        builder.Property(j => j.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(j => j.StoreId, "ix_sync_jobs_store_id");

        builder.HasIndex(j => j.Status, "ix_sync_jobs_status");

        builder.HasIndex(j => j.EntityType, "ix_sync_jobs_entity_type");

        builder.HasIndex(j => j.CreatedAt, "ix_sync_jobs_created_at");

        builder.HasIndex(j => new { j.StoreId, j.Status }, "ix_sync_jobs_store_id_status");
    }
}