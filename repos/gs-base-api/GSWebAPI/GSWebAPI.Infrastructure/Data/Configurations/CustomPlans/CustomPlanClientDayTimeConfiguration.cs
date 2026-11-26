using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.CustomPlans
{
    /// <summary>
    /// Table <c>custom_plan_client_day_times</c>: time slots of a schedule day (<c>start_time</c> TIME in the slot's
    /// <c>timezone</c>). Unique (day, <c>start_time</c>); FK to the day cascades on delete. Hidden when the plan is
    /// soft-deleted.
    /// </summary>
    public class CustomPlanClientDayTimeConfiguration : IEntityTypeConfiguration<CustomPlanClientDayTime>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CustomPlanClientDayTime> builder)
        {
            builder.ToTable("custom_plan_client_day_times");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.StartTime).HasColumnType("time");
            builder.Property(e => e.Timezone).HasMaxLength(64).IsRequired();
            builder.HasAuditColumns();

            builder.HasOne(e => e.Day)
                .WithMany(d => d.Times)
                .HasForeignKey(e => e.CustomPlanClientDayId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.CustomPlanClientDayId, e.StartTime }).IsUnique();

            builder.HasQueryFilter(e => e.Day.CustomPlanClient.DeletedAt == null);
        }
    }
}
