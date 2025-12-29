using ShopifyHub.Application.DTOs;

namespace ShopifyHub.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync(int storeId);
    }
}
