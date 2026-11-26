using MongoDB.Driver;

namespace GSAIServicesAPI.Infrastructure.Interfaces
{
    public interface IMongoRepository<T> where T : class
    {
        Task InsertAsync(T entity, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        Task UpdateAsync(string id, T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Applies <paramref name="update"/> to the single document matching <paramref name="filter"/>,
        /// or inserts a new one built from the filter's equalities plus the update when none matches.
        /// Use for at-most-one records identified by a natural key (not <c>_id</c>), such as a per-viewer
        /// watermark. Update-based (not replace) so MongoDB generates the <c>_id</c> on insert and leaves
        /// it untouched on update, and so only the named fields change.
        /// </summary>
        Task UpsertAsync(
            FilterDefinition<T> filter,
            UpdateDefinition<T> update,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(string id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<T>> GetByFilterAsync(
            FilterDefinition<T> filter,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<T>> GetByFilterAsync(
            FilterDefinition<T> filter,
            int limit,
            int skip = 0,
            SortDefinition<T>? sort = null,
            CancellationToken cancellationToken = default);

        Task<long> CountAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Runs an aggregation pipeline (filter, group, count) inside MongoDB and returns the summary
        /// rows it produces. The work happens on the database server, so only the rows travel back —
        /// never the matched documents.
        /// </summary>
        Task<IReadOnlyList<TResult>> AggregateAsync<TResult>(
            PipelineDefinition<T, TResult> pipeline,
            CancellationToken cancellationToken = default);
    }
}
