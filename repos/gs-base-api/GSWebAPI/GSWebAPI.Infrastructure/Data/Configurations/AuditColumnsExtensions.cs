using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Configurations
{
    internal static class AuditColumnsExtensions
    {
        /// <summary>Fixed timestamp for HasData rows, so the model snapshot does not change on every build.</summary>
        public static readonly DateTime SeedDate = new(2026, 11, 5, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>created_at / updated_at as TIMESTAMP. Values are set by the SaveChanges interceptor.</summary>
        public static void HasAuditColumns<T>(this EntityTypeBuilder<T> builder)
            where T : class, IAuditableEntity
        {
            builder.Property(e => e.CreatedAt).HasColumnType("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(e => e.UpdatedAt).HasColumnType("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP");
        }

        /// <summary>created_at / updated_at / deleted_at as TIMESTAMP.</summary>
        public static void HasSoftDeleteColumns<T>(this EntityTypeBuilder<T> builder)
            where T : class, ISoftDeletableEntity
        {
            builder.HasAuditColumns();
            builder.Property(e => e.DeletedAt).HasColumnType("timestamp").IsRequired(false);
        }
    }
}
