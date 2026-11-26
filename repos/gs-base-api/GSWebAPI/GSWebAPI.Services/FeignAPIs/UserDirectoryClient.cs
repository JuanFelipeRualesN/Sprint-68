using System.Net.Http.Headers;
using System.Text;
using GSWebAPI.GSWebAPI.Domain.Models.General.Requests.APIs;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using GSWebAPI.GSWebAPI.Services.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GSWebAPI.GSWebAPI.Services.FeignAPIs
{
    /// <summary>Reads user details owned by the Users API.</summary>
    public interface IUserDirectoryClient
    {
        /// <summary>
        /// Full names by user id, via <c>POST usersapi/users/fullnames</c>. Users the API does not return are left out;
        /// names are display data only, so a failure returns an empty dictionary instead of failing the request.
        /// </summary>
        Task<IReadOnlyDictionary<ulong, string>> GetFullNamesAsync(RequestUser user, IEnumerable<ulong> userIds, CancellationToken cancellationToken = default);

        /// <summary>
        /// True if <paramref name="studentId"/> is a student profile that the client in session can manage, via
        /// <c>POST usersapi/profiles</c> (the same call TeachersAPI uses in GetStudentsThatCanBeHandled).
        /// Null if the Users API could not be reached.
        /// </summary>
        Task<bool?> IsStudentOfClientAsync(RequestUser client, ulong studentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// The student profiles the client in session can manage (their children), via <c>POST usersapi/profiles</c> with
        /// RoleId = student, read page by page (<c>PageTotals</c>). Null if the Users API could not be reached.
        /// </summary>
        Task<IReadOnlyList<StudentProfile>?> GetStudentsOfClientAsync(RequestUser client, CancellationToken cancellationToken = default);
    }

    /// <summary>A student profile of the client.</summary>
    /// <param name="UserId">ul_user_accounts.id.</param>
    /// <param name="FullName">Given name and surname.</param>
    public sealed record StudentProfile(ulong UserId, string FullName);

    /// <inheritdoc />
    public sealed class UserDirectoryClient : IUserDirectoryClient
    {
        private static readonly JsonSerializerSettings JsonSettings = new() { NullValueHandling = NullValueHandling.Ignore };

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly GSWebModuleSettings _settings;
        private readonly ILogger<UserDirectoryClient> _logger;

        public UserDirectoryClient(HttpClient httpClient, IConfiguration configuration, GSWebModuleSettings settings, ILogger<UserDirectoryClient> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _settings = settings;
            _logger = logger;
        }

        public async Task<IReadOnlyDictionary<ulong, string>> GetFullNamesAsync(RequestUser user, IEnumerable<ulong> userIds, CancellationToken cancellationToken = default)
        {
            var ids = userIds.Distinct().Select(id => id.ToString()).ToList();
            if (ids.Count == 0)
            {
                return new Dictionary<ulong, string>();
            }

            try
            {
                // The endpoint binds [FromBody] List<string>: send the raw list (as MessagesAPI does).
                using var request = CreateRequest(user, _configuration["AppSettings:gus_enpoint_user_full_names_general"] ?? "usersapi/users/fullnames", ids);
                using var response = await _httpClient.SendAsync(request, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Users API full names returned {StatusCode}.", (int)response.StatusCode);
                    return new Dictionary<ulong, string>();
                }

                var body = await response.Content.ReadAsStringAsync(cancellationToken);
                var names = JsonConvert.DeserializeObject<List<UserFullName>>(body, JsonSettings) ?? new List<UserFullName>();
                var result = new Dictionary<ulong, string>();
                foreach (var name in names)
                {
                    if (ulong.TryParse(name.UserId, out var id))
                    {
                        result[id] = string.Join(" ", new[] { name.GivenName, name.Surname, name.Suffix }.Where(p => !string.IsNullOrWhiteSpace(p)));
                    }
                }

                return result;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
            {
                _logger.LogWarning(ex, "Could not read full names from the Users API.");
                return new Dictionary<ulong, string>();
            }
        }

        public async Task<bool?> IsStudentOfClientAsync(RequestUser client, ulong studentId, CancellationToken cancellationToken = default)
        {
            var filter = new UserRequestModel
            {
                UserIds = new List<string> { studentId.ToString() },
                RoleId = _settings.StudentRoleId,
                Limiter = 0,
            };

            try
            {
                using var request = CreateRequest(client, _configuration["AppSettings:gus_endpoint_call_users_handle"] ?? "usersapi/profiles", filter);
                using var response = await _httpClient.SendAsync(request, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Users API profiles returned {StatusCode}.", (int)response.StatusCode);
                    return null;
                }

                var body = await response.Content.ReadAsStringAsync(cancellationToken);
                var profiles = JsonConvert.DeserializeObject<UserResponseModel>(body, JsonSettings);
                return profiles?.UserInfo?.Any(u => u.UserId == studentId.ToString()) == true;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
            {
                _logger.LogWarning(ex, "Could not check the student profiles in the Users API.");
                return null;
            }
        }

        public async Task<IReadOnlyList<StudentProfile>?> GetStudentsOfClientAsync(RequestUser client, CancellationToken cancellationToken = default)
        {
            const int pageSize = 50;
            const int maxPages = 10;
            var students = new Dictionary<ulong, StudentProfile>();
            try
            {
                for (var page = 0; page < maxPages; page++)
                {
                    var filter = new UserRequestModel { RoleId = _settings.StudentRoleId, PageFrom = page, Limiter = pageSize };
                    using var request = CreateRequest(client, _configuration["AppSettings:gus_endpoint_call_users_handle"] ?? "usersapi/profiles", filter);
                    using var response = await _httpClient.SendAsync(request, cancellationToken);
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("Users API profiles returned {StatusCode}.", (int)response.StatusCode);
                        return null;
                    }

                    var profiles = JsonConvert.DeserializeObject<UserResponseModel>(await response.Content.ReadAsStringAsync(cancellationToken), JsonSettings);
                    var users = profiles?.UserInfo ?? new List<UserResponse>();
                    foreach (var user in users)
                    {
                        if (ulong.TryParse(user.UserId, out var id) && id != client.UserId)
                        {
                            var name = string.Join(" ", new[] { user.GivenName, user.Surname }.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p.Trim()));
                            students[id] = new StudentProfile(id, name.Length > 0 ? name : user.Username ?? id.ToString());
                        }
                    }

                    if (users.Count < pageSize || page + 1 >= (profiles?.PageTotals ?? 0))
                    {
                        break;
                    }
                }

                return students.Values.OrderBy(s => s.FullName, StringComparer.CurrentCultureIgnoreCase).ToList();
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }

                _logger.LogWarning(ex, "Could not read the student profiles from the Users API.");
                return null;
            }
        }

        private HttpRequestMessage CreateRequest(RequestUser user, string endpoint, object body)
        {
            var baseUrl = _configuration["AppSettings:gus_web_service_url"]
                ?? throw new InvalidOperationException("AppSettings:gus_web_service_url is not configured.");
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(baseUrl), endpoint))
            {
                Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"),
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("Token", user.Token ?? string.Empty);
            if (!string.IsNullOrWhiteSpace(user.LangCode))
            {
                request.Headers.Add("Accept-Language", user.LangCode);
            }

            return request;
        }

        private sealed class UserFullName
        {
            public string? UserId { get; set; }

            public string? GivenName { get; set; }

            public string? Surname { get; set; }

            public string? Suffix { get; set; }
        }
    }
}
