using System;
using System.Collections.Generic;
using TeachersAPI.Domain.Models.Generals;
using Newtonsoft.Json;
using CacheHandlerLibrary;

namespace TeachersAPI.CustomClasses.CacheHandler
{
    /// <summary>
    /// To handle cache related to files.
    /// </summary>
    public class FilesCache : DefaultCache
    {
        private const string FILES_STRING = "filesta";
        private const string COLON_STRING = ":";
        private const string RULES_STRING = "rules";

        /// <summary>
        /// To get the file attachment rules from Cache.
        /// </summary>
        /// <returns>Returns the list of attachment rule models..</returns>
        public List<SupportedFileModel> GetFileAttachmentRules()
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
            {
                return null;
            }

            if (!ConfigCache.IsDebugMode())
            {
                var value = Read($"{FILES_STRING}{COLON_STRING}{RULES_STRING}");
                if (!value.IsNullOrEmpty)
                    return JsonConvert.DeserializeObject<List<SupportedFileModel>>(value.ToString());
            }
           

            return null;
        }

        /// <summary>
        /// Sets the files attachment rules in the cache.
        /// </summary>
        /// <param name="attachmentRuleModels">Attachments rules list to set.</param>
        public void SetFileAttachmentRules(List<SupportedFileModel> attachmentRuleModels)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
            {
                return;
            }

            var expirationHours = ConfigCache.GetFileAttachmentsRulesExpiration();

            if (!ConfigCache.IsDebugMode())
            {
                var obtToStore = JsonConvert.SerializeObject(attachmentRuleModels);
                Write($"{FILES_STRING}{COLON_STRING}{RULES_STRING}", obtToStore, expirationHours);
            }
           
        }
    }
}