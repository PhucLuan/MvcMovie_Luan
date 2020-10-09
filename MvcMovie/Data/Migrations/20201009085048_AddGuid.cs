using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcMovie.Data.Migrations
{
    public partial class AddGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MaHoaDonTam",
                table: "HoaDons",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaHoaDonTam",
                table: "HoaDons");
        }
    }
}
