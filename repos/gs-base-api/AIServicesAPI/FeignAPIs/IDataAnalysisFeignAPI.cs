namespace GSAIServicesAPI.FeignAPIs
{
    /// <summary>
    /// Answer of the data analysis module: the status code, the raw body and the file headers (for the exports).
    /// </summary>
    /// <param name="StatusCode">HTTP status code of the answer.</param>
    /// <param name="Content">Raw body.</param>
    /// <param name="ContentType">Content type of the body, when the module sent one.</param>
    /// <param name="FileName">File name of an export (from Content-Disposition), when there is one.</param>
    public sealed record DataAnalysisApiResponse(int StatusCode, byte[] Content, string? ContentType, string? FileName)
    {
        /// <summary>
        /// Gets a value indicating whether the status code is a success (2xx).
        /// </summary>
        public bool IsSuccess => StatusCode is >= 200 and < 300;

        /// <summary>
        /// Gets the body as UTF-8 text.
        /// </summary>
        /// <returns>The body text.</returns>
        public string GetText() => System.Text.Encoding.UTF8.GetString(Content);
    }

    /// <summary>
    /// Calls the usage insights routes of the data analysis module. The session token and the language of the caller are forwarded,
    /// so the module validates the session again and applies the data scope of that caller: whoever calls this API can never see more
    /// than their own session allows.
    /// </summary>
    public interface IDataAnalysisFeignAPI
    {
        /// <summary>
        /// Runs one usage insights query.
        /// </summary>
        /// <param name="queryName">The query name (for example connections-by-school).</param>
        /// <param name="queryString">The query parameters, already URL encoded, without the leading question mark.</param>
        /// <param name="token">The session token of the caller (forwarded, never logged).</param>
        /// <param name="language">The language code of the caller.</param>
        /// <param name="cancellationToken">Token used to cancel the outbound call.</param>
        /// <returns>The module answer.</returns>
        Task<DataAnalysisApiResponse> GetUsageInsightAsync(string queryName, string queryString, string token, string language, CancellationToken cancellationToken = default);

        /// <summary>
        /// Exports one usage insights query as an Excel workbook (xlsx) or as a chart specification (chart).
        /// </summary>
        /// <param name="queryName">The query name.</param>
        /// <param name="format">xlsx or chart.</param>
        /// <param name="queryString">The query parameters, already URL encoded, without the leading question mark.</param>
        /// <param name="token">The session token of the caller (forwarded, never logged).</param>
        /// <param name="language">The language code of the caller.</param>
        /// <param name="cancellationToken">Token used to cancel the outbound call.</param>
        /// <returns>The module answer.</returns>
        Task<DataAnalysisApiResponse> ExportUsageInsightAsync(string queryName, string format, string queryString, string token, string language,
            CancellationToken cancellationToken = default);
    }
}
