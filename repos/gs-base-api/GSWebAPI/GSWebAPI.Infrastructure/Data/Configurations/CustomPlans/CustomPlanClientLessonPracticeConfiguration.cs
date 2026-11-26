using GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.CustomPlans
{
    /// <summary>
    /// Table <c>custom_plan_client_lesson_practices</c>: practices scheduled for a lesson, with attempt and score
    /// tracking. CHECKs: <c>score</c> and <c>best_score</c> null or BETWEEN 0 AND 100 (DECIMAL(5,2)); unique (lesson,
    /// <c>practice_id</c>); FK to the lesson cascades on delete; practice ids live in Athenas (no FK). <c>status</c> is
    /// a MySQL ENUM defaulting to not_started. Indexed by <c>due_date_time</c> (UTC). Hidden when the plan is
    /// soft-deleted.
    /// </summary>
    public class CustomPlanClientLessonPracticeConfiguration : IEntityTypeConfiguration<CustomPlanClientLessonPractice>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CustomPlanClientLessonPractice> builder)
        {
            builder.ToTable("custom_plan_client_lesson_practices", t =>
            {
                t.HasCheckConstraint("CK_custom_plan_client_lesson_practices_score", "`score` IS NULL OR `score` BETWEEN 0 AND 100");
                t.HasCheckConstraint("CK_custom_plan_client_lesson_practices_best_score", "`best_score` IS NULL OR `best_score` BETWEEN 0 AND 100");
            });
            builder.HasKey(e => e.Id);

            // practice_id points to Athenas: no FK constraint.
            builder.Property(e => e.AssignedDate).HasColumnType("date");
            builder.Property(e => e.DueDateTime).HasColumnType("datetime");
            builder.Property(e => e.Timezone).HasMaxLength(64).IsRequired();
            builder.Property(e => e.Status).IsDbEnum().HasDefaultValue(PlanActivityStatus.NotStarted);
            builder.Property(e => e.Score).HasPrecision(5, 2);
            builder.Property(e => e.BestScore).HasPrecision(5, 2);
            builder.Property(e => e.Attempts).HasDefaultValue((ushort)0);
            builder.Property(e => e.LastAttemptAt).HasColumnType("datetime");
            builder.Property(e => e.CompletedAt).HasColumnType("datetime");
            builder.HasAuditColumns();

            builder.HasOne(e => e.Lesson)
                .WithMany(l => l.Practices)
                .HasForeignKey(e => e.CustomPlanClientLessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.CustomPlanClientLessonId, e.PracticeId }).IsUnique();
            builder.HasIndex(e => e.DueDateTime);

            builder.HasQueryFilter(e => e.Lesson.CustomPlanClient.DeletedAt == null);
        }
    }
}
