using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addImageOrderDisplay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Showcase",
                table: "ProductImageFile");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "ProductImageFile",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "ProductImageFile");

            migrationBuilder.AddColumn<bool>(
                name: "Showcase",
                table: "ProductImageFile",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
