using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GSAIServicesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddQuotaSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "quota_resource_limits",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    resource_type_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    scope = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    scope_id = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    max_amount = table.Column<long>(type: "bigint", nullable: false),
                    period_type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    custom_period_days = table.Column<int>(type: "int", nullable: true),
                    rollover_enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    rollover_max_amount = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quota_resource_limits", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "quota_resource_types",
                columns: table => new
                {
                    code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    unit = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quota_resource_types", x => x.code);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "quota_settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    quotas_enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ai_enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    default_scope = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quota_settings", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "quota_usage_log",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    resource_type_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usage_period_id = table.Column<long>(type: "bigint", nullable: true),
                    reservation_id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    action = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    reference = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quota_usage_log", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "quota_usage_periods",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    resource_type_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    limit_id = table.Column<int>(type: "int", nullable: false),
                    period_start = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    period_end = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    allocated_amount = table.Column<long>(type: "bigint", nullable: false),
                    consumed_amount = table.Column<long>(type: "bigint", nullable: false),
                    rollover_carried_in = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quota_usage_periods", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "quota_resource_types",
                columns: new[] { "code", "created_at", "is_active", "name", "unit", "updated_at" },
                values: new object[,]
                {
                    { "IMAGE_COUNT", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Image generation", "images", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { "TEXT_TOKENS", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Text generation (tokens)", "tokens", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "quota_settings",
                columns: new[] { "id", "ai_enabled", "created_at", "default_scope", "quotas_enabled", "updated_at" },
                values: new object[] { 1, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Global", false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.CreateIndex(
                name: "IX_quota_resource_limits_resource_type_code_scope_scope_id",
                table: "quota_resource_limits",
                columns: new[] { "resource_type_code", "scope", "scope_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_quota_usage_log_user_id_resource_type_code_created_at",
                table: "quota_usage_log",
                columns: new[] { "user_id", "resource_type_code", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_quota_usage_periods_user_id_resource_type_code_period_start_~",
                table: "quota_usage_periods",
                columns: new[] { "user_id", "resource_type_code", "period_start", "limit_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "quota_resource_limits");

            migrationBuilder.DropTable(
                name: "quota_resource_types");

            migrationBuilder.DropTable(
                name: "quota_settings");

            migrationBuilder.DropTable(
                name: "quota_usage_log");

            migrationBuilder.DropTable(
                name: "quota_usage_periods");
        }
    }
}
