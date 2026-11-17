using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using UsersAPI.CustomClasses.CacheHandler;
using UsersAPI.UsersAPI.Domain.Models.Generals.Requests.APIs;
using UsersAPI.UsersAPI.Domain.Models.Generals.Responses.APIs;
using UsersAPI.UsersAPI.Domain.Models.Generals.Responses;
using System.Reflection;
using Dapper;
using UsersAPI.UsersAPI.Domain.Models.PersonalDetails;
using MySqlConnector;
using System.CodeDom;

namespace UsersAPI.UsersAPI.Services.FeignAPIs
{
    /// <summary>
    /// To handle subscription details in Subscription's API.
    /// </summary>
    public static class SubscriptionDetailsHandler
    {
        public static string AttachProfileToSubscriptionEmergency(string subscriptionId, string profileId)
        {
            string messsage = "Profile subscription: ";
            string query = @"
              SELECT profiles_quantity FROM gs_subscription_api_db_prod.subst_plans pl 
 INNER JOIN `gs_subscription_api_db_prod`.`subst_subscriptions` sub ON sub.plan_platform_id = pl.platform_id
            WHERE sub.id = @subscriptionId; ";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@subscriptionId", subscriptionId, System.Data.DbType.String, System.Data.ParameterDirection.Input);

            MySqlConnection connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"]
            };

            connection.Open();
            int profilesQuantity = 0;
            try
            {
                  profilesQuantity = connection.QuerySingleOrDefault<int>(query, parameters);
            }
            catch (Exception ex)
            {
                messsage += ex.ToString() + "\n";
            }
            finally
            {
                  connection.Close();
            }

            // Read the profiles related to the subscription.
            query = @"SELECT COUNT(*) as total 
FROM gs_subscription_api_db_prod.subst_subscription_profiles
 WHERE subscription_id = @subscriptionId
 GROUP BY subscription_id";

            parameters = new DynamicParameters();
            parameters.Add("@subscriptionId", subscriptionId, System.Data.DbType.String, System.Data.ParameterDirection.Input);

            connection = new MySqlConnection
            {
                ConnectionString = AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"]
            };

            connection.Open();
            int totalProfiles = 0;
            try
            {
                totalProfiles = connection.QuerySingleOrDefault<int>(query, parameters);
            }
            catch (Exception ex)
            {
                messsage += ex.ToString() + "\n";
            }
            finally
            {
                connection.Close();
            }

