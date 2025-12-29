namespace ShopifyHub.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int? VariantId { get; set; }
    public long ShopifyLineItemId { get; set; }
    public long? ShopifyProductId { get; set; }
    public long? ShopifyVariantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string? SKU { get; set; }
    public bool RequiresShipping { get; set; }

    // Navigation properties
    public Order Order { get; set; } = null!;
    public ProductVariant? Variant { get; set; }
}