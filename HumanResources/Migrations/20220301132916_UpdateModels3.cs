using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanResources.Migrations
{
    public partial class UpdateModels3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalAdvance_AdvancePayment_AdvancePaymentID",
                table: "PersonalAdvance");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalAdvance_Companies_CompanyID",
                table: "PersonalAdvance");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalAdvance_Status_StatusID",
                table: "PersonalAdvance");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalAdvance_Users_UserId",
                table: "PersonalAdvance");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalExpense_Companies_CompanyID",
                table: "PersonalExpense");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalExpense_Expense_ExpenseID",
                table: "PersonalExpense");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalExpense_Status_StatusID",
                table: "PersonalExpense");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalExpense_Users_UserId",
                table: "PersonalExpense");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalPermit_Companies_CompanyID",
                table: "PersonalPermit");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalPermit_Permission_PermissionID",
                table: "PersonalPermit");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalPermit_Status_StatusID",
                table: "PersonalPermit");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalPermit_Users_UserId",
                table: "PersonalPermit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Status",
                table: "Status");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalPermit",
                table: "PersonalPermit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalExpense",
                table: "PersonalExpense");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalAdvance",
                table: "PersonalAdvance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permission",
                table: "Permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expense",
                table: "Expense");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdvancePayment",
                table: "AdvancePayment");

            migrationBuilder.RenameTable(
                name: "Status",
                newName: "Statuses");

            migrationBuilder.RenameTable(
                name: "PersonalPermit",
                newName: "PersonalPermits");

            migrationBuilder.RenameTable(
                name: "PersonalExpense",
                newName: "PersonalExpenses");

            migrationBuilder.RenameTable(
                name: "PersonalAdvance",
                newName: "PersonalAdvances");

            migrationBuilder.RenameTable(
                name: "Permission",
                newName: "Permissions");

            migrationBuilder.RenameTable(
                name: "Expense",
                newName: "Expenses");

            migrationBuilder.RenameTable(
                name: "AdvancePayment",
                newName: "AdvancePayments");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalPermit_UserId",
                table: "PersonalPermits",
                newName: "IX_PersonalPermits_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalPermit_StatusID",
                table: "PersonalPermits",
                newName: "IX_PersonalPermits_StatusID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalPermit_PermissionID",
                table: "PersonalPermits",
                newName: "IX_PersonalPermits_PermissionID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalPermit_CompanyID",
                table: "PersonalPermits",
                newName: "IX_PersonalPermits_CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalExpense_UserId",
                table: "PersonalExpenses",
                newName: "IX_PersonalExpenses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalExpense_StatusID",
                table: "PersonalExpenses",
                newName: "IX_PersonalExpenses_StatusID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalExpense_ExpenseID",
                table: "PersonalExpenses",
                newName: "IX_PersonalExpenses_ExpenseID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalExpense_CompanyID",
                table: "PersonalExpenses",
                newName: "IX_PersonalExpenses_CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalAdvance_UserId",
                table: "PersonalAdvances",
                newName: "IX_PersonalAdvances_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalAdvance_StatusID",
                table: "PersonalAdvances",
                newName: "IX_PersonalAdvances_StatusID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalAdvance_CompanyID",
                table: "PersonalAdvances",
                newName: "IX_PersonalAdvances_CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalAdvance_AdvancePaymentID",
                table: "PersonalAdvances",
                newName: "IX_PersonalAdvances_AdvancePaymentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalPermits",
                table: "PersonalPermits",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalExpenses",
                table: "PersonalExpenses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalAdvances",
                table: "PersonalAdvances",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdvancePayments",
                table: "AdvancePayments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalAdvances_AdvancePayments_AdvancePaymentID",
                table: "PersonalAdvances",
                column: "AdvancePaymentID",
                principalTable: "AdvancePayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalAdvances_Companies_CompanyID",
                table: "PersonalAdvances",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalAdvances_Statuses_StatusID",
                table: "PersonalAdvances",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalAdvances_Users_UserId",
                table: "PersonalAdvances",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalExpenses_Companies_CompanyID",
                table: "PersonalExpenses",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalExpenses_Expenses_ExpenseID",
                table: "PersonalExpenses",
                column: "ExpenseID",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalExpenses_Statuses_StatusID",
                table: "PersonalExpenses",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalExpenses_Users_UserId",
                table: "PersonalExpenses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalPermits_Companies_CompanyID",
                table: "PersonalPermits",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalPermits_Permissions_PermissionID",
                table: "PersonalPermits",
                column: "PermissionID",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalPermits_Statuses_StatusID",
                table: "PersonalPermits",
                column: "StatusID",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalPermits_Users_UserId",
                table: "PersonalPermits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalAdvances_AdvancePayments_AdvancePaymentID",
                table: "PersonalAdvances");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalAdvances_Companies_CompanyID",
                table: "PersonalAdvances");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalAdvances_Statuses_StatusID",
                table: "PersonalAdvances");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalAdvances_Users_UserId",
                table: "PersonalAdvances");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalExpenses_Companies_CompanyID",
                table: "PersonalExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalExpenses_Expenses_ExpenseID",
                table: "PersonalExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalExpenses_Statuses_StatusID",
                table: "PersonalExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalExpenses_Users_UserId",
                table: "PersonalExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalPermits_Companies_CompanyID",
                table: "PersonalPermits");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalPermits_Permissions_PermissionID",
                table: "PersonalPermits");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalPermits_Statuses_StatusID",
                table: "PersonalPermits");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalPermits_Users_UserId",
                table: "PersonalPermits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalPermits",
                table: "PersonalPermits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalExpenses",
                table: "PersonalExpenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalAdvances",
                table: "PersonalAdvances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdvancePayments",
                table: "AdvancePayments");

            migrationBuilder.RenameTable(
                name: "Statuses",
                newName: "Status");

            migrationBuilder.RenameTable(
                name: "PersonalPermits",
                newName: "PersonalPermit");

            migrationBuilder.RenameTable(
                name: "PersonalExpenses",
                newName: "PersonalExpense");

            migrationBuilder.RenameTable(
                name: "PersonalAdvances",
                newName: "PersonalAdvance");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "Permission");

            migrationBuilder.RenameTable(
                name: "Expenses",
                newName: "Expense");

            migrationBuilder.RenameTable(
                name: "AdvancePayments",
                newName: "AdvancePayment");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalPermits_UserId",
                table: "PersonalPermit",
                newName: "IX_PersonalPermit_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalPermits_StatusID",
                table: "PersonalPermit",
                newName: "IX_PersonalPermit_StatusID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalPermits_PermissionID",
                table: "PersonalPermit",
                newName: "IX_PersonalPermit_PermissionID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalPermits_CompanyID",
                table: "PersonalPermit",
                newName: "IX_PersonalPermit_CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalExpenses_UserId",
                table: "PersonalExpense",
                newName: "IX_PersonalExpense_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalExpenses_StatusID",
                table: "PersonalExpense",
                newName: "IX_PersonalExpense_StatusID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalExpenses_ExpenseID",
                table: "PersonalExpense",
                newName: "IX_PersonalExpense_ExpenseID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalExpenses_CompanyID",
                table: "PersonalExpense",
                newName: "IX_PersonalExpense_CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalAdvances_UserId",
                table: "PersonalAdvance",
                newName: "IX_PersonalAdvance_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalAdvances_StatusID",
                table: "PersonalAdvance",
                newName: "IX_PersonalAdvance_StatusID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalAdvances_CompanyID",
                table: "PersonalAdvance",
                newName: "IX_PersonalAdvance_CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalAdvances_AdvancePaymentID",
                table: "PersonalAdvance",
                newName: "IX_PersonalAdvance_AdvancePaymentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Status",
                table: "Status",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalPermit",
                table: "PersonalPermit",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalExpense",
                table: "PersonalExpense",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalAdvance",
                table: "PersonalAdvance",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permission",
                table: "Permission",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expense",
                table: "Expense",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdvancePayment",
                table: "AdvancePayment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalAdvance_AdvancePayment_AdvancePaymentID",
                table: "PersonalAdvance",
                column: "AdvancePaymentID",
                principalTable: "AdvancePayment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalAdvance_Companies_CompanyID",
                table: "PersonalAdvance",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalAdvance_Status_StatusID",
                table: "PersonalAdvance",
                column: "StatusID",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalAdvance_Users_UserId",
                table: "PersonalAdvance",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalExpense_Companies_CompanyID",
                table: "PersonalExpense",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalExpense_Expense_ExpenseID",
                table: "PersonalExpense",
                column: "ExpenseID",
                principalTable: "Expense",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalExpense_Status_StatusID",
                table: "PersonalExpense",
                column: "StatusID",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalExpense_Users_UserId",
                table: "PersonalExpense",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalPermit_Companies_CompanyID",
                table: "PersonalPermit",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalPermit_Permission_PermissionID",
                table: "PersonalPermit",
                column: "PermissionID",
                principalTable: "Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalPermit_Status_StatusID",
                table: "PersonalPermit",
                column: "StatusID",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalPermit_Users_UserId",
                table: "PersonalPermit",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
