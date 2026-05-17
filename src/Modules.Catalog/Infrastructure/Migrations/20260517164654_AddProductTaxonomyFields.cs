using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductTaxonomyFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessoryType",
                schema: "catalog",
                table: "Products",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "None");

            migrationBuilder.AddColumn<string>(
                name: "JewelleryType",
                schema: "catalog",
                table: "Products",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "None");

            migrationBuilder.AddColumn<string>(
                name: "MetalColor",
                schema: "catalog",
                table: "Products",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "None");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                schema: "catalog",
                table: "ProductImages",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessoryType",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "JewelleryType",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MetalColor",
                schema: "catalog",
                table: "Products");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                schema: "catalog",
                table: "ProductImages",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);
        }
    }
}
