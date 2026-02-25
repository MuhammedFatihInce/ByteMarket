using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryImageFileId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CategoryImageFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Storage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryImageFile", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryImageFileId",
                table: "Categories",
                column: "CategoryImageFileId",
                unique: true,
                filter: "[CategoryImageFileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_CategoryImageFile_CategoryImageFileId",
                table: "Categories",
                column: "CategoryImageFileId",
                principalTable: "CategoryImageFile",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_CategoryImageFile_CategoryImageFileId",
                table: "Categories");

            migrationBuilder.DropTable(
                name: "CategoryImageFile");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CategoryImageFileId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoryImageFileId",
                table: "Categories");
        }
    }
}
