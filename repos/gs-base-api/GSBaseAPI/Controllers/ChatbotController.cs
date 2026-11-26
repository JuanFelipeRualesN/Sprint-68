using GSBaseAPI.Domain.DTOs.Chatbot;
using GSBaseAPI.Services.FeignAPI;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Crmf;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;

namespace GSBaseAPI.Controllers
{
    [ApiController]
    [Route("baseapi/chatbot")]
    public class ChatbotController : ControllerBase
    {
        /// <summary>
        /// Call the orchestrator function that makes the RAG proccess.
        /// The orchestrator answers with SSE.
        /// </summary>
        /// <returns>The orchestrator SSE stream</returns>
        [HttpPost("ask")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequestDto request)
        {
            // Validate the user session
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");

            if (responseSession == null || responseSession.SessionValid == false)
            {
                return Unauthorized("Invalid or expired session.");
            }

            string clientPrincipalId = HttpContext.Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"].FirstOrDefault();
            string clientPrincipalName = HttpContext.Request.Headers["X-MS-CLIENT-PRINCIPAL-NAME"].FirstOrDefault();

            var orcKey = AppConfiguration.Configuration["AppSettings:AzureOrcKey"];
            var orcEndpoint = AppConfiguration.Configuration["AppSettings:azure_orc_endpoint"];

            using var httpClient = new HttpClient();

            bool isSearchIncluded = request.Module?.ToLower() != "assignments";

            var payload = JsonSerializer.Serialize(new
            {
                conversation_id = request.ConversationId,
                question = request.Query,
                module = request.Module,
                profile = request.Profile,
                client_principal_id = clientPrincipalId,
                client_principal_name =  clientPrincipalName,
                is_search_included = isSearchIncluded,
                detailed_planning = request.DetailedPlanning
            });

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, orcEndpoint)
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json")
            };
            requestMessage.Headers.Add("X-API-KEY", orcKey);
            requestMessage.Headers.Add("Token", responseSession.Token);

            var cancellationToken = HttpContext.RequestAborted;

            HttpResponseMessage response;
            try
            {
                // Call the Azure orchestration endpoint
                response = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                return StatusCode(503, $"Error connection with RAG service: {ex.Message}");
            }

            using (response)
            {
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    return StatusCode((int)response.StatusCode, errorContent);
                }

                HttpContext.Features.Get<IHttpResponseBodyFeature>()?.DisableBuffering();
                Response.ContentType = "text/event-stream";
                Response.Headers.CacheControl = "no-cache, no-transform";
                Response.Headers["X-Accel-Buffering"] = "no";

                try
                {
                    await using var upstream = await response.Content.ReadAsStreamAsync(cancellationToken);
                    var buffer = new byte[4096];
                    int read;
                    while ((read = await upstream.ReadAsync(buffer, cancellationToken)) > 0)
                    {
                        await Response.Body.WriteAsync(buffer.AsMemory(0, read), cancellationToken);
                        await Response.Body.FlushAsync(cancellationToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    // The client left; the orchestrator still saves the conversation on its side.
                }
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Call the orchestrator function for planning suggestions.
        /// </summary>
        /// <returns>An object with the chatbot response</returns>
        [HttpPost("planning/suggestions")]
        public async Task<IActionResult> GetPlanningSuggestions([FromBody] SuggestionsRequestDto request)
        {
            // Validate the user session
            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");

            if (responseSession == null || responseSession.SessionValid == false)
            {
                return Unauthorized("Invalid or expired session.");
            }

            var suggestionsKey = AppConfiguration.Configuration["AppSettings:azure_suggestions_key"];
            var suggestionsEndpoint = AppConfiguration.Configuration["AppSettings:azure_suggestions_endpoint"];

            using var httpClient = new HttpClient();

            var payload = JsonSerializer.Serialize(new
            {
                field_name = request.FieldName,
                user_input = request.UserInput,
                planning_context = new
                {
                    subject = request.PlanningContext.Subject,
                    grade_code = request.PlanningContext.GradeCode
                }
            });

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", suggestionsKey);

            HttpResponseMessage response;
            try
            {
                // Call the Azure orchestration endpoint
                response = await httpClient.PostAsync(suggestionsEndpoint, content);
            }
            catch (Exception ex)
            {
                return StatusCode(503, $"Error connection with RAG service: {ex.Message}");
            }

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return Content(jsonResponse, "application/json");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorContent);
            }
        }
    }
}
