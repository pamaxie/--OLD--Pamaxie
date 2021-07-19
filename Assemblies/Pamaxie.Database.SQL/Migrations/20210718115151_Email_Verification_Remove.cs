using Microsoft.EntityFrameworkCore.Migrations;

namespace Pamaxie.Database.Sql.Migrations
{
    public partial class Email_Verification_Remove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerified",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailVerified",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
