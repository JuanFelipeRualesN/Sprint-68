using DataAnalysisAPI.Controllers;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAnalysisAPI.DataAnalysisAPI.Services
{
    public class PowerBI
    {
        private readonly string _tenantId;
        private readonly string _clientId;
        private readonly string _clientSecret;

        public PowerBI(string tenantId, string clientId, string clientSecret)
        {
            _tenantId = tenantId;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        /// <summary>
        /// Embeds the Power BI report and generates an embed token.
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="workspaceId"></param>
        /// <param name="tenantId"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="oreId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<PowerBIModel>> EmbedPowerBIReport(string reportId, string workspaceId, int? oreId = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (string.IsNullOrEmpty(_tenantId) || string.IsNullOrEmpty(_clientId) || string.IsNullOrEmpty(_clientSecret))
                    {
                        return ServiceResult<PowerBIModel>.Fail("Power BI configuration is missing.");
                    }

                    var authorityUrl = $"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/token";

                    var authRequestBody = new Dictionary<string, string>
                    {
                        { "grant_type", "client_credentials" },
                        { "client_id", _clientId },
                        { "client_secret", _clientSecret },
                        { "scope", "https://analysis.windows.net/powerbi/api/.default" }
                    };

                    var authResponse = await client.PostAsync(authorityUrl, new FormUrlEncodedContent(authRequestBody));

                    if (!authResponse.IsSuccessStatusCode)
                    {
                        var errorContent = await authResponse.Content.ReadAsStringAsync();
                        return ServiceResult<PowerBIModel>.Fail($"Power BI Authentication Error: {errorContent}");
                    }

                    var authContent = await authResponse.Content.ReadAsStringAsync();
                    using var authJson = JsonDocument.Parse(authContent);
                    string? accessToken = authJson.RootElement.GetProperty("access_token").GetString();

                    var pbiUrl = $"https://api.powerbi.com/v1.0/myorg/groups/{workspaceId}/reports/{reportId}/GenerateToken";

                    var embedRequest = new HttpRequestMessage(HttpMethod.Post, pbiUrl);
                    embedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var datasetId = await GetDatasetIdFromReport(workspaceId, reportId, accessToken);
                    if (oreId != null)
                    {
                        var identity = new
                        {
                            username = "ORE",
                            roles = new[] { "Admin_ORE_Filter" },
                            datasets = new[] { datasetId }
                        };

                        embedRequest.Content = new StringContent(
                            JsonSerializer.Serialize(new { accessLevel = "View", allowSaveAs = false, identities = new[] { identity } }),
                            System.Text.Encoding.UTF8,
                            "application/json"
                        );
                    }
                    else
                    {
                        var identity = new
                        {
                            username = "AdminUser",
                            roles = new[] { "ViewAll" },
                            datasets = new[] { datasetId }
                        };

                        embedRequest.Content = new StringContent(
                            JsonSerializer.Serialize(new { accessLevel = "View", allowSaveAs = false, identities = new[] { identity } }),
                            System.Text.Encoding.UTF8,
                            "application/json"
                        );
                    }
                    var embedResponse = await client.SendAsync(embedRequest);

                    if (!embedResponse.IsSuccessStatusCode)
                    {
                        var errorContent = await embedResponse.Content.ReadAsStringAsync();

                        if (embedResponse.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            return ServiceResult<PowerBIModel>.Fail("Power BI Token Error: 403 Forbidden. Please check the logs for more details.");
                        }

                        return ServiceResult<PowerBIModel>.Fail($"Power BI Token Error: {errorContent}");
                    }

                    var embedContent = await embedResponse.Content.ReadAsStringAsync();

                    using var embedJson = JsonDocument.Parse(embedContent);
                    string? tokenString = embedJson.RootElement.GetProperty("token").GetString();

                    if (string.IsNullOrEmpty(tokenString))
                    {
                        return ServiceResult<PowerBIModel>.Fail("Failed to retrieve embed token from Power BI response.");
                    }

                    var response = new PowerBIModel
                    {
                        embedToken = tokenString,
                        reportId = reportId,
                        embedUrl = $"https://app.powerbi.com/reportEmbed?reportId={reportId}&groupId={workspaceId}"
                    };

                    return ServiceResult<PowerBIModel>.Ok(response);
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<PowerBIModel>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<string> GetDatasetIdFromReport(string workspaceId, string reportId, string accessToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var url = $"https://api.powerbi.com/v1.0/myorg/groups/{workspaceId}/reports/{reportId}";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to get Report Details. Status: {response.StatusCode}. Error: {error}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(content);

                if (json.RootElement.TryGetProperty("datasetId", out var dsElement))
                {
                    var dsId = dsElement.GetString();
                    if (!string.IsNullOrEmpty(dsId))
                    {
                        return dsId;
                    }
                }

                throw new Exception($"The report {reportId} does not have a valid 'datasetId'.");
            }
        }
    }

    public class ServiceResult<T>
    {
        public bool Success { get; set; }

        public T Data { get; set; }

        public string ErrorMessage { get; set; }

        public static ServiceResult<T> Ok(T data) => new ServiceResult<T> { Success = true, Data = data };

        public static ServiceResult<T> Fail(string message) => new ServiceResult<T> { Success = false, ErrorMessage = message };
    }
}
