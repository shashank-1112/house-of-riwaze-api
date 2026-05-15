namespace Modules.Catalog.Application.DTOs;

public sealed class ProductQuery
{
    public string? Search { get; set; }
    public string? Category { get; set; }
    public string? Metal { get; set; }
    public string? Visibility { get; set; }
    public bool? IsFeatured { get; set; }
    public int Limit { get; set; } = 100;
}

public sealed class ProductStoneDto
{
    public string StoneType { get; set; } = string.Empty;
    public decimal Carat { get; set; }
    public string Clarity { get; set; } = string.Empty;
    public string Cut { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public decimal Cost { get; set; }
}

public sealed class ProductResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;
    public string? SubCategory { get; set; }

    public string MetalType { get; set; } = string.Empty;
    public string Purity { get; set; } = string.Empty;

    public decimal GrossWeight { get; set; }
    public decimal NetWeight { get; set; }

    public string MakingChargesType { get; set; } = "per_gram";
    public decimal MakingCharges { get; set; }
    public decimal? PriceOverride { get; set; }

    public int StockQuantity { get; set; }
    public int MinStockThreshold { get; set; }

    public List<string> Images { get; set; } = new();

    public string Description { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;

    public string Visibility { get; set; } = "Published";
    public string Gender { get; set; } = "Unisex";
    public string Occasion { get; set; } = "Any";

    public bool IsFeatured { get; set; }

    public List<ProductStoneDto> StoneDetails { get; set; } = new();

    public bool TryOnEnabled { get; set; }
    public string TryOnType { get; set; } = "ring";
    public string? TryOnAsset { get; set; }
    public decimal TryOnScale { get; set; }
    public decimal TryOnOffsetX { get; set; }
    public decimal TryOnOffsetY { get; set; }
    public decimal TryOnRotation { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public sealed class SaveProductRequest
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

    public List<string> Images { get; set; } = new();

    public string Description { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;

    public string Visibility { get; set; } = "Published";
    public string Gender { get; set; } = "Unisex";
    public string Occasion { get; set; } = "Any";

    public bool IsFeatured { get; set; }

    public List<ProductStoneDto> StoneDetails { get; set; } = new();

    public bool TryOnEnabled { get; set; }
    public string TryOnType { get; set; } = "ring";
    public string? TryOnAsset { get; set; }
    public decimal TryOnScale { get; set; } = 1;
    public decimal TryOnOffsetX { get; set; }
    public decimal TryOnOffsetY { get; set; }
    public decimal TryOnRotation { get; set; }
}