using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addUsageLimitPerUserforCoupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsageLimitPerUser",
                table: "Coupons",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsageLimitPerUser",
                table: "Coupons");
        }
    }
}
