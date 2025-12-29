using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopifyHub.Domain.Entities;

namespace ShopifyHub.Infrastructure.Data.Configurations;

public class SyncJobConfiguration : IEntityTypeConfiguration<SyncJob>
{
    public void Configure(EntityTypeBuilder<SyncJob> builder)
    {
        builder.ToTable("SyncJobs");

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

        // Indexes for monitoring and reporting
        builder.HasIndex(j => j.StoreId, "IX_SyncJobs_StoreId");

        builder.HasIndex(j => j.Status, "IX_SyncJobs_Status");

        builder.HasIndex(j => j.EntityType, "IX_SyncJobs_EntityType");

        builder.HasIndex(j => j.CreatedAt, "IX_SyncJobs_CreatedAt");

        builder.HasIndex(j => new { j.StoreId, j.Status }, "IX_SyncJobs_StoreId_Status");
    }
}