using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PersonalWebsite.Migrations
{
    public partial class CreateContentsAndTranslations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Content",
                columns: table => new
                {
                    ContentGuid = table.Column<Guid>(nullable: false, defaultValueSql: "newsequentialid()"),
                    InternalCaption = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.ContentGuid);
                });
            migrationBuilder.CreateTable(
                name: "Translation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContentContentGuid = table.Column<Guid>(nullable: true),
                    ContentId = table.Column<int>(nullable: false),
                    ContentMarkup = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    UrlName = table.Column<string>(nullable: false),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Translation_Content_ContentContentGuid",
                        column: x => x.ContentContentGuid,
                        principalTable: "Content",
                        principalColumn: "ContentGuid");
                });
            migrationBuilder.CreateIndex(
                name: "IX_Translation_Version_ContentId",
                table: "Translation",
                columns: new[] { "Version", "ContentId" },
                unique: true);
            migrationBuilder.CreateIndex(
                name: "IX_Translation_Version_UrlName",
                table: "Translation",
                columns: new[] { "Version", "UrlName" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Translation");
            migrationBuilder.DropTable("Content");
        }
    }
}
