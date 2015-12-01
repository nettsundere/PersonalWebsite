using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace PersonalWebsite.Migrations.DataDb
{
    public partial class ReplaceGuidWithIdForContentsAndTranslations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Translation_Content_ContentContentGuid", table: "Translation");
            migrationBuilder.DropPrimaryKey(name: "PK_Content", table: "Content");
            migrationBuilder.DropColumn(name: "ContentContentGuid", table: "Translation");
            migrationBuilder.DropColumn(name: "ContentGuid", table: "Content");
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Content",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
            migrationBuilder.AddPrimaryKey(
                name: "PK_Content",
                table: "Content",
                column: "Id");
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
            migrationBuilder.DropPrimaryKey(name: "PK_Content", table: "Content");
            migrationBuilder.DropColumn(name: "Id", table: "Content");
            migrationBuilder.AddColumn<Guid>(
                name: "ContentContentGuid",
                table: "Translation",
                nullable: true);
            migrationBuilder.AddColumn<Guid>(
                name: "ContentGuid",
                table: "Content",
                nullable: false,
                defaultValueSql: "newsequentialid()");
            migrationBuilder.AddPrimaryKey(
                name: "PK_Content",
                table: "Content",
                column: "ContentGuid");
            migrationBuilder.AddForeignKey(
                name: "FK_Translation_Content_ContentContentGuid",
                table: "Translation",
                column: "ContentContentGuid",
                principalTable: "Content",
                principalColumn: "ContentGuid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
