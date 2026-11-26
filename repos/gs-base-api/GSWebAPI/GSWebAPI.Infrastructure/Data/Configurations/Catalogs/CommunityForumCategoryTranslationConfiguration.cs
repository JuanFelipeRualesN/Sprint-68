using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.Catalogs
{
    /// <summary>
    /// Table <c>community_forum_category_translations</c>: category names per language. Primary key
    /// <c>(community_forum_category_id, lang_code)</c>; FK to the category with CASCADE. Seeded in English and Spanish for
    /// the seven seeded categories.
    /// </summary>
    public class CommunityForumCategoryTranslationConfiguration : IEntityTypeConfiguration<CommunityForumCategoryTranslation>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CommunityForumCategoryTranslation> builder)
        {
            builder.ToTable("community_forum_category_translations");
            builder.HasKey(e => new { e.CommunityForumCategoryId, e.LangCode });
            builder.Property(e => e.LangCode).HasMaxLength(5).IsRequired();
            builder.Property(e => e.Name).HasMaxLength(60).IsRequired();
            builder.HasAuditColumns();

            builder.HasOne(e => e.Category)
                .WithMany(c => c.Translations)
                .HasForeignKey(e => e.CommunityForumCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            const string en = CommunityForumCategoryTranslation.English;
            const string es = CommunityForumCategoryTranslation.Spanish;
            builder.HasData(
                Seed(CommunityForumCategory.GeneralId, en, "General"),
                Seed(CommunityForumCategory.GeneralId, es, "General"),
                Seed(CommunityForumCategory.MathId, en, "Math"),
                Seed(CommunityForumCategory.MathId, es, "Matemáticas"),
                Seed(CommunityForumCategory.ReadingWritingId, en, "Reading and Writing"),
                Seed(CommunityForumCategory.ReadingWritingId, es, "Lectura y Escritura"),
                Seed(CommunityForumCategory.ScienceId, en, "Science"),
                Seed(CommunityForumCategory.ScienceId, es, "Ciencias"),
                Seed(CommunityForumCategory.OrganizationId, en, "Organization"),
                Seed(CommunityForumCategory.OrganizationId, es, "Organización"),
                Seed(CommunityForumCategory.LegalDeprId, en, "Legal and DEPR"),
                Seed(CommunityForumCategory.LegalDeprId, es, "Legal y DEPR"),
                Seed(CommunityForumCategory.FirstStepsId, en, "First Steps"),
                Seed(CommunityForumCategory.FirstStepsId, es, "Primeros pasos"));
        }

        private static CommunityForumCategoryTranslation Seed(byte categoryId, string langCode, string name) => new()
        {
            CommunityForumCategoryId = categoryId,
            LangCode = langCode,
            Name = name,
            CreatedAt = AuditColumnsExtensions.SeedDate,
            UpdatedAt = AuditColumnsExtensions.SeedDate,
        };
    }
}
