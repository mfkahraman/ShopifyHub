namespace ShopifyHub.Application.DTOs
{
    public class ProductVariantDto
    {
        public int Id { get; set; }
        public long ShopifyVariantId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? SKU { get; set; }
        public decimal Price { get; set; }
        public decimal? CompareAtPrice { get; set; }
        public int? InventoryQuantity { get; set; }
        public string? Barcode { get; set; }
        public bool RequiresShipping { get; set; }
    }
}