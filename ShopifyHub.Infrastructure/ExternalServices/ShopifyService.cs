using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShopifyHub.Application.DTOs;
using ShopifyHub.Application.Interfaces;
using ShopifySharp;
using ShopifySharp.Filters;
using ShopifySharp.Utilities;

namespace ShopifyHub.Infrastructure.ExternalServices;

public class ShopifyService(IConfiguration configuration, ILogger<ShopifyService> logger) : IShopifyIntegrationService
{
    private readonly string _clientId = configuration["Shopify:ClientId"] ?? throw new ArgumentNullException(nameof(configuration));
    private readonly string _clientSecret = configuration["Shopify:ClientSecret"] ?? throw new ArgumentNullException(nameof(configuration));
    private readonly string _apiVersion = configuration["Shopify:ApiVersion"] ?? "2024-10";
    private readonly ILogger<ShopifyService> _logger = logger;

    #region OAuth Methods (Not Deprecated)

    public string GetAuthorizationUrl(string shopDomain, string redirectUri, string state)
    {
        // Collection initialization simplified
        var scopes = new List<string>
        {
            "read_orders",
            "write_orders",
            "read_inventory",
            "write_inventory"
        };

        var options = new AuthorizationUrlOptions
        {
            Scopes = scopes,
            ShopDomain = shopDomain,
            ClientId = _clientId,
            RedirectUrl = redirectUri,
            State = state
        };

        var oauthUtility = new ShopifyOauthUtility();
        var authorizationUrl = oauthUtility.BuildAuthorizationUrl(options);

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Generated authorization URL for shop: {ShopDomain}", shopDomain);
        }

