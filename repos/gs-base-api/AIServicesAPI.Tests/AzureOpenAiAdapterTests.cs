using System.Text.Json.Nodes;
using AIServicesAPI.Tests.Infrastructure;
using Azure;
using Azure.AI.OpenAI;
using GSAIServicesAPI.Models.DataAssistant;
using GSAIServicesAPI.Services.DataAssistant;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AIServicesAPI.Tests
{
    /// <summary>
    /// The adapter of the Azure OpenAI client, against a fake Azure endpoint that speaks the Chat Completions REST contract. This is the test that
    /// proves the request is accepted by the real Azure SDK pipeline (route with the deployment, api-version and api-key header).
    /// </summary>
    public sealed class AzureOpenAiAdapterTests : IAsyncLifetime
    {
        private WebApplication _server = null!;
        private string _baseUrl = string.Empty;
        private int _status = 200;
        private string _body = string.Empty;
        private string _path = string.Empty;
        private string? _apiKey;
        private string _requestBody = string.Empty;

        public async Task InitializeAsync()
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls("http://127.0.0.1:0");
            builder.Logging.ClearProviders();
            _server = builder.Build();
            _server.MapPost("/{**path}", async (HttpRequest request) =>
            {
                _path = request.Path + request.QueryString;
                _apiKey = request.Headers["api-key"];
                using var reader = new StreamReader(request.Body);
                _requestBody = await reader.ReadToEndAsync();
                return Results.Content(_body, "application/json", System.Text.Encoding.UTF8, _status);
            });
            await _server.StartAsync();
            _baseUrl = _server.Urls.First();
        }

        public async Task DisposeAsync()
        {
            await _server.StopAsync();
            await _server.DisposeAsync();
        }

        private AzureOpenAiDataAssistantChatClient CreateClient(string? specificDeployment = null) =>
            new(new AzureOpenAIClient(new Uri(_baseUrl), new AzureKeyCredential("the-azure-key")),
                new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["AppSettings:azure_openai_deployment"] = "generic-deployment",
                    ["AppSettings:ai_data_assistant_deployment_name"] = specificDeployment,
                }).Build(),
                new DataAssistantToolCatalog());

        private static readonly IReadOnlyList<DataAssistantChatMessage> Conversation = new[]
        {
            new DataAssistantChatMessage(DataAssistantChatRole.System, "instructions"),
            new DataAssistantChatMessage(DataAssistantChatRole.User, "question"),
            new DataAssistantChatMessage(DataAssistantChatRole.Assistant, null, new[] { new DataAssistantToolCall("call_1", "search_schools", """{"text":"high"}""") }),
            new DataAssistantChatMessage(DataAssistantChatRole.Tool, """{"Results":[]}""", ToolCallId: "call_1"),
        };

        [Fact]
        public async Task A_tool_call_answer_is_parsed_and_the_request_has_the_azure_route_the_key_the_messages_and_the_tools()
        {
            _body = """{"id":"x","choices":[{"index":0,"finish_reason":"tool_calls","message":{"role":"assistant","content":null,"tool_calls":[{"id":"call_9","type":"function","function":{"name":"get_connections_by_school","arguments":"{\"from\":\"2026-09-01\"}"}}]}}],"usage":{"prompt_tokens":100,"completion_tokens":20,"total_tokens":120}}""";

            var reply = await CreateClient().CompleteAsync(Conversation, CancellationToken.None);

            var call = Assert.Single(reply.ToolCalls);
            Assert.Equal(("call_9", "get_connections_by_school"), (call.Id, call.Name));
            Assert.Equal("""{"from":"2026-09-01"}""", call.ArgumentsJson);
            Assert.Equal(120, reply.TotalTokens);
            Assert.Null(reply.Content);

            Assert.StartsWith("/openai/deployments/generic-deployment/chat/completions?api-version=", _path);
            Assert.Equal("the-azure-key", _apiKey);
            var request = JsonNode.Parse(_requestBody)!;
            Assert.Equal(new[] { "system", "user", "assistant", "tool" }, request["messages"]!.AsArray().Select(message => message!["role"]!.GetValue<string>()));
            Assert.Equal("call_1", request["messages"]![2]!["tool_calls"]![0]!["id"]!.GetValue<string>());
            Assert.Equal("search_schools", request["messages"]![2]!["tool_calls"]![0]!["function"]!["name"]!.GetValue<string>());
            Assert.Equal("""{"text":"high"}""", request["messages"]![2]!["tool_calls"]![0]!["function"]!["arguments"]!.GetValue<string>());
            Assert.Equal("call_1", request["messages"]![3]!["tool_call_id"]!.GetValue<string>());
            Assert.Equal(13, request["tools"]!.AsArray().Count);
            Assert.Equal("function", request["tools"]![0]!["type"]!.GetValue<string>());
        }

        [Fact]
        public async Task A_final_answer_is_parsed_with_its_tokens()
        {
            _body = """{"choices":[{"index":0,"finish_reason":"stop","message":{"role":"assistant","content":"Done."}}],"usage":{"total_tokens":77}}""";

            var reply = await CreateClient().CompleteAsync(Conversation, CancellationToken.None);

            Assert.Equal("Done.", reply.Content);
            Assert.Empty(reply.ToolCalls);
            Assert.Equal(77, reply.TotalTokens);
        }

        [Fact]
        public async Task The_specific_deployment_wins_over_the_generic_one()
        {
            _body = """{"choices":[{"finish_reason":"stop","message":{"content":"Ok."}}]}""";

            await CreateClient("data-assistant-deployment").CompleteAsync(Conversation, CancellationToken.None);

            Assert.StartsWith("/openai/deployments/data-assistant-deployment/chat/completions", _path);
        }

        [Fact]
        public async Task The_content_filter_the_denied_credentials_and_service_failures_are_told_apart()
        {
            _status = 400;
            _body = """{"error":{"code":"content_filter","message":"The response was filtered."}}""";
            var filtered = await Assert.ThrowsAsync<ModelInvocationException>(() => CreateClient().CompleteAsync(Conversation, CancellationToken.None));

            _status = 401;
            _body = """{"error":{"code":"401","message":"Access denied due to invalid subscription key."}}""";
            var denied = await Assert.ThrowsAsync<ModelInvocationException>(() => CreateClient().CompleteAsync(Conversation, CancellationToken.None));

            _status = 500;
            _body = """{"error":{"message":"internal error"}}""";
            var failed = await Assert.ThrowsAsync<ModelInvocationException>(() => CreateClient().CompleteAsync(Conversation, CancellationToken.None));

            _status = 200;
            _body = """{"choices":[{"finish_reason":"content_filter","message":{"content":null}}]}""";
            var finishedFiltered = await Assert.ThrowsAsync<ModelInvocationException>(() => CreateClient().CompleteAsync(Conversation, CancellationToken.None));

            Assert.True(filtered.IsContentFiltered);
            Assert.True(denied.IsAccessDenied);
            Assert.False(denied.IsContentFiltered);
            Assert.Equal(500, failed.StatusCode);
            Assert.False(failed.IsAccessDenied);
            Assert.True(finishedFiltered.IsContentFiltered);
        }

        [Fact]
        public async Task The_key_never_appears_in_the_exception_text()
        {
            _status = 401;
            _body = """{"error":{"message":"Access denied"}}""";

            var denied = await Assert.ThrowsAsync<ModelInvocationException>(() => CreateClient().CompleteAsync(Conversation, CancellationToken.None));

            Assert.DoesNotContain("the-azure-key", denied.Message);
        }
    }
}
