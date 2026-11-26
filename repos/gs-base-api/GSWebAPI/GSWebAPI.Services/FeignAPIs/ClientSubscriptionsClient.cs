using System.Net.Http.Headers;
using GSWebAPI.GSWebAPI.Services.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GSWebAPI.GSWebAPI.Services.FeignAPIs
{
    /// <summary>The subscriptions of the client in session, with their related users and courses (Subscriptions API).</summary>
    public interface IClientSubscriptionsClient
    {
        /// <summary>
        /// <c>GET subscriptionsapi/subscriptions/clients?includeProfiles=true&amp;includeCourses=true</c> with the user's token.
        /// </summary>
        /// <param name="user">The client in session (its token and language are forwarded).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The subscriptions, or null when the Subscriptions API could not be read.</returns>
        Task<IReadOnlyList<ClientSubscription>?> GetSubscriptionsAsync(RequestUser user, CancellationToken cancellationToken = default);
    }

    /// <summary>A subscription of the client (only the fields the assistant uses).</summary>
    public sealed class ClientSubscription
    {
        /// <summary>Subscription id.</summary>
        public string? SubscriptionId { get; set; }

        /// <summary>Status, e.g. active.</summary>
        public string? Status { get; set; }

        /// <summary>Users related to the subscription (subst_subscription_profiles).</summary>
        public List<ClientSubscriptionProfileUser>? SubscriptionProfileUsers { get; set; }

        /// <summary>Courses of the subscription's plan (subject and level).</summary>
        public List<ClientSubscriptionCourse>? SubscriptionCourses { get; set; }
    }

    /// <summary>A user related to a subscription.</summary>
    public sealed class ClientSubscriptionProfileUser
    {
        /// <summary>ul_user_accounts.id.</summary>
        public string? UserId { get; set; }
    }

    /// <summary>A course of a subscription.</summary>
    public sealed class ClientSubscriptionCourse
    {
        /// <summary>Product name.</summary>
        public string? Name { get; set; }

        /// <summary>Athenas subject code, e.g. mat-sp.</summary>
        public string? Subject { get; set; }

        /// <summary>Subject name.</summary>
        public string? SubjectName { get; set; }

        /// <summary>Athenas level code, e.g. k, 1, 2.</summary>
        public string? Level { get; set; }
    }

    /// <inheritdoc />
    public sealed class ClientSubscriptionsClient : IClientSubscriptionsClient
    {
        private static readonly TimeSpan CallTimeout = TimeSpan.FromSeconds(15);
        private static readonly JsonSerializerSettings JsonSettings = new() { NullValueHandling = NullValueHandling.Ignore };

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ClientSubscriptionsClient> _logger;

        /// <summary>Creates the client.</summary>
        /// <param name="httpClient">HTTP client.</param>
        /// <param name="configuration">AppSettings:subscriptions_api_base_url and, optionally, AppSettings:subscriptions_endpoint_clients.</param>
        /// <param name="logger">Logger.</param>
        public ClientSubscriptionsClient(HttpClient httpClient, IConfiguration configuration, ILogger<ClientSubscriptionsClient> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<ClientSubscription>?> GetSubscriptionsAsync(RequestUser user, CancellationToken cancellationToken = default)
        {
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeout.CancelAfter(CallTimeout);
            try
            {
                var baseUrl = _configuration["AppSettings:subscriptions_api_base_url"]
                    ?? throw new InvalidOperationException("AppSettings:subscriptions_api_base_url is not configured.");
                var endpoint = _configuration["AppSettings:subscriptions_endpoint_clients"] ?? "subscriptionsapi/subscriptions/clients";
                using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(new Uri(baseUrl), endpoint + "?includeProfiles=true&includeCourses=true"));
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("Token", user.Token ?? string.Empty);
                if (!string.IsNullOrWhiteSpace(user.LangCode))
                {
                    request.Headers.Add("Accept-Language", user.LangCode);
                }

                using var response = await _httpClient.SendAsync(request, timeout.Token);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Subscriptions API clients returned {StatusCode}.", (int)response.StatusCode);
                    return null;
                }

                return JsonConvert.DeserializeObject<List<ClientSubscription>>(await response.Content.ReadAsStringAsync(timeout.Token), JsonSettings)
                    ?? new List<ClientSubscription>();
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or InvalidOperationException or UriFormatException)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }

                _logger.LogWarning(ex, "Could not read the client subscriptions from the Subscriptions API.");
                return null;
            }
        }
    }
}
