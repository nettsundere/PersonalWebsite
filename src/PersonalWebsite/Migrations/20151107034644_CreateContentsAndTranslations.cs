using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using Microsoft.EntityFrameworkCore;

namespace PersonalWebsite.Migrations
{
    public partial class CreateContentsAndTranslations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var valueGenerationStrategy = ValueGenerationStrategyHelper.GetAutoIncrementGenerationStrategy(migrationBuilder);
            
            migrationBuilder.CreateTable(
                name: "Content",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation(valueGenerationStrategy.Key, valueGenerationStrategy.Val),
                    InternalCaption = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Translation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation(valueGenerationStrategy.Key, valueGenerationStrategy.Val),
                    ContentId = table.Column<int>(nullable: false),
                    CustomHeaderMarkup = table.Column<string>(nullable: true),
                    ContentMarkup = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    UrlName = table.Column<string>(nullable: false, maxLength: 450),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Translation_Content_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Content",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
