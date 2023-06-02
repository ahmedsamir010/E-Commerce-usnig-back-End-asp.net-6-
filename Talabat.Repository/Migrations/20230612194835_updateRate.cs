using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.Migrations
{
    public partial class updateRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "message",
                table: "ProductRatings",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ProductRatings",
                newName: "UserName");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "ProductRatings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "ProductRatings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ProductRatings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "ProductRatings");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ProductRatings");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "ProductRatings",
                newName: "message");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "ProductRatings",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "message",
                table: "ProductRatings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
