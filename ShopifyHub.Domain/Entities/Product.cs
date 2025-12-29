namespace ShopifyHub.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public long ShopifyProductId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Vendor { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty;
    public string Handle { get; set; } = string.Empty;
    public string Status { get; set; } = "active";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastSyncedAt { get; set; }

    // Navigation properties
    public Store Store { get; set; } = null!;
    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
}