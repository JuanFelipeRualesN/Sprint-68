using GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.Community
{
    /// <summary>
    /// Table <c>community_resources</c>: shared links and uploaded files (blob_name set), of a group or platform-wide (null group). Soft-deletable;
    /// <c>resource_type</c> is a MySQL ENUM defaulting to link. FK to the group is optional and RESTRICT;
    /// <c>author_id</c> lives in the users DB (no FK) and is indexed.
    /// </summary>
    public class CommunityResourceConfiguration : IEntityTypeConfiguration<CommunityResource>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CommunityResource> builder)
        {
            builder.ToTable("community_resources");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Title).HasMaxLength(150).IsRequired();
            builder.Property(e => e.Description).HasColumnType("text");
            builder.Property(e => e.ResourceType).IsDbEnum().HasDefaultValue(CommunityResourceType.Link);
            builder.Property(e => e.Link).HasMaxLength(500).IsRequired();
            builder.Property(e => e.BlobName).HasMaxLength(500);
            builder.Property(e => e.FileName).HasMaxLength(255);
            builder.Property(e => e.ContentType).HasMaxLength(100);
            builder.Property(e => e.PublishedDate).HasColumnType("datetime");
            builder.HasSoftDeleteColumns();

            // Resources are not in the CASCADE list: RESTRICT. Null group = platform-wide resource.
            builder.HasOne(e => e.Group)
                .WithMany(g => g.Resources)
                .HasForeignKey(e => e.CommunityGroupId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // author_id points to the users DB: no FK constraint.
            builder.HasIndex(e => e.AuthorId);

            builder.HasQueryFilter(e => e.DeletedAt == null);
        }
    }
}
