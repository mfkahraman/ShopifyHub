namespace ShopifyHub.Domain.Entities;

public class ProductVariant
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public long ShopifyVariantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? SKU { get; set; }
    public decimal Price { get; set; }
    public decimal? CompareAtPrice { get; set; }
    public string? Barcode { get; set; }
    public int? InventoryQuantity { get; set; }
    public string? InventoryPolicy { get; set; }
    public bool RequiresShipping { get; set; } = true;
    public decimal? Weight { get; set; }
    public string? WeightUnit { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public Product Product { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<InventoryHistory> InventoryHistories { get; set; } = new List<InventoryHistory>();
}