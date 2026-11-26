using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSWebAPI.Migrations
{
    /// <summary>
    /// Uploaded files for community resources: four nullable columns on <c>community_resources</c> (existing rows stay
    /// external links) and the <c>audio</c> value appended to <c>resource_type</c>. Only this table is touched; no
    /// database-level change.
    /// </summary>
    public partial class AddCommunityResourceFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "blob_name",
                table: "community_resources",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "file_name",
                table: "community_resources",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "content_type",
                table: "community_resources",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<ulong>(
                name: "size_bytes",
                table: "community_resources",
                type: "bigint unsigned",
                nullable: true);

            // Appending a value keeps every stored value as it is.
            migrationBuilder.AlterColumn<string>(
                name: "resource_type",
                table: "community_resources",
                type: "enum('link','document','video','audio')",
                nullable: false,
                defaultValue: "link",
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "enum('link','document','video')",
                oldDefaultValue: "link",
                oldCollation: "utf8mb4_0900_ai_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Audio rows become documents so the enum can shrink without failing.
            migrationBuilder.Sql("UPDATE `community_resources` SET `resource_type` = 'document' WHERE `resource_type` = 'audio';");

            migrationBuilder.AlterColumn<string>(
                name: "resource_type",
                table: "community_resources",
                type: "enum('link','document','video')",
                nullable: false,
                defaultValue: "link",
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "enum('link','document','video','audio')",
                oldDefaultValue: "link",
                oldCollation: "utf8mb4_0900_ai_ci");

            migrationBuilder.DropColumn(
                name: "size_bytes",
                table: "community_resources");

            migrationBuilder.DropColumn(
                name: "content_type",
                table: "community_resources");

            migrationBuilder.DropColumn(
                name: "file_name",
                table: "community_resources");

            migrationBuilder.DropColumn(
                name: "blob_name",
                table: "community_resources");
        }
    }
}
