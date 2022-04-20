using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanResources.Migrations
{
    public partial class RejectProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectReason",
                table: "PersonalPermits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectReason",
                table: "PersonalExpenses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectReason",
                table: "PersonalAdvances",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectReason",
                table: "PersonalPermits");

            migrationBuilder.DropColumn(
                name: "RejectReason",
                table: "PersonalExpenses");

            migrationBuilder.DropColumn(
                name: "RejectReason",
                table: "PersonalAdvances");
        }
    }
}
