using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanResources.Migrations
{
    public partial class ForeignKeyUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalAdvances_Users_UserId",
                table: "PersonalAdvances");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalExpenses_Users_UserId",
                table: "PersonalExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalPermits_Users_UserId",
                table: "PersonalPermits");

            migrationBuilder.DropIndex(
                name: "IX_PersonalPermits_UserId",
                table: "PersonalPermits");

            migrationBuilder.DropIndex(
                name: "IX_PersonalExpenses_UserId",
                table: "PersonalExpenses");

            migrationBuilder.DropIndex(
                name: "IX_PersonalAdvances_UserId",
                table: "PersonalAdvances");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PersonalPermits");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PersonalExpenses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PersonalAdvances");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalPermits_PersonalID",
                table: "PersonalPermits",
                column: "PersonalID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalExpenses_PersonalID",
                table: "PersonalExpenses",
                column: "PersonalID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalAdvances_PersonalID",
                table: "PersonalAdvances",
                column: "PersonalID");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalAdvances_Users_PersonalID",
                table: "PersonalAdvances",
                column: "PersonalID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalExpenses_Users_PersonalID",
                table: "PersonalExpenses",
                column: "PersonalID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalPermits_Users_PersonalID",
                table: "PersonalPermits",
                column: "PersonalID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalAdvances_Users_PersonalID",
                table: "PersonalAdvances");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalExpenses_Users_PersonalID",
                table: "PersonalExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalPermits_Users_PersonalID",
                table: "PersonalPermits");

            migrationBuilder.DropIndex(
                name: "IX_PersonalPermits_PersonalID",
                table: "PersonalPermits");

            migrationBuilder.DropIndex(
                name: "IX_PersonalExpenses_PersonalID",
                table: "PersonalExpenses");

            migrationBuilder.DropIndex(
                name: "IX_PersonalAdvances_PersonalID",
                table: "PersonalAdvances");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PersonalPermits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PersonalExpenses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PersonalAdvances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalPermits_UserId",
                table: "PersonalPermits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalExpenses_UserId",
                table: "PersonalExpenses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalAdvances_UserId",
                table: "PersonalAdvances",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalAdvances_Users_UserId",
                table: "PersonalAdvances",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalExpenses_Users_UserId",
                table: "PersonalExpenses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalPermits_Users_UserId",
                table: "PersonalPermits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
