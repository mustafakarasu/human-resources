using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanResources.Migrations
{
    public partial class CompanyUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PackageEndDate",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageName",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageTotalPrice",
                table: "Companies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageEndDate",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "PackageName",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "PackageTotalPrice",
                table: "Companies");
        }
    }
}
