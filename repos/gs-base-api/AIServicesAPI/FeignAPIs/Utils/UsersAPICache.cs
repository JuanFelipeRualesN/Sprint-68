using GSAIServicesAPI.FeignAPIs.Models;
using Microsoft.Extensions.Caching.Memory;

namespace GSAIServicesAPI.FeignAPIs.Utils
{
    /// <summary>
    /// To handle the cache for the users API. This class provides methods to get and set user access details in a local memory cache. It uses the IMemoryCache interface from Microsoft.Extensions.Caching.Memory to store and retrieve cached data. The cache is enabled or disabled based on configuration settings, and the expiration time for cached tokens can also be configured. This class is designed to improve the performance of the users API by reducing the number of requests to the underlying data source.
    /// </summary>
    public class UsersAPICache
    {
        private const string USERS_STRING = "users";
        private const int DEFAULT_TOKEN_EXPIRATION_HOURS = 1;

        private static readonly IMemoryCache LocalCache = new MemoryCache(new MemoryCacheOptions());

        private static readonly bool IsCacheEnabled =
            AppConfiguration.Configuration["AppSettings:cache_on_off"] != "0";

        private static readonly int TokenExpirationHours =
            int.TryParse(AppConfiguration.Configuration["AppSettings:cache_token_expiration"], out var hours)
                ? hours
                : DEFAULT_TOKEN_EXPIRATION_HOURS;

        /// <summary>
        /// Gets the user access details from cache.
        /// </summary>
        /// <param name="token">The user's access token.</param>
        /// <param name="permission">The permission's name.</param>
        /// <param name="ipAddress">The user's IP Address.</param>
        /// <returns>Validation details if present; otherwise null.</returns>
        public ValidationResponseModel? GetUserAccessDetails(string token, string permission, string ipAddress)
        {
            if (!IsCacheEnabled)
                return null;

            var key = BuildCacheKey(token, permission, ipAddress);

            return LocalCache.TryGetValue(key, out ValidationResponseModel? cached) ? cached : null;
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
            if (!IsCacheEnabled)
                return;

            var key = BuildCacheKey(token, permission, ipAddress);

            LocalCache.Set(key, responseSession, TimeSpan.FromHours(TokenExpirationHours));
        }

        /// <summary>
        /// Builds the cache key using the token, permission, and IP address. This method constructs a unique cache key by concatenating the USERS_STRING constant with the provided token, permission, and IP address. The resulting key is used to store and retrieve user access details in the cache, ensuring that each combination of token, permission, and IP address has its own distinct cache entry.
        /// </summary>
        /// <param name="token">The user's access token.</param>
        /// <param name="permission">The permission's name.</param>
        /// <param name="ipAddress">The user's IP Address.</param>
        /// <returns>The cache key.</returns>
        private static string BuildCacheKey(string token, string permission, string ipAddress)
            => $"{USERS_STRING}:{token}:{permission}:{ipAddress}";
    }
}