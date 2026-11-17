using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using UsersAPI.CustomClasses.CacheHandler;

namespace UsersAPI.UsersAPI.Services.FeignAPIs
{
    /// <summary>
    /// To handle details related to school in the School API.
    /// </summary>
    public class SchoolDetailsHandler
    {
        private readonly ExternalAPIsCache _externalAPIsCache;

        public SchoolDetailsHandler(ExternalAPIsCache externalAPIsCache = null)
        {
            _externalAPIsCache = externalAPIsCache;
        }

        /// <summary>
        /// To get the ore schools ids.
        /// </summary>
        /// <param name="oreId">The ore id.</param>
        /// <returns>Return the list of school ids by ore id.</returns>
        public static List<string> GetORESchoolIds(string oreId)
        {
            // First, check the cache.
            try
            {
                // If cache is null, then call the school api.
                List<string> schoolIds = new List<string>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:school_api_base_url"]);
                    client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:sch_platform_id"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string endpoint = AppConfiguration.Configuration["AppSettings:schools_get_ore_schools_ids"];
                    endpoint = endpoint.Replace("{oreId}", oreId);
                    var response = client.GetAsync(endpoint).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseAPI = response.Content.ReadAsStringAsync().Result;

                        schoolIds = JsonConvert.DeserializeObject<List<string>>(
                        responseAPI,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                        });

                        return schoolIds;
                    }
                    else
                    {
                        return schoolIds;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Returns the subscription school.
        /// </summary>
        /// <param name="subscriptionId">The id of the Subscription.</param>
        public string GetSchoolIdOfSubscription(string subscriptionId)
        {
            // First, check the cache. TODO: Return cache
            /*string cacheValue = _externalAPIsCache.GetSchoolIdOfSubscription(subscriptionId);
            if (!string.IsNullOrWhiteSpace(cacheValue))
            {
                return cacheValue;
            } */

            try
            {
                // If cache is null, then call the school api.
                string schoolId = string.Empty;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:school_api_base_url"]);
                    client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:sch_platform_id"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(AppConfiguration.Configuration["AppSettings:schools_get_school_id_by_subscription_endpoint"] + subscriptionId).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseAPI = response.Content.ReadAsStringAsync().Result;

                        schoolId = JsonConvert.DeserializeObject<string>(
                        responseAPI,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                        });

                        // TODO: Return cache _externalAPIsCache.SetSchoolIdOfSubscription(subscriptionId, schoolId);

                        return schoolId;
                    }
                    else
                    {
                        return schoolId;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Returns the entity general details.
        /// </summary>
        /// <param name="entityIds">Entities' ids list.</param>
        /// <returns>Returns the entity details response list.</returns>
        public  List<Domain.Models.Generals.Responses.APIs.EntityGeneralDetailsResponseModel> GetEntityGeneralDetails(List<string> entityIds)
        {
            // First, check the cache.
            /* TODO: Return cache
            var cacheValue = _externalAPIsCache.GetEntityGeneralDetails(entityIds);
            if (cacheValue != null && entityIds.Count == cacheValue.Count)
            {
                return cacheValue; 
            }

            // Delete from entity Ids to call if exists in cache, so we do not have to call them twice.
            if (cacheValue != null && cacheValue.Count > 0)
            {
                foreach (Domain.Models.Generals.Responses.APIs.EntityGeneralDetailsResponseModel entityGeneralDetailsResponse in cacheValue)
                {
                    entityIds.RemoveAll(x => x == entityGeneralDetailsResponse.EntityId);
                }
            }*/

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:school_api_base_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:sch_platform_id"]);

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(entityIds), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:schools_get_school_details"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {

                    string responseString = response.Content.ReadAsStringAsync().Result;

                    List<Domain.Models.Generals.Responses.APIs.EntityGeneralDetailsResponseModel> modelObject = JsonConvert.DeserializeObject<List<Domain.Models.Generals.Responses.APIs.EntityGeneralDetailsResponseModel>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    // Add them to the cache
                    // TODO: Return cache: _externalAPIsCache.SetEntityGeneralDetails(modelObject);

                    return modelObject;
                }
                else
                {
                    return new List<Domain.Models.Generals.Responses.APIs.EntityGeneralDetailsResponseModel>();
                }
            }
        }
    }
}