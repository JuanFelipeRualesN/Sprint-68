using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.Generals.Responses;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using TeachersAPI;

namespace TeachersAPI.Utils
{
    /// <summary>
    /// Class to interact with the microsoft teams api.
    /// </summary>
    public class GraphAPI
    {
        /// <summary>
        /// Base graph api endpoint and version.
        /// </summary>
        private static readonly string GraphApiEndpoint = AppConfiguration.Configuration["AppSettings:graph_api_endpoint"];
        private static readonly string GraphGuidPlan = AppConfiguration.Configuration["AppSettings:graph_guid_plan"];
        private static readonly string GraphGuidGroup = AppConfiguration.Configuration["AppSettings:graph_guid_group"];

        /// <summary>
        /// Create an event in the Microsoft Teams calendar to all the indicated emails.
        /// </summary>
        /// <param name="users">List of users for whom the event will be created.</param>
        /// <param name="responseSession">Session with teacher info.</param>
        /// <param name="designation">The lesson or assignment that contains the event information.</param>
        /// <param name="planId">The Plan Id.</param>
        /// <param name="groupId">The Group Id.</param>
        /// <param name="exam">The exam object.</param>
        /// <param name="examNum">The exam number.</param>
        /// <returns></returns>
        public static async Task CreateEventAsync(List<UserResponse> users, ValidationResponseModel responseSession, PlanningAttachedDetailsModel designation, string planId, string groupId, TeacherPlanLessonQuizModel exam = null, int? examNum = null)
        {
            // Get de access token.
            string accessToken = await GetAccessTokenAsync();

            if (!string.IsNullOrEmpty(accessToken))
            {
                // Format date.
                string dueDateProperety = exam == null ? designation.DueDate : exam.DueDate;
                DateTime startDate = DateTime.ParseExact(dueDateProperety, "MM/dd/yyyy HH:mm:ss", null);

                // Set a default time (08:30:00 AM) for Assignement, TIME is always 08:30 AM 
                DateTime modifiedStartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 8, 30, 0);

                // Set due time.
                // In assignement, TIME is always 09:30 AM 
                DateTime dueDate = exam == null ? modifiedStartDate.AddHours(1) : startDate.AddMinutes(int.Parse(exam.Duration));

                // Convert to the desired format
                string format = "yyyy-MM-ddTHH:mm:ss";
                string dueDateString = dueDate.ToString(format);
                string startDateString = exam == null ? modifiedStartDate.ToString(format) : startDate.ToString(format);

                string timezone = exam == null ? AppConfiguration.Configuration["AppSettings:azure_timezone"] : exam.DueDateTimezone;

                // Build the attendees array dynamically in JSON string format
                string attendeesJson = BuildAttendeesJson(users) ?? string.Empty;

                // Create the event JSON string
                string eventJson = $@"
                                    {{
                                        ""subject"": ""{designation.Title}{(examNum == null ? "" : " - Exam " + examNum)}"",
                                        ""body"": {{
                                            ""contentType"": ""HTML"",
                                            ""content"": ""{designation.Title}""
                                        }},
                                        ""start"": {{
                                            ""dateTime"": ""{startDateString}"",
                                            ""timeZone"": ""{timezone}""
                                        }},
                                        ""end"": {{
                                            ""dateTime"": ""{dueDateString}"",
                                            ""timeZone"": ""{timezone}""
                                        }},
                                        ""location"": {{
                                            ""displayName"": ""Microsoft Teams Meeting""
                                        }},
                                        ""organizer"": {{
                                            ""emailAddress"": {{
                                              ""address"": ""{responseSession.Email}"",
                                              ""name"": ""{responseSession.FullName}""
                                            }}
                                        }}
                                        {(string.IsNullOrEmpty(attendeesJson) ? "" : $",\n\"attendees\": {attendeesJson}")},
                                        ""singleValueExtendedProperties"": [
                                            {{
                                                ""id"": ""String {GraphGuidPlan} Name PlanId"",
                                                ""value"": ""{planId}""
                                            }},
                                            {{
                                                ""id"": ""String {GraphGuidGroup} Name GroupId"",
                                                ""value"": ""{groupId}""
                                            }}
                                        ]
                                    }}";

                await CreateOrUpdateEventForUserAsync(accessToken, responseSession.Email, eventJson);
            }
        }

