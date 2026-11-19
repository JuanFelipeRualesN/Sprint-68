using GSBaseAPI.GPTModule.Models;
using System.Text;
using System.Text.Json;

namespace GSBaseAPI.GPTModule.Services.ExternalAPIs
{
    public interface ITeachersHomeworkApi
    {
        /// <summary>
        /// Creates a quiz and returns the generated QuizId.
        /// </summary>
        Task<string> CreateQuizAsync(string createQuizJson);

        /// <summary>
        /// Publishes a quiz (ReadyToPublish = 1).
        /// </summary>
        Task PublishQuizAsync(string publishQuizJson);

        /// <summary>
        /// Attaches quiz to planning.
        /// </summary>
        Task AttachQuizToPlanningAsync(AttachQuizPayload payload);
    }

    public sealed class TeachersHomeworkApi : ITeachersHomeworkApi
    {
        // Provided URLs
        private const string AttachQuizUrl = "https://baseapi-uat.genialskillsweb.com/teachersapi/planning/attach-quiz/";
        private const string CreateQuizUrl = "https://baseapi-uat.genialskillsweb.com/teachersapi/homework/create/";
        private const string ModifyQuizUrl = "https://baseapi-uat.genialskillsweb.com/teachersapi/homework/modify/";

        // Provided header token (should be stored in secret store)
        private const string TokenHeaderValue =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiMzE4NzQiLCJleHAiOjE3NzUzOTQ0OTgsImlzcyI6Imh0dHBzOi8vYmFzZWFwaS11YXQuZ2VuaWFsc2tpbGxzd2ViLmNvbSIsImF1ZCI6Imh0dHBzOi8vYmFzZWFwaS11YXQuZ2VuaWFsc2tpbGxzd2ViLmNvbSJ9.NomdhsbeyFuqtEaGkPBEtLICMxAOLQo2JF-gqFci948";

        private readonly HttpClient _http;

        public TeachersHomeworkApi(HttpClient http) => _http = http;

        public async Task<string> CreateQuizAsync(string createQuizJson)
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, CreateQuizUrl);
            req.Headers.Add("Token", TokenHeaderValue);
            req.Content = new StringContent(createQuizJson, Encoding.UTF8, "application/json");

            using var resp = await _http.SendAsync(req);
            var raw = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                throw new InvalidOperationException($"CreateQuiz failed {(int)resp.StatusCode}: {raw}");

            // Extract: QuizRequestModel.QuizModel.Id
            using var doc = JsonDocument.Parse(raw);
            var quizId = doc.RootElement
                .GetProperty("QuizRequestModel")
                .GetProperty("QuizModel")
                .GetProperty("Id")
                .GetString();

            if (string.IsNullOrWhiteSpace(quizId))
                throw new InvalidOperationException("Quiz created but QuizId missing in response.");

            return quizId;
        }

        public async Task PublishQuizAsync(string publishQuizJson)
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, ModifyQuizUrl);
            req.Headers.Add("Token", TokenHeaderValue);
            req.Content = new StringContent(publishQuizJson, Encoding.UTF8, "application/json");

            using var resp = await _http.SendAsync(req);
            var raw = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                throw new InvalidOperationException($"Publish (modify) failed {(int)resp.StatusCode}: {raw}");

            // Optionally: validate TransactionValid/Message if your API returns it.
        }

        public async Task AttachQuizToPlanningAsync(AttachQuizPayload payload)
        {
            var json = JsonSerializer.Serialize(payload);

            using var req = new HttpRequestMessage(HttpMethod.Post, AttachQuizUrl);
            req.Headers.Add("Token", TokenHeaderValue);
            req.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using var resp = await _http.SendAsync(req);
            var raw = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                throw new InvalidOperationException($"AttachQuiz failed {(int)resp.StatusCode}: {raw}");
        }
    }
}
