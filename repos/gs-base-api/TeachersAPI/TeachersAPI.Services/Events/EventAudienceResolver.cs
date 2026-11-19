using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeachersAPI.Domain.Models.Events;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.TeachersAPI.Services.FeignAPI;

namespace TeachersAPI.TeachersAPI.Services.Events
{
    /// <summary>
    /// Resolves an event's audience (Roles, Schools, Regions, Specific users) into the
    /// final list of users to sync with Microsoft Graph. Combination rule is strictly OR (Roles OR
    /// Schools OR Regions OR Specific users) — never AND, per the mockup ("las categorías se
    /// combinan con unión (OR)"), which is authoritative over the ticket's PDF text. Regions
    /// resolve by translating each RegionId into its member SchoolIds via SchoolsAPI's
    /// api/ore/{oreId}/schools_ids and merging them into the same Schools resolution path — there
    /// is no separate Región pipeline. Knows nothing about Microsoft Graph event/attendee JSON —
    /// only resolves users.
    /// </summary>
    public static class EventAudienceResolver
    {
        // SchoolIds/UserIds are applied in api/profiles/emails' WHERE clause, so splitting a
        // large list into chunks reduces server-side work per call. RoleId can't be split.
        private const int FilterChunkSize = 200;

        // Caps simultaneous chunk requests so a large SchoolIds/UserIds list can't flood Users API.
        private const int MaxConcurrentCalls = 5;

        // Page size for GetAllUsersPaged. Keeps every response bounded regardless of how many
        // users a RoleId or chunk resolves to. Only runs from the background sync worker.
        private const int PageSize = 5000;

        // Safety cap on pages per fetch, as a backstop against an unexpectedly large population.
        private const int MaxPages = 500;

        private static readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("EventAudienceResolver");

        /// <summary>
        /// Resolves the final, deduplicated list of users for an event's audience.
        /// </summary>
        /// <param name="roles">The event's role criteria.</param>
        /// <param name="schools">The event's school criteria.</param>
        /// <param name="regions">The event's region criteria.</param>
        /// <param name="users">The event's specific-user criteria.</param>
        /// <param name="token">Session token used to call Users API and SchoolsAPI.</param>
        /// <returns>The resolved, deduplicated, domain-valid list of users.</returns>
        public static async Task<List<UserResponse>> ResolveAudience(List<EventRoleModel> roles, List<EventSchoolModel> schools, List<EventRegionModel> regions, List<EventUserModel> users, string token)
        {
            List<string> distinctRoleIds = (roles ?? new List<EventRoleModel>())
                .Select(role => role.RoleId)
                .Where(roleId => !string.IsNullOrWhiteSpace(roleId))
                .Distinct()
                .ToList();

            List<string> schoolIds = (schools ?? new List<EventSchoolModel>())
                .Select(school => school.SchoolIdentifier)
                .Where(schoolId => !string.IsNullOrWhiteSpace(schoolId))
                .Distinct()
                .ToList();

            List<string> distinctRegionIds = (regions ?? new List<EventRegionModel>())
                .Select(region => region.RegionId)
                .Where(regionId => !string.IsNullOrWhiteSpace(regionId))
                .Distinct()
                .ToList();

            foreach (string regionId in distinctRegionIds)
            {
                schoolIds.AddRange(SchoolsDetailsHandler.GetSchoolIdsForRegion(regionId, token));
            }

            schoolIds = schoolIds.Distinct().ToList();

            List<string> userIds = (users ?? new List<EventUserModel>())
                .Select(user => user.UserId)
                .Where(userId => !string.IsNullOrWhiteSpace(userId))
                .Distinct()
                .ToList();

            // Every fetch is paged (GetAllUsersPaged), so no single response is unbounded.
            // Different fetches run concurrently, bounded by MaxConcurrentCalls below.
            List<Func<Task<List<UserResponse>>>> operations = distinctRoleIds
                .Select(roleId => (Func<Task<List<UserResponse>>>)(() => GetAllUsersPaged(roleId, null, null, token)))
                .ToList();

            if (schoolIds.Count > 0)
            {
                operations.AddRange(Chunk(schoolIds, FilterChunkSize)
                    .Select(chunk => (Func<Task<List<UserResponse>>>)(() => GetAllUsersPaged(null, chunk, null, token))));
            }

            if (userIds.Count > 0)
            {
                operations.AddRange(Chunk(userIds, FilterChunkSize)
                    .Select(chunk => (Func<Task<List<UserResponse>>>)(() => GetAllUsersPaged(null, null, chunk, token))));
            }

            List<UserResponse> resolvedUsers = (await RunWithBoundedConcurrency(operations, MaxConcurrentCalls))
                .SelectMany(result => result)
                .ToList();

            string azureEmailDomain = AppConfiguration.Configuration["AppSettings:azure_email_domain"];

            return resolvedUsers
                .GroupBy(user => user.UserId)
                .Select(group => group.First())
                .Where(user => user.Email != null)
                .Where(user => user.Email != "")
                .GroupBy(user => user.Email)
                .Select(group => group.First())
                .Where(user => IsValidEmailForDomain(user.Email, azureEmailDomain))
                .ToList();
        }