            if (totalProfiles < profilesQuantity)
            {
                // Insert the profile in the process.
                query = @"INSERT INTO gs_subscription_api_db_prod.subst_subscription_profiles (subscription_id, profile_id)";
                query += " VALUES (@subscriptionId, @profileId)";
                parameters = new DynamicParameters();
                parameters.Add("@subscriptionId", subscriptionId, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                parameters.Add("@profileId", profileId, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                connection = new MySqlConnection
                {
                    ConnectionString = AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"]
                };
                connection.Open();
                try
                {
                    connection.Execute(query, parameters);
                }
                catch (Exception ex)
                {
                    messsage += ex.ToString() + "\n";
                }
                finally
                {
                    connection.Close();
                }


            }
            return messsage;
        }

        /// <summary>
        /// Returns the profile list.
        /// </summary>
        /// <param name="userToken">User token.</param>
        /// <param name="langCode">The language code.</param>
        /// <returns>Return a list of subscription profiles.</returns>
        public static List<SubscriptionProfileModel> GetProfilesOfLastSubscription(string userToken, string langCode)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:subscriptions_api_base_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", userToken);
                client.DefaultRequestHeaders.Add("Accept-Language", langCode);


                var response = client.GetAsync(AppConfiguration.Configuration["AppSettings:subscriptions_endpoint_profiles_last_subscription"]).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    List<SubscriptionProfileModel> modelObject = JsonConvert.DeserializeObject<List<SubscriptionProfileModel>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return modelObject;
                }
                else
                {
                    return new List<SubscriptionProfileModel>();
                }
            }
        }

        /// <summary>
        /// Returns the subscriptions.
        /// </summary>
        /// <param name="token">User token.</param>
        ///  <param name="subscriptionId">Subscription Id.</param>
        /// <param name="langCode">The language code</param>
        /// <returns>Returns the subscription details.</returns>
        public static SubscriptionResponseModel GetSubscriptionDetails(string token, string ipAddress, string subscriptionId, string langCode)
        { 
            // If not, read from the api. 
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:subscriptions_api_base_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);
                client.DefaultRequestHeaders.Add("Accept-Language", langCode);

                SubscriptionRequestModel jsonPostModel = new SubscriptionRequestModel
                {
                    Token = token,
                    IpAddress = ipAddress,
                    Subscription = new Subscription()
                    {
                        SubscriptionId = subscriptionId,
                    },
                };
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(jsonPostModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:subscriptions_endpoint_get_subscription_details_client"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    SubscriptionResponseModel modelObject = JsonConvert.DeserializeObject<SubscriptionResponseModel>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    return modelObject;
                }
                else
                {
                    return new SubscriptionResponseModel();
                }
            }
        }

        /// <summary>
        /// To create the customer in Stripe.
        /// </summary>
        /// <param name="ipAddress">IP Address.</param>
        /// <param name="langCode">Language Code.</param>
        /// <param name="email">Customer Email.</param>
        /// <param name="userId">The User Id.</param>
        /// <param name="stripeRequired">If stripe is required, set true, else, false.</param>
        /// <returns>Returns the customer platform id.</returns>
        public static string CreateCustomer(string ipAddress, string langCode, string email, string userId, bool stripeRequired)
        {
            using (var client = new HttpClient())
            {
                var baseUrl = AppConfiguration.Configuration["AppSettings:subscriptions_api_base_url"];
                var endpoint = AppConfiguration.Configuration["AppSettings:subscriptions_endpoint_create_customer"];

                // Append the query parameter `stripeRequired`
                string requestUrl = $"{baseUrl.TrimEnd('/')}/{endpoint.TrimStart('?')}?stripeRequired={stripeRequired.ToString().ToLower()}";

                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Accept-Language", langCode);

                var jsonPostModel = new CustomerRequestModel
                {
                    IpAddress = ipAddress,
                    Email = email,
                    UserId = userId
                };

                var stringContent = new StringContent(
                    JsonConvert.SerializeObject(jsonPostModel),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = client.PostAsync(requestUrl, stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    string modelObject = JsonConvert.DeserializeObject<string>(
                        responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return modelObject;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// To Attach the profile to the subscription.
        /// </summary>
        /// <param name="ipAddress">The IP Address.</param>
        /// <param name="studentRoleId">The student role id. Needed in the Subscription API when evaluating if discount only the students.</param>
        /// <param name="profileId">Profile Id.</param>
        /// <param name="profileRoleId">Profile Role Id.</param>
        /// <param name="subscriptionId">The Subscription Id.</param>
        /// <param name="langCode">Language code.</param>
        /// <param name="token">User session token.</param>
        /// <returns>Returns a General Response with transaction details.</returns>
        public static GeneralResponseModel AttachProfileToSubscription(string ipAddress, string studentRoleId, string profileId, string profileRoleId, string subscriptionId, string langCode, string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:subscriptions_api_base_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Accept-Language", langCode);
                client.DefaultRequestHeaders.Add("Token", token);

                ProfileAssignProductRequestModel jsonPostModel = new ProfileAssignProductRequestModel
                {
                    ProfileId = profileId,
                    IpAddress = ipAddress,
                    StudentRoleId = studentRoleId,
                    SubscriptionId = subscriptionId,
                    RoleId = profileRoleId,

                };

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(jsonPostModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:subscriptions_endpoint_attach_profile"], stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    GeneralResponseModel modelObject = JsonConvert.DeserializeObject<GeneralResponseModel>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return modelObject;
                }
                else
                {
                    try
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;

                        GeneralResponseModel modelObject = JsonConvert.DeserializeObject<GeneralResponseModel>(responseString,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

                        return modelObject;
                    }
                    catch (Exception)
                    {
                        return new GeneralResponseModel();
                    }
                }
            }
        }

        /// <summary>
        /// To Attach multiple profiles to the subscription.
        /// </summary>
        /// <param name="ipAddress">The IP Address.</param>
        /// <param name="profileIds">Profile Ids list.</param>
        /// <param name="subscriptionId">The Subscription Id.</param>
        /// <param name="langCode">Language code.</param>
        /// <param name="token">User Session Token.</param>
        /// <returns>Returns a General Response with transaction details.</returns>
        public static GeneralResponseModel AttachProfilesToSubscription(string ipAddress, List<string> profileIds, string subscriptionId, string langCode, string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:subscriptions_api_base_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Accept-Language", langCode);
                client.DefaultRequestHeaders.Add("Token", token);

                ProfileAssignProductRequestModel jsonPostModel = new ProfileAssignProductRequestModel
                {
                    ProfileIds = profileIds,
                    IpAddress = ipAddress,
                    SubscriptionId = subscriptionId,
                };

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(jsonPostModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:subscriptions_endpoint_attach_profiles"], stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    GeneralResponseModel modelObject = JsonConvert.DeserializeObject<GeneralResponseModel>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return modelObject;
                }
                else
                {
                    return new GeneralResponseModel();
                }
            }
        }

        /// <summary>
        /// Returns the subscription profiles.
        /// </summary>
        /// <param name="ipAddress">The IP Address.</param>
        /// <param name="langCode">Language code.</param>
        /// <param name="token">The user Token.</param>
        /// <param name="subscriptionIds">Subscription Ids list.</param>
        /// <returns>Returns a subscription profile model list.</returns>
        public static List<SubscriptionProfileModel> GetSubscriptionsProfileModels(string ipAddress, string langCode, string token, List<string> subscriptionIds)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:subscriptions_api_base_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);
                client.DefaultRequestHeaders.Add("Accept-Language", langCode);

                SubscriptionRequestModel jsonPostModel = new SubscriptionRequestModel
                {
                    Token = token,
                    IpAddress = ipAddress,
                    SubscriptionIds = subscriptionIds,
                };
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(jsonPostModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:subscriptions_endpoint_get_subscription_profile_models"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    List<SubscriptionProfileModel> modelObject = JsonConvert.DeserializeObject<List<SubscriptionProfileModel>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return modelObject;
                }
                else
                {
                    return new List<SubscriptionProfileModel>();
                }
            }
        }

        /// <summary>
        /// To check if the student is a homeschool user.
        /// </summary>
        /// <param name="token">The user's token.</param>
        /// <returns>Returns true if homeschool, else, false.</returns>
        public static bool CheckIStudentIsHomeschool(string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:subscriptions_api_base_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                var response = client.GetAsync(AppConfiguration.Configuration["AppSettings:subs_endpoint_user_homeschool"]).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    return JsonConvert.DeserializeObject<bool>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Commercial: To create the client with the subscription in Stripe.
        /// </summary>
        /// <param name="clientSubscriptionRegisterRequestModel">The client details.</param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        public static SubscriptionResponseModel CreateClientAndSubscriptionInStripe(ClientSubscriptionRegisterRequestModel clientSubscriptionRegisterRequestModel, string langCode)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:subscriptions_api_base_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Accept-Language", langCode);

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(clientSubscriptionRegisterRequestModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:subscriptions_endpoint_create_client"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    var modelObject = JsonConvert.DeserializeObject<SubscriptionResponseModel>(responseString,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                        });
                   
                    return modelObject;
                }
                else
                {
                    try
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        var modelObject = JsonConvert.DeserializeObject<SubscriptionResponseModel>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                       

                        return modelObject;
                    }
                    catch (Exception ex)
                    {
                        return new SubscriptionResponseModel();
                    }
                }
            }
        }

        /// <summary>
        /// Commercial: Validates the coupon by platform id.
        /// </summary>
        /// <param name="couponPlatformId">Coupon Platform Id.</param>
        /// <param name="langCode">Language code.</param>
        /// <returns>Returns a General Response with properties as if the coupon is valid and the reason if not.</returns>
        public static CouponResponseModel ValidateCouponByPlatform(string couponPlatformId, string langCode)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:subscriptions_api_base_url"]);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Accept-Language", langCode);

                var response = client.GetAsync(AppConfiguration.Configuration["AppSettings:subscriptions_endpoint_validate_coupon_by_platform"] + couponPlatformId).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    var modelObject = JsonConvert.DeserializeObject<CouponResponseModel>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return modelObject;
                }
                else
                {
                    return new CouponResponseModel();
                }
            }
        }
    }
}