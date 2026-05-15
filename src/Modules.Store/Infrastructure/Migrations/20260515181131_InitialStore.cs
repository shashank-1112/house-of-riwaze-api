using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Modules.Store.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "store");

            migrationBuilder.CreateTable(
                name: "MetalRates",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Gold24kPerGram = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Gold22kPerGram = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Gold18kPerGram = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Silver999PerGram = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    PlatinumPerGram = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    McxGoldPerGram = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    McxSilverPerGram = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    IbjaGoldPerGram = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Unit = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Source = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsFallback = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RawResponseJson = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetalRates", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetalRates",
                schema: "store");
        }
    }
}
