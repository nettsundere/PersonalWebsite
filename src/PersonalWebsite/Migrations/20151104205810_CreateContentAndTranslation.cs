using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace PersonalWebsite.Migrations
{
    public partial class CreateContentAndTranslation : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.CreateSequence(
                name: "DefaultSequence",
                type: "bigint",
                startWith: 1L,
                incrementBy: 10);
            migration.CreateTable(
                name: "Content",
                columns: table => new
                {
                    ContentGuid = table.Column(type: "uniqueidentifier", nullable: false, defaultExpression: "newsequentialid()"),
                    ContentId = table.Column(type: "int", nullable: false),
                    InternalCaption = table.Column(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.ContentGuid);
                    table.Unique("AK_Content_ContentId", x => x.ContentId);
                });
            migration.CreateTable(
                name: "Translation",
                columns: table => new
                {
                    Id = table.Column(type: "int", nullable: false),
                    ContentId = table.Column(type: "int", nullable: false),
                    ContentMarkup = table.Column(type: "nvarchar(max)", nullable: false),
                    Description = table.Column(type: "nvarchar(max)", nullable: false),
                    State = table.Column(type: "int", nullable: false),
                    Title = table.Column(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column(type: "datetime2", nullable: false),
                    Version = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Translation_Content_ContentId",
                        columns: x => x.ContentId,
                        referencedTable: "Content",
                        referencedColumn: "ContentId");
                });
            migration.CreateIndex(
                name: "IX_Translation_Version_ContentId",
                table: "Translation",
                columns: new[] { "Version", "ContentId" });
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropSequence("DefaultSequence");
            migration.DropTable("Content");
            migration.DropTable("Translation");
        }
    }
}
