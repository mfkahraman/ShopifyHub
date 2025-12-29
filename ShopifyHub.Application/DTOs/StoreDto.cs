namespace ShopifyHub.Application.DTOs
{
    public class StoreDto
    {
        public int Id { get; set; }
        public string ShopifyDomain { get; set; } = string.Empty;
        public string StoreName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
