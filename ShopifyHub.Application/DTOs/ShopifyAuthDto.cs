namespace ShopifyHub.Application.DTOs
{
    public class ShopifyAuthDto
    {
        public string ShopDomain { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}
