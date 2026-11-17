using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using UsersAPI.CustomClasses.CacheHandler;

namespace UsersAPI.UsersAPI.Services.FeignAPIs
{
    /// <summary>
    /// Handles address details in Address API.
    /// </summary>
    public class AddressDetailsHandler
    {
        // TODO: Return cache

        /// <summary>
        /// Gets the standard name of the country.
        /// </summary>
        /// <param name="isoCode">ISO code.</param>
        /// <returns>Returns the standard name of the country.</returns>
        public string GetCountryStandardName(string isoCode)
        {
            /// TODO: Return cache
            // If there is nothing in the cache, call the api.
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:adrs_api_base_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(AppConfiguration.Configuration["AppSettings:adrs_endpoint_get_country_iso"] + isoCode).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    string country = JsonConvert.DeserializeObject<string>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    // Store in the cache.
                    return country;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}