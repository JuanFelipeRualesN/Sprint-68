using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses;
using CacheHandlerLibrary; // Assumes DefaultCache, ConfigCache, Utils.RedisCacheHandler
// Also assumes an IConfiguration-backed AppConfiguration.Configuration["AppSettings:*"] as used elsewhere

namespace SubscriptionsAPI.CustomClasses.CacheHandler
{
    /// <summary>
    /// Handles cache related to courses (local in-memory + Redis via DefaultCache helpers).
    /// </summary>
    public class CoursesCache : DefaultCache
    {
        private static readonly IMemoryCache LocalCache = new MemoryCache(new MemoryCacheOptions());

        private const string STUDENT_STRING = "student";
        private const string COLON_STRING = ":";
        private const string COURSES_STRING = "courses";
        private const string USER_STRING = "user";
        private const string SUBSCRIPTION_STRING = "subscription";

        /// <summary>
        /// Get user-related subscriptions from cache: prefer local in-memory (short TTL), otherwise Redis (with 4s timeout).
        /// </summary>
        public List<SubscriptionAcademicFullResponseModel>? GetUserRelatedSubscriptions(string token)
        {
            var cacheKey = $"{USER_STRING}{COLON_STRING}{SUBSCRIPTION_STRING}{COLON_STRING}{token}";

            // 1) Local in-memory first
            if (LocalCache.TryGetValue(cacheKey, out List<SubscriptionAcademicFullResponseModel> localHit))
                return localHit;

            // 2) Redis disabled?
            if (!ConfigCache.IsRedisOn())
                return null;

            try
            {
                // 3) Race Redis read vs timeout
                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(4));
                var redisTask = Task.Run(() =>
                {
                    var value = Read(cacheKey);
                    var s = value.ToString();
                    return !string.IsNullOrWhiteSpace(s)
                        ? JsonConvert.DeserializeObject<List<SubscriptionAcademicFullResponseModel>>(s!)
                        : null;
                });

                var completed = Task.WhenAny(redisTask, timeoutTask).GetAwaiter().GetResult();

                if (completed == timeoutTask)
                {
                    // Timed out -> no Redis result
                    return null;
                }

                var result = redisTask.GetAwaiter().GetResult();
                if (result != null)
                {
                    // Warm local cache (short TTL)
                    LocalCache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
                }
                return result;
            }
            catch
            {
                // Swallow per original behavior (return null on exception)
                return null;
            }
        }

        /// <summary>
        /// Store user-related subscriptions in Redis + local in-memory cache.
        /// </summary>
        public void StoreUserRelatedSubscriptions(string token, List<SubscriptionAcademicFullResponseModel> subscriptions)
        {
            if (!ConfigCache.IsRedisOn())
                return;

            var cacheKey = $"{USER_STRING}{COLON_STRING}{SUBSCRIPTION_STRING}{COLON_STRING}{token}";
            var payload = JsonConvert.SerializeObject(subscriptions);

            // Redis (expiration in hours from config)
            Write(cacheKey, payload, ConfigCache.GetStudentCoursesExpiration());

            // Local in-memory (short TTL)
            LocalCache.Set(cacheKey, subscriptions, TimeSpan.FromMinutes(5));
        }

        /// <summary>
        /// Get the student courses from cache.
        /// </summary>
        public UserCoursesResponseModel? GetUserCourses(string token)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return null;

            var key = $"{STUDENT_STRING}{COLON_STRING}{token}{COLON_STRING}{COURSES_STRING}";

            // Debug mode: use local in-memory (replaces HttpContext.Cache)
            return LocalCache.TryGetValue(key, out UserCoursesResponseModel cached)
                ? cached
                : null;

        }

        /// <summary>
        /// Save student courses into cache.
        /// </summary>
        public void SetStudentCourses(string token, UserCoursesResponseModel studentCourses)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return;

            var key = $"{STUDENT_STRING}{COLON_STRING}{token}{COLON_STRING}{COURSES_STRING}";
            var expirationHours = ConfigCache.GetStudentCoursesExpiration();


            // Debug mode: use local in-memory (replaces System.Web.* Cache)
            LocalCache.Set(key, studentCourses, TimeSpan.FromHours(expirationHours));

        }
    }
}
