using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.CustomPlans
{
    /// <summary>
    /// Table <c>custom_plan_types</c>: catalog of plan types. Unique <c>code</c>. Seeded with homeschool (1),
    /// reinforcements (2), specific_task (3) and customized (4).
    /// </summary>
    public class CustomPlanTypeConfiguration : IEntityTypeConfiguration<CustomPlanType>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<CustomPlanType> builder)
        {
            builder.ToTable("custom_plan_types");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Code).HasMaxLength(30).IsRequired();
            builder.Property(e => e.Name).HasMaxLength(60).IsRequired();
            builder.HasIndex(e => e.Code).IsUnique();
            builder.HasAuditColumns();

            var seed = AuditColumnsExtensions.SeedDate;
            builder.HasData(
                new CustomPlanType { Id = 1, Code = "homeschool", Name = "Homeschool", CreatedAt = seed, UpdatedAt = seed },
                new CustomPlanType { Id = 2, Code = "reinforcements", Name = "Reinforcements", CreatedAt = seed, UpdatedAt = seed },
                new CustomPlanType { Id = 3, Code = "specific_task", Name = "Specific task", CreatedAt = seed, UpdatedAt = seed },
                new CustomPlanType { Id = 4, Code = "customized", Name = "Customized", CreatedAt = seed, UpdatedAt = seed });
        }
    }
}
