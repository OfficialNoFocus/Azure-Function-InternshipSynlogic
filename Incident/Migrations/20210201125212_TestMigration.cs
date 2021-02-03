using Microsoft.EntityFrameworkCore.Migrations;

namespace Incident.App.Migrations
{
    public partial class TestMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Incidents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Weight",
                table: "Incidents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Incidents");
        }
    }
}
