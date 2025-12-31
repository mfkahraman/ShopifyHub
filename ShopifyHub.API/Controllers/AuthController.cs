using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopifyHub.Application.Interfaces;
using ShopifyHub.Infrastructure.Data;

namespace ShopifyHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IShopifyIntegrationService _shopifyService;
    private readonly AppDbContext _context;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IShopifyIntegrationService shopifyService,
        AppDbContext context,
        ILogger<AuthController> logger)
    {
        _shopifyService = shopifyService;
        _context = context;
        _logger = logger;
    }

    [HttpGet("install")]
    public IActionResult Install([FromQuery] string shop)
    {
        if (string.IsNullOrEmpty(shop))
        {
            return BadRequest("Shop parameter is required");
        }

        // Ensure shop domain is in correct format
        if (!shop.Contains(".myshopify.com"))
        {
            shop = $"{shop}.myshopify.com";
        }

        _logger.LogInformation("Generating authorization URL for shop: {Shop}", shop);

        // Generate a random state for security
        var state = Guid.NewGuid().ToString();

        // Build the OAuth authorization URL
        var redirectUri = $"{Request.Scheme}://{Request.Host}/api/auth/callback";
        var authUrl = _shopifyService.GetAuthorizationUrl(shop, redirectUri, state);

        _logger.LogInformation("Redirecting to Shopify authorization URL");

        return Ok(new { authorizationUrl = authUrl });
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string shop, [FromQuery] string state)
    {
        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(shop))
        {
            return BadRequest("Code and shop parameters are required");
        }

        try
        {
            _logger.LogInformation("Processing OAuth callback for shop: {Shop}", shop);

            // Exchange the code for an access token
            var authResult = await _shopifyService.ExchangeCodeForTokenAsync(shop, code);

            _logger.LogInformation("Successfully obtained access token for shop: {Shop}", shop);

            // Check if store already exists
            var existingStore = await _context.Stores
                .FirstOrDefaultAsync(s => s.ShopifyDomain == shop);

            if (existingStore != null)
            {
                // Update existing store
                existingStore.AccessToken = authResult.AccessToken;
                existingStore.IsActive = true;
                existingStore.UpdatedAt = DateTime.UtcNow;

                _logger.LogInformation("Updated existing store: {Shop}", shop);
            }
            else
            {
                // Create new store
                var newStore = new ShopifyHub.Domain.Entities.Store
                {
                    ShopifyDomain = shop,
                    AccessToken = authResult.AccessToken,
                    StoreName = shop.Replace(".myshopify.com", ""),
                    Email = "", // We'll get this later from Shopify API
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Stores.AddAsync(newStore);

                _logger.LogInformation("Created new store: {Shop}", shop);
            }

            // Save changes to database
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Store connected successfully!",
                shop = shop
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to complete OAuth for shop: {Shop}", shop);
            return StatusCode(500, new { error = "Failed to connect store", details = ex.Message });
        }
    }
}