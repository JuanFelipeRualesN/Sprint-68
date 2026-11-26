using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.UsageInsights;
using DataAnalysisAPI.DataAnalysisAPI.Services;
using DataAnalysisAPI.DataAnalysisAPI.Services.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace DataAnalysisAPI.Tests.Infrastructure
{
    /// <summary>
    /// Shortcuts to build query contexts and run the usage insights queries the way the controller does.
    /// </summary>
    internal static class UsageInsightsTestHelper
    {
        /// <summary>
        /// Builds a context with the test configuration: days in Puerto Rico time and the test account exclusion of the synthetic data.
        /// </summary>
        public static UsageInsightsQueryContext Context(DataScope? scope = null, bool excludeTestAccounts = true) =>
            new(scope ?? DataScope.All,
                new TestAccountExclusion(excludeTestAccounts, new[] { "DEMOSTRACION" }, new[] { "internal.example" }, new[] { "Facilitators School" }),
                -240, 30, "status", "cancellation_date");

        /// <summary>
        /// Turns "a=1&amp;b=2" into a query collection.
        /// </summary>
        public static IQueryCollection Query(string queryString)
        {
            var values = new Dictionary<string, StringValues>();
            foreach (var pair in queryString.Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = pair.Split('=', 2);
                values[parts[0]] = Uri.UnescapeDataString(parts.Length > 1 ? parts[1] : string.Empty);
            }

            return new QueryCollection(values);
        }

        /// <summary>
        /// Runs a query.
        /// </summary>
        public static UsageInsightResponse Run(string query, string queryString, UsageInsightsQueryContext? context = null, string language = "en",
            bool isExport = false, bool ignoreLimit = false) =>
            UsageInsights.Execute(query, new UsageInsightsQueryParser(Query(queryString), isExport, ignoreLimit), context ?? Context(), language);

        public static List<T> Rows<T>(UsageInsightResponse response) => (List<T>)response.Results;

        public static Dictionary<string, object?> Summary(UsageInsightResponse response) => (Dictionary<string, object?>)response.Summary!;
    }
}
