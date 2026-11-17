using System;
using Newtonsoft.Json;
using CacheHandlerLibrary;
using Microsoft.Extensions.Caching.Memory;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs;



namespace SubscriptionsAPI.CustomClasses.CacheHandler
{
    /// <summary>
    /// Cache helper for user-related data (supports Redis via DefaultCache + local IMemoryCache for debug mode).
    /// </summary>
    public class UsersAPICache : DefaultCache
    {
        private const string USERS_STRING = "users";
        private const string COLON_STRING = ":";

        // Replaces HttpContext.Cache in .NET Framework debug paths
        private static readonly IMemoryCache LocalCache = new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// Gets the user access details from cache.
        /// </summary>
        /// <param name="token">The user's access token.</param>
        /// <param name="permission">The permission's name.</param>
        /// <param name="ipAddress">The user's IP Address.</param>
        /// <returns>Validation details if present; otherwise null.</returns>
        public ValidationResponseModel? GetUserAccessDetails(string token, string permission, string ipAddress)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return null;

            var key = $"{USERS_STRING}{COLON_STRING}{token}{COLON_STRING}{permission}{COLON_STRING}{ipAddress}";


            // Debug mode: use local in-memory cache
            return LocalCache.TryGetValue(key, out ValidationResponseModel cached) ? cached : null;

        }

        /// <summary>
        /// Sets the access details in cache.
        /// </summary>
        /// <param name="token">The user's access token.</param>
        /// <param name="permission">The permission's name.</param>
        /// <param name="ipAddress">The user's IP Address.</param>
        /// <param name="responseSession">The validation response to cache.</param>
        public void SetAccessDetails(string token, string permission, string ipAddress, ValidationResponseModel responseSession)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return;

            var key = $"{USERS_STRING}{COLON_STRING}{token}{COLON_STRING}{permission}{COLON_STRING}{ipAddress}";
            var hours = ConfigCache.GetTokenExpiration();

            // Debug mode: store locally
            LocalCache.Set(key, responseSession, TimeSpan.FromHours(hours));

        }
    }
}

