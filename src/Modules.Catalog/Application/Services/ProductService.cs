using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Application.DTOs;
using Modules.Catalog.Core.Entities;
using Modules.Catalog.Infrastructure;

namespace Modules.Catalog.Application.Services;

public interface IProductService
{
    Task<List<ProductResponse>> GetProductsAsync(
        ProductQuery query,
        bool admin,
        CancellationToken cancellationToken);

    Task<ProductResponse?> GetProductByIdAsync(
        int id,
        bool admin,
        CancellationToken cancellationToken);

    Task<ProductResponse> CreateProductAsync(
        SaveProductRequest request,
        CancellationToken cancellationToken);

    Task<ProductResponse?> UpdateProductAsync(
        int id,
        SaveProductRequest request,
        CancellationToken cancellationToken);

    Task<bool> DeleteProductAsync(
        int id,
        CancellationToken cancellationToken);
}

public sealed class ProductService : IProductService
{
    private readonly CatalogDbContext _dbContext;

    public ProductService(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ProductResponse>> GetProductsAsync(
        ProductQuery query,
        bool admin,
        CancellationToken cancellationToken)
    {
        var dbQuery = _dbContext.Products
            .AsNoTracking()
            .Include(x => x.Images)
            .Include(x => x.Stones)
            .Where(x => !x.IsDeleted);

        if (!admin)
        {
            dbQuery = dbQuery.Where(x => x.Visibility == "Published");
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim().ToLower();

            dbQuery = dbQuery.Where(x =>
                x.Name.ToLower().Contains(search) ||
                x.Sku.ToLower().Contains(search));
        }

        if (ShouldApplyFilter(query.Category))
        {
            var category = query.Category!.Trim();
            dbQuery = dbQuery.Where(x => x.Category == category);
        }

        if (ShouldApplyFilter(query.Metal))
        {
            var metal = query.Metal!.Trim();
            dbQuery = dbQuery.Where(x => x.MetalType == metal);
        }

        if (ShouldApplyFilter(query.JewelleryType))
        {
            var jewelleryType = query.JewelleryType!.Trim();
            dbQuery = dbQuery.Where(x => x.JewelleryType == jewelleryType);
        }

        if (ShouldApplyFilter(query.MetalColor))
        {
            var metalColor = query.MetalColor!.Trim();
            dbQuery = dbQuery.Where(x => x.MetalColor == metalColor);
        }

        if (ShouldApplyFilter(query.AccessoryType))
        {
            var accessoryType = query.AccessoryType!.Trim();
            dbQuery = dbQuery.Where(x => x.AccessoryType == accessoryType);
        }

        if (ShouldApplyFilter(query.Visibility))
        {
            var visibility = query.Visibility!.Trim();
            dbQuery = dbQuery.Where(x => x.Visibility == visibility);
        }

        if (query.IsFeatured.HasValue)
        {
            dbQuery = dbQuery.Where(x => x.IsFeatured == query.IsFeatured.Value);
        }

        var limit = query.Limit <= 0 ? 100 : Math.Min(query.Limit, 500);

        var products = await dbQuery
            .OrderByDescending(x => x.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return products.Select(ToResponse).ToList();
    }

    public async Task<ProductResponse?> GetProductByIdAsync(
        int id,
        bool admin,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Products
            .AsNoTracking()
            .Include(x => x.Images)
            .Include(x => x.Stones)
            .Where(x => !x.IsDeleted && x.Id == id);

        if (!admin)
        {
            query = query.Where(x => x.Visibility == "Published");
        }

        var product = await query.FirstOrDefaultAsync(cancellationToken);

        return product is null ? null : ToResponse(product);
    }

    public async Task<ProductResponse> CreateProductAsync(
        SaveProductRequest request,
        CancellationToken cancellationToken)
    {
        ValidateRequest(request);

        var sku = request.Sku.Trim();

        var skuExists = await _dbContext.Products
            .AnyAsync(x => !x.IsDeleted && x.Sku == sku, cancellationToken);

        if (skuExists)
        {
            throw new InvalidOperationException("SKU already exists.");
        }

        var product = new Product();

        ApplyRequest(product, request);

        _dbContext.Products.Add(product);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(product);
    }

    public async Task<ProductResponse?> UpdateProductAsync(
        int id,
        SaveProductRequest request,
        CancellationToken cancellationToken)
    {
        ValidateRequest(request);

        var product = await _dbContext.Products
            .Include(x => x.Images)
            .Include(x => x.Stones)
            .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id, cancellationToken);

        if (product is null)
        {
            return null;
        }

        var sku = request.Sku.Trim();

        var skuExists = await _dbContext.Products
            .AnyAsync(x => !x.IsDeleted && x.Sku == sku && x.Id != id, cancellationToken);

        if (skuExists)
        {
            throw new InvalidOperationException("SKU already exists.");
        }

        product.Images.Clear();
        product.Stones.Clear();

        ApplyRequest(product, request);

        product.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(product);
    }

    public async Task<bool> DeleteProductAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id, cancellationToken);

