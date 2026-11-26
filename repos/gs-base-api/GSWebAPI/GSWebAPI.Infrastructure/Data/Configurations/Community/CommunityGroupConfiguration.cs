using GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.Community
{
    /// <summary>
    /// Table <c>community_groups</c>: community groups. Soft-deletable; <c>visibility</c> is a MySQL ENUM defaulting to
    /// public. <c>created_by_user_id</c> lives in the users DB (no FK) and is indexed.
    /// </summary>
    public class CommunityGroupConfiguration : IEntityTypeConfiguration<CommunityGroup>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CommunityGroup> builder)
        {
            builder.ToTable("community_groups");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.CommunityName).HasMaxLength(120).IsRequired();
            builder.Property(e => e.CommunityLocation).HasMaxLength(255);
            builder.Property(e => e.CommunityDescription).HasColumnType("text");
            builder.Property(e => e.Visibility).IsDbEnum().HasDefaultValue(CommunityGroupVisibility.Public);
            builder.HasSoftDeleteColumns();

            // created_by_user_id points to the users DB: no FK constraint.
            builder.HasIndex(e => e.CreatedByUserId);

            builder.HasQueryFilter(e => e.DeletedAt == null);
        }
    }
}
