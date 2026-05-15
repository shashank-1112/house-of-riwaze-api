using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Modules.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalog");

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Sku = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Category = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    SubCategory = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    MetalType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Purity = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    GrossWeight = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false),
                    NetWeight = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false),
                    MakingChargesType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    MakingCharges = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceOverride = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false),
                    MinStockThreshold = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Tags = table.Column<string>(type: "text", nullable: false),
                    Visibility = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Gender = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Occasion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    TryOnEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    TryOnType = table.Column<string>(type: "text", nullable: false),
                    TryOnAssetUrl = table.Column<string>(type: "text", nullable: true),
                    TryOnScale = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    TryOnOffsetX = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    TryOnOffsetY = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    TryOnRotation = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Url = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "catalog",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductStones",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StoneType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Carat = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false),
                    Clarity = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Cut = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Color = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductStones_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "catalog",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                schema: "catalog",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Sku",
                schema: "catalog",
                table: "Products",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductStones_ProductId",
                schema: "catalog",
                table: "ProductStones",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImages",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "ProductStones",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "catalog");
        }
    }
}
