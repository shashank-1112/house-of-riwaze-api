using HouseOfRiwaze.Shared.Abstractions;

namespace Modules.Catalog.Core.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;
    public string? SubCategory { get; set; }

    public string MetalType { get; set; } = "Gold";
    public string Purity { get; set; } = "22K";

    public decimal GrossWeight { get; set; }
    public decimal NetWeight { get; set; }

    public string MakingChargesType { get; set; } = "per_gram";
    public decimal MakingCharges { get; set; }
    public decimal? PriceOverride { get; set; }

    public int StockQuantity { get; set; }
    public int MinStockThreshold { get; set; } = 5;

    public string Description { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;

    public string Visibility { get; set; } = "Published";
    public string Gender { get; set; } = "Unisex";
    public string Occasion { get; set; } = "Any";

    public bool IsFeatured { get; set; }

    public bool TryOnEnabled { get; set; }
    public string TryOnType { get; set; } = "ring";
    public string? TryOnAssetUrl { get; set; }
    public decimal TryOnScale { get; set; } = 1;
    public decimal TryOnOffsetX { get; set; }
    public decimal TryOnOffsetY { get; set; }
    public decimal TryOnRotation { get; set; }

    public List<ProductImage> Images { get; set; } = new();
    public List<ProductStone> Stones { get; set; } = new();
}