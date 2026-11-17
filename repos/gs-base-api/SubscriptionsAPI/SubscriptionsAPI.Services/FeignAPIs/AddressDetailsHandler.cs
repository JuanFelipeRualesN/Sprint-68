using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.FeignAPIs
{
    /// <summary>
    /// Handles address details.
    /// </summary>
    public class AddressDetailsHandler
    {

        /// <summary>
        /// Gets the standard name of the country.
        /// </summary>
        /// <param name="isoCode">ISO code.</param>
        /// <returns>Returns the standard name of the country.</returns>
        public static string GetCountryIsoByISO2(string isoCode)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:adrs_api_base_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(AppConfiguration.Configuration["AppSettings:adrs_endpoint_get_country_iso_2_by_iso_1"] + isoCode).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<string>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                }
                else
                {
                    return null;
                }
            }
        }
    }
}