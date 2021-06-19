using Microsoft.EntityFrameworkCore.Migrations;

namespace Pamaxie.Database.Sql.Migrations
{
    public partial class ApplicationRenameDisabledThings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Blocked",
                table: "Applications",
                newName: "Disabled");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Disabled",
                table: "Applications",
                newName: "Blocked");
        }
    }
}
