using GSBaseAPI.GPTModule.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GSBaseAPI.GPTModule.Services.ExternalAPIs
{
    public interface IOpenAiClient
    {
        /// <summary>
        /// Sends a request to OpenAI Responses API and returns raw JSON.
        /// </summary>
        Task<string> CreateResponseRawAsync(OpenAiRequestPayload payload);
    }

    public sealed class OpenAiClient : IOpenAiClient
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public OpenAiClient(HttpClient http)
        {
            _http = http;
            _apiKey = AppConfiguration.Configuration["AppSettings:OpenAiApiKey"];
        }

        public async Task<string> CreateResponseRawAsync(OpenAiRequestPayload payload)
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/responses");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using var resp = await _http.SendAsync(req);
            var raw = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                throw new InvalidOperationException($"OpenAI error {(int)resp.StatusCode}: {raw}");

            return raw;
        }
    }
}
