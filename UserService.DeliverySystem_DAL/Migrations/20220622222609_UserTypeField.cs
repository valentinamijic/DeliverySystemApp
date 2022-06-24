using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.DeliverySystem_DAL.Migrations
{
    public partial class UserTypeField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeOfUser",
                table: "Users",
                newName: "UserType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "Users",
                newName: "TypeOfUser");
        }
    }
}
