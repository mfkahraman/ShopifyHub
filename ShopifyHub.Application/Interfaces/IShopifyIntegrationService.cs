using ShopifyHub.Application.DTOs;

namespace ShopifyHub.Application.Interfaces
{
    public interface IShopifyIntegrationService
    {
        // OAuth
        string GetAuthorizationUrl(string shopDomain, string redirectUri, string state);
        Task<ShopifyAuthResponseDto> ExchangeCodeForTokenAsync(string shopDomain, string code);

        // Products
        Task<List<ProductDto>> GetProductsAsync(string shopDomain, string accessToken);
        Task<ProductDto?> GetProductByIdAsync(string shopDomain, string accessToken, long productId);
        Task<bool> UpdateInventoryAsync(string shopDomain, string accessToken, long variantId, int quantity);

        // Orders
        Task<List<OrderDto>> GetOrdersAsync(string shopDomain, string accessToken, DateTime? since = null);
        Task<OrderDto?> GetOrderByIdAsync(string shopDomain, string accessToken, long orderId);

        // Webhooks
        Task<bool> CreateWebhookAsync(string shopDomain, string accessToken, string topic, string address);
        Task<bool> VerifyWebhookAsync(string hmacHeader, string requestBody);
    }
}
