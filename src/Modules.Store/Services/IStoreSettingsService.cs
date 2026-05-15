namespace Modules.Store.Services;

public interface IStoreSettingsService
{
    Task<StoreSettingsResponse> GetSettingsAsync(CancellationToken cancellationToken);

    Task<StoreSettingsResponse> SaveSettingsAsync(
        SaveStoreSettingsRequest request,
        CancellationToken cancellationToken);
}

public sealed class StoreSettingsResponse
{
    public string StoreName { get; set; } = string.Empty;

    public string Tagline { get; set; } = string.Empty;

    public string LogoUrl { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Whatsapp { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Instagram { get; set; } = string.Empty;

    public string Facebook { get; set; } = string.Empty;

    public Dictionary<string, decimal> DefaultMakingCharges { get; set; } = new();

    public DateTime? UpdatedAt { get; set; }
}

public sealed class SaveStoreSettingsRequest
{
    public string StoreName { get; set; } = string.Empty;

    public string Tagline { get; set; } = string.Empty;

    public string LogoUrl { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Whatsapp { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Instagram { get; set; } = string.Empty;

    public string Facebook { get; set; } = string.Empty;

    public Dictionary<string, decimal> DefaultMakingCharges { get; set; } = new();
}