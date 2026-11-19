using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TeachersAPI.Domain.Models.Events;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;

namespace TeachersAPI.Utils
{
    /// <summary>
    /// Handles the Microsoft Graph calendar sync exclusively for the Events module. Deliberately
    /// self-contained: does not reference GraphAPI.cs (used by Planning/TeacherGroup) in any way,
    /// so changes here can never affect that existing integration. Some logic (token acquisition,
    /// attendees JSON, domain validation) is intentionally duplicated rather than shared.
    /// Strategy: one event in the organizer's calendar, with the resolved audience as attendees
    /// (no per-user events, no online meetings, no shared calendar).
    /// </summary>
    public class GraphEventsAPI
    {
        private static readonly string GraphApiEndpoint = AppConfiguration.Configuration["AppSettings:graph_api_endpoint"];

        /// <summary>
        /// Creates the event in Microsoft Graph, in the organizer's calendar, with the resolved
        /// audience as attendees.
        /// </summary>
        /// <param name="eventModel">The event to create.</param>
        /// <param name="attendees">The resolved audience.</param>
        /// <param name="organizerSession">Session info of the admin creating the event.</param>
        /// <returns>The Graph event id, or null if the organizer's email is not valid for the configured tenant.</returns>
        public static async Task<string> CreateEventAsync(EventModel eventModel, List<UserResponse> attendees, EventOrganizerModel organizerSession)
        {
            if (!IsValidEmailForDomain(organizerSession.Email, AppConfiguration.Configuration["AppSettings:azure_email_domain"]))
            {
                return null;
            }

            string accessToken = await GetAccessTokenAsync();
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Failed to obtain a Graph access token.");
            }

            string attendeesJson = BuildAttendeesJson(attendees) ?? string.Empty;
            string eventJson = BuildEventJson(eventModel, organizerSession, attendeesJson);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsync($"{GraphApiEndpoint}/users/{organizerSession.Email}/events", new StringContent(eventJson, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to create Graph event for user {organizerSession.Email}. Status code: {response.StatusCode}. Response content: {responseContent}");
                }

                string createdEventContent = await response.Content.ReadAsStringAsync();
                dynamic createdEvent = JsonConvert.DeserializeObject(createdEventContent);
                return createdEvent.id;
            }
        }

        /// <summary>
        /// Updates the event in Microsoft Graph by its stored id.
        /// </summary>
        /// <param name="eventModel">The event to update. Must have TeamsEventId set.</param>
        /// <param name="attendees">The resolved audience.</param>
        /// <param name="organizerSession">Session info of the admin editing the event.</param>
        /// <returns>False if Graph responded that the event no longer exists (404). True on success.</returns>
        public static async Task<bool> UpdateEventAsync(EventModel eventModel, List<UserResponse> attendees, EventOrganizerModel organizerSession)
        {
            if (!IsValidEmailForDomain(organizerSession.Email, AppConfiguration.Configuration["AppSettings:azure_email_domain"]))
            {
                return false;
            }

            string accessToken = await GetAccessTokenAsync();
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Failed to obtain a Graph access token.");
            }

