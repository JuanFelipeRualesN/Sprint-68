using Azure.Core;
using Azure.Identity;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GSAIServicesAPI.Services.Plannings
{
    /// <summary>
    /// A chunk of the plannings index: the summary of a week, or a day of the week.
    /// </summary>
    public class PlanningChunk
    {
        [JsonPropertyName("planning_id")] public string? PlanningId { get; set; }
        [JsonPropertyName("type")] public string? Type { get; set; }
        [JsonPropertyName("week")] public string? Week { get; set; }
        [JsonPropertyName("unit")] public string? Unit { get; set; }
        [JsonPropertyName("day")] public string? Day { get; set; }
        [JsonPropertyName("day_order")] public int? DayOrder { get; set; }
        [JsonPropertyName("lesson_title")] public string? LessonTitle { get; set; }
        [JsonPropertyName("content")] public List<string>? Content { get; set; }
        [JsonPropertyName("standards")] public List<string>? Standards { get; set; }
        [JsonPropertyName("expectations")] public List<string>? Expectations { get; set; }
    }

    /// <summary>
    /// Reads the plannings index of Azure AI Search with filters only, without vector search.
    /// </summary>
    public class PlanningsSearchClient
    {
        private const string ApiVersion = "2023-11-01";
        private const string DefaultIndexName = "plannings-v2-ragindex";
        private const int PageSize = 1000;
        private static readonly string[] Scopes = ["https://search.azure.com/.default"];
        private static readonly Lazy<TokenCredential> Credential = new(CreateCredential);

        /// <summary>
        /// The identity to read the indexes with. In Azure it is always the managed identity
        /// </summary>
        private static TokenCredential CreateCredential()
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("IDENTITY_ENDPOINT")))
                return new DefaultAzureCredential();

            var clientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID");
            return new ManagedIdentityCredential(string.IsNullOrWhiteSpace(clientId)
                ? ManagedIdentityId.SystemAssigned
                : ManagedIdentityId.FromUserAssignedClientId(clientId));
        }

        private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

        private readonly HttpClient _httpClient;
        private readonly string? _endpoint;
        private readonly string _indexName;
        private readonly string? _apiKey;

        /// <summary>
        /// Initializes a new instance of <see cref="PlanningsSearchClient"/>.
        /// Without "plannings_search_api_key" the indexes are read with the Azure identity
        /// To test locally without Data Contributor role, set a key in appsettings
        /// </summary>
        /// <param name="httpClient">The client used to send requests.</param>
        /// <param name="configuration">Configuration with the search endpoint, index and optional api key.</param>
        public PlanningsSearchClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _endpoint = configuration["AppSettings:PlanningsSearchEndpoint"]?.TrimEnd('/');
            _indexName = configuration["AppSettings:PlanningsSearchIndex"] is { Length: > 0 } index ? index : DefaultIndexName;
            _apiKey = configuration["AppSettings:plannings_search_api_key"];
        }

        /// <summary>
        /// To get every chunk matching a filter, reading all the pages.
        /// </summary>
        /// <param name="filter">The OData filter.</param>
        /// <param name="select">The fields to return, comma separated.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the matching chunks.</returns>
        public async Task<List<PlanningChunk>> SearchAsync(string filter, string select, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_endpoint))
                throw new InvalidOperationException("Missing config: PlanningsSearchEndpoint");

            var chunks = new List<PlanningChunk>();
            for (var skip = 0; ; skip += PageSize)
            {
                var page = await SearchPageAsync(filter, select, skip, cancellationToken);
                chunks.AddRange(page);
                if (page.Count < PageSize)
                    return chunks;
            }
        }

        private Task<List<PlanningChunk>> SearchPageAsync(string filter, string select, int skip, CancellationToken cancellationToken)
            => PostSearchAsync<PlanningChunk>(_indexName, new { search = "*", filter, select, top = PageSize, skip }, cancellationToken);

        private async Task<List<T>> PostSearchAsync<T>(string indexName, object query, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_endpoint))
                throw new InvalidOperationException("Missing config: PlanningsSearchEndpoint");

            var body = JsonSerializer.Serialize(query);
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{_endpoint}/indexes/{Uri.EscapeDataString(indexName)}/docs/search?api-version={ApiVersion}")
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            await AuthorizeAsync(request, cancellationToken);

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Index {indexName} search failed with status {(int)response.StatusCode}.", null, response.StatusCode);

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var result = await JsonSerializer.DeserializeAsync<SearchResponse<T>>(stream, JsonOptions, cancellationToken);
            return result?.Value ?? [];
        }

        private async Task AuthorizeAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(_apiKey))
            {
                request.Headers.Add("api-key", _apiKey);
                return;
            }

            var token = await Credential.Value.GetTokenAsync(new TokenRequestContext(Scopes), cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        }

        /// <summary>
        /// To escape a value inside an OData string literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Returns the value with its single quotes doubled.</returns>
        public static string EscapeLiteral(string value) => value.Replace("'", "''");

        private class SearchResponse<T>
        {
            [JsonPropertyName("value")] public List<T>? Value { get; set; }
        }
    }
}
