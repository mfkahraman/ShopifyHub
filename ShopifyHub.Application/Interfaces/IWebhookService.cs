namespace ShopifyHub.Application.Interfaces
{
    public interface IWebhookService
    {
        Task ProcessProductCreateWebhookAsync(string payload, int storeId);
        Task ProcessProductUpdateWebhookAsync(string payload, int storeId);
        Task ProcessOrderCreateWebhookAsync(string payload, int storeId);
        Task ProcessOrderUpdateWebhookAsync(string payload, int storeId);
        Task LogWebhookAsync(string topic, string payload, bool success, string? errorMessage = null);
    }
}
