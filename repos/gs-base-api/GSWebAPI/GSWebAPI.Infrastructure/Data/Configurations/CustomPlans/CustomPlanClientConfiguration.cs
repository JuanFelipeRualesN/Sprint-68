using GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.CustomPlans
{
    /// <summary>
    /// Table <c>custom_plan_client</c>: a custom plan of a client for one student. Soft-deletable (query filter on
    /// <c>deleted_at</c>); <c>status</c> is a MySQL ENUM defaulting to draft. FKs to the plan type and the optional
    /// curriculum framework are RESTRICT; client, student and level ids live in other databases and have no FK.
    /// Indexed by <c>client_id</c> and by (<c>student_id</c>, <c>status</c>).
    /// </summary>
    public class CustomPlanClientConfiguration : IEntityTypeConfiguration<CustomPlanClient>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CustomPlanClient> builder)
        {
            builder.ToTable("custom_plan_client");
            builder.HasKey(e => e.Id);

            // client_id, student_id and level_id point to other databases (users DB / Athenas): no FK constraint,
            // existence is checked in CustomPlanService.
            builder.Property(e => e.StartDate).HasColumnType("date");
            builder.Property(e => e.EndDate).HasColumnType("date");
            builder.Property(e => e.Status).IsDbEnum().HasDefaultValue(CustomPlanStatus.Draft);
            builder.Property(e => e.TotalStudyMinutes).HasDefaultValue(0u);
            builder.HasSoftDeleteColumns();

            builder.HasOne(e => e.CustomPlanType)
                .WithMany()
                .HasForeignKey(e => e.CustomPlanTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.CurriculumFramework)
                .WithMany()
                .HasForeignKey(e => e.CurriculumFrameworkId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.ClientId);
            builder.HasIndex(e => new { e.StudentId, e.Status });

            builder.HasQueryFilter(e => e.DeletedAt == null);
        }
    }
}
