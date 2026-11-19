using GSAIServicesAPI.Infrastructure.Context;
using GSAIServicesAPI.Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GSAIServicesAPI.Infrastructure.Repositories
{
    public class MongoRepository<T> : IMongoRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(MongoDbContext context)
        {
            _collection = context.GetCollection<T>();
        }

        public MongoRepository(MongoDbContext context, string collectionName)
        {
            _collection = context.GetCollection<T>(collectionName);
        }

        public MongoRepository(MongoDbContext context, string databaseName, string collectionName)
        {
            _collection = context.GetCollection<T>(databaseName, collectionName);
        }

        public async Task InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _collection.Find(_ => true).ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return null;
            }

            var filter = Builders<T>.Filter.Eq("_id", objectId);
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateAsync(string id, T entity, CancellationToken cancellationToken = default)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return;
            }

            var filter = Builders<T>.Filter.Eq("_id", objectId);
            await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
        }

        public async Task UpsertAsync(
            FilterDefinition<T> filter,
            UpdateDefinition<T> update,
            CancellationToken cancellationToken = default)
        {
            await _collection.UpdateOneAsync(
                filter,
                update,
                new UpdateOptions { IsUpsert = true },
                cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return;
            }

            var filter = Builders<T>.Filter.Eq("_id", objectId);
            await _collection.DeleteOneAsync(filter, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetByFilterAsync(
            FilterDefinition<T> filter,
            CancellationToken cancellationToken = default)
        {
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetByFilterAsync(
            FilterDefinition<T> filter,
            int limit,
            int skip = 0,
            SortDefinition<T>? sort = null,
            CancellationToken cancellationToken = default)
        {
            var query = _collection.Find(filter).Skip(skip).Limit(limit);

            if (sort is not null)
            {
                query = query.Sort(sort);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<long> CountAsync(
            FilterDefinition<T> filter,
            CancellationToken cancellationToken = default)
        {
            return await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyList<TResult>> AggregateAsync<TResult>(
            PipelineDefinition<T, TResult> pipeline,
            CancellationToken cancellationToken = default)
        {
            var resultCursor = await _collection.AggregateAsync(pipeline, cancellationToken: cancellationToken);
            return await resultCursor.ToListAsync(cancellationToken);
        }
    }
}
