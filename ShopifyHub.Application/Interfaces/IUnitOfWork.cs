using ShopifyHub.Domain.Entities;

namespace ShopifyHub.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Store> Stores { get; }
        IRepository<Product> Products { get; }
        IRepository<ProductVariant> ProductVariants { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderItem> OrderItems { get; }
        IRepository<InventoryHistory> InventoryHistories { get; }
        IRepository<WebhookLog> WebhookLogs { get; }
        IRepository<SyncJob> SyncJobs { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
