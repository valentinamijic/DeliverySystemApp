using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantService.DeliverySystem_DAL.Migrations
{
    public partial class TotalAmountCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalAmount",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Carts");
        }
    }
}
