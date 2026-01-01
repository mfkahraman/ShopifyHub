using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopifyHub.Application.Interfaces;
using ShopifyHub.Infrastructure.Data;

namespace ShopifyHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IShopifyIntegrationService shopifyService,
                                    ILogger<ProductsController> logger,
                                    AppDbContext context)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 50, [FromQuery] string? search = null)
        {
            try
            {
                var query = context.Products
                    .Include(p => p.Variants)
                        .ThenInclude(v => v.InventoryHistories)
                    .Include(p => p.Store)
                    .AsQueryable();

                // Search filter
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(p =>
                        p.Title.Contains(search) ||
                        p.Variants.Any(v => v.SKU != null && v.SKU.Contains(search)));
                }

                var totalCount = await query.CountAsync();

                var products = await query
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new
                    {
                        p.Id,
                        p.ShopifyProductId,
                        p.Title,
                        p.Description,
                        p.ProductType,
                        p.Vendor,
                        p.Status,
                        p.CreatedAt,
                        p.UpdatedAt,
                        Store = new
                        {
                            p.Store.Id,
                            p.Store.StoreName,
                            p.Store.ShopifyDomain
                        },
                        Variants = p.Variants.Select(v => new
                        {
                            v.Id,
                            v.ShopifyVariantId,
                            v.Title,
                            v.SKU,
                            v.Price,
                            v.CompareAtPrice,
                            v.Barcode,
                            Inventory = v.InventoryHistories != null ? new
                            {
                                v.InventoryHistories.Quantity,
                                v.Inventory.ReservedQuantity,
                                Available = v.Inventory.Quantity - v.Inventory.ReservedQuantity,
                                v.Inventory.LastSyncedAt
                            } : null
                        }).ToList()
                    })
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    data = products,
                    pagination = new
                    {
                        page,
                        pageSize,
                        totalCount,
                        totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                    }
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving products");
                return StatusCode(500, new { error = "Failed to retrieve products", details = ex.Message });
            }
        }
    }
}
