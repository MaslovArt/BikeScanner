using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeScanner.Data.Postgre.Migrations
{
    public partial class ContentCreatedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contents_Created",
                table: "Contents");

            migrationBuilder.AddColumn<DateTime>(
                name: "Published",
                table: "Contents",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Contents_Published",
                table: "Contents",
                column: "Published");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contents_Published",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "Published",
                table: "Contents");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_Created",
                table: "Contents",
                column: "Created");
        }
    }
}
