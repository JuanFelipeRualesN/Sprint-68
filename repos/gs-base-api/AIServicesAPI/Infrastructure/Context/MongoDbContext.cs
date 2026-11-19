using GSAIServicesAPI.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GSAIServicesAPI.Infrastructure.Context
{
    /// <summary>
    /// Exposes a singleton MongoClient and IMongoDatabase, reusable for any Mongo operation.
    /// </summary>
    /// <remarks>
    /// The client is created lazily on first use, not in the constructor. This context can be an
    /// indirect dependency of endpoints whose main job is not the database: building them must never
    /// fail just because Mongo is unconfigured or unreachable, or the whole endpoint would 500 before
    /// its own error handling runs. A missing setting therefore surfaces when the database is actually
    /// touched, where the caller can guard the operation.
    /// </remarks>
    public class MongoDbContext
    {
        private readonly MongoDbSettings _settings;
        private readonly Lazy<IMongoClient> _client;

        public IMongoClient Client => _client.Value;
        public IMongoDatabase Database => Client.GetDatabase(_settings.DatabaseName);

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            _settings = settings.Value;
            _client = new Lazy<IMongoClient>(CreateClient);
        }

        /// <summary>
        /// Builds the MongoClient the first time the database is used. The configuration checks live
        /// here, not in the constructor, so an unconfigured Mongo cannot break controller construction.
        /// </summary>
        private IMongoClient CreateClient()
        {
            if (string.IsNullOrWhiteSpace(_settings.ConnectionString))
            {
                throw new InvalidOperationException("MongoDb:ConnectionString is not configured.");
            }

            if (string.IsNullOrWhiteSpace(_settings.DatabaseName))
            {
                throw new InvalidOperationException("MongoDb:DatabaseName is not configured.");
            }

            return new MongoClient(_settings.ConnectionString);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            return GetCollection<T>(_settings.DatabaseName, _settings.GetCollectionName<T>());
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return GetCollection<T>(_settings.DatabaseName, collectionName);
        }

        public IMongoCollection<T> GetCollection<T>(string databaseName, string collectionName)
        {
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new ArgumentException("Database name is required.", nameof(databaseName));
            }

            if (string.IsNullOrWhiteSpace(collectionName))
            {
                throw new ArgumentException("Collection name is required.", nameof(collectionName));
            }

            return Client.GetDatabase(databaseName).GetCollection<T>(collectionName);
        }
    }
}
