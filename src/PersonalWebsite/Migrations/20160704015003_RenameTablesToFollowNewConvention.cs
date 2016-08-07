using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalWebsite.Migrations.DataDb
{
    public partial class RenameTablesToFollowNewConvention : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalWebsite.Models.Translation_PersonalWebsite.Models.Content_ContentId",
                table: "PersonalWebsite.Models.Translation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalWebsite.Models.Translation",
                table: "PersonalWebsite.Models.Translation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalWebsite.Models.Content",
                table: "PersonalWebsite.Models.Content");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Translations",
                table: "PersonalWebsite.Models.Translation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contents",
                table: "PersonalWebsite.Models.Content",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_Contents_ContentId",
                table: "PersonalWebsite.Models.Translation",
                column: "ContentId",
                principalTable: "PersonalWebsite.Models.Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameIndex(
                name: "IX_PersonalWebsite.Models.Translation_Version_UrlName",
                table: "PersonalWebsite.Models.Translation",
                newName: "IX_Translations_Version_UrlName");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalWebsite.Models.Translation_Version_ContentId",
                table: "PersonalWebsite.Models.Translation",
                newName: "IX_Translations_Version_ContentId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalWebsite.Models.Translation_ContentId",
                table: "PersonalWebsite.Models.Translation",
                newName: "IX_Translations_ContentId");

            migrationBuilder.RenameTable(
                name: "PersonalWebsite.Models.Translation",
                newName: "Translation");

            migrationBuilder.RenameTable(
                name: "PersonalWebsite.Models.Content",
                newName: "Content");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Contents_ContentId",
                table: "Translation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Translations",
                table: "Translation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contents",
                table: "Content");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalWebsite.Models.Translation",
                table: "Translation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalWebsite.Models.Content",
                table: "Content",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalWebsite.Models.Translation_PersonalWebsite.Models.Content_ContentId",
                table: "Translation",
                column: "ContentId",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_Translations_Version_UrlName",
                table: "Translation",
                newName: "IX_PersonalWebsite.Models.Translation_Version_UrlName");

            migrationBuilder.RenameIndex(
                name: "IX_Translations_Version_ContentId",
                table: "Translation",
                newName: "IX_PersonalWebsite.Models.Translation_Version_ContentId");

            migrationBuilder.RenameIndex(
                name: "IX_Translations_ContentId",
                table: "Translation",
                newName: "IX_PersonalWebsite.Models.Translation_ContentId");

            migrationBuilder.RenameTable(
                name: "Translation",
                newName: "PersonalWebsite.Models.Translation");

            migrationBuilder.RenameTable(
                name: "Content",
                newName: "PersonalWebsite.Models.Content");
        }
    }
}
