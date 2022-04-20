using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanResources.Migrations
{
    public partial class ApprovalRejectionDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovalDate",
                table: "PersonalPermits",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectionDate",
                table: "PersonalPermits",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovalDate",
                table: "PersonalExpenses",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectionDate",
                table: "PersonalExpenses",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovalDate",
                table: "PersonalAdvances",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectionDate",
                table: "PersonalAdvances",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalDate",
                table: "PersonalPermits");

            migrationBuilder.DropColumn(
                name: "RejectionDate",
                table: "PersonalPermits");

            migrationBuilder.DropColumn(
                name: "ApprovalDate",
                table: "PersonalExpenses");

            migrationBuilder.DropColumn(
                name: "RejectionDate",
                table: "PersonalExpenses");

            migrationBuilder.DropColumn(
                name: "ApprovalDate",
                table: "PersonalAdvances");

            migrationBuilder.DropColumn(
                name: "RejectionDate",
                table: "PersonalAdvances");
        }
    }
}
