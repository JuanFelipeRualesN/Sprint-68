using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSWebAPI.Migrations
{
    /// <summary>
    /// Forum category names per language: creates <c>community_forum_category_translations</c> (a new table; no table
    /// structure is altered), upserts the seven categories and seeds their English and Spanish names.
    /// </summary>
    public partial class AddForumCategoryTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "community_forum_category_translations",
                columns: table => new
                {
                    community_forum_category_id = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    lang_code = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    name = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_forum_category_translations", x => new { x.community_forum_category_id, x.lang_code });
                    table.ForeignKey(
                        name: "FK_community_forum_category_translations_community_forum_catego~",
                        column: x => x.community_forum_category_id,
                        principalTable: "community_forum_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            // Categories first (a new database still has the original seed), then their names in English and Spanish.
            migrationBuilder.Sql(ForumCategorySeedSql.Categories);
            migrationBuilder.Sql(ForumCategorySeedSql.Translations);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "community_forum_category_translations");
        }
    }
}
