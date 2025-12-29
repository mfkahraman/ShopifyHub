using ShopifyHub.Application.DTOs;

namespace ShopifyHub.Application.Interfaces
{
    public interface IStoreService
    {
        Task<StoreDto?> GetStoreByIdAsync(int storeId);
        Task<StoreDto?> GetStoreByDomainAsync(string shopifyDomain);
        Task<StoreDto> CreateStoreAsync(string shopifyDomain, string accessToken, string storeName, string email);
        Task<bool> UpdateStoreAsync(int storeId, StoreDto storeDto);
        Task<bool> DeactivateStoreAsync(int storeId);
    }
}
