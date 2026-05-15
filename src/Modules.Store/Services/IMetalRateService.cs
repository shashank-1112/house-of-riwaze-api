namespace Modules.Store.Services;

public interface IMetalRateService
{
    Task<MetalRateResponse> GetLatestSavedOrFallbackAsync(
        CancellationToken cancellationToken);

    Task<MetalRateResponse> RefreshFromProviderAsync(
        CancellationToken cancellationToken);
}

public sealed class MetalRateResponse
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
    public DateTime LastUpdated { get; set; }
}