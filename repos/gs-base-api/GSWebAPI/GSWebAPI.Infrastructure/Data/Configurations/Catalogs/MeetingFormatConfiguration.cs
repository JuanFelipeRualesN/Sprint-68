using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Catalogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations.Catalogs
{
    /// <summary>
    /// Table <c>meeting_formats</c>: catalog of community event formats. Unique <c>code</c>. Seeded with online and
    /// in_person.
    /// </summary>
    public class MeetingFormatConfiguration : IEntityTypeConfiguration<MeetingFormat>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<MeetingFormat> builder)
        {
            builder.ToTable("meeting_formats");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Code).HasMaxLength(30).IsRequired();
            builder.Property(e => e.Name).HasMaxLength(60).IsRequired();
            builder.HasIndex(e => e.Code).IsUnique();
            builder.HasAuditColumns();

            builder.HasData(
                new MeetingFormat { Id = MeetingFormat.OnlineId, Code = "online", Name = "Online", CreatedAt = AuditColumnsExtensions.SeedDate, UpdatedAt = AuditColumnsExtensions.SeedDate },
                new MeetingFormat { Id = MeetingFormat.InPersonId, Code = "in_person", Name = "In person", CreatedAt = AuditColumnsExtensions.SeedDate, UpdatedAt = AuditColumnsExtensions.SeedDate });
        }
    }
}
