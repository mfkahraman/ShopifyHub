namespace ShopifyHub.Application.DTOs
{
    public class InventoryUpdateDto
    {
        public int VariantId { get; set; }
        public int NewQuantity { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
