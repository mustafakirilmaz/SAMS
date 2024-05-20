using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    public partial class _202405172 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_equal_expense_business_project_BusinessProjectId",
                table: "equal_expense");

            migrationBuilder.DropForeignKey(
                name: "FK_fixture_expense_business_project_BusinessProjectId",
                table: "fixture_expense");

            migrationBuilder.DropForeignKey(
                name: "FK_proportional_expense_business_project_BusinessProjectId",
                table: "proportional_expense");

            migrationBuilder.DropForeignKey(
                name: "FK_units_business_project_BusinessProjectId",
                table: "units");

            migrationBuilder.DropIndex(
                name: "IX_units_BusinessProjectId",
                table: "units");

            migrationBuilder.DropIndex(
                name: "IX_proportional_expense_BusinessProjectId",
                table: "proportional_expense");

            migrationBuilder.DropIndex(
                name: "IX_fixture_expense_BusinessProjectId",
                table: "fixture_expense");

            migrationBuilder.DropIndex(
                name: "IX_equal_expense_BusinessProjectId",
                table: "equal_expense");

            migrationBuilder.DropColumn(
                name: "BusinessProjectId",
                table: "units");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BusinessProjectId",
                table: "units",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_units_BusinessProjectId",
                table: "units",
                column: "BusinessProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_proportional_expense_BusinessProjectId",
                table: "proportional_expense",
                column: "BusinessProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_fixture_expense_BusinessProjectId",
                table: "fixture_expense",
                column: "BusinessProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_equal_expense_BusinessProjectId",
                table: "equal_expense",
                column: "BusinessProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_equal_expense_business_project_BusinessProjectId",
                table: "equal_expense",
                column: "BusinessProjectId",
                principalTable: "business_project",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_fixture_expense_business_project_BusinessProjectId",
                table: "fixture_expense",
                column: "BusinessProjectId",
                principalTable: "business_project",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_proportional_expense_business_project_BusinessProjectId",
                table: "proportional_expense",
                column: "BusinessProjectId",
                principalTable: "business_project",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_units_business_project_BusinessProjectId",
                table: "units",
                column: "BusinessProjectId",
                principalTable: "business_project",
                principalColumn: "Id");
        }
    }
}
