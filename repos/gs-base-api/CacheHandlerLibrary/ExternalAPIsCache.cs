using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

namespace CacheHandlerLibrary
{
    /// <summary>
    /// To handle the cache for external apis.
    /// </summary>
    public class ExternalAPIsCache : DefaultCache
    {
        private const string USERS_STRING = "users";
        private const string COLON_STRING = ":";
        private readonly static string configCacheOnOff = AppConfiguration.Configuration["AppSettings:cache_on_off"];


        /// <summary>
        /// To get the user access details from Redis Cache.
        /// </summary>
        /// <param name="token">The user's access token.</param>
        /// <param name="permission">The permission's name.</param>
        /// <param name="ipAddress">The user's IP Address.</param>
        /// <returns>Returns the Validation Response Model with user details if valid, else, it will return null.</returns>
        public dynamic GetUserAccessDetails(string token, string permission, string ipAddress)
        {
            if (configCacheOnOff == "0")
                return null;

            string cacheKey = $"{USERS_STRING}{COLON_STRING}{token}{COLON_STRING}{permission}{COLON_STRING}{ipAddress}";

            if (ConfigCache.IsRedisOn())
            {
                var value = Read(cacheKey);
                if (!value.IsNullOrEmpty)
                {
                    return JsonConvert.DeserializeObject<dynamic>(value.ToString());
                }
            }
            else
            {
                var httpContext = new HttpContextAccessor().HttpContext;
                if (httpContext != null)
                {
                    var memoryCache = httpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;

                    if (memoryCache != null && memoryCache.TryGetValue(cacheKey, out string cachedJson))
                    {
                        return JsonConvert.DeserializeObject<dynamic>(cachedJson);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Sets the access details in the cache.
        /// </summary>
        /// <param name="token">The user's access token.</param>
        /// <param name="permission">The permission's name.</param>
        /// <param name="ipAddress">The user's IP Address.</param>
        /// <param name="responseSession">Returns a ValidationResponseModel if not null.</param>
        public void SetAccessDetails(string token, string permission, string ipAddress, JToken jsonToken)
        {
            if (configCacheOnOff == "0") return;

            var expirationHours = ConfigCache.GetTokenExpiration();
            string cacheKey = $"{USERS_STRING}{COLON_STRING}{token}{COLON_STRING}{permission}{COLON_STRING}{ipAddress}";
            string json = jsonToken?.ToString(Newtonsoft.Json.Formatting.None);

            if (ConfigCache.IsRedisOn())
            {
                Write(cacheKey, json, expirationHours);
            }
            else
            {
                var httpContext = new HttpContextAccessor().HttpContext;
                if (httpContext == null) return;

                var memoryCache = httpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;
                if (memoryCache == null) return;

                memoryCache.Set(cacheKey, json, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(expirationHours),
                    Priority = CacheItemPriority.Normal
                });
            }
        }
    }
}