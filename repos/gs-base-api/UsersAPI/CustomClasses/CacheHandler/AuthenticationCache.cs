using CacheHandlerLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace UsersAPI.CustomClasses.CacheHandler
{
    /// <summary>
    /// Handles cache related to authentication. Updated for .NET 6+ with DI and no per-call service lookups.
    /// </summary>
    public class AuthenticationCache : DefaultCache
    {
        private const string USERS_STRING = "users";
        private const string COLON_STRING = ":";

        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _debugCache;
        private readonly IHttpContextAccessor? _httpContextAccessor;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configuration">App configuration (reads AppSettings:cache_on_off).</param>
        /// <param name="debugCache">IMemoryCache used in DebugMode() only.</param>
        /// <param name="httpContextAccessor">Optional: used only to read/write HttpContext.Items in debug fallback.</param>
        public AuthenticationCache(
            IConfiguration configuration,
            IMemoryCache debugCache,
            IHttpContextAccessor? httpContextAccessor = null)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _debugCache = debugCache ?? throw new ArgumentNullException(nameof(debugCache));
            _httpContextAccessor = httpContextAccessor; // optional
        }

        private bool IsCacheEnabled() =>
            !string.Equals(AppConfiguration.Configuration["AppSettings:AppSettings:cache_on_off"], "0", StringComparison.Ordinal);

        /// <summary>
        /// Gets the user access details from cache (Redis in normal mode, IMemoryCache in debug).
        /// </summary>
        public UsersAPI.Domain.Models.Generals.ValidationResponseModel? GetUserAccessDetails(
            string token, string permission, string ipAddress)
        {
            if (!IsCacheEnabled()) return null;

            var key = BuildKey(token, permission, ipAddress);


            // Debug: try IMemoryCache first
            if (_debugCache.TryGetValue(key, out UsersAPI.Domain.Models.Generals.ValidationResponseModel objFromMem))
                return objFromMem;

            // Optional per-request fallback via HttpContext.Items
            var items = _httpContextAccessor?.HttpContext?.Items;
            if (items != null && items.TryGetValue(key, out var item) &&
                item is UsersAPI.Domain.Models.Generals.ValidationResponseModel vm)
            {
                return vm;
            }

            return null;

        }

        /// <summary>
        /// Sets the access details in cache.
        /// </summary>
        public void SetAccessDetails(
            string token,
            string permission,
            string ipAddress,
            UsersAPI.Domain.Models.Generals.ValidationResponseModel responseSession)
        {
            if (!IsCacheEnabled()) return;

            var key = BuildKey(token, permission, ipAddress);
            var expirationHours = ConfigCache.GetTokenExpiration();

            if (!ConfigCache.IsDebugMode())
            {
                // Non-debug: use DefaultCache backend (e.g., Redis)
                var objToStore = JsonConvert.SerializeObject(responseSession);
                Write(key, objToStore, expirationHours);
            }
            else
            {
                // Debug: store in IMemoryCache; optional per-request Items fallback
                _debugCache.Set(key, responseSession,
                    new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(expirationHours)
                    });

                var items = _httpContextAccessor?.HttpContext?.Items;
                if (items != null)
                {
                    items[key] = responseSession; // cleared at end of request
                }
            }
        }

        /// <summary>
        /// Deletes the access details from cache.
        /// </summary>
        public void DeleteAccessDetails(string token, string permission, string ipAddress)
        {
            if (!IsCacheEnabled()) return;

            var key = BuildKey(token, permission, ipAddress);

            try
            {

                // Debug: remove from IMemoryCache and optional Items
                _debugCache.Remove(key);

                var ctx = _httpContextAccessor?.HttpContext;
                if (ctx?.Items != null && ctx.Items.ContainsKey(key))
                    ctx.Items.Remove(key);

            }
            catch
            {
                // Intentionally ignored to preserve original behavior
            }
        }

        private static string BuildKey(string token, string permission, string ip)
            => $"{USERS_STRING}{COLON_STRING}{token}{COLON_STRING}{permission}{COLON_STRING}{ip}";
    }
}
