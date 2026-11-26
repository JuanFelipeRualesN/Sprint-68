using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSWebAPI.Migrations
{
    /// <summary>
    /// Custom plan type names per language: creates <c>custom_plan_type_translations</c> (a new table; no existing table is
    /// altered) and writes the English and Spanish names, matching the types by <c>code</c> so they fit whatever ids the
    /// catalog has. Codes missing from the catalog are skipped; running it again only updates the names.
    /// </summary>
    public partial class AddCustomPlanTypeTranslations : Migration
    {
        /// <summary>English and Spanish name of each type, by code.</summary>
        private static readonly (string Code, string English, string Spanish)[] Names =
        {
            ("homeschool", "Homeschool", "Educación en el hogar"),
            ("reinforcements", "Reinforcements", "Refuerzos"),
            ("specific_task", "Specific task", "Tarea específica"),
            ("customized", "Customized", "Personalizado"),
        };

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "custom_plan_type_translations",
                columns: table => new
                {
                    custom_plan_type_id = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    lang_code = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    name = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_custom_plan_type_translations", x => new { x.custom_plan_type_id, x.lang_code });
                    table.ForeignKey(
                        name: "FK_custom_plan_type_translations_custom_plan_types_custom_plan_~",
                        column: x => x.custom_plan_type_id,
                        principalTable: "custom_plan_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            foreach (var (code, english, spanish) in Names)
            {
                foreach (var (lang, name) in new[] { ("en", english), ("es", spanish) })
                {
                    migrationBuilder.Sql($@"INSERT INTO `custom_plan_type_translations` (`custom_plan_type_id`, `lang_code`, `name`, `created_at`, `updated_at`)
SELECT t.`id`, '{lang}', '{name}', '2026-11-05 00:00:00', '2026-11-05 00:00:00' FROM `custom_plan_types` t WHERE t.`code` = '{code}'
ON DUPLICATE KEY UPDATE `name` = VALUES(`name`);");
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "custom_plan_type_translations");
        }
    }
}
