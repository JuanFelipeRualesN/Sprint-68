using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.CustomPlans
{
    /// <summary>
    /// Table <c>curriculum_frameworks</c>: curriculum standards frameworks a plan can be aligned to. Unique <c>code</c>;
    /// <c>country_code</c> is CHAR(2); <c>is_active</c> defaults to true. Seeded with the Puerto Rico Department of
    /// Education framework (id 1).
    /// </summary>
    public class CurriculumFrameworkConfiguration : IEntityTypeConfiguration<CurriculumFramework>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CurriculumFramework> builder)
        {
            builder.ToTable("curriculum_frameworks");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Code).HasMaxLength(30).IsRequired();
            builder.Property(e => e.Name).HasMaxLength(120).IsRequired();
            builder.Property(e => e.CountryCode).HasColumnType("char(2)").IsRequired();
            builder.Property(e => e.Region).HasMaxLength(80);
            builder.Property(e => e.IsActive).HasField("_isActive").HasDefaultValue(true);
            builder.HasIndex(e => e.Code).IsUnique();
            builder.HasAuditColumns();

            builder.HasData(new CurriculumFramework
            {
                Id = 1,
                Code = CurriculumFramework.DeprCode,
                Name = "Departamento de Educación de Puerto Rico",
                CountryCode = "PR",
                Region = null,
                IsActive = true,
                CreatedAt = AuditColumnsExtensions.SeedDate,
                UpdatedAt = AuditColumnsExtensions.SeedDate,
            });
        }
    }
}
