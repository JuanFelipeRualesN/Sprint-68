using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using CacheHandlerLibrary;

namespace UsersAPI.CustomClasses.CacheHandler
{
    /// <summary>
    /// Cache handler for external API requests (updated for .NET).
    /// Uses IDistributedCache (e.g., Redis). If ConfigCache.IsDebugMode() is true,
    /// it uses IMemoryCache so you can inspect keys during development.
    /// </summary>
    public class ExternalAPIsCache
    {
        private const string COUNTRIES_STRING = "countries";
        private const string COUNTRY_STRING = "country";
        private const string META_DATA_STRING = "metadata";
        private const string ISO_STRING = "iso";
        private const string COLON_STRING = ":";
        private const string SUBSCRIPTION_STRING = "subscription";
        private const string SCHOOL_STRING = "school";
        private const string SCHOOL_ID_STRING = "school_ids";
        private const string DETAILS_STRING = "detail";
        private const string ORE_STRING = "ore";

        private readonly IDistributedCache _distributed;
        private readonly IMemoryCache _memory; // used only in debug mode
        private readonly IConfiguration _config;

        public ExternalAPIsCache(
            IDistributedCache distributedCache,
            IMemoryCache memoryCache,
            IConfiguration configuration)
        {
            _distributed = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
            _memory = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _config = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        private bool CacheOn => _config["AppSettings:cache_on_off"] != "0";

        private static DistributedCacheEntryOptions ExpireInHours(int hours) =>
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(hours)
            };

        // --- helpers (string) ---

        private string ReadString(string key)
        {
            if (ConfigCache.IsDebugMode())
            {
                return _memory.TryGetValue(key, out string s) ? s : null;
            }

            return _distributed.GetString(key);
        }

        private void WriteString(string key, string value, int expirationHours)
        {
            if (ConfigCache.IsDebugMode())
            {
                _memory.Set(key, value, TimeSpan.FromHours(expirationHours));
                return;
            }

            _distributed.SetString(key, value, ExpireInHours(expirationHours));
        }

        // --- helpers (typed) ---

        private T ReadJson<T>(string key)
        {
            var s = ReadString(key);
            if (string.IsNullOrEmpty(s)) return default;
            return JsonConvert.DeserializeObject<T>(s);
        }

        private void WriteJson<T>(string key, T value, int expirationHours)
        {
            var s = JsonConvert.SerializeObject(value);
            WriteString(key, s, expirationHours);
        }

        // ===========================================================
        // Countries
        // ===========================================================

        /// <summary>Get the country standard name by ISO code.</summary>
        public string GetCountryStandardNameByISO(string isoCode)
        {
            if (!CacheOn) return null;
            var key = $"{COUNTRIES_STRING}{COLON_STRING}{COUNTRY_STRING}{COLON_STRING}{isoCode}";
            return ReadJson<string>(key);
        }

        /// <summary>Set the country standard name by ISO code.</summary>
        public void SetCountryStandardNameByISO(string isoCode, string valueToStore)
        {
            if (!CacheOn) return;
            var key = $"{COUNTRIES_STRING}{COLON_STRING}{COUNTRY_STRING}{COLON_STRING}{isoCode}";
            var hours = ConfigCache.GetCountryISOCodeExpiration();
            WriteJson(key, valueToStore, hours);
        }

        /// <summary>Get ISO (likely ISO3) by ISO2 code.</summary>
        public string GetCountryISOByISO2(string isoCode)
        {
            if (!CacheOn) return null;
            var key = $"{COUNTRIES_STRING}{COLON_STRING}{ISO_STRING}{COLON_STRING}{isoCode}";
            return ReadJson<string>(key);
        }

        /// <summary>Set ISO (likely ISO3) by ISO2 code.</summary>
        public void SetCountryISOByISO2(string isoCode, string valueToStore)
        {
            if (!CacheOn) return;
            var key = $"{COUNTRIES_STRING}{COLON_STRING}{ISO_STRING}{COLON_STRING}{isoCode}";
            var hours = ConfigCache.GetCountryISOCodeExpiration();
            WriteJson(key, valueToStore, hours);
        }

        // ===========================================================
        // Subscription → SchoolId
        // ===========================================================

        /// <summary>Get school id for a given subscription id.</summary>
        public string GetSchoolIdOfSubscription(string subscriptionId)
        {
            if (!CacheOn) return null;
            var key = $"{SUBSCRIPTION_STRING}{COLON_STRING}{SCHOOL_STRING}{COLON_STRING}{subscriptionId}";
            return ReadJson<string>(key);
        }

