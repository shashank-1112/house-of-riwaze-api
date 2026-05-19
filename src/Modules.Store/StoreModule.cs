using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Auth;
using Modules.Store.Infrastructure;
using Modules.Store.Services;

namespace Modules.Store;

public static class StoreModule
{
    public static IServiceCollection AddStoreModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<StoreDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddHttpClient<MetalsDevClient>((provider, client) =>
        {
            var baseUrl = configuration["MetalsDev:BaseUrl"] ?? "https://api.metals.dev";

            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(20);
        });

        services.AddScoped<IMetalRateService, MetalRateService>();
        services.AddScoped<IStoreSettingsService, StoreSettingsService>();

        return services;
    }

    public static IEndpointRouteBuilder MapStoreEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/metal-rates", async (
            IMetalRateService metalRateService,
            CancellationToken cancellationToken) =>
        {
            var rates = await metalRateService.GetLatestSavedOrFallbackAsync(
                cancellationToken);

            return Results.Ok(rates);
        })
        .RequireAuthorization(AdminAuthorization.Policy)
        .WithTags("Metal Rates");

        app.MapPost("/api/admin/metal-rates/refresh", async (
            IMetalRateService metalRateService,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var rates = await metalRateService.RefreshFromProviderAsync(
                    cancellationToken);

                return Results.Ok(rates);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new
                {
                    message = ex.Message
                });
            }
        })
        .RequireAuthorization(AdminAuthorization.Policy)
        .WithTags("Admin Metal Rates");

        app.MapGet("/api/store-settings", async (
            IStoreSettingsService storeSettingsService,
            CancellationToken cancellationToken) =>
        {
            var settings = await storeSettingsService.GetSettingsAsync(
                cancellationToken);

            return Results.Ok(settings);
        })
        .WithTags("Store Settings");

        app.MapGet("/api/admin/store-settings", async (
            IStoreSettingsService storeSettingsService,
            CancellationToken cancellationToken) =>
        {
            var settings = await storeSettingsService.GetSettingsAsync(
                cancellationToken);

            return Results.Ok(settings);
        })
        .RequireAuthorization(AdminAuthorization.Policy)
        .WithTags("Admin Store Settings");

        app.MapPut("/api/admin/store-settings", async (
            SaveStoreSettingsRequest request,
            IStoreSettingsService storeSettingsService,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var settings = await storeSettingsService.SaveSettingsAsync(
                    request,
                    cancellationToken);

                return Results.Ok(settings);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new
                {
                    message = ex.Message
                });
            }
        })
        .RequireAuthorization(AdminAuthorization.Policy)
        .WithTags("Admin Store Settings");

        return app;
    }
}
