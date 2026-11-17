using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Subscriptions;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using CacheHandlerLibrary;

namespace GSWebAPI.CustomClasses.CacheHandler
{
    public class SubscriptionAPICache : DefaultCache
    {
        private const string SUBSCRIPTION_STRING = "subscription";
        private const string COLON_STRING = ":";
        private const string TRY_STRING = "try";

        private readonly IMemoryCache _memoryCache;

        // Inject IMemoryCache via DI
        public SubscriptionAPICache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// To get the subscription profiles.
        /// </summary>
        /// <param name="profileId">The profile (student) id.</param>
        /// <returns>Returns a list of subscription profile try response model.</returns>
        public List<SubscriptionProfileTryResponseModel>? GetSubscriptionProfileTryResponse(string profileId)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return null;

            var key = $"{SUBSCRIPTION_STRING}{COLON_STRING}{TRY_STRING}{COLON_STRING}{profileId}";


            // Debug mode path replaces HttpContext.Current.Cache with IMemoryCache
            return _memoryCache.TryGetValue(key, out List<SubscriptionProfileTryResponseModel> cached)
                ? cached
                : null;

        }

        /// <summary>
        /// Sets the subscription profiles tries response.
        /// </summary>
        /// <param name="profileId">The profile's id.</param>
        /// <param name="subscriptionProfileTriesList">The subscription profiles to store.</param>
        public void SetSubscriptionProfileTryResponse(string profileId, List<SubscriptionProfileTryResponseModel> subscriptionProfileTriesList)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return;

            var expirationHours = ConfigCache.GetSubscriptionProfilesTry();
            var key = $"{SUBSCRIPTION_STRING}{COLON_STRING}{TRY_STRING}{COLON_STRING}{profileId}";


            _memoryCache.Set(
                key,
                subscriptionProfileTriesList,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(expirationHours)
                });

        }
    }
}