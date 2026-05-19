using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Auth;
using Modules.Catalog.Application.DTOs;
using Modules.Catalog.Application.Services;
using Modules.Catalog.Infrastructure;

namespace Modules.Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IProductService, ProductService>();

        return services;
    }

    public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        var publicGroup = app.MapGroup("/api/products")
            .WithTags("Products");

        publicGroup.MapGet("/", async (
            [AsParameters] ProductQuery query,
            IProductService productService,
            CancellationToken cancellationToken) =>
        {
            var products = await productService.GetProductsAsync(
                query,
                admin: false,
                cancellationToken);

            return Results.Ok(products);
        });

        publicGroup.MapGet("/featured", async (
            IProductService productService,
            CancellationToken cancellationToken) =>
        {
            var products = await productService.GetProductsAsync(
                new ProductQuery
                {
                    Visibility = "Published",
                    IsFeatured = true,
                    Limit = 8
                },
                admin: false,
                cancellationToken);

            return Results.Ok(products);
        });

        publicGroup.MapGet("/{id:int}", async (
            int id,
            IProductService productService,
            CancellationToken cancellationToken) =>
        {
            var product = await productService.GetProductByIdAsync(
                id,
                admin: false,
                cancellationToken);

            return product is null
                ? Results.NotFound()
                : Results.Ok(product);
        });

        var adminGroup = app.MapGroup("/api/admin/products")
            .RequireAuthorization(AdminAuthorization.Policy)
            .WithTags("Admin Products");

        adminGroup.MapGet("/", async (
            [AsParameters] ProductQuery query,
            IProductService productService,
            CancellationToken cancellationToken) =>
        {
            var products = await productService.GetProductsAsync(
                query,
                admin: true,
                cancellationToken);

            return Results.Ok(products);
        });

        adminGroup.MapGet("/{id:int}", async (
            int id,
            IProductService productService,
            CancellationToken cancellationToken) =>
        {
            var product = await productService.GetProductByIdAsync(
                id,
                admin: true,
                cancellationToken);

            return product is null
                ? Results.NotFound()
                : Results.Ok(product);
        });

        adminGroup.MapPost("/", async (
            SaveProductRequest request,
            IProductService productService,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var product = await productService.CreateProductAsync(
                    request,
                    cancellationToken);

                return Results.Created($"/api/admin/products/{product.Id}", product);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new
                {
                    message = ex.Message
                });
            }
        });

        adminGroup.MapPut("/{id:int}", async (
            int id,
            SaveProductRequest request,
            IProductService productService,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var product = await productService.UpdateProductAsync(
                    id,
                    request,
                    cancellationToken);

                return product is null
                    ? Results.NotFound()
                    : Results.Ok(product);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new
                {
                    message = ex.Message
                });
            }
        });

        adminGroup.MapDelete("/{id:int}", async (
            int id,
            IProductService productService,
            CancellationToken cancellationToken) =>
        {
            var deleted = await productService.DeleteProductAsync(
                id,
                cancellationToken);

            return deleted
                ? Results.NoContent()
                : Results.NotFound();
        });

        return app;
    }
}
