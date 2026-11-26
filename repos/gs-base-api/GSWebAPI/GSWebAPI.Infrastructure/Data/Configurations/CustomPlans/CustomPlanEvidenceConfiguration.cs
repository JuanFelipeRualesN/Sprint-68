using GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.CustomPlans
{
    /// <summary>
    /// Table <c>custom_plan_evidences</c>: evidence (uploaded file or external link) attached to a plan and optionally
    /// to a lesson. Soft-deletable, and also hidden when the plan is soft-deleted. FK to the plan cascades on delete;
    /// FK to the lesson is SET NULL, so the evidence survives when draft lessons are replaced. <c>evidence_type</c> is
    /// a MySQL ENUM defaulting to evidence. Indexed by (plan, lesson) and by <c>uploaded_by_user_id</c>.
    /// </summary>
    public class CustomPlanEvidenceConfiguration : IEntityTypeConfiguration<CustomPlanEvidence>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CustomPlanEvidence> builder)
        {
            builder.ToTable("custom_plan_evidences");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.EvidenceType).IsDbEnum().HasDefaultValue(CustomPlanEvidenceType.Evidence);
            builder.Property(e => e.Title).HasMaxLength(150).IsRequired();
            builder.Property(e => e.Description).HasColumnType("text");
            builder.Property(e => e.Url).HasMaxLength(1000).IsRequired();
            builder.Property(e => e.BlobName).HasMaxLength(500);
            builder.Property(e => e.FileName).HasMaxLength(255);
            builder.Property(e => e.ContentType).HasMaxLength(100);
            builder.HasSoftDeleteColumns();

            builder.HasOne(e => e.CustomPlanClient)
                .WithMany(p => p.Evidences)
                .HasForeignKey(e => e.CustomPlanClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Lessons are hard-deleted when a draft structure changes: keep the evidence on the plan.
            builder.HasOne(e => e.Lesson)
                .WithMany(l => l.Evidences)
                .HasForeignKey(e => e.CustomPlanClientLessonId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(e => new { e.CustomPlanClientId, e.CustomPlanClientLessonId });
            builder.HasIndex(e => e.UploadedByUserId);

            builder.HasQueryFilter(e => e.DeletedAt == null && e.CustomPlanClient.DeletedAt == null);
        }
    }
}
