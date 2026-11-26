using System.ClientModel;
using System.ClientModel.Primitives;
using System.Text.Json.Nodes;
using Azure.AI.OpenAI;
using GSAIServicesAPI.Models.DataAssistant;
using OpenAI.Chat;

namespace GSAIServicesAPI.Services.DataAssistant
{
    /// <summary>
    /// Adapter of the Azure OpenAI chat client (Chat Completions with tool calling). It uses the singleton <see cref="AzureOpenAIClient"/> built from
    /// AzureOpenAI:Endpoint and AzureOpenAI:ApiKey, like the rest of this module. The deployment comes from AppSettings:ai_data_assistant_deployment_name
    /// and, when it is empty, from AppSettings:azure_openai_deployment.
    /// </summary>
    /// <remarks>
    /// It calls the PROTOCOL method of the chat client with the JSON of the request, not the typed methods (CompleteChatAsync with messages and options).
    /// With the packages of this solution (Azure.AI.OpenAI 2.1.0 on top of OpenAI 2.9.1, which is pinned for the Foundry client) every typed call of
    /// AzureChatClient fails at runtime with a MissingMethodException on ChatCompletionOptions.get_SerializedAdditionalRawData; the protocol method
    /// goes through the same Azure pipeline (deployment route, api-version and api-key header) without that code path.
    /// </remarks>
    public sealed class AzureOpenAiDataAssistantChatClient : IDataAssistantChatClient
    {
        private readonly AzureOpenAIClient _client;
        private readonly string? _deployment;
        private readonly DataAssistantToolCatalog _catalog;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureOpenAiDataAssistantChatClient"/> class.
        /// </summary>
        /// <param name="client">The Azure OpenAI client.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="catalog">The tool catalog.</param>
        public AzureOpenAiDataAssistantChatClient(AzureOpenAIClient client, IConfiguration configuration, DataAssistantToolCatalog catalog)
        {
            _client = client;
            _catalog = catalog;
            _deployment = configuration["AppSettings:ai_data_assistant_deployment_name"] is { Length: > 0 } specific
                ? specific
                : configuration["AppSettings:azure_openai_deployment"];
        }

        /// <inheritdoc />
        public async Task<DataAssistantModelReply> CompleteAsync(IReadOnlyList<DataAssistantChatMessage> messages, CancellationToken cancellationToken)
        {
            var request = new JsonObject
            {
                ["messages"] = new JsonArray(messages.Select(ToJson).ToArray()),
                ["tools"] = _catalog.BuildToolsJson(),
            };

            string responseBody;
            try
            {
                ChatClient chatClient = _client.GetChatClient(_deployment);
                var result = await chatClient.CompleteChatAsync(BinaryContent.Create(BinaryData.FromString(request.ToJsonString())),
                    new RequestOptions { CancellationToken = cancellationToken });
                responseBody = result.GetRawResponse().Content.ToString();
            }
            catch (ClientResultException exception)
            {
                var body = exception.GetRawResponse()?.Content.ToString() ?? string.Empty;
                throw new ModelInvocationException(exception.Status, body.Contains("content_filter", StringComparison.OrdinalIgnoreCase), exception);
            }

            return ParseReply(responseBody);
        }

        private static DataAssistantModelReply ParseReply(string responseBody)
        {
            var choice = JsonNode.Parse(responseBody)?["choices"]?[0];
            if (choice is null)
            {
                throw new ModelInvocationException(502);
            }

            if (choice["finish_reason"]?.GetValue<string>() == "content_filter")
            {
                throw new ModelInvocationException(400, isContentFiltered: true);
            }

            var message = choice["message"];
            var toolCalls = (message?["tool_calls"] as JsonArray ?? new JsonArray())
                .Select(call => new DataAssistantToolCall(
                    call!["id"]!.GetValue<string>(),
                    call["function"]!["name"]!.GetValue<string>(),
                    call["function"]!["arguments"]?.GetValue<string>() ?? "{}"))
                .ToList();
            var totalTokens = JsonNode.Parse(responseBody)?["usage"]?["total_tokens"]?.GetValue<long>() ?? 0;
            return new DataAssistantModelReply(message?["content"]?.GetValue<string>(), toolCalls, totalTokens);
        }

        private static JsonNode ToJson(DataAssistantChatMessage message)
        {
            switch (message.Role)
            {
                case DataAssistantChatRole.System:
                    return new JsonObject { ["role"] = "system", ["content"] = message.Content };
                case DataAssistantChatRole.User:
                    return new JsonObject { ["role"] = "user", ["content"] = message.Content };
                case DataAssistantChatRole.Tool:
                    return new JsonObject { ["role"] = "tool", ["tool_call_id"] = message.ToolCallId, ["content"] = message.Content };
                default:
                    var assistant = new JsonObject { ["role"] = "assistant", ["content"] = message.Content };
                    if (message.ToolCalls is { Count: > 0 })
                    {
                        assistant["tool_calls"] = new JsonArray(message.ToolCalls.Select(call => (JsonNode)new JsonObject
                        {
                            ["id"] = call.Id,
                            ["type"] = "function",
                            ["function"] = new JsonObject { ["name"] = call.Name, ["arguments"] = call.ArgumentsJson },
                        }).ToArray());
                    }

                    return assistant;
            }
        }
    }
}
