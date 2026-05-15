using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Modules.Store.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreSettings",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StoreName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Tagline = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    LogoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Address = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Whatsapp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Instagram = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Facebook = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DefaultMakingChargesJson = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "{}"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreSettings",
                schema: "store");
        }
    }
}
