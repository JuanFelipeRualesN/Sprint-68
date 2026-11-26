using GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.CustomPlans
{
    /// <summary>
    /// Table <c>custom_plan_client_lessons</c>: lessons of a plan with their learning window and progress flags.
    /// CHECK <c>learning_due_date</c> &gt;= <c>learning_assigned_date</c>; unique (plan, <c>lesson_id</c>); FK to the plan
    /// cascades on delete; lesson and standard ids live in Athenas (no FK). <c>lesson_status</c> is a MySQL ENUM
    /// defaulting to not_started and every completion flag defaults to false. Indexed by (plan, sequence), (plan,
    /// learning due date) and (plan, subject). Hidden when the plan is soft-deleted.
    /// </summary>
    public class CustomPlanClientLessonConfiguration : IEntityTypeConfiguration<CustomPlanClientLesson>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CustomPlanClientLesson> builder)
        {
            builder.ToTable("custom_plan_client_lessons", t =>
                t.HasCheckConstraint("CK_custom_plan_client_lessons_learning_dates", "`learning_due_date` >= `learning_assigned_date`"));
            builder.HasKey(e => e.Id);
            builder.Property(e => e.LearningAssignedDate).HasColumnType("date");
            builder.Property(e => e.LearningDueDate).HasColumnType("date");
            builder.Property(e => e.LearningTimezone).HasMaxLength(64).IsRequired();
            builder.Property(e => e.LearningCompletedAt).HasColumnType("datetime");

            // lesson_id and standard_id point to Athenas: no FK constraint.
            builder.Property(e => e.LessonStatus).IsDbEnum().HasDefaultValue(PlanLessonStatus.NotStarted);
            builder.Property(e => e.TotalStudyMinutes).HasDefaultValue(0u);
            builder.Property(e => e.ConceptCompleted).HasDefaultValue(false);
            builder.Property(e => e.DefinitionsCompleted).HasDefaultValue(false);
            builder.Property(e => e.ExamplesCompleted).HasDefaultValue(false);
            builder.Property(e => e.PreCompleted).HasDefaultValue(false);
            builder.Property(e => e.PracticesCompleted).HasDefaultValue(false);
            builder.Property(e => e.TestCompleted).HasDefaultValue(false);
            builder.Property(e => e.PostCompleted).HasDefaultValue(false);
            builder.Property(e => e.CompletedAt).HasColumnType("datetime");
            builder.HasAuditColumns();

            builder.HasOne(e => e.CustomPlanClient)
                .WithMany(p => p.Lessons)
                .HasForeignKey(e => e.CustomPlanClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.CustomPlanClientId, e.LessonId }).IsUnique();
            builder.HasIndex(e => new { e.CustomPlanClientId, e.Sequence });
            builder.HasIndex(e => new { e.CustomPlanClientId, e.LearningDueDate });
            builder.HasIndex(e => new { e.CustomPlanClientId, e.SubjectId });

            builder.HasQueryFilter(e => e.CustomPlanClient.DeletedAt == null);
        }
    }
}
