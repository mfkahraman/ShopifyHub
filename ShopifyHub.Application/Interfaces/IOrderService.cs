using ShopifyHub.Application.DTOs;

namespace ShopifyHub.Application.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllOrdersAsync(int storeId);
        Task<OrderDto?> GetOrderByIdAsync(int orderId);
        Task<List<OrderDto>> GetOrdersByStatusAsync(int storeId, string status);
        Task<SyncResultDto> SyncOrdersFromShopifyAsync(int storeId);
        Task<bool> MarkOrderAsFulfilledAsync(int orderId);
    }
}
