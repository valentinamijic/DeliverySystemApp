using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantService.DeliverySystem_DAL.Migrations
{
    public partial class StripeTokenOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeToken",
                table: "Carts");

            migrationBuilder.AddColumn<string>(
                name: "StripeToken",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeToken",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "StripeToken",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
