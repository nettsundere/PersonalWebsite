using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalWebsite.Migrations.DataDb
{
    public partial class AddCustomHeaderMarkup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomHeaderMarkup",
                table: "Translation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomHeaderMarkup",
                table: "Translation");
        }
    }
}
