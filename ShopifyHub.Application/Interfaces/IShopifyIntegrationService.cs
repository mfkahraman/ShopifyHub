// Path: ShopifyHub.Application/Interfaces/IShopifyIntegrationService.cs

using ShopifyHub.Application.DTOs;

namespace ShopifyHub.Application.Interfaces;

public interface IShopifyIntegrationService
{
    // OAuth - Not deprecated
    string GetAuthorizationUrl(string shopDomain, string redirectUri, string state);
    Task<ShopifyAuthResponseDto> ExchangeCodeForTokenAsync(string shopDomain, string code);

    // Inventory - Not deprecated (uses InventoryLevelService which is still supported)
    Task<bool> UpdateInventoryAsync(string shopDomain, string accessToken, long variantId, int quantity);

    // Orders - Not deprecated
    Task<List<OrderDto>> GetOrdersAsync(string shopDomain, string accessToken, DateTime? since = null);
    Task<OrderDto?> GetOrderByIdAsync(string shopDomain, string accessToken, long orderId);

    // Webhooks - Not deprecated
    Task<bool> CreateWebhookAsync(string shopDomain, string accessToken, string topic, string address);
    Task<bool> VerifyWebhookAsync(string hmacHeader, string requestBody);
}