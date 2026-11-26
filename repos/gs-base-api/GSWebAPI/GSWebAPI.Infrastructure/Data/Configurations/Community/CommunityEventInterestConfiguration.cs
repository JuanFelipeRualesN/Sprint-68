using GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.Community
{
    /// <summary>
    /// Table <c>community_events_interests</c>: a member's response to an event. CHECK
    /// <c>quantity_of_persons_assisting</c> &gt;= 1 (default 1); <c>response</c> is a MySQL ENUM defaulting to going.
    /// Soft-deletable. Unique (event, <c>user_id</c>), including soft-deleted rows; FK to the event cascades on delete.
    /// Indexed by <c>user_id</c>.
    /// </summary>
    public class CommunityEventInterestConfiguration : IEntityTypeConfiguration<CommunityEventInterest>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CommunityEventInterest> builder)
        {
            builder.ToTable("community_events_interests", t =>
                t.HasCheckConstraint(
                    "CK_community_events_interests_quantity",
                    "`quantity_of_persons_assisting` >= 1"));
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Response).IsDbEnum().HasDefaultValue(CommunityEventResponse.Going);
            builder.Property(e => e.QuantityOfPersonsAssisting).HasDefaultValue((byte)1);
            builder.HasSoftDeleteColumns();

            builder.HasOne(e => e.Event)
                .WithMany(ev => ev.Interests)
                .HasForeignKey(e => e.CommunityEventId)
                .OnDelete(DeleteBehavior.Cascade);

            // The unique key also covers soft-deleted rows: registering again restores the old row.
            builder.HasIndex(e => new { e.CommunityEventId, e.UserId }).IsUnique();
            builder.HasIndex(e => e.UserId);

            builder.HasQueryFilter(e => e.DeletedAt == null);
        }
    }
}
