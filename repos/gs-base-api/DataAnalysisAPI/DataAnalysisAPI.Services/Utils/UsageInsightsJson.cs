using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace DataAnalysisAPI.DataAnalysisAPI.Services.Utils
{
    /// <summary>
    /// JSON options of the usage insights: PascalCase names (as the rest of the platform) and no nulls, so the response, the Excel workbook and
    /// the chart are all computed over the same JSON the caller sees.
    /// </summary>
    public static class UsageInsightsJson
    {
        /// <summary>
        /// Gets the serializer options.
        /// </summary>
        public static readonly JsonSerializerOptions Options = new()
        {
            PropertyNamingPolicy = null,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        /// <summary>
        /// Serializes a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Returns the JSON text.</returns>
        public static string Serialize(object value) => JsonSerializer.Serialize(value, value.GetType(), Options);

        /// <summary>
        /// Serializes a value to a JSON node.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Returns the node.</returns>
        public static JsonNode ToNode(object value) => JsonSerializer.SerializeToNode(value, value.GetType(), Options)!;
    }
}
