using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcMovie.Data.Migrations
{
    public partial class Abc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DaThanhToan",
                table: "HoaDons",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaThanhToan",
                table: "HoaDons");
        }
    }
}
