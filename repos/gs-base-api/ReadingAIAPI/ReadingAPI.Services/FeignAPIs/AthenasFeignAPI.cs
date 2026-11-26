using ReadingAPI.Domain.DTOs;
using Newtonsoft.Json;
using ReadingAPI;
using System.Net.Http.Headers;
using System.Web;

namespace ReadingAPI.ReadingAPI.Services.FeignAPIs
{
    public class AthenasFeignAPI
    {
        public static List<LessonUnitDto> GetReadingLessons()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:athenas_web_service_api_access_key"]);

                string url = AppConfiguration.Configuration["AppSettings:athenas_web_api_get_reading_lessons"];

                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    var lessons = JsonConvert.DeserializeObject<List<LessonUnitDto>>(jsonResponse,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return lessons ?? new List<LessonUnitDto>();
                }

                return new List<LessonUnitDto>();
            }
        }

        public static PaginationModel<ReadingDiagnosticQuestionDto> GetReadingDiagnosticQuestions(
            string subjectCode = null,
            string levelCode = null,
            string constructedCodeLike = null,
            List<int> questionIds = null,
            int fromPage = 1,
            int limit = 10)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:athenas_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:athenas_web_service_api_access_key"]);

                string url = AppConfiguration.Configuration["AppSettings:athenas_api_get_reading_diagnostic_questions"];

                var query = HttpUtility.ParseQueryString(string.Empty);
                if (!string.IsNullOrWhiteSpace(subjectCode))       query["subjectCode"] = subjectCode;
                if (!string.IsNullOrWhiteSpace(levelCode))          query["levelCode"] = levelCode;
                if (!string.IsNullOrWhiteSpace(constructedCodeLike)) query["constructedCodeLike"] = constructedCodeLike;
                if (questionIds?.Count > 0)                          query["questionIds"] = string.Join(",", questionIds);
                query["fromPage"] = fromPage.ToString();
                query["limit"] = limit.ToString();

                string queryString = query.ToString();
                if (!string.IsNullOrEmpty(queryString))
                    url = $"{url}?{queryString}";

                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<PaginationModel<ReadingDiagnosticQuestionDto>>(
                        responseString, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    return result ?? new PaginationModel<ReadingDiagnosticQuestionDto>(0, limit, new List<ReadingDiagnosticQuestionDto>());
                }

                return new PaginationModel<ReadingDiagnosticQuestionDto>(0, limit, new List<ReadingDiagnosticQuestionDto>());
            }
        }
    }
}
