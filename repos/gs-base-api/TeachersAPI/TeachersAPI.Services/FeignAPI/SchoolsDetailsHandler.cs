using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using TeachersAPI.Domain.Models.FeignAPI;
using System.Linq;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Services.FeignAPI
{
    /// <summary>
    /// To handle details related to schools.
    /// </summary>
    public static class SchoolsDetailsHandler
    {
        /// <summary>
        /// To get the academic periods filtered by an academic period.
        /// </summary>
        /// <param name="academicPeriodToFilter">Academic period to filter.</param>
        /// <returns>Returns a list of academic periods.</returns>
        public static List<AcademicPeriodModel> GetAcademicPeriodModels(AcademicPeriodModel academicPeriodToFilter)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:schools_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonPostModel = new List<AcademicPeriodModel>()
                {
                    academicPeriodToFilter
                };

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(jsonPostModel), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:schools_endpoint_call_academics"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    var academicPeriods = JsonConvert.DeserializeObject<List<AcademicPeriodModel>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return academicPeriods;
                }
                else
                {
                    return new List<AcademicPeriodModel>();
                }
            }
        }

        /// <summary>
        /// To get the academic periods filtered by an academic period.
        /// </summary>
        /// <param name="token">The user access token.</param>
        /// <param name="academicPeriodsToFilter">Academic period to filter.</param>
        /// <returns>Returns a list of academic periods.</returns>
        public static List<AcademicDetailRequestModel> GetAcademicPeriodsFullDetails(string token, List<string> academicPeriodsToFilter)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:schools_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(academicPeriodsToFilter), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:schools_endpoint_call_periods_details"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    var academicPeriods = JsonConvert.DeserializeObject<List<AcademicDetailRequestModel>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    foreach (var item in academicPeriods)
                    {
                        item.AcademicDetail = new Domain.Models.FeignAPI.AcademicDetailModel
                        {
                            Id = item.TermId,
                            TermId = item.TermId,
                            PeriodQuantity = item.PeriodQuantity.ToString(),
                            DaysToClosePeriod = item.DaysToClosePeriod.ToString(),
                            FullTermStartDate = item.FullTermStartDate,
                            FullTermEndDate = item.FullTermEndDate,
                            ModifiedBy = item.PeriodModifiedBy,
                            CreatedAt = item.PeriodCreatedDate,
                            UpdatedAt = item.PeriodUpdatedDate
                        };

                        item.AcademicPeriods.AddRange(academicPeriods.Select(s => new Domain.Models.FeignAPI.AcademicPeriodModel
                        {
                            Id = s.PeriodId,
                            EntityAcademicDetailId = s.EntityAcademicDetailId,
                            PeriodNumber = s.PeriodNumber.ToString(),
                            PeriodFrom = s.PeriodFrom,
                            PeriodTo = s.PeriodTo,
                            ModifiedBy = s.PeriodModifiedBy,
                            CreatedAt = s.PeriodCreatedDate,
                            UpdatedAt = s.PeriodUpdatedDate
                        }));
                    }

                    return academicPeriods;
                }
                else
                {
                    return new List<AcademicDetailRequestModel>();
                }
            }
        }


        /// <summary>
        /// To get the active academic years by school ids.
        /// </summary>
        /// <param name="token">The token of the user that is making the request.</param>
        /// <param name="schoolIds">The school ids.</param>
        /// <returns>Returns the list of academic years.</returns>
        public static List<string> GetActiveAcademicYearsIdsBySchoolIds(string token, List<string> schoolIds)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:schools_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(schoolIds), Encoding.UTF8, "application/json");
                var response = client.PostAsync(AppConfiguration.Configuration["AppSettings:schools_endpoint_call_active_academic_year_ids"], stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    var academicYears = JsonConvert.DeserializeObject<List<string>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return academicYears;
                }
                else
                {
                    return new List<string>();
                }
            }
        }

        /// <summary>
        /// To get the active academic years.
        /// </summary>
        /// <returns>Return a list of academic years.</returns>
        public static List<string> GetActiveAcademicYearsByAcademicYearsIds()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:schools_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-API-KEY", AppConfiguration.Configuration["AppSettings:schools_api_key"]);

                var response = client.GetAsync(AppConfiguration.Configuration["AppSettings:schools_endpoint_call_active_academic_year_ids_by_ids"]).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    var academicYears = JsonConvert.DeserializeObject<List<string>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return academicYears;
                }
                else
                {
                    return new List<string>();
                }
            }
        }

        /// <summary>
        /// To get the school ids that belong to a region (ORE). Used exclusively by
        /// EventAudienceResolver to translate a RegionId into the same SchoolIds shape already
        /// consumed by the School audience path.
        /// </summary>
        /// <param name="regionId">The region (ORE) id.</param>
        /// <param name="token">User token.</param>
        /// <returns>Returns the school ids belonging to the region.</returns>
        public static List<string> GetSchoolIdsForRegion(string regionId, string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:schools_web_service_url"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", token);

                var response = client.GetAsync(AppConfiguration.Configuration["AppSettings:schools_endpoint_ore_schools_ids"] + regionId + "/schools_ids").Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    List<string> schoolIds = JsonConvert.DeserializeObject<List<string>>(responseString,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    return schoolIds ?? new List<string>();
                }

                return new List<string>();
            }
        }

        /// <summary>
        /// Asynchronously retrieves school ORE (address) models for the specified list of school IDs via a GET request.
        /// </summary>
        /// <param name="schoolIds">A list of school identifiers.</param>
        /// <param name="token">The authorization token required by the web service.</param>
        /// <returns>A task containing a list of address models.</returns>
        public static async Task<List<AddressModel>> GetSchoolOresAsync(List<int> schoolIds, string token)
        {
            if (schoolIds == null || schoolIds.Count == 0)
            {
                return new List<AddressModel>();
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppConfiguration.Configuration["AppSettings:schools_web_service_url"]);
                client.DefaultRequestHeaders.Add("Token", token);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var queryParams = string.Join("&", schoolIds.Select(id => $"schoolIds={id}"));
                string endpoint = $"{AppConfiguration.Configuration["AppSettings:schools_endpoint_call_ores"]}?{queryParams}";

                var response = await client.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    var addressModels = JsonConvert.DeserializeObject<List<AddressModel>>(responseString,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    return addressModels ?? new List<AddressModel>();
                }
                else
                {
                    return new List<AddressModel>();
                }
            }
        }
    }
}