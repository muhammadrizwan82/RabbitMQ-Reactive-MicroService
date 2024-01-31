using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactiveMicroService.CustomerService.API.Migrations
{
    public partial class _3rdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerDevices_Customers_CustomerId",
                table: "CustomerDevices");

            migrationBuilder.DropIndex(
                name: "IX_CustomerDevices_CustomerId",
                table: "CustomerDevices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CustomerDevices_CustomerId",
                table: "CustomerDevices",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerDevices_Customers_CustomerId",
                table: "CustomerDevices",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
