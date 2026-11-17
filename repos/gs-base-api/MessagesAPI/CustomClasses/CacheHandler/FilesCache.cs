using System;
using System.Collections.Generic;
using System.Configuration;
using MessagesAPI.MessageAPI.Domain.Models.EmailSystem;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using CacheHandlerLibrary;

namespace MessagesAPI.CustomClasses.CacheHandler
{
    /// <summary>
    /// To handle the cache for files details, as file rules
    /// </summary>
    public class FilesCache : DefaultCache
    {
        private const string FILES_STRING = "files";
        private const string COLON_STRING = ":";
        private const string RULES_STRING = "rules";
        private readonly static string configCacheOnOff = AppConfiguration.Configuration["AppSettings:cache_on_off"];

        /// <summary>
        /// To get the file attachment rules from Cache.
        /// </summary>
        /// <returns>Returns the list of attachment rule models..</returns>
        public List<AttachmentRuleModel> GetFileAttachmentRules()
        {
            if (configCacheOnOff == "0")
            {
                return null;
            }

            string cacheKey = $"{FILES_STRING}{COLON_STRING}{RULES_STRING}";

            if (ConfigCache.IsRedisOn())
            {
                var value = Read(cacheKey); // Your existing Redis reader
                if (!value.IsNullOrEmpty)
                {
                    return JsonConvert.DeserializeObject<List<AttachmentRuleModel>>(value.ToString());
                }
            }
            else
            {
                var httpContext = new HttpContextAccessor().HttpContext;

                if (httpContext != null)
                {
                    var memoryCache = httpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;

                    if (memoryCache != null && memoryCache.TryGetValue(cacheKey, out List<AttachmentRuleModel> cachedRules))
                    {
                        return cachedRules;
                    }
                }

                return null;
            }

            return null;
        }

        /// <summary>
        /// Sets the files attachment rules in the cache.
        /// </summary>
        /// <param name="attachmentRuleModels">Attachments rules list to set.</param>
        public void SetFileAttachmentRules(List<AttachmentRuleModel> attachmentRuleModels)
        {
            if (configCacheOnOff == "0")
            {
                return;
            }

            var expirationHours = ConfigCache.GetFileAttachmentsRulesExpiration();
            string cacheKey = $"{FILES_STRING}{COLON_STRING}{RULES_STRING}";

            if (ConfigCache.IsRedisOn())
            {
                var objToStore = JsonConvert.SerializeObject(attachmentRuleModels);
                Write(cacheKey, objToStore, expirationHours); // Assumes Redis `Write()` is implemented
            }
            else
            {
                var httpContext = new HttpContextAccessor().HttpContext;

                if (httpContext != null)
                {
                    var memoryCache = httpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;

                    if (memoryCache != null)
                    {
                        memoryCache.Set(cacheKey, attachmentRuleModels, new MemoryCacheEntryOptions
                        {
                            AbsoluteExpiration = DateTimeOffset.Now.AddHours(expirationHours),
                            Priority = CacheItemPriority.Normal
                        });
                    }
                }
            }
        }
    }
}