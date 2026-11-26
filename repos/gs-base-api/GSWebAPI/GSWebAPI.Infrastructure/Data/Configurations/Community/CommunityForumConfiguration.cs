using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.Community
{
    /// <summary>
    /// Table <c>community_forums</c>: discussion threads of a group. Soft-deletable; <c>is_pinned</c> and
    /// <c>is_locked</c> default to false. FK to the group cascades on delete; FK to the category is RESTRICT;
    /// <c>user_id</c> lives in the users DB (no FK). Indexed by (group, <c>is_pinned</c>) and by <c>user_id</c>.
    /// </summary>
    public class CommunityForumConfiguration : IEntityTypeConfiguration<CommunityForum>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CommunityForum> builder)
        {
            builder.ToTable("community_forums");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Title).HasMaxLength(150).IsRequired();
            builder.Property(e => e.Description).HasColumnType("text");
            builder.Property(e => e.IsPinned).HasDefaultValue(false);
            builder.Property(e => e.IsLocked).HasDefaultValue(false);
            builder.HasSoftDeleteColumns();

            builder.HasOne(e => e.Group)
                .WithMany(g => g.Forums)
                .HasForeignKey(e => e.CommunityGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CommunityForumCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Group listing: pinned first. user_id points to the users DB: no FK constraint.
            builder.HasIndex(e => new { e.CommunityGroupId, e.IsPinned });
            builder.HasIndex(e => e.UserId);

            builder.HasQueryFilter(e => e.DeletedAt == null);
        }
    }
}
