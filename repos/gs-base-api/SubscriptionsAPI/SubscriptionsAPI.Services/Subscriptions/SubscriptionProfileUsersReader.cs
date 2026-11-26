using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs.Users;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.FeignAPIs;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions
{
    /// <summary>
    /// The users related to a set of subscriptions (subst_subscription_profiles), with their names and roles.
    /// Built for many subscriptions at once:
    /// <list type="bullet">
    /// <item>One query for the profiles of every subscription (the existing <see cref="SubscriptionProfileDAO"/> batch read).</item>
    /// <item>One Users API call for all the names and one for all the roles, for the distinct users, run in parallel.</item>
    /// <item>Names and roles are display data: each call waits at most <see cref="UsersApiWait"/>. When one fails or is slow,
    /// the users are still listed with null names or roles.</item>
    /// </list>
    /// </summary>
    public static class SubscriptionProfileUsersReader
    {
        /// <summary>Longest wait for each Users API call (names, roles).</summary>
        public static readonly TimeSpan UsersApiWait = TimeSpan.FromSeconds(5);

        /// <summary>
        /// The related users of each subscription, keyed by subscription id (subscriptions without profiles get an empty list).
        /// </summary>
        /// <param name="readConnectionString">Reader connection string of the subscriptions database.</param>
        /// <param name="token">The session token, forwarded to the Users API.</param>
        /// <param name="ipAddress">The client IP address, forwarded to the Users API.</param>
        /// <param name="langCode">The session language, forwarded to the Users API.</param>
        /// <param name="subscriptionIds">The subscriptions to read.</param>
        /// <param name="currentUserId">The user in session (marked with <see cref="SubscriptionProfileUserResponseModel.IsCurrentUser"/>).</param>
        /// <returns>The related users per subscription, ordered by when they were related.</returns>
        public static async Task<Dictionary<string, List<SubscriptionProfileUserResponseModel>>> ReadAsync(
            string readConnectionString, string token, string ipAddress, string langCode, IReadOnlyCollection<string> subscriptionIds, string currentUserId)
        {
            var result = subscriptionIds.Distinct().ToDictionary(id => id, _ => new List<SubscriptionProfileUserResponseModel>());
            if (result.Count == 0)
            {
                return result;
            }

            // 1) All the profiles in one query: WHERE (subscription_id = @0) OR (subscription_id = @1) ...
            var where = result.Keys.Select(id => new SubscriptionProfileModel { SubscriptionId = id }).ToList();
            var profiles = await Task.Run(() => new SubscriptionProfileDAO().Read(readConnectionString, where) ?? new List<SubscriptionProfileModel>());
            var userIds = profiles.Where(p => !string.IsNullOrWhiteSpace(p.ProfileId)).Select(p => p.ProfileId).Distinct().ToList();

            // 2) Names and roles for the distinct users, in parallel; each one waits at most UsersApiWait.
            var namesTask = WithinAsync(GetUserDetails.GetUserFullNamesAsync(token, langCode, userIds), new List<UserFullNameModel>());
            var rolesTask = WithinAsync(Task.Run(() => userIds.Count > 0 ? GetUserDetails.GetUserRoles(token, ipAddress, userIds) : new List<UserRoleModel>()), new List<UserRoleModel>());
            await Task.WhenAll(namesTask, rolesTask);

            var names = (namesTask.Result ?? new List<UserFullNameModel>())
                .Where(n => !string.IsNullOrWhiteSpace(n?.UserId))
                .GroupBy(n => n.UserId)
                .ToDictionary(g => g.Key, g => g.First());
            var roles = (rolesTask.Result ?? new List<UserRoleModel>())
                .Where(r => !string.IsNullOrWhiteSpace(r?.UserId))
                .GroupBy(r => r.UserId)
                .ToDictionary(g => g.Key, g => g.First().RoleId);

            // 3) One entry per subscription and user (a duplicated row is listed once).
            foreach (var profile in profiles
                .Where(p => !string.IsNullOrWhiteSpace(p.ProfileId) && p.SubscriptionId != null && result.ContainsKey(p.SubscriptionId))
                .GroupBy(p => (p.SubscriptionId, p.ProfileId))
                .Select(g => g.First())
                // The DAO returns created_at as a culture-formatted string; the AUTO_INCREMENT id keeps the same order.
                .OrderBy(p => ulong.TryParse(p.Id, out var id) ? id : ulong.MaxValue))
            {
                names.TryGetValue(profile.ProfileId, out var name);
                result[profile.SubscriptionId].Add(new SubscriptionProfileUserResponseModel
                {
                    SubscriptionProfileId = profile.Id,
                    SubscriptionId = profile.SubscriptionId,
                    UserId = profile.ProfileId,
                    GivenName = name?.GivenName,
                    Surname = name?.Surname,
                    Suffix = name?.Suffix,
                    FullName = name == null ? null : FullName(name),
                    RoleId = roles.TryGetValue(profile.ProfileId, out var roleId) ? roleId : null,
                    IsCurrentUser = profile.ProfileId == currentUserId,
                    CreatedAt = profile.CreatedAt,
                });
            }

            return result;
        }

        private static string FullName(UserFullNameModel name)
        {
            var full = string.Join(" ", new[] { name.GivenName, name.Surname, name.Suffix }.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p.Trim()));
            return full.Length > 0 ? full : null;
        }

        /// <summary>The task's result if it finishes within <see cref="UsersApiWait"/> without failing; otherwise <paramref name="fallback"/>.</summary>
        private static async Task<T> WithinAsync<T>(Task<T> task, T fallback)
        {
            var finished = await Task.WhenAny(task, Task.Delay(UsersApiWait));
            if (finished != task)
            {
                // Observe a later failure so it never becomes an unobserved task exception.
                _ = task.ContinueWith(t => t.Exception, TaskContinuationOptions.OnlyOnFaulted);
                return fallback;
            }

            try
            {
                return await task;
            }
            catch (Exception)
            {
                return fallback;
            }
        }
    }
}
