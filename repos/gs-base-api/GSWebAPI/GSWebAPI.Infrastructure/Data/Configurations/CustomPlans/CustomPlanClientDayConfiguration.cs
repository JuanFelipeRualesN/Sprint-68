using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.CustomPlans
{
    /// <summary>
    /// Table <c>custom_plan_client_days</c>: weekdays of the plan schedule. CHECK <c>day_of_week</c> BETWEEN 1 AND 7;
    /// unique (plan, <c>day_of_week</c>); FK to the plan cascades on delete. Hidden when the plan is soft-deleted.
    /// </summary>
    public class CustomPlanClientDayConfiguration : IEntityTypeConfiguration<CustomPlanClientDay>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CustomPlanClientDay> builder)
        {
            builder.ToTable("custom_plan_client_days", t =>
                t.HasCheckConstraint("CK_custom_plan_client_days_day_of_week", "`day_of_week` BETWEEN 1 AND 7"));
            builder.HasKey(e => e.Id);
            builder.HasAuditColumns();

            builder.HasOne(e => e.CustomPlanClient)
                .WithMany(p => p.Days)
                .HasForeignKey(e => e.CustomPlanClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.CustomPlanClientId, e.DayOfWeek }).IsUnique();

            builder.HasQueryFilter(e => e.CustomPlanClient.DeletedAt == null);
        }
    }
}
