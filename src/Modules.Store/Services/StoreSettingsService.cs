using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Modules.Store.Core.Entities;
using Modules.Store.Infrastructure;

namespace Modules.Store.Services;

public sealed class StoreSettingsService : IStoreSettingsService
{
    private readonly StoreDbContext _dbContext;

    public StoreSettingsService(StoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<StoreSettingsResponse> GetSettingsAsync(
        CancellationToken cancellationToken)
    {
        var settings = await _dbContext.StoreSettings
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (settings is null)
        {
            return GetDefaultSettings();
        }

        return ToResponse(settings);
    }

    public async Task<StoreSettingsResponse> SaveSettingsAsync(
        SaveStoreSettingsRequest request,
        CancellationToken cancellationToken)
    {
        ValidateRequest(request);

        var settings = await _dbContext.StoreSettings
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (settings is null)
        {
            settings = new StoreSetting();
            _dbContext.StoreSettings.Add(settings);
        }

        settings.StoreName = request.StoreName.Trim();
        settings.Tagline = request.Tagline?.Trim() ?? string.Empty;
        settings.LogoUrl = request.LogoUrl?.Trim() ?? string.Empty;
        settings.Address = request.Address?.Trim() ?? string.Empty;
        settings.Whatsapp = request.Whatsapp?.Trim() ?? string.Empty;
        settings.Email = request.Email?.Trim() ?? string.Empty;
        settings.Instagram = request.Instagram?.Trim() ?? string.Empty;
        settings.Facebook = request.Facebook?.Trim() ?? string.Empty;

        settings.DefaultMakingChargesJson = JsonSerializer.Serialize(
            request.DefaultMakingCharges ?? new Dictionary<string, decimal>());

        settings.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(settings);
    }

    private static void ValidateRequest(SaveStoreSettingsRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.StoreName))
        {
            throw new InvalidOperationException("Store name is required.");
        }

        if (!string.IsNullOrWhiteSpace(request.Email) &&
            !request.Email.Contains('@'))
        {
            throw new InvalidOperationException("Valid email is required.");
        }

        foreach (var item in request.DefaultMakingCharges ?? new Dictionary<string, decimal>())
        {
            if (item.Value < 0)
            {
                throw new InvalidOperationException(
                    $"Making charge for {item.Key} cannot be negative.");
            }
        }
    }

    private static StoreSettingsResponse GetDefaultSettings()
    {
        return new StoreSettingsResponse
        {
            StoreName = "House of Riwaze",
            Tagline = "Timeless jewellery for every occasion",
            LogoUrl = string.Empty,
            Address = string.Empty,
            Whatsapp = string.Empty,
            Email = string.Empty,
            Instagram = string.Empty,
            Facebook = string.Empty,
            DefaultMakingCharges = new Dictionary<string, decimal>
            {
                ["Rings"] = 450,
                ["Necklaces"] = 280,
                ["Earrings"] = 350,
                ["Bracelets"] = 300,
                ["Bangles"] = 320,
                ["Pendants"] = 300,
                ["Chains"] = 250,
                ["Sets"] = 300,
                ["Other"] = 300
            },
            UpdatedAt = null
        };
    }

    private static StoreSettingsResponse ToResponse(StoreSetting settings)
    {
        Dictionary<string, decimal> defaultMakingCharges;

        try
        {
            defaultMakingCharges =
                JsonSerializer.Deserialize<Dictionary<string, decimal>>(
                    settings.DefaultMakingChargesJson) ?? new Dictionary<string, decimal>();
        }
        catch
        {
            defaultMakingCharges = new Dictionary<string, decimal>();
        }

        return new StoreSettingsResponse
        {
            StoreName = settings.StoreName,
            Tagline = settings.Tagline,
            LogoUrl = settings.LogoUrl,
            Address = settings.Address,
            Whatsapp = settings.Whatsapp,
            Email = settings.Email,
            Instagram = settings.Instagram,
            Facebook = settings.Facebook,
            DefaultMakingCharges = defaultMakingCharges,
            UpdatedAt = settings.UpdatedAt
        };
    }
}