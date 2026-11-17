using GSWebAPI.CustomClasses.CacheHandler;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace GSWebAPI.GSWebAPI.Services.FeignAPIs
{
    /// <summary>
    /// Handles the address details in Address API.
    /// </summary>
    public class AddressDetailsHandler
    {
        /// <summary>
        /// Gets the standard name of the country.
        /// </summary>
        /// <param name="isos">ISOs list.</param>
        /// <param name="langCode">Language code.</param>
        /// <returns>Returns the standard name of the country.</returns>
        public List<ResponseISOModel> GetCountriesFullDetails(HttpContext context, List<string> isos, string langCode)
        {
            var addressApiCache = context.RequestServices.GetRequiredService<AddressAPICache>();
            try
            {
                var responseIsoModelList = addressApiCache.GetCountriesByISO(isos);

                // If iso list is equal to country able, then return the cache details.
                if (responseIsoModelList.Count == isos.Count)
                {
                    return responseIsoModelList;
                }
            }
            catch(Exception)
            {
                // Do nothing and continue to get from API.
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:adrs_api_base_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Accept-Language", langCode);
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(isos), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:adrs_endpoint_get_country_iso_by_iso_list"], stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    List<ResponseISOModel> responseIsos = JsonConvert.DeserializeObject<List<ResponseISOModel>>(
                        responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                        });

                    try
                    {
                        addressApiCache.SetCountriesByISO(responseIsos);
                    }
                    catch (Exception)
                    {
                        // Do nothing if cache fails.
                    }

                    return responseIsos;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}