        /// <summary>
        /// Get access token to interact with graph API.
        /// </summary>
        /// <returns>The access token.</returns>
        private static async Task<string> GetAccessTokenAsync()
        {
            string TenantId = AppConfiguration.Configuration["AppSettings:azure_tenant_id"];
            string ClientId = AppConfiguration.Configuration["AppSettings:azure_client_id"];
            string ClientSecret = AppConfiguration.Configuration["AppSettings:azure_client_secret"];

            var client = new HttpClient();
            var tokenEndpoint = $"https://login.microsoftonline.com/{TenantId}/oauth2/v2.0/token";

            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
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
        /// Build attendees JSON array string.
        /// </summary>
        /// <param name="users">List of users.</param>
        /// <returns>Attendees json string.</returns>
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
        /// Create or update an event in the Teams calendar for the specified user.
        /// </summary>
        /// <param name="accessToken">Access token.</param>
        /// <param name="email">User email for whom the event is created.</param>
        /// <param name="eventJson">Request body.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static async Task CreateOrUpdateEventForUserAsync(string accessToken, string email, string eventJson)
        {
            if (!IsValidEmailForDomain(email, AppConfiguration.Configuration["AppSettings:azure_email_domain"]))
            {
                return;
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Get user's existing events
                HttpResponseMessage existingEventsResponse = await client.GetAsync($"{GraphApiEndpoint}/users/{email}/events");
                if (!existingEventsResponse.IsSuccessStatusCode)
                    throw new Exception($"Failed to retrieve existing events for user {email}. Status code: {existingEventsResponse.StatusCode}");

                string existingEventsContent = await existingEventsResponse.Content.ReadAsStringAsync();
                List<dynamic> existingEvents = JObject.Parse(existingEventsContent)["value"].ToObject<List<dynamic>>();

                // Check if any existing event has the same title as the new event
                bool eventExists = existingEvents.Any(e => e["subject"].ToString() == JObject.Parse(eventJson)["subject"].ToString());

                if (eventExists)
                {
                    // If event with the same title exists, update the event
                    dynamic existingEvent = existingEvents.First(e => e["subject"].ToString() == JObject.Parse(eventJson)["subject"].ToString());
                    dynamic eventId = existingEvent["id"].ToString();
                    HttpResponseMessage updateResponse = await client.PatchAsync($"{GraphApiEndpoint}/users/{email}/events/{eventId}", new StringContent(eventJson, Encoding.UTF8, "application/json"), new CancellationTokenSource().Token);

                    if (!updateResponse.IsSuccessStatusCode)
                    {
                        string updateResponseContent = await updateResponse.Content.ReadAsStringAsync();
                        throw new Exception($"Failed to update event for user {email}. Status code: {updateResponse.StatusCode}. Response content: {updateResponseContent}");
                    }
                }
                else
                {
                    // If no event with the same title exists, create a new event
                    HttpResponseMessage createResponse = await client.PostAsync($"{GraphApiEndpoint}/users/{email}/events", new StringContent(eventJson, Encoding.UTF8, "application/json"));

                    if (!createResponse.IsSuccessStatusCode)
                    {
                        string createResponseContent = await createResponse.Content.ReadAsStringAsync();
                        throw new Exception($"Failed to create event for user {email}. Status code: {createResponse.StatusCode}. Response content: {createResponseContent}");
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the user's email corresponds to the specified tenant.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <param name="domainToCheck">Domain to check.</param>
        /// <returns></returns>
        private static bool IsValidEmailForDomain(string email, string domainToCheck)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@" + Regex.Escape(domainToCheck) + "$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

        /// <summary>
        /// Delete an event in the Teams calendar for the specified user by title value.
        /// </summary>
        /// <param name="email">The User email.</param>
        /// <param name="planId">The Plan Id.</param>
        /// <param name="groupId">The Group Id.</param>
        /// <param name="eventTitle">Event title value.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task DeleteEventAsync(string email, string planId, string groupId, string eventTitle = null)
        {
            if (!IsValidEmailForDomain(email, AppConfiguration.Configuration["AppSettings:azure_email_domain"]))
            {
                return;
            }

            // Get de access token.
            string accessToken = await GetAccessTokenAsync();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Construct the request URL
                string requestUrl = $"{GraphApiEndpoint}/users/{email}/events";

                if (!string.IsNullOrEmpty(eventTitle))
                {
                    requestUrl += $"?$filter=subject eq '{eventTitle}'";
                }
                else
                {
                    requestUrl += $"?$filter=singleValueExtendedProperties/Any(ep: ep/id eq 'String {GraphGuidGroup} Name GroupId' and ep/value eq '{groupId}')";
                }

                requestUrl += $"&$expand=singleValueExtendedProperties($filter=id eq 'String {GraphGuidGroup} Name GroupId' or id eq 'String {GraphGuidPlan} Name PlanId')";

                // Send a GET request to retrieve the event with the specified title
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string eventDataContent = await response.Content.ReadAsStringAsync();
                    dynamic eventData = JsonConvert.DeserializeObject(eventDataContent);

                    foreach (var item in eventData.value)
                    {
                        bool finded = false;
                        foreach (var obj in item.singleValueExtendedProperties)
                        {
                            // If planId is null remove entire group plan calendar.
                            if ((obj.id == "String {" + GraphGuidPlan + "} Name PlanId" && obj.value == planId) || string.IsNullOrEmpty(planId))
                            {
                                finded = true;
                                break;
                            }
                        }

                        if (finded)
                        {
                            // If an event is found, delete it
                            string eventId = item.id;
                            HttpResponseMessage deleteResponse = await client.DeleteAsync($"{GraphApiEndpoint}/users/{email}/events/{eventId}");

                            if (!deleteResponse.IsSuccessStatusCode)
                                throw new Exception($"Failed to delete event with ID '{eventId}' for user {email}. Status code: {deleteResponse.StatusCode}");
                        }
                    }
                }
                else
                    throw new Exception($"Failed to retrieve events for user {email}. Status code: {response.StatusCode}");
            }
        }

        /// <summary>
        /// Patch an event in the Teams calendar for add or exclude specified user.
        /// </summary>
        /// <param name="email">The User email (Teacher).</param>
        /// <param name="student">The student.</param>
        /// <param name="groupId">The Group Id.</param>
        /// <param name="remove">Indicates whether the participant should be added or excluded.</param>
        /// <returns></returns>
        public static async Task PatchEventAsync(string email, UserResponse student, string groupId, bool remove = false)
        {
            if (!IsValidEmailForDomain(email, AppConfiguration.Configuration["AppSettings:azure_email_domain"]))
            {
                return;
            }

            // Get de access token.
            string accessToken = await GetAccessTokenAsync();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Construct the request URL
                string requestUrl = $"{GraphApiEndpoint}/users/{email}/events";

                requestUrl += $"?$filter=singleValueExtendedProperties/Any(ep: ep/id eq 'String {GraphGuidGroup} Name GroupId' and ep/value eq '{groupId}')" +
                              $"&$expand=singleValueExtendedProperties($filter=id eq 'String {GraphGuidGroup} Name GroupId' or id eq 'String {GraphGuidPlan} Name PlanId')";

                // Send a GET request to retrieve the event with the specified title
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to get the event data
                    string eventDataContent = await response.Content.ReadAsStringAsync();
                    dynamic eventData = JsonConvert.DeserializeObject(eventDataContent);

                    foreach (var item in eventData.value)
                    {
                        // If an event is found, add or remove the studentEmail as a participant
                        string eventId = item.id;
                        string patchRequestUrl = $"{GraphApiEndpoint}/users/{email}/events/{eventId}";

                        // Retrieve the existing attendees
                        var existingAttendees = new List<dynamic>(item.attendees) ?? new List<dynamic>();
                        var updatedAttendees = new List<dynamic>();

                        if (remove)
                        {
                            // Iterate through existing attendees and exclude studentEmail
                            foreach (var attendee in existingAttendees)
                            {
                                string existingEmail = attendee.emailAddress.address;

                                if (existingEmail != student.Email)
                                {
                                    updatedAttendees.Add(new
                                    {
                                        emailAddress = new
                                        {
                                            address = existingEmail
                                        }
                                    });
                                }
                            }
                        }
                        else
                        {
                            // Add the new student to the list of attendees
                            existingAttendees.Add(new
                            {
                                emailAddress = new
                                {
                                    name = $"{student.GivenName} {student.Surname}",
                                    address = student.Email
                                }
                            });
                        }

                        // Prepare the patch request body with updated attendees
                        string patchContent = $"{{ \"attendees\": {JsonConvert.SerializeObject(remove ? updatedAttendees : existingAttendees)} }}";

                        // Send the patch request to remove the student as a participant
                        HttpResponseMessage patchResponse = await client.PatchAsync(patchRequestUrl, new StringContent(patchContent, Encoding.UTF8, "application/json"), new CancellationTokenSource().Token);

                        if (!patchResponse.IsSuccessStatusCode)
                            throw new Exception($"Failed to add/remove={remove} participant {student.Email} from event with ID '{eventId}' for user {email}. Status code: {patchResponse.StatusCode}");
                    }
                }
                else
                    throw new Exception($"Failed to retrieve events for user {email}. Status code: {response.StatusCode}");
            }
        }
    }

    /// <summary>
    /// This class contains extension methods for HttpClient to support PATCH requests.
    /// </summary>
    public static class HttpClientExtention
    {
        /// <summary>
        /// Extension method for sending a PATCH request asynchronously with HttpClient.
        /// </summary>
        /// <param name="client">The HttpClient instance.</param>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <param name="content">The HTTP content to send.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            HttpResponseMessage response = await client.SendAsync(request, cancellationToken);
            return response;
        }
    }
}