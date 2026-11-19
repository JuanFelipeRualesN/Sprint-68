using System.Text;
using System.Text.Json;

namespace GSBaseAPI.GPTModule.Services.ExternalAPIs
{

    public static class OpenAiResponseParsing
    {
        public static string ExtractText(string rawJson)
        {
            using var doc = JsonDocument.Parse(rawJson);

            if (!doc.RootElement.TryGetProperty("output", out var output) || output.ValueKind != JsonValueKind.Array)
                return "";

            var sb = new StringBuilder();

            foreach (var item in output.EnumerateArray())
            {
                if (!item.TryGetProperty("content", out var content) || content.ValueKind != JsonValueKind.Array)
                    continue;

                foreach (var c in content.EnumerateArray())
                {
                    if (c.TryGetProperty("type", out var type) &&
                        type.GetString() == "output_text" &&
                        c.TryGetProperty("text", out var text))
                    {
                        sb.Append(text.GetString());
                    }
                }
            }

            return sb.ToString();
        }
    }
}
