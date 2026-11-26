namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common
{
    /// <summary>
    /// Entity with created_at / updated_at columns. Both are set by <c>AuditAndSoftDeleteInterceptor</c>; never set them by hand.
    /// </summary>
    public interface IAuditableEntity
    {
        /// <summary>When the row was inserted (UTC, whole seconds). Never changed by later updates.</summary>
        DateTime CreatedAt { get; set; }

        /// <summary>When the row was last modified or soft-deleted (UTC, whole seconds).</summary>
        DateTime UpdatedAt { get; set; }
    }
}
