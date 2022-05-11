using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeScanner.Data.Postgre.Migrations
{
    public partial class M1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contents_IndexEpoch",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "AdUrl",
                table: "NotificationsQueue");

            migrationBuilder.DropColumn(
                name: "IndexEpoch",
                table: "Contents");

            migrationBuilder.RenameColumn(
                name: "SearchQuery",
                table: "NotificationsQueue",
                newName: "Text");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_Created",
                table: "Contents",
                column: "Created");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contents_Created",
                table: "Contents");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "NotificationsQueue",
                newName: "SearchQuery");

            migrationBuilder.AddColumn<string>(
                name: "AdUrl",
                table: "NotificationsQueue",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "IndexEpoch",
                table: "Contents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Contents_IndexEpoch",
                table: "Contents",
                column: "IndexEpoch");
        }
    }
}
