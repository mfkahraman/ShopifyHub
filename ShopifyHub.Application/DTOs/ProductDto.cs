namespace ShopifyHub.Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public long ShopifyProductId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Vendor { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastSyncedAt { get; set; }
        public List<ProductVariantDto> Variants { get; set; } = new();
    }
}
