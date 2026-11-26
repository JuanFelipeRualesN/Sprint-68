using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.CustomPlans
{
    /// <summary>
    /// Table <c>custom_plan_client_lesson_dates</c>: study dates assigned to a lesson. Unique (lesson,
    /// <c>assigned_date</c>); FK to the lesson cascades on delete. Hidden when the plan is soft-deleted.
    /// </summary>
    public class CustomPlanClientLessonDateConfiguration : IEntityTypeConfiguration<CustomPlanClientLessonDate>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CustomPlanClientLessonDate> builder)
        {
            builder.ToTable("custom_plan_client_lesson_dates");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.AssignedDate).HasColumnType("date");
            builder.HasAuditColumns();

            builder.HasOne(e => e.Lesson)
                .WithMany(l => l.Dates)
                .HasForeignKey(e => e.CustomPlanClientLessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.CustomPlanClientLessonId, e.AssignedDate }).IsUnique();

            builder.HasQueryFilter(e => e.Lesson.CustomPlanClient.DeletedAt == null);
        }
    }
}
