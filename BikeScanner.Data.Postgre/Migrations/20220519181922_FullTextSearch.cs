using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeScanner.Data.Postgre.Migrations
{
    public partial class FullTextSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Contents_Text",
                table: "Contents",
                column: "Text")
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "russian");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contents_Text",
                table: "Contents");
        }
    }
}
