using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.CustomPlans
{
    /// <summary>
    /// Table <c>custom_plan_client_subjects</c>: subjects of a plan (subject ids live in Athenas, no FK). Unique (plan,
    /// <c>subject_id</c>); FK to the plan cascades on delete. Hidden when the plan is soft-deleted.
    /// </summary>
    public class CustomPlanClientSubjectConfiguration : IEntityTypeConfiguration<CustomPlanClientSubject>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CustomPlanClientSubject> builder)
        {
            builder.ToTable("custom_plan_client_subjects");
            builder.HasKey(e => e.Id);
            builder.HasAuditColumns();

            builder.HasOne(e => e.CustomPlanClient)
                .WithMany(p => p.Subjects)
                .HasForeignKey(e => e.CustomPlanClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.CustomPlanClientId, e.SubjectId }).IsUnique();

            // No deleted_at: hidden through the soft-deleted parent.
            builder.HasQueryFilter(e => e.CustomPlanClient.DeletedAt == null);
        }
    }
}
