namespace ShopifyHub.Domain.Entities;

public class InventoryHistory
{
    public int Id { get; set; }
    public int VariantId { get; set; }
    public int? OldQuantity { get; set; }
    public int NewQuantity { get; set; }
    public int QuantityChange { get; set; }
    public string ChangeType { get; set; } = string.Empty; // "sync", "manual", "order", "adjustment"
    public string? ChangedBy { get; set; }
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ProductVariant Variant { get; set; } = null!;
}