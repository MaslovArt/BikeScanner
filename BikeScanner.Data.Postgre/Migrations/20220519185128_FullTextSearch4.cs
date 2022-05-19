using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeScanner.Data.Postgre.Migrations
{
    public partial class FullTextSearch4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contents_Text",
                table: "Contents");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_Text",
                table: "Contents",
                column: "Text")
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "english");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contents_Text",
                table: "Contents");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_Text",
                table: "Contents",
                column: "Text")
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "russian");
        }
    }
}
