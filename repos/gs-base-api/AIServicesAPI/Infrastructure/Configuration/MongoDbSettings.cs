namespace GSAIServicesAPI.Infrastructure.Configuration
{
    public class MongoDbSettings
    {
        public const string SectionName = "MongoDb";

        public string ConnectionString { get; set; } = string.Empty;

        public string DatabaseName { get; set; } = string.Empty;

        public Dictionary<string, string> Collections { get; set; } = new();

        public string GetCollectionName<T>()
        {
            var modelName = typeof(T).Name;
            return Collections.TryGetValue(modelName, out var collectionName) && !string.IsNullOrWhiteSpace(collectionName)
                ? collectionName
                : modelName;
        }
    }
}
