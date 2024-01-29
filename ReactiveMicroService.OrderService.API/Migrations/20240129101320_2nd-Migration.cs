using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactiveMicroService.OrderService.API.Migrations
{
    public partial class _2ndMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ProductDiscountedAmount",
                table: "OrderDetails",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductDiscountedAmount",
                table: "OrderDetails");
        }
    }
}
