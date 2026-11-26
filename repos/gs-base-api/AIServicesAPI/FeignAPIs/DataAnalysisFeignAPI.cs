namespace GSAIServicesAPI.FeignAPIs
{
    /// <summary>
    /// Feign client of the data analysis module. The monolith hosts both modules, but they never reference each other: they talk over HTTP
    /// (base URL in AppSettings:data_analysis_api_url), like every other Feign API of this module.
    /// </summary>
    public class DataAnalysisFeignAPI : IDataAnalysisFeignAPI
    {
        private const string RoutePrefix = "dataanalysisapi/usage-insights/";
        private const int DefaultTimeoutSeconds = 30;
        private readonly HttpClient _httpClient;
        private readonly bool _isConfigured;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAnalysisFeignAPI"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client; its base address and timeout are set here from the configuration.</param>
        /// <param name="configuration">The application configuration.</param>
        public DataAnalysisFeignAPI(HttpClient httpClient, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            // A missing URL must not stop the whole monolith from starting: the call fails with a clear message when it is used.
            var baseUrl = configuration["AppSettings:data_analysis_api_url"];
            if (!string.IsNullOrWhiteSpace(baseUrl) && Uri.TryCreate(baseUrl.TrimEnd('/') + "/", UriKind.Absolute, out var baseAddress))
            {
                _httpClient.BaseAddress = baseAddress;
                _isConfigured = true;
            }

            _httpClient.Timeout = TimeSpan.FromSeconds(
                int.TryParse(configuration["AppSettings:data_analysis_api_timeout_seconds"], out var seconds) && seconds > 0 ? seconds : DefaultTimeoutSeconds);
        }

        /// <inheritdoc />
        public Task<DataAnalysisApiResponse> GetUsageInsightAsync(string queryName, string queryString, string token, string language,
            CancellationToken cancellationToken = default) =>
            SendAsync(BuildPath(queryName, null, queryString), token, language, cancellationToken);

        /// <inheritdoc />
        public Task<DataAnalysisApiResponse> ExportUsageInsightAsync(string queryName, string format, string queryString, string token, string language,
            CancellationToken cancellationToken = default) =>
            SendAsync(BuildPath(queryName, $"format={Uri.EscapeDataString(format)}", queryString, isExport: true), token, language, cancellationToken);

        private static string BuildPath(string queryName, string? firstParameter, string queryString, bool isExport = false)
        {
            var path = RoutePrefix + Uri.EscapeDataString(queryName) + (isExport ? "/export" : string.Empty);
            var parameters = string.Join("&", new[] { firstParameter, queryString }.Where(part => !string.IsNullOrEmpty(part)));
            return parameters.Length == 0 ? path : path + "?" + parameters;
        }

        /// <summary>
        /// Sends the GET request with the caller token and language. The token travels only in the Token header and is never logged.
        /// </summary>
        private async Task<DataAnalysisApiResponse> SendAsync(string relativePath, string token, string language, CancellationToken cancellationToken)
        {
            if (!_isConfigured)
            {
                throw new InvalidOperationException("Missing or invalid config: AppSettings:data_analysis_api_url");
            }

            using var request = new HttpRequestMessage(HttpMethod.Get, relativePath);
            request.Headers.Add("Token", token);
            request.Headers.AcceptLanguage.ParseAdd(string.IsNullOrWhiteSpace(language) ? "en" : language);

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            var content = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            var disposition = response.Content.Headers.ContentDisposition;
            return new DataAnalysisApiResponse(
                (int)response.StatusCode,
                content,
                response.Content.Headers.ContentType?.ToString(),
                disposition?.FileNameStar ?? disposition?.FileName?.Trim('"'));
        }
    }
}
