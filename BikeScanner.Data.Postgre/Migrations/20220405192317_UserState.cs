using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeScanner.Data.Postgre.Migrations
{
    public partial class UserState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountStatus",
                table: "Users",
                newName: "State");

            migrationBuilder.RenameIndex(
                name: "IX_Users_AccountStatus",
                table: "Users",
                newName: "IX_Users_State");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "Users",
                newName: "AccountStatus");

            migrationBuilder.RenameIndex(
                name: "IX_Users_State",
                table: "Users",
                newName: "IX_Users_AccountStatus");
        }
    }
}
