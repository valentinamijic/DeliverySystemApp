using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantService.DeliverySystem_DAL.Migrations
{
    public partial class StripeToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeToken",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeToken",
                table: "Carts");
        }
    }
}
