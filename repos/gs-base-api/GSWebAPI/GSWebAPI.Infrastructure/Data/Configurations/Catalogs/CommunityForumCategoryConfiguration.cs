using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.Catalogs
{
    /// <summary>
    /// Table <c>community_forum_categories</c>: catalog of forum categories. Unique <c>code</c>; <c>moderators_only</c>
    /// defaults to false. Seeded with general, math, reading_writing, science, organization, legal_depr and first_steps (the
    /// production catalog). <c>name</c> is the default name; the names per language are in the translations table.
    /// </summary>
    public class CommunityForumCategoryConfiguration : IEntityTypeConfiguration<CommunityForumCategory>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CommunityForumCategory> builder)
        {
            builder.ToTable("community_forum_categories");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Code).HasMaxLength(30).IsRequired();
            builder.Property(e => e.Name).HasMaxLength(60).IsRequired();
            builder.Property(e => e.ModeratorsOnly).HasDefaultValue(false);
            builder.HasIndex(e => e.Code).IsUnique();
            builder.HasAuditColumns();

            // Same rows (and dates) as the production catalog.
            var added = new DateTime(2026, 11, 19, 14, 46, 19, DateTimeKind.Utc);
            builder.HasData(
                Seed(CommunityForumCategory.GeneralId, "general", "General"),
                Seed(CommunityForumCategory.MathId, "math", "Matemáticas"),
                Seed(CommunityForumCategory.ReadingWritingId, "reading_writing", "Lectura y Escritura"),
                Seed(CommunityForumCategory.ScienceId, "science", "Ciencias"),
                Seed(CommunityForumCategory.OrganizationId, "organization", "Organización"),
                Seed(CommunityForumCategory.LegalDeprId, "legal_depr", "Legal y DEPR", added),
                Seed(CommunityForumCategory.FirstStepsId, "first_steps", "Primeros pasos", added));
        }

        private static CommunityForumCategory Seed(byte id, string code, string name, DateTime? createdAt = null) => new()
        {
            Id = id,
            Code = code,
            Name = name,
            ModeratorsOnly = false,
            CreatedAt = createdAt ?? AuditColumnsExtensions.SeedDate,
            UpdatedAt = createdAt ?? AuditColumnsExtensions.SeedDate,
        };
    }
}
