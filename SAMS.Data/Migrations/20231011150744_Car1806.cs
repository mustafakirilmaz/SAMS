using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    public partial class Car1806 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "common");

            migrationBuilder.RenameTable(
                name: "Towns",
                schema: "Common",
                newName: "Towns",
                newSchema: "common");

            migrationBuilder.RenameTable(
                name: "Districts",
                schema: "Common",
                newName: "Districts",
                newSchema: "common");

            migrationBuilder.RenameTable(
                name: "Cities",
                schema: "Common",
                newName: "Cities",
                newSchema: "common");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Common");

            migrationBuilder.RenameTable(
                name: "Towns",
                schema: "common",
                newName: "Towns",
                newSchema: "Common");

            migrationBuilder.RenameTable(
                name: "Districts",
                schema: "common",
                newName: "Districts",
                newSchema: "Common");

            migrationBuilder.RenameTable(
                name: "Cities",
                schema: "common",
                newName: "Cities",
                newSchema: "Common");
        }
    }
}
