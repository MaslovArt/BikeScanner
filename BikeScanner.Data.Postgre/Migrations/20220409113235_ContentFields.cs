using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeScanner.Data.Postgre.Migrations
{
    public partial class ContentFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Contents_AdUrl",
                table: "Contents");

            migrationBuilder.RenameColumn(
                name: "Published",
                table: "Contents",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "AdUrl",
                table: "Contents",
                newName: "Url");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_Published",
                table: "Contents",
                newName: "IX_Contents_Created");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Contents_Url",
                table: "Contents",
                column: "Url");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Contents_Url",
                table: "Contents");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Contents",
                newName: "AdUrl");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Contents",
                newName: "Published");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_Created",
                table: "Contents",
                newName: "IX_Contents_Published");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Contents_AdUrl",
                table: "Contents",
                column: "AdUrl");
        }
    }
}