        /// <summary>Set school id for a given subscription id.</summary>
        public void SetSchoolIdOfSubscription(string subscriptionId, string valueToStore)
        {
            if (!CacheOn) return;
            var key = $"{SUBSCRIPTION_STRING}{COLON_STRING}{SCHOOL_STRING}{COLON_STRING}{subscriptionId}";
            var hours = ConfigCache.GetSchoolIdOfSubscriptionExpiration();
            WriteJson(key, valueToStore, hours);
        }

        // ===========================================================
        // ORE → School Ids
        // ===========================================================

        /// <summary>Get ORE school ids (list) by ore id.</summary>
        public List<string> GetOreSchoolIds(string oreId)
        {
            if (!CacheOn) return new List<string>();
            var key = $"{ORE_STRING}{COLON_STRING}{SCHOOL_ID_STRING}{COLON_STRING}{oreId}";
            return ReadJson<List<string>>(key) ?? new List<string>();
        }

        /// <summary>Set ORE school ids (list) by ore id.</summary>
        public void SetOreSchoolIds(string oreId, List<string> valueToStore)
        {
            if (!CacheOn) return;
            var key = $"{ORE_STRING}{COLON_STRING}{SCHOOL_ID_STRING}{COLON_STRING}{oreId}";
            var hours = ConfigCache.GetOreSchoolsExpiration();
            WriteJson(key, valueToStore, hours);
        }

        // ===========================================================
        // Entity general details (school entities)
        // ===========================================================

        /// <summary>Get entity general details for a list of entity ids.</summary>
        public List<UsersAPI.Domain.Models.Generals.Responses.APIs.EntityGeneralDetailsResponseModel> GetEntityGeneralDetails(List<string> entityIds)
        {
            if (!CacheOn) return null;

            var list = new List<UsersAPI.Domain.Models.Generals.Responses.APIs.EntityGeneralDetailsResponseModel>();
            foreach (var entityId in entityIds)
            {
                var key = $"{SCHOOL_STRING}{COLON_STRING}{entityId}";
                var entity = ReadJson<UsersAPI.Domain.Models.Generals.Responses.APIs.EntityGeneralDetailsResponseModel>(key);

                if (ConfigCache.IsDebugMode())
                {
                    // In debug mode we want to allow "missing" keys to just be skipped (as in original).
                    if (entity == null) return null;
                }

                if (entity != null)
                {
                    list.Add(entity);
                }
            }
            return list;
        }

        /// <summary>Set (only if absent) entity general details for each entity.</summary>
        public void SetEntityGeneralDetails(List<UsersAPI.Domain.Models.Generals.Responses.APIs.EntityGeneralDetailsResponseModel> entities)
        {
            if (!CacheOn) return;

            var hours = ConfigCache.GetEntityDetailsExpiration();
            foreach (var entity in entities)
            {
                var key = $"{SCHOOL_STRING}{COLON_STRING}{entity.EntityId}";
                // Write only if not present (mimic original logic).
                var existing = ReadString(key);
                if (string.IsNullOrEmpty(existing))
                {
                    WriteJson(key, entity, hours);
                }
            }
        }

        // ===========================================================
        // Subscription details
        // ===========================================================

        /// <summary>Get subscription details by subscription id.</summary>
        public UsersAPI.Domain.Models.Generals.Responses.APIs.SubscriptionResponseModel GetSubscriptionDetails(string subscriptionId)
        {
            if (!CacheOn) return null;
            var key = $"{SUBSCRIPTION_STRING}{COLON_STRING}{subscriptionId}{COLON_STRING}{DETAILS_STRING}";
            return ReadJson<UsersAPI.Domain.Models.Generals.Responses.APIs.SubscriptionResponseModel>(key);
        }

        /// <summary>Set subscription details by subscription id.</summary>
        public void SetSubscriptionDetails(string subscriptionId, UsersAPI.Domain.Models.Generals.Responses.APIs.SubscriptionResponseModel subscriptionResponseModel)
        {
            if (!CacheOn) return;
            var key = $"{SUBSCRIPTION_STRING}{COLON_STRING}{subscriptionId}{COLON_STRING}{DETAILS_STRING}";
            var hours = ConfigCache.GetSubscriptionDetailsExpiration();
            WriteJson(key, subscriptionResponseModel, hours);
        }
    }
}
