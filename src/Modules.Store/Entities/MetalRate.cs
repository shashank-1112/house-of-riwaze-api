using HouseOfRiwaze.Shared.Abstractions;

namespace Modules.Store.Core.Entities;

public class MetalRate : BaseEntity
{
    public decimal Gold24kPerGram { get; set; }
    public decimal Gold22kPerGram { get; set; }
    public decimal Gold18kPerGram { get; set; }

    public decimal Silver999PerGram { get; set; }
    public decimal PlatinumPerGram { get; set; }

    public decimal? McxGoldPerGram { get; set; }
    public decimal? McxSilverPerGram { get; set; }
    public decimal? IbjaGoldPerGram { get; set; }

    public string Currency { get; set; } = "INR";
    public string Unit { get; set; } = "g";
    public string Source { get; set; } = "metals.dev";

    public bool IsFallback { get; set; }
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

    public string? RawResponseJson { get; set; }
}