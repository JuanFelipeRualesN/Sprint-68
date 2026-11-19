using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using GSAIServicesAPI.FeignAPIs.Models;

namespace GSAIServicesAPI.Services.PromptMonitoring
{
    /// <inheritdoc />
    /// <remarks>
    /// Reads the viewer's students from the users API profiles endpoint, forwarding the viewer's own
    /// token so the result is already scoped to them. One call, no paging loop: pagination is turned
    /// off so every student comes back. Only students are asked for, since only their prompts can ever
    /// become alerts.
    ///
    /// The standalone service sized this from a companion total-items endpoint first; BaseAPI has no
    /// such route, and its profiles query already materialises the whole result set before paging, so
    /// the extra round trip does nothing here.
    /// </remarks>
    public class UsersApiViewerStudentsProvider : IViewerStudentsProvider
    {
        private static readonly HttpClient HttpClient = new();

        private static readonly JsonSerializerOptions JsonOptions =
            new() { PropertyNameCaseInsensitive = true };

        private readonly string _usersApiBaseUrl;
        private readonly string _profilesPath;
        private readonly string _studentRoleId;

        /// <summary>
        /// Page size that asks the users API for no pagination at all, it reads zero as return everything
        /// </summary>
        private const int AllStudentsPageSize = 0;

        public UsersApiViewerStudentsProvider(IConfiguration config)
        {
            _usersApiBaseUrl = config["AppSettings:gus_endpoint_web_service_url"] ?? "";
            _profilesPath = config["AppSettings:gus_endpoint_profiles_api"] ?? "api/profiles";
            _studentRoleId = config["AppSettings:gus_profiles_student_role_id"] ?? "3";
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<string>> GetStudentIdsAsync(
            string viewerToken,
            CancellationToken cancellationToken = default)
        {
            var profiles = await GetStudentProfilesAsync(viewerToken, AllStudentsPageSize, cancellationToken);

            // No students means no alerts to scope to.
            if (profiles.UserInfo.Count == 0)
            {
                return [];
            }

            return profiles.UserInfo
                .Select(profile => profile.UserId)
                .Where(userId => !string.IsNullOrWhiteSpace(userId))
                .Select(userId => userId!)
                .ToList();
        }

        /// <summary>Reads the viewer's students in a single page sized to <paramref name="pageSize"/>.</summary>
        private async Task<StudentProfilesResponseModel> GetStudentProfilesAsync(
            string viewerToken, int pageSize, CancellationToken cancellationToken)
        {
            var query = BuildStudentsQuery(pageSize: pageSize, pageNumber: 0);
            var responseText = await PostAsync(_profilesPath, viewerToken, query, cancellationToken);

            return JsonSerializer.Deserialize<StudentProfilesResponseModel>(responseText, JsonOptions)
                ?? new StudentProfilesResponseModel();
        }

        /// <summary>
        /// Builds the users API filter body. Fixed to the student role, since only students produce
        /// alerts, and includes every account (active or not) so no alert is missed.
        /// </summary>
        private string BuildStudentsQuery(int pageSize, int pageNumber) =>
            JsonSerializer.Serialize(new
            {
                OnlyActive = false,
                Limiter = pageSize,
                PageFrom = pageNumber,
                Username = (string?)null,
                FirstName = (string?)null,
                LastName = (string?)null,
                RoleId = _studentRoleId
            });

        /// <summary>POSTs <paramref name="jsonBody"/> to a users API path with the viewer's token, and returns the raw body.</summary>
        private async Task<string> PostAsync(
            string path, string viewerToken, string jsonBody, CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Post, new Uri(new Uri(_usersApiBaseUrl), path));
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("Token", viewerToken);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await HttpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
    }
}