        /// <summary>
        /// Fetches every user matching the given filter (a single RoleId, or a School/User
        /// chunk), paging sequentially. Stops on the first empty page.
        /// </summary>
        private static async Task<List<UserResponse>> GetAllUsersPaged(string roleId, List<string> schoolIds, List<string> userIds, string token)
        {
            List<UserResponse> allUsers = new List<UserResponse>();

            for (int pageFrom = 0; pageFrom < MaxPages; pageFrom++)
            {
                List<UserResponse> page = await GetUserDetails.GetUserEmailsForEvents(roleId, schoolIds, userIds, token, pageFrom, PageSize);

                if (page.Count == 0)
                {
                    return allUsers;
                }

                allUsers.AddRange(page);
            }

            _logger.LogWarning(
                "GetAllUsersPaged hit the {MaxPages}-page safety cap (RoleId={RoleId}, {SchoolCount} school id(s), {UserCount} user id(s)) — {FetchedSoFar} users fetched so far; stopping early.",
                MaxPages, roleId, schoolIds?.Count ?? 0, userIds?.Count ?? 0, allUsers.Count);

            return allUsers;
        }

        /// <summary>
        /// Splits a list into fixed-size, order-preserving chunks.
        /// </summary>
        private static IEnumerable<List<string>> Chunk(List<string> source, int size)
        {
            for (int i = 0; i < source.Count; i += size)
            {
                yield return source.GetRange(i, Math.Min(size, source.Count - i));
            }
        }

        /// <summary>
        /// Runs the given operations concurrently, capped at maxConcurrency in flight at any time.
        /// </summary>
        private static async Task<List<T>> RunWithBoundedConcurrency<T>(List<Func<Task<T>>> operations, int maxConcurrency)
        {
            using (var gate = new System.Threading.SemaphoreSlim(maxConcurrency))
            {
                var tasks = operations.Select(async operation =>
                {
                    await gate.WaitAsync();
                    try
                    {
                        return await operation();
                    }
                    finally
                    {
                        gate.Release();
                    }
                });

                T[] results = await Task.WhenAll(tasks);
                return results.ToList();
            }
        }

        /// <summary>
        /// Checks if the user's email corresponds to the configured Azure tenant domain.
        /// Self-contained (no reference to GraphAPI/GraphEventsAPI) by design.
        /// </summary>
        private static bool IsValidEmailForDomain(string email, string domainToCheck)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(domainToCheck))
                return false;

            string pattern = @"^[a-zA-Z0-9._%+-]+@" + Regex.Escape(domainToCheck) + "$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }
    }
}
