using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateBasketCoupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Coupons_CouponId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_CouponId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "CouponId",
                table: "Baskets");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireTime",
                table: "Coupons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BasketCoupon",
                columns: table => new
                {
                    BasketsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CouponsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketCoupon", x => new { x.BasketsId, x.CouponsId });
                    table.ForeignKey(
                        name: "FK_BasketCoupon_Baskets_BasketsId",
                        column: x => x.BasketsId,
                        principalTable: "Baskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasketCoupon_Coupons_CouponsId",
                        column: x => x.CouponsId,
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketCoupon_CouponsId",
                table: "BasketCoupon",
                column: "CouponsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasketCoupon");

            migrationBuilder.DropColumn(
                name: "ExpireTime",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Coupons");

            migrationBuilder.AddColumn<Guid>(
                name: "CouponId",
                table: "Baskets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_CouponId",
                table: "Baskets",
                column: "CouponId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Coupons_CouponId",
                table: "Baskets",
                column: "CouponId",
                principalTable: "Coupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
