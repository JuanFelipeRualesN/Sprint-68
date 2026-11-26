using GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.Community
{
    /// <summary>
    /// Table <c>community_events</c>: events of a community group. CHECK <c>event_end_date_time</c> null or after
    /// <c>event_date_time</c> (both UTC). Soft-deletable. FKs to the group and the meeting format are RESTRICT;
    /// <c>organizer_user_id</c> lives in the users DB (no FK). <c>event_availability</c> is a MySQL ENUM defaulting to
    /// open. Indexed by (group, <c>event_date_time</c>) and by organizer.
    /// </summary>
    public class CommunityEventConfiguration : IEntityTypeConfiguration<CommunityEvent>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CommunityEvent> builder)
        {
            builder.ToTable("community_events", t =>
                t.HasCheckConstraint(
                    "CK_community_events_end_after_start",
                    "`event_end_date_time` IS NULL OR `event_end_date_time` > `event_date_time`"));
            builder.HasKey(e => e.Id);
            builder.Property(e => e.EventTitle).HasMaxLength(150).IsRequired();
            builder.Property(e => e.EventDescription).HasColumnType("text");
            builder.Property(e => e.EventLocation).HasMaxLength(255);
            builder.Property(e => e.EventDateTime).HasColumnType("datetime");
            builder.Property(e => e.EventEndDateTime).HasColumnType("datetime");
            builder.Property(e => e.EventTimezone).HasMaxLength(64).IsRequired();
            builder.Property(e => e.EventAvailability).IsDbEnum().HasDefaultValue(CommunityEventAvailability.Open);
            builder.HasSoftDeleteColumns();

            // Events are not in the CASCADE list: RESTRICT (groups are soft-deleted anyway).
            builder.HasOne(e => e.Group)
                .WithMany(g => g.Events)
                .HasForeignKey(e => e.CommunityGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.MeetingFormat)
                .WithMany()
                .HasForeignKey(e => e.MeetingFormatId)
                .OnDelete(DeleteBehavior.Restrict);

            // organizer_user_id points to the users DB: no FK constraint.
            builder.HasIndex(e => new { e.CommunityGroupId, e.EventDateTime });
            builder.HasIndex(e => e.OrganizerUserId);

            builder.HasQueryFilter(e => e.DeletedAt == null);
        }
    }
}
