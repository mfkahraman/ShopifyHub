namespace ShopifyHub.Domain.Entities;

public class WebhookLog
{
    public int Id { get; set; }
    public string Topic { get; set; } = string.Empty;
    public long? ShopifyResourceId { get; set; }
    public string Payload { get; set; } = string.Empty;
    public bool IsProcessed { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
}