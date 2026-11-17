using GSWebAPI.GSWebAPI.Domain.Models.General;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using CacheHandlerLibrary;


namespace GSWebAPI.CustomClasses.CacheHandler
{
    /// <summary>
    /// To handle the APP versions in the cache (and avoid multiple calls in the DB). Force
    /// </summary>
    public class VersionCache : DefaultCache
    {
        private const string UNITY_STRING = "unity";
        private const string COLON_STRING = ":";
        private const string VERSION_STRING = "version";

        private readonly IMemoryCache _memoryCache;

        // Inject IMemoryCache via DI
        public VersionCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Gets the Unity app versions from cache.
        /// </summary>
        public List<GameVersionModel>? GetVersions()
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return null;

            var key = $"{UNITY_STRING}{COLON_STRING}{VERSION_STRING}";


            return _memoryCache.TryGetValue(key, out List<GameVersionModel> list) ? list : null;

        }

        /// <summary>
        /// Sets the Unity app versions in cache.
        /// </summary>
        public void SetVersions(List<GameVersionModel> objToStore)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return;

            var expirationHours = ConfigCache.GetVersionsExpiration();
            var key = $"{UNITY_STRING}{COLON_STRING}{VERSION_STRING}";

            // Only set if not already present (mirrors your original behavior)
            if (!_memoryCache.TryGetValue(key, out _))
            {
                _memoryCache.Set(
                    key,
                    objToStore,
                    new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(expirationHours)
                    });
            }

        }
    }
}