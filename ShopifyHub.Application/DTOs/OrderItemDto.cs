namespace ShopifyHub.Application.DTOs
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? SKU { get; set; }
    }
}