        return authorizationUrl.ToString();
    }

    public async Task<ShopifyAuthResponseDto> ExchangeCodeForTokenAsync(string shopDomain, string code)
    {
        try
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Exchanging authorization code for access token. Shop: {ShopDomain}", shopDomain);
            }

            var oauthUtility = new ShopifyOauthUtility();

            var result = await oauthUtility.AuthorizeAsync(
                code,
                shopDomain,
                _clientId,
                _clientSecret
            );

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Successfully obtained access token for shop: {ShopDomain}", shopDomain);
            }

            return new ShopifyAuthResponseDto
            {
                AccessToken = result.AccessToken,
                Scope = "read_orders,write_orders,read_inventory,write_inventory"
            };
        }
        catch (Exception ex)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(ex, "Failed to exchange code for token. Shop: {ShopDomain}", shopDomain);
            }
            throw;
        }
    }

    #endregion

    #region Inventory Methods (Not Deprecated)

    public async Task<bool> UpdateInventoryAsync(string shopDomain, string accessToken, long variantId, int quantity)
    {
        try
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Updating inventory for variant {VariantId} to {Quantity}", variantId, quantity);
            }

            var inventoryService = new InventoryLevelService(shopDomain, accessToken);

            var inventoryLevels = await inventoryService.ListAsync(new InventoryLevelListFilter());

            var inventoryLevel = inventoryLevels.Items.FirstOrDefault();

            if (inventoryLevel?.LocationId == null)
            {
                if (_logger.IsEnabled(LogLevel.Warning))
                {
                    _logger.LogWarning("No inventory location found for variant {VariantId}", variantId);
                }
                return false;
            }

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Successfully updated inventory for variant {VariantId}", variantId);
            }
            return true;
        }
        catch (Exception ex)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(ex, "Failed to update inventory for variant {VariantId}", variantId);
            }
            return false;
        }
    }

    #endregion

    #region Order Methods (Not Deprecated)

    public async Task<List<OrderDto>> GetOrdersAsync(string shopDomain, string accessToken, DateTime? since = null)
    {
        try
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Fetching orders from Shopify. Shop: {ShopDomain}", shopDomain);
            }

            var orderService = new OrderService(shopDomain, accessToken);

            var filter = new OrderListFilter
            {
                Limit = 250,
                Status = "any"
            };

            if (since.HasValue)
            {
                filter.CreatedAtMin = new DateTimeOffset(since.Value);
            }

            var orders = await orderService.ListAsync(filter);

            var orderDtos = orders.Items.Select(o => new OrderDto
            {
                ShopifyOrderId = o.Id ?? 0,
                OrderNumber = o.Name ?? string.Empty,
                Email = o.Email ?? string.Empty,
                TotalPrice = o.TotalPrice ?? 0,
                Currency = o.Currency ?? "USD",
                FinancialStatus = o.FinancialStatus ?? string.Empty,
                FulfillmentStatus = o.FulfillmentStatus ?? string.Empty,
                CustomerName = o.Customer?.FirstName + " " + o.Customer?.LastName,
                CreatedAt = o.CreatedAt?.UtcDateTime ?? DateTime.UtcNow,
                Items = o.LineItems?.Select(li => new OrderItemDto
                {
                    Title = li.Title ?? string.Empty,
                    Quantity = li.Quantity ?? 0,
                    Price = li.Price ?? 0,
                    SKU = li.SKU
                }).ToList() ?? new List<OrderItemDto>()
            }).ToList();

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Successfully fetched {Count} orders from Shopify", orderDtos.Count);
            }

            return orderDtos;
        }
        catch (Exception ex)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(ex, "Failed to fetch orders from Shopify. Shop: {ShopDomain}", shopDomain);
            }
            throw;
        }
    }

    public async Task<OrderDto?> GetOrderByIdAsync(string shopDomain, string accessToken, long orderId)
    {
        try
        {
            var orderService = new OrderService(shopDomain, accessToken);
            var order = await orderService.GetAsync(orderId);

            if (order == null) return null;

            return new OrderDto
            {
                ShopifyOrderId = order.Id ?? 0,
                OrderNumber = order.Name ?? string.Empty,
                Email = order.Email ?? string.Empty,
                TotalPrice = order.TotalPrice ?? 0,
                Currency = order.Currency ?? "USD",
                FinancialStatus = order.FinancialStatus ?? string.Empty,
                FulfillmentStatus = order.FulfillmentStatus ?? string.Empty,
                CustomerName = order.Customer?.FirstName + " " + order.Customer?.LastName,
                CreatedAt = order.CreatedAt?.UtcDateTime ?? DateTime.UtcNow,
                Items = order.LineItems?.Select(li => new OrderItemDto
                {
                    Title = li.Title ?? string.Empty,
                    Quantity = li.Quantity ?? 0,
                    Price = li.Price ?? 0,
                    SKU = li.SKU
                }).ToList() ?? new List<OrderItemDto>()
            };
        }
        catch (Exception ex)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(ex, "Failed to fetch order {OrderId} from Shopify", orderId);
            }
            throw;
        }
    }

    #endregion

    #region Webhook Methods (Not Deprecated)

    public async Task<bool> CreateWebhookAsync(string shopDomain, string accessToken, string topic, string address)
    {
        try
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Creating webhook for topic {Topic}", topic);
            }

            var webhookService = new WebhookService(shopDomain, accessToken);

            var webhook = await webhookService.CreateAsync(new ShopifySharp.Webhook
            {
                Topic = topic,
                Address = address,
                Format = "json"
            });

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Successfully created webhook {WebhookId} for topic {Topic}", webhook.Id, topic);
            }
            return true;
        }
        catch (Exception ex)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(ex, "Failed to create webhook for topic {Topic}", topic);
            }
            return false;
        }
    }

    public async Task<bool> VerifyWebhookAsync(string hmacHeader, string requestBody)
    {
        try
        {
            var headers = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "X-Shopify-Hmac-Sha256", new Microsoft.Extensions.Primitives.StringValues(hmacHeader) }
            };

            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(requestBody));

            var validationUtility = new ShopifyRequestValidationUtility();
            var isValid = await validationUtility.IsAuthenticWebhookAsync(headers, stream, _clientSecret);

            if (!isValid)
            {
                if (_logger.IsEnabled(LogLevel.Warning))
                {
                    _logger.LogWarning("Webhook HMAC verification failed");
                }
            }

            return isValid;
        }
        catch (Exception ex)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(ex, "Error verifying webhook HMAC");
            }
            return false;
        }
    }

    #endregion
}