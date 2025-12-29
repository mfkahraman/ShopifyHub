using ShopifyHub.Application.DTOs;

namespace ShopifyHub.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendLowStockAlertAsync(List<ProductVariantDto> lowStockItems, string recipientEmail);
        Task SendOrderNotificationAsync(OrderDto order, string recipientEmail);
        Task SendSyncCompletedNotificationAsync(SyncResultDto result, string recipientEmail);
    }
}
