using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.DeliverySystem_DAL.Migrations
{
    public partial class AcceptedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "Users");
        }
    }
}
