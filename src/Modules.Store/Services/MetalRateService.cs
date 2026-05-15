using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Modules.Store.Core.Entities;
using Modules.Store.Infrastructure;

namespace Modules.Store.Services;

public sealed class MetalRateService : IMetalRateService
{
    private readonly StoreDbContext _dbContext;
    private readonly MetalsDevClient _metalsDevClient;
    private readonly IConfiguration _configuration;

    public MetalRateService(
        StoreDbContext dbContext,
        MetalsDevClient metalsDevClient,
        IConfiguration configuration)
    {
        _dbContext = dbContext;
        _metalsDevClient = metalsDevClient;
        _configuration = configuration;
    }

    public async Task<MetalRateResponse> GetLatestSavedOrFallbackAsync(
        CancellationToken cancellationToken)
    {
        var latest = await _dbContext.MetalRates
            .AsNoTracking()
            .OrderByDescending(x => x.LastUpdatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (latest is not null)
        {
            return ToResponse(latest);
        }

        return new MetalRateResponse
        {
            Gold24kPerGram = 7450,
            Gold22kPerGram = 6825,
            Gold18kPerGram = 5588,
            Silver999PerGram = 92,
            PlatinumPerGram = 3150,
            Currency = "INR",
            Unit = "g",
            Source = "fallback",
            IsFallback = true,
            LastUpdated = DateTime.UtcNow
        };
    }

    public async Task<MetalRateResponse> RefreshFromProviderAsync(
        CancellationToken cancellationToken)
    {
        var providerResponse = await _metalsDevClient.GetLatestRatesAsync(cancellationToken);

        var useIndianReferenceRate = bool.TryParse(
            _configuration["MetalsDev:UseIndianReferenceRate"],
            out var useIndianRate) && useIndianRate;

        var gold24k = useIndianReferenceRate && providerResponse.Metals.IbjaGold.HasValue
            ? providerResponse.Metals.IbjaGold.Value
            : providerResponse.Metals.Gold;

        if (gold24k <= 0)
        {
            throw new InvalidOperationException("Gold rate from provider is invalid.");
        }

        var silver = providerResponse.Metals.Silver;
        var platinum = providerResponse.Metals.Platinum;

        if (silver <= 0)
        {
            throw new InvalidOperationException("Silver rate from provider is invalid.");
        }

        var rate = new MetalRate
        {
            Gold24kPerGram = decimal.Round(gold24k, 4),
            Gold22kPerGram = decimal.Round(gold24k * 22 / 24, 4),
            Gold18kPerGram = decimal.Round(gold24k * 18 / 24, 4),

            Silver999PerGram = decimal.Round(silver, 4),
            PlatinumPerGram = decimal.Round(platinum, 4),

            McxGoldPerGram = providerResponse.Metals.McxGold,
            McxSilverPerGram = providerResponse.Metals.McxSilver,
            IbjaGoldPerGram = providerResponse.Metals.IbjaGold,

            Currency = providerResponse.Currency,
            Unit = providerResponse.Unit,
            Source = useIndianReferenceRate ? "metals.dev:ibja_gold" : "metals.dev:gold",
            IsFallback = false,
            LastUpdatedAt = providerResponse.Timestamps.Metal ?? DateTime.UtcNow,
            RawResponseJson = providerResponse.RawJson
        };

        _dbContext.MetalRates.Add(rate);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(rate);
    }

    private static MetalRateResponse ToResponse(MetalRate rate)
    {
        return new MetalRateResponse
        {
            Gold24kPerGram = rate.Gold24kPerGram,
            Gold22kPerGram = rate.Gold22kPerGram,
            Gold18kPerGram = rate.Gold18kPerGram,

            Silver999PerGram = rate.Silver999PerGram,
            PlatinumPerGram = rate.PlatinumPerGram,

            McxGoldPerGram = rate.McxGoldPerGram,
            McxSilverPerGram = rate.McxSilverPerGram,
            IbjaGoldPerGram = rate.IbjaGoldPerGram,

            Currency = rate.Currency,
            Unit = rate.Unit,
            Source = rate.Source,
            IsFallback = rate.IsFallback,
            LastUpdated = rate.LastUpdatedAt
        };
    }
}