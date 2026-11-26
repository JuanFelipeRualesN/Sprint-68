using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSWebAPI.Migrations
{
    /// <summary>
    /// Study time: adds <c>total_study_minutes</c> (INT UNSIGNED, default 0) to <c>custom_plan_client</c> and
    /// <c>custom_plan_client_lessons</c>. Existing rows start at 0. Only these two columns are added; no other change.
    /// </summary>
    public partial class AddPlanStudyMinutes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "total_study_minutes",
                table: "custom_plan_client",
                type: "int unsigned",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "total_study_minutes",
                table: "custom_plan_client_lessons",
                type: "int unsigned",
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_study_minutes",
                table: "custom_plan_client_lessons");

            migrationBuilder.DropColumn(
                name: "total_study_minutes",
                table: "custom_plan_client");
        }
    }
}
