namespace ShopifyHub.Domain.Entities;

public class SyncJob
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public string EntityType { get; set; } = string.Empty; // "products", "orders"
    public string Status { get; set; } = "pending"; // pending, running, completed, failed
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int RecordsProcessed { get; set; }
    public int RecordsFailed { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}