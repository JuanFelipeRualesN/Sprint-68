using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests.APIs;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs.Schools;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.FeignAPIs
{
    /// <summary>
    /// To handle the school details in the SchoolAPI.
    /// </summary>
    public class SchoolsDetailsHandler
    {

        /// <summary>
        /// Returns the list of academic details according to subscription ids.
        /// </summary>
        /// <param name="subscriptionIds">The subscription's id list.</param>
        /// <returns>Returns a list of academic details.</returns>
        public static List<EntityAcademicDetailFullResponseModel> GetAcademicDetailsBySubscriptionIdList(List<string> subscriptionIds)
        {
            try
            {
                List<EntityAcademicDetailFullResponseModel> schoolDetails = new List<EntityAcademicDetailFullResponseModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:schools_api_base_url"]);
                    client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:sch_platform_id"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(subscriptionIds), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:schools_endpoint_academic_details"], stringContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseAPI = response.Content.ReadAsStringAsync().Result;

                        schoolDetails = JsonConvert.DeserializeObject<List<EntityAcademicDetailFullResponseModel>>(
                        responseAPI,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                        });

                        return schoolDetails;
                    }
                    else
                    {
                        return schoolDetails;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Relates the subscriptions to entities.
        /// </summary>
        /// <param name="token">The user token.</param>
        /// <param name="ipAddress">The Ip Address.</param>
        /// <param name="subscriptionRelateRequestModel">Subscription relate details.</param>
        public static void RelateSubscriptionToEntitiesSchools(string token, string ipAddress, SubscriptionRelateRequestModel subscriptionRelateRequestModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:schools_api_base_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:sch_platform_id"]);

                subscriptionRelateRequestModel.IpAddress = ipAddress;
                subscriptionRelateRequestModel.Token = token;
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(subscriptionRelateRequestModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:schools_endpoint_relate_school_subscription"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Ok, to debug
                }
            }
        }
    }
}