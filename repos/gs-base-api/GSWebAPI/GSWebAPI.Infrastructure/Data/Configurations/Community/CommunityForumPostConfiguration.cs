using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.Community
{
    /// <summary>
    /// Table <c>community_forum_posts</c>: posts and replies of a forum. Soft-deletable. FK to the forum cascades on
    /// delete; self FK <c>parent_post_id</c> is ClientNoAction (replies are kept when the parent is soft-deleted, and the
    /// database rejects hard-deleting a post with replies). Indexed by (forum, <c>created_at</c>) and by <c>user_id</c>.
    /// </summary>
    public class CommunityForumPostConfiguration : IEntityTypeConfiguration<CommunityForumPost>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CommunityForumPost> builder)
        {
            builder.ToTable("community_forum_posts");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.PostText).HasColumnType("text").IsRequired();
            builder.Property(e => e.EditedAt).HasColumnType("datetime");
            builder.HasSoftDeleteColumns();

            builder.HasOne(e => e.Forum)
                .WithMany(f => f.Posts)
                .HasForeignKey(e => e.CommunityForumId)
                .OnDelete(DeleteBehavior.Cascade);

            // Replies are kept when their parent is (soft) deleted: ClientNoAction stops EF from cascading or
            // nulling tracked replies, and the DB rejects hard-deleting a post that still has replies.
            builder.HasOne(e => e.ParentPost)
                .WithMany(p => p.Replies)
                .HasForeignKey(e => e.ParentPostId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientNoAction);

            // Posts of a forum in order. user_id points to the users DB: no FK constraint.
            builder.HasIndex(e => new { e.CommunityForumId, e.CreatedAt });
            builder.HasIndex(e => e.UserId);

            builder.HasQueryFilter(e => e.DeletedAt == null);
        }
    }
}
