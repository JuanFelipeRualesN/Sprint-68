namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common
{
    /// <summary>
    /// Entity with a deleted_at column. <c>Remove()</c> is turned into setting <see cref="DeletedAt"/>, and a global
    /// query filter hides rows where it is not null.
    /// </summary>
    public interface ISoftDeletableEntity : IAuditableEntity
    {
        /// <summary>When the row was soft-deleted (UTC, whole seconds); null while the row is live.</summary>
        DateTime? DeletedAt { get; set; }
    }
}
