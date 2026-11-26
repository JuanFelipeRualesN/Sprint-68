using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSWebAPI.Migrations
{
    /// <summary>
    /// Aligns the forum categories with the production catalog (general, math, reading_writing, science, organization,
    /// legal_depr, first_steps) and their English / Spanish names. Data only, as idempotent upserts: it fixes a database
    /// that ran an earlier version of <see cref="AddForumCategoryTranslations"/> and changes nothing on one that already
    /// has the right rows. No table structure is altered.
    /// </summary>
    public partial class AlignForumCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(ForumCategorySeedSql.Categories);
            migrationBuilder.Sql(ForumCategorySeedSql.Translations);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Nothing to undo: the rows are the catalog the application expects.
        }
    }
}
