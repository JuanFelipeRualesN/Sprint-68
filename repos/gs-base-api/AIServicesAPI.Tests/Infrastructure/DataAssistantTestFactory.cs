using GSAIServicesAPI.Models.DataAssistant;
using GSAIServicesAPI.Services.DataAssistant;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace AIServicesAPI.Tests.Infrastructure
{
    /// <summary>
    /// Builds the data assistant with fakes for the model and for the data analysis module.
    /// </summary>
    internal sealed class DataAssistantTestFactory
    {
        public DataAssistantTestFactory(DateTimeOffset? now = null, TimeSpan? conversationLifetime = null, IConfiguration? configuration = null)
        {
            Configuration = configuration ?? TestConfiguration.Build();
            if (configuration is null && GSAIServicesAPI.AppConfiguration.Configuration is null)
            {
                GSAIServicesAPI.AppConfiguration.Initialize(Configuration);   // the token estimator reads the static configuration
            }

            Clock = new ManualTimeProvider(now ?? new DateTimeOffset(2026, 11, 23, 15, 0, 0, TimeSpan.Zero));
            Conversations = new DataAssistantConversationStore(new MemoryCache(new MemoryCacheOptions()), conversationLifetime);
            Downloads = new DataAssistantDownloadRegistry(Clock);
            Catalog = new DataAssistantToolCatalog();
            Service = new DataAssistantService(Model, DataAnalysis, Catalog, Conversations, Downloads, Configuration, NullLogger<DataAssistantService>.Instance, Clock);
        }

        public IConfiguration Configuration { get; }

        public ManualTimeProvider Clock { get; }

        public FakeDataAssistantChatClient Model { get; } = new();

        public FakeDataAnalysisFeignAPI DataAnalysis { get; } = new();

        public DataAssistantConversationStore Conversations { get; }

        public DataAssistantDownloadRegistry Downloads { get; }

        public DataAssistantToolCatalog Catalog { get; }

        public DataAssistantService Service { get; }

        public static DataAssistantCaller Caller(string userId = "42", string token = "token-of-42", string language = "en") => new(userId, token, language);

        public Task<DataAssistantAskResult> AskAsync(string question, DataAssistantCaller? caller = null, string? conversationId = null, DashboardContextDto? context = null,
            string? language = null) =>
            Service.AskAsync(new DataAssistantAskRequest(question, conversationId, context, language), caller ?? Caller(), CancellationToken.None);
    }

    internal static class TestConfiguration
    {
        public static IConfiguration Build(IDictionary<string, string?>? extra = null)
        {
            var values = new Dictionary<string, string?>
            {
                ["AppSettings:token_encoding"] = "o200k_base",
                ["AppSettings:cache_on_off"] = "0",
                ["AppSettings:usage_insights_utc_offset_minutes"] = "-240",
            };
            foreach (var pair in extra ?? new Dictionary<string, string?>())
            {
                values[pair.Key] = pair.Value;
            }

            return new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.aiservices.json"), optional: false)
                .AddInMemoryCollection(values)
                .Build();
        }
    }
}
