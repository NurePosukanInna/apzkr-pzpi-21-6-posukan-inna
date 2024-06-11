using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryAPI.Migrations
{
    public partial class addProductFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "Product",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFresh",
                table: "Product",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeasureOfUnits",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Volume",
                table: "Product",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "IsFresh",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "MeasureOfUnits",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Product");
        }
    }
}
