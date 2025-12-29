namespace ShopifyHub.Domain.Entities;

public class Store
{
    public int Id { get; set; }
    public string ShopifyDomain { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string StoreName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}