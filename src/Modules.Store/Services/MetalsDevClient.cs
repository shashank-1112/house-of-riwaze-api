using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace Modules.Store.Services;

public sealed class MetalsDevClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public MetalsDevClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<MetalsDevLatestResponse> GetLatestRatesAsync(
        CancellationToken cancellationToken)
    {
        var apiKey = _configuration["MetalsDev:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Metals.dev API key is missing.");
        }

        var currency = _configuration["MetalsDev:Currency"] ?? "INR";
        var unit = _configuration["MetalsDev:Unit"] ?? "g";

        var url =
            $"/v1/latest?api_key={Uri.EscapeDataString(apiKey)}" +
            $"&currency={Uri.EscapeDataString(currency)}" +
            $"&unit={Uri.EscapeDataString(unit)}";

        var response = await _httpClient.GetAsync(url, cancellationToken);

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Metals.dev request failed. StatusCode={response.StatusCode}. Body={json}");
        }

        var data = JsonSerializer.Deserialize<MetalsDevLatestResponse>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (data is null || data.Status != "success")
        {
            throw new InvalidOperationException("Invalid Metals.dev response.");
        }

        data.RawJson = json;

        return data;
    }
}

public sealed class MetalsDevLatestResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "INR";

    [JsonPropertyName("unit")]
    public string Unit { get; set; } = "g";

    [JsonPropertyName("metals")]
    public MetalsDevMetals Metals { get; set; } = new();

    [JsonPropertyName("timestamps")]
    public MetalsDevTimestamps Timestamps { get; set; } = new();

    public string? RawJson { get; set; }
}

public sealed class MetalsDevMetals
{
    [JsonPropertyName("gold")]
    public decimal Gold { get; set; }

    [JsonPropertyName("silver")]
    public decimal Silver { get; set; }

    [JsonPropertyName("platinum")]
    public decimal Platinum { get; set; }

    [JsonPropertyName("mcx_gold")]
    public decimal? McxGold { get; set; }

    [JsonPropertyName("mcx_silver")]
    public decimal? McxSilver { get; set; }

    [JsonPropertyName("ibja_gold")]
    public decimal? IbjaGold { get; set; }
}

public sealed class MetalsDevTimestamps
{
    [JsonPropertyName("metal")]
    public DateTime? Metal { get; set; }

    [JsonPropertyName("currency")]
    public DateTime? Currency { get; set; }
}