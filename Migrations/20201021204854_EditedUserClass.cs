using Microsoft.EntityFrameworkCore.Migrations;

namespace devbook.api.Migrations
{
    public partial class EditedUserClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "skill",
                table: "Skills",
                newName: "Skill");

            migrationBuilder.RenameColumn(
                name: "interest",
                table: "Interests",
                newName: "Interest");

            migrationBuilder.AddColumn<string>(
                name: "Interest",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Interest",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Skill",
                table: "Skills",
                newName: "skill");

            migrationBuilder.RenameColumn(
                name: "Interest",
                table: "Interests",
                newName: "interest");
        }
    }
}
