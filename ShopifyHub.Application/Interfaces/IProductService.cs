using ShopifyHub.Application.DTOs;

namespace ShopifyHub.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync(int storeId);
        Task<ProductDto?> GetProductByIdAsync(int productId);
        Task<List<ProductDto>> GetLowStockProductsAsync(int storeId, int threshold = 10);
        Task<SyncResultDto> SyncProductsFromShopifyAsync(int storeId);
        Task<bool> UpdateInventoryAsync(int variantId, int quantity, string reason, string changedBy);
    }
}
