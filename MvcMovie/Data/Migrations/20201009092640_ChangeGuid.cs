using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcMovie.Data.Migrations
{
    public partial class ChangeGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MaHoaDonTam",
                table: "HoaDons",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "MaHoaDonTam",
                table: "HoaDons",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
