using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalWebsite.Migrations.DataDb
{
    public partial class FixForeignKeyForContents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Translation_Content_ContentId", table: "Translation");
            migrationBuilder.AddForeignKey(
                name: "FK_Translation_Content_ContentId",
                table: "Translation",
                column: "ContentId",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Translation_Content_ContentId", table: "Translation");
            migrationBuilder.AddForeignKey(
                name: "FK_Translation_Content_ContentId",
                table: "Translation",
                column: "ContentId",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