            string attendeesJson = BuildAttendeesJson(attendees) ?? string.Empty;
            string eventJson = BuildEventJson(eventModel, organizerSession, attendeesJson);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var patchRequest = new HttpRequestMessage(new HttpMethod("PATCH"), $"{GraphApiEndpoint}/users/{organizerSession.Email}/events/{eventModel.TeamsEventId}")
                {
                    Content = new StringContent(eventJson, Encoding.UTF8, "application/json")
                };
                HttpResponseMessage response = await client.SendAsync(patchRequest, CancellationToken.None);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }

                if (!response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to update Graph event '{eventModel.TeamsEventId}' for user {organizerSession.Email}. Status code: {response.StatusCode}. Response content: {responseContent}");
                }

                return true;
            }
        }

        /// <summary>
        /// Deletes the event in Microsoft Graph by its stored id.
        /// </summary>
        /// <param name="teamsEventId">The Graph event id to delete.</param>
        /// <param name="organizerEmail">The organizer's email (owner of the calendar).</param>
        /// <returns>Return a async task.</returns>
        public static async Task DeleteEventAsync(string teamsEventId, string organizerEmail)
        {
            if (!IsValidEmailForDomain(organizerEmail, AppConfiguration.Configuration["AppSettings:azure_email_domain"]))
            {
                return;
            }

            string accessToken = await GetAccessTokenAsync();
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception("Failed to obtain a Graph access token.");
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.DeleteAsync($"{GraphApiEndpoint}/users/{organizerEmail}/events/{teamsEventId}");

                if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception($"Failed to delete Graph event '{teamsEventId}' for user {organizerEmail}. Status code: {response.StatusCode}");
                }
            }
        }

        /// <summary>
        /// Builds the Graph event JSON body from an EventModel.
        /// </summary>
        private static string BuildEventJson(EventModel eventModel, EventOrganizerModel organizerSession, string attendeesJson)
        {
            // EndDate is optional at the domain level (a punctual event) — Graph still requires
            // an end, so fall back to StartDate, same convention as EventStatusCalculator.
            string effectiveEndDate = string.IsNullOrEmpty(eventModel.EndDate) ? eventModel.StartDate : eventModel.EndDate;

            return $@"
                    {{
                        ""subject"": ""{eventModel.Title}"",
                        ""body"": {{
                            ""contentType"": ""HTML"",
                            ""content"": ""{eventModel.Description}""
                        }},
                        ""start"": {{
                            ""dateTime"": ""{eventModel.StartDate}"",
                            ""timeZone"": ""{eventModel.TimeZoneCode}""
                        }},
                        ""end"": {{
                            ""dateTime"": ""{effectiveEndDate}"",
                            ""timeZone"": ""{eventModel.TimeZoneCode}""
                        }},
                        ""location"": {{
                            ""displayName"": ""{eventModel.Location}""
                        }},
                        ""organizer"": {{
                            ""emailAddress"": {{
                              ""address"": ""{organizerSession.Email}"",
                              ""name"": ""{organizerSession.FullName}""
                            }}
                        }}
                        {(string.IsNullOrEmpty(attendeesJson) ? "" : $",\n\"attendees\": {attendeesJson}")}
                    }}";
        }

        /// <summary>
        /// Get access token to interact with graph API. Duplicated from GraphAPI.cs on purpose —
        /// see class-level remarks.
        /// </summary>
        private static async Task<string> GetAccessTokenAsync()
        {
            string tenantId = AppConfiguration.Configuration["AppSettings:azure_tenant_id"];
            string clientId = AppConfiguration.Configuration["AppSettings:azure_client_id"];
            string clientSecret = AppConfiguration.Configuration["AppSettings:azure_client_secret"];

            var client = new HttpClient();
            var tokenEndpoint = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default"),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            var response = await client.PostAsync(tokenEndpoint, requestContent);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                dynamic json = JsonConvert.DeserializeObject(result);
                return json.access_token;
            }

            return null;
        }

        /// <summary>
        /// Build attendees JSON array string. Duplicated from GraphAPI.cs on purpose — see
        /// class-level remarks.
        /// </summary>
        private static string BuildAttendeesJson(List<UserResponse> users)
        {
            if (users == null)
                return null;

            var attendees = new List<string>();

            foreach (UserResponse user in users)
            {
                string attendeeJson = $@"
                                        {{
                                            ""emailAddress"": {{
                                                ""address"": ""{user.Email}"",
                                                ""name"": ""{user.GivenName} {user.Surname}""
                                            }},
                                            ""type"": ""required""
                                        }}";

                attendees.Add(attendeeJson);
            }
            return $"[{string.Join(",", attendees)}]";
        }

        /// <summary>
        /// Checks if the user's email corresponds to the specified tenant. Duplicated from
        /// GraphAPI.cs on purpose — see class-level remarks.
        /// </summary>
        private static bool IsValidEmailForDomain(string email, string domainToCheck)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[a-zA-Z0-9._%+-]+@" + Regex.Escape(domainToCheck) + "$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }
    }
}
