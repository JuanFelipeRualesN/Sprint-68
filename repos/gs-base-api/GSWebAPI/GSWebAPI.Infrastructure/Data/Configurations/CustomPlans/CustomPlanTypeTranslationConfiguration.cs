using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.CustomPlans
{
    /// <summary>
    /// Table <c>custom_plan_type_translations</c>: plan type names per language. Primary key
    /// <c>(custom_plan_type_id, lang_code)</c>; FK to the type with CASCADE. The English and Spanish rows are written by the
    /// migration matching the types by <c>code</c> (not by id), so they fit whatever ids the catalog has.
    /// </summary>
    public class CustomPlanTypeTranslationConfiguration : IEntityTypeConfiguration<CustomPlanTypeTranslation>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CustomPlanTypeTranslation> builder)
        {
            builder.ToTable("custom_plan_type_translations");
            builder.HasKey(e => new { e.CustomPlanTypeId, e.LangCode });
            builder.Property(e => e.LangCode).HasMaxLength(5).IsRequired();
            builder.Property(e => e.Name).HasMaxLength(60).IsRequired();
            builder.HasAuditColumns();

            builder.HasOne(e => e.CustomPlanType)
                .WithMany(t => t.Translations)
                .HasForeignKey(e => e.CustomPlanTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