        if (product is null)
        {
            return false;
        }

        product.IsDeleted = true;
        product.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static void ValidateRequest(SaveProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new InvalidOperationException("Product name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Sku))
        {
            throw new InvalidOperationException("SKU is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Category))
        {
            throw new InvalidOperationException("Category is required.");
        }

        if (string.IsNullOrWhiteSpace(request.MetalType))
        {
            throw new InvalidOperationException("Metal type is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Purity))
        {
            throw new InvalidOperationException("Purity is required.");
        }

        if (request.GrossWeight < 0 || request.NetWeight < 0)
        {
            throw new InvalidOperationException("Weight cannot be negative.");
        }

        if (request.NetWeight > request.GrossWeight)
        {
            throw new InvalidOperationException("Net weight cannot be greater than gross weight.");
        }

        if (request.StockQuantity < 0)
        {
            throw new InvalidOperationException("Stock quantity cannot be negative.");
        }

        if (request.MinStockThreshold < 0)
        {
            throw new InvalidOperationException("Minimum stock threshold cannot be negative.");
        }

        if (request.MakingCharges < 0)
        {
            throw new InvalidOperationException("Making charges cannot be negative.");
        }

        if (request.PriceOverride.HasValue && request.PriceOverride.Value < 0)
        {
            throw new InvalidOperationException("Price override cannot be negative.");
        }

        foreach (var stone in request.StoneDetails)
        {
            if (stone.Carat < 0)
            {
                throw new InvalidOperationException("Stone carat cannot be negative.");
            }

            if (stone.Cost < 0)
            {
                throw new InvalidOperationException("Stone cost cannot be negative.");
            }
        }
    }

    private static void ApplyRequest(Product product, SaveProductRequest request)
    {
        product.Name = request.Name.Trim();
        product.Sku = request.Sku.Trim();

        product.Category = request.Category.Trim();
        product.SubCategory = request.SubCategory?.Trim();

        product.MetalType = NormalizeRequired(request.MetalType, "Gold");
        product.JewelleryType = NormalizeOptional(request.JewelleryType);
        product.MetalColor = NormalizeOptional(request.MetalColor);
        product.AccessoryType = NormalizeOptional(request.AccessoryType);
        product.Purity = NormalizeRequired(request.Purity, "22K");

        product.GrossWeight = request.GrossWeight;
        product.NetWeight = request.NetWeight;

        product.MakingChargesType = NormalizeRequired(
            request.MakingChargesType,
            "per_gram");

        product.MakingCharges = request.MakingCharges;
        product.PriceOverride = request.PriceOverride;

        product.StockQuantity = request.StockQuantity;
        product.MinStockThreshold = request.MinStockThreshold;

        product.Description = request.Description ?? string.Empty;
        product.Tags = request.Tags ?? string.Empty;

        product.Visibility = NormalizeRequired(request.Visibility, "Published");
        product.Gender = NormalizeRequired(request.Gender, "Unisex");
        product.Occasion = NormalizeRequired(request.Occasion, "Any");

        product.IsFeatured = request.IsFeatured;

        product.TryOnEnabled = request.TryOnEnabled;
        product.TryOnType = NormalizeRequired(request.TryOnType, "ring");
        product.TryOnAssetUrl = request.TryOnAsset;
        product.TryOnScale = request.TryOnScale <= 0 ? 1 : request.TryOnScale;
        product.TryOnOffsetX = request.TryOnOffsetX;
        product.TryOnOffsetY = request.TryOnOffsetY;
        product.TryOnRotation = request.TryOnRotation;

        product.Images = request.Images
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select((url, index) => new ProductImage
            {
                Url = url.Trim(),
                SortOrder = index,
                IsPrimary = index == 0
            })
            .ToList();

        product.Stones = request.StoneDetails
            .Where(stone =>
                !string.IsNullOrWhiteSpace(stone.StoneType) ||
                stone.Carat > 0 ||
                !string.IsNullOrWhiteSpace(stone.Clarity) ||
                !string.IsNullOrWhiteSpace(stone.Cut) ||
                !string.IsNullOrWhiteSpace(stone.Color) ||
                stone.Cost > 0)
            .Select(stone => new ProductStone
            {
                StoneType = stone.StoneType?.Trim() ?? string.Empty,
                Carat = stone.Carat,
                Clarity = stone.Clarity?.Trim() ?? string.Empty,
                Cut = stone.Cut?.Trim() ?? string.Empty,
                Color = stone.Color?.Trim() ?? string.Empty,
                Cost = stone.Cost
            })
            .ToList();
    }

    private static ProductResponse ToResponse(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,

            Name = product.Name,
            Sku = product.Sku,

            Category = product.Category,
            SubCategory = product.SubCategory,

            MetalType = product.MetalType,
            JewelleryType = product.JewelleryType,
            MetalColor = product.MetalColor,
            AccessoryType = product.AccessoryType,
            Purity = product.Purity,

            GrossWeight = product.GrossWeight,
            NetWeight = product.NetWeight,

            MakingChargesType = product.MakingChargesType,
            MakingCharges = product.MakingCharges,
            PriceOverride = product.PriceOverride,

            StockQuantity = product.StockQuantity,
            MinStockThreshold = product.MinStockThreshold,

            Images = product.Images
                .OrderBy(x => x.SortOrder)
                .Select(x => x.Url)
                .ToList(),

            Description = product.Description,
            Tags = product.Tags,

            Visibility = product.Visibility,
            Gender = product.Gender,
            Occasion = product.Occasion,

            IsFeatured = product.IsFeatured,

            StoneDetails = product.Stones
                .OrderBy(x => x.Id)
                .Select(stone => new ProductStoneDto
                {
                    StoneType = stone.StoneType,
                    Carat = stone.Carat,
                    Clarity = stone.Clarity,
                    Cut = stone.Cut,
                    Color = stone.Color,
                    Cost = stone.Cost
                })
                .ToList(),

            TryOnEnabled = product.TryOnEnabled,
            TryOnType = product.TryOnType,
            TryOnAsset = product.TryOnAssetUrl,
            TryOnScale = product.TryOnScale,
            TryOnOffsetX = product.TryOnOffsetX,
            TryOnOffsetY = product.TryOnOffsetY,
            TryOnRotation = product.TryOnRotation,

            CreatedDate = product.CreatedAt,
            UpdatedDate = product.UpdatedAt
        };
    }

    private static bool ShouldApplyFilter(string? value)
    {
        return !string.IsNullOrWhiteSpace(value) &&
               !string.Equals(value.Trim(), "all", StringComparison.OrdinalIgnoreCase);
    }

    private static string NormalizeRequired(string? value, string fallback)
    {
        return string.IsNullOrWhiteSpace(value)
            ? fallback
            : value.Trim();
    }

    private static string NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? "None"
            : value.Trim();
    }
}