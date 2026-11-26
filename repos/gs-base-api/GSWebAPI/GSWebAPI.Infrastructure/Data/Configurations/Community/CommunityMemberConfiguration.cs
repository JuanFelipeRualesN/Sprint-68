using GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.Community
{
    /// <summary>
    /// Table <c>community_members</c>: memberships of users in groups. <c>role</c> (default member) and <c>status</c>
    /// (default active) are MySQL ENUMs. Soft-deletable. Unique (group, <c>user_id</c>), including soft-deleted rows, so
    /// re-joining restores the old row; FK to the group cascades on delete. Indexed by <c>user_id</c>.
    /// </summary>
    public class CommunityMemberConfiguration : IEntityTypeConfiguration<CommunityMember>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CommunityMember> builder)
        {
            builder.ToTable("community_members");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Role).IsDbEnum().HasDefaultValue(CommunityMemberRole.Member);
            builder.Property(e => e.Status).IsDbEnum().HasDefaultValue(CommunityMemberStatus.Active);
            builder.Property(e => e.JoinedAt).HasColumnType("datetime");
            builder.HasSoftDeleteColumns();

            builder.HasOne(e => e.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(e => e.CommunityGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            // The unique key also covers soft-deleted rows: re-joining restores the old row.
            builder.HasIndex(e => new { e.CommunityGroupId, e.UserId }).IsUnique();
            builder.HasIndex(e => e.UserId);

            builder.HasQueryFilter(e => e.DeletedAt == null);
        }
    }
}
