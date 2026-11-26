using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Interceptors
{
    /// <summary>
    /// Sets created_at/updated_at automatically and turns Remove() of an <see cref="ISoftDeletableEntity"/> into
    /// setting deleted_at.
    /// <para>
    /// The context uses <see cref="Microsoft.EntityFrameworkCore.ChangeTracking.CascadeTiming.OnSaveChanges"/>, so
    /// this runs before EF cascades a delete to tracked children. A soft-deleted parent therefore never hard-deletes
    /// its children: children with deleted_at are soft-deleted explicitly by the services, and children without it
    /// are hidden through the parent's query filter.
    /// </para>
    /// </summary>
    public sealed class AuditAndSoftDeleteInterceptor : SaveChangesInterceptor
    {
        private readonly TimeProvider _timeProvider;

        /// <summary>Creates the interceptor.</summary>
        /// <param name="timeProvider">Clock for the timestamps (UTC, truncated to whole seconds).</param>
        public AuditAndSoftDeleteInterceptor(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        /// <inheritdoc/>
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            Apply(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        /// <inheritdoc/>
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            Apply(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void Apply(DbContext? context)
        {
            if (context == null)
            {
                return;
            }

            // Truncate to whole seconds: TIMESTAMP/DATETIME columns have no fractional part.
            var now = _timeProvider.GetUtcNow().UtcDateTime;
            now = new DateTime(now.Ticks - (now.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc);

            context.ChangeTracker.DetectChanges();
            foreach (var entry in context.ChangeTracker.Entries().ToList())
            {
                if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeletableEntity)
                {
                    entry.State = EntityState.Unchanged;
                    entry.Property(nameof(ISoftDeletableEntity.DeletedAt)).CurrentValue = now;
                    entry.Property(nameof(ISoftDeletableEntity.DeletedAt)).IsModified = true;
                    entry.Property(nameof(IAuditableEntity.UpdatedAt)).CurrentValue = now;
                    entry.Property(nameof(IAuditableEntity.UpdatedAt)).IsModified = true;
                    continue;
                }

                if (entry.Entity is not IAuditableEntity)
                {
                    continue;
                }

                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(IAuditableEntity.CreatedAt)).CurrentValue = now;
                    entry.Property(nameof(IAuditableEntity.UpdatedAt)).CurrentValue = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(IAuditableEntity.UpdatedAt)).CurrentValue = now;
                    entry.Property(nameof(IAuditableEntity.UpdatedAt)).IsModified = true;
                    entry.Property(nameof(IAuditableEntity.CreatedAt)).IsModified = false;
                }
            }
        }
    }
}
