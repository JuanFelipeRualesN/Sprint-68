using GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.CustomPlans
{
    /// <summary>
    /// Table <c>custom_plan_client_lesson_tests</c>: pre-tests and post-tests scheduled for a lesson. CHECK
    /// <c>duration_minutes</c> BETWEEN 1 AND 480; unique (lesson, <c>test_type</c>, <c>test_id</c>); FK to the lesson
    /// cascades on delete; optional FK <c>gwp_user_result_id</c> to <c>usqui_user_quiz_results</c> is RESTRICT; test
    /// ids live in Athenas (no FK). <c>test_type</c> and <c>status</c> are MySQL ENUMs. Indexed by <c>due_date_time</c>
    /// (UTC). Hidden when the plan is soft-deleted.
    /// </summary>
    public class CustomPlanClientLessonTestConfiguration : IEntityTypeConfiguration<CustomPlanClientLessonTest>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CustomPlanClientLessonTest> builder)
        {
            builder.ToTable("custom_plan_client_lesson_tests", t =>
                t.HasCheckConstraint("CK_custom_plan_client_lesson_tests_duration", "`duration_minutes` BETWEEN 1 AND 480"));
            builder.HasKey(e => e.Id);

            // test_id points to Athenas: no FK constraint.
            builder.Property(e => e.TestType).IsDbEnum();
            builder.Property(e => e.AssignedDate).HasColumnType("date");
            builder.Property(e => e.DueDateTime).HasColumnType("datetime");
            builder.Property(e => e.Timezone).HasMaxLength(64).IsRequired();
            builder.Property(e => e.Status).IsDbEnum().HasDefaultValue(PlanActivityStatus.NotStarted);
            builder.Property(e => e.CompletedAt).HasColumnType("datetime");

            // Signed INT to match usqui_user_quiz_results.id (real FK, same database).
            builder.Property(e => e.GwpUserResultId).HasColumnType("int");
            builder.HasAuditColumns();

            builder.HasOne(e => e.Lesson)
                .WithMany(l => l.Tests)
                .HasForeignKey(e => e.CustomPlanClientLessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.GwpUserResult)
                .WithMany()
                .HasForeignKey(e => e.GwpUserResultId)
                .HasConstraintName("FK_cpc_lesson_tests_usqui_user_quiz_results")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.CustomPlanClientLessonId, e.TestType, e.TestId }).IsUnique();
            builder.HasIndex(e => e.DueDateTime);

            builder.HasQueryFilter(e => e.Lesson.CustomPlanClient.DeletedAt == null);
        }
    }
}
