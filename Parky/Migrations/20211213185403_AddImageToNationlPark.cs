using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkyApi.Migrations
{
    public partial class AddImageToNationlPark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "NationalPark",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "NationalPark");
        }
    }
}
