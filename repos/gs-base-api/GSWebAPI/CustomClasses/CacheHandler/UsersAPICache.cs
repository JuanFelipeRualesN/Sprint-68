using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using CacheHandlerLibrary;

namespace GSWebAPI.CustomClasses.CacheHandler
{
    /// <summary>
    /// To handle the cache from users API.
    /// </summary>
    public class UsersAPICache : DefaultCache
    {
        private const string USERS_STRING = "users";
        private const string COLON_STRING = ":";
        private const string USER_STRING = "user";

        private readonly IMemoryCache _memoryCache;

        // Inject IMemoryCache via DI
        public UsersAPICache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// To get the user access details from Cache.
        /// </summary>
        /// <returns>ValidationResponseModel if found; otherwise null.</returns>
        public ValidationResponseModel? GetUserAccessDetails(string token, string permission, string ipAddress)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return null;

            string key = $"{USERS_STRING}{COLON_STRING}{token}{COLON_STRING}{permission}{COLON_STRING}{ipAddress}";


            return _memoryCache.TryGetValue(key, out ValidationResponseModel model) ? model : null;

        }

        /// <summary>
        /// Sets the user access details in the cache.
        /// </summary>
        public void SetAccessDetails(string token, string permission, string ipAddress, ValidationResponseModel responseSession)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return;

            int expirationHours = ConfigCache.GetTokenExpiration();
            string key = $"{USERS_STRING}{COLON_STRING}{token}{COLON_STRING}{permission}{COLON_STRING}{ipAddress}";


            _memoryCache.Set(
                key,
                responseSession,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(expirationHours)
                });

        }

        /// <summary>
        /// To get if user A can handle user B.
        /// </summary>
        public bool GetUserCanHandleUser(string userA, string userB)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return false;

            string key = $"{USER_STRING}{COLON_STRING}{userA}{COLON_STRING}{USER_STRING}{COLON_STRING}{userB}";


            return _memoryCache.TryGetValue(key, out _);

        }

        /// <summary>
        /// To set if user A can handle user B.
        /// Only stores the positives (true) to avoid caching transient negatives.
        /// </summary>
        public void SetUserCanHandleUser(string userA, string userB, bool canBeHandle)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return;

            if (!canBeHandle) return; // keep your original behavior

            int expirationHours = ConfigCache.GetUserCanHandleUserExpiration();
            string key = $"{USER_STRING}{COLON_STRING}{userA}{COLON_STRING}{USER_STRING}{COLON_STRING}{userB}";
            _memoryCache.Set(
                key,
                canBeHandle,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(expirationHours)
                });
        }
    }
}