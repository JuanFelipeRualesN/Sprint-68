using GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.CustomPlans
{
    /// <summary>
    /// Table <c>custom_plan_client_lesson_exam_dates</c>: exams scheduled for a lesson. CHECK <c>duration_minutes</c>
    /// BETWEEN 1 AND 480; unique (lesson, <c>exam_id</c>, <c>assigned_date</c>); FK to the lesson cascades on delete;
    /// optional FK <c>gwp_user_result_id</c> to <c>usqui_user_quiz_results</c> is RESTRICT. <c>status</c> is a MySQL
    /// ENUM defaulting to not_started. Indexed by <c>due_date_time</c> (UTC). Hidden when the plan is soft-deleted.
    /// </summary>
    public class CustomPlanClientLessonExamDateConfiguration : IEntityTypeConfiguration<CustomPlanClientLessonExamDate>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CustomPlanClientLessonExamDate> builder)
        {
            builder.ToTable("custom_plan_client_lesson_exam_dates", t =>
                t.HasCheckConstraint("CK_custom_plan_client_lesson_exam_dates_duration", "`duration_minutes` BETWEEN 1 AND 480"));
            builder.HasKey(e => e.Id);
            builder.Property(e => e.AssignedDate).HasColumnType("date");
            builder.Property(e => e.DueDateTime).HasColumnType("datetime");
            builder.Property(e => e.Timezone).HasMaxLength(64).IsRequired();

            // Signed INT to match usqui_user_quiz_results.id (real FK, same database).
            builder.Property(e => e.GwpUserResultId).HasColumnType("int");
            builder.Property(e => e.CompletedAt).HasColumnType("datetime");
            builder.Property(e => e.Status).IsDbEnum().HasDefaultValue(PlanActivityStatus.NotStarted);
            builder.HasAuditColumns();

            builder.HasOne(e => e.Lesson)
                .WithMany(l => l.ExamDates)
                .HasForeignKey(e => e.CustomPlanClientLessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.GwpUserResult)
                .WithMany()
                .HasForeignKey(e => e.GwpUserResultId)
                .HasConstraintName("FK_cpc_lesson_exam_dates_usqui_user_quiz_results")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.CustomPlanClientLessonId, e.ExamId, e.AssignedDate }).IsUnique();
            builder.HasIndex(e => e.DueDateTime);

            builder.HasQueryFilter(e => e.Lesson.CustomPlanClient.DeletedAt == null);
        }
    }
}
