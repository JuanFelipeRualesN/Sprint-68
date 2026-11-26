using System.Text;
using Azure.AI.Extensions.OpenAI;
using Azure.AI.Projects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenAI.Responses;

#pragma warning disable OPENAI001 // Responses API types are marked experimental in the OpenAI .NET SDK

namespace GSWebAPI.GSWebAPI.Services.CustomPlans.Assistant
{
    /// <summary>The model that reads the user's free text. It only interprets: it never gets tokens, ids it can invent, or write access.</summary>
    public interface IAssistantLanguageModel
    {
        /// <summary>Runs one completion.</summary>
        /// <param name="instructions">Developer instructions (added on top of the agent's own instructions).</param>
        /// <param name="input">The user turn (data, delimited).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The answer text, or null when the model is not configured, failed or timed out.</returns>
        Task<string?> CompleteAsync(string instructions, string input, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// The Foundry agent of GenerateController: the same <see cref="AIProjectClient"/> (Foundry:ProjectEndpoint, registered
    /// in GSBaseAPI with DefaultAzureCredential) and the same call (<c>GetProjectResponsesClientForAgent</c> +
    /// <c>CreateResponseStreamingAsync</c>), with the agent from CustomPlanAssistant:AgentName or Foundry:AgentName. The text
    /// is collected instead of streamed to the client. No quota and no audit (they do not apply here).
    /// </summary>
    public sealed class FoundryAssistantLanguageModel : IAssistantLanguageModel
    {
        private readonly IServiceProvider _services;
        private readonly CustomPlanAssistantSettings _settings;
        private readonly ILogger<FoundryAssistantLanguageModel> _logger;

        /// <summary>Creates the model client.</summary>
        /// <param name="services">Resolves the shared AIProjectClient (absent in environments without Foundry).</param>
        /// <param name="settings">Agent name and timeout.</param>
        /// <param name="logger">Logger.</param>
        public FoundryAssistantLanguageModel(IServiceProvider services, CustomPlanAssistantSettings settings, ILogger<FoundryAssistantLanguageModel> logger)
        {
            _services = services;
            _settings = settings;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<string?> CompleteAsync(string instructions, string input, CancellationToken cancellationToken = default)
        {
            var projectClient = _services.GetService<AIProjectClient>();
            if (projectClient == null || string.IsNullOrWhiteSpace(_settings.AgentName))
            {
                _logger.LogWarning("The custom plan assistant has no Foundry agent configured.");
                return null;
            }

            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeout.CancelAfter(TimeSpan.FromSeconds(_settings.AgentTimeoutSeconds));
            var answer = new StringBuilder();
            try
            {
                ProjectResponsesClient responsesClient = projectClient.ProjectOpenAIClient.GetProjectResponsesClientForAgent(defaultAgent: _settings.AgentName);
                var inputItems = new List<ResponseItem>
                {
                    ResponseItem.CreateDeveloperMessageItem(instructions),
                    ResponseItem.CreateUserMessageItem(input),
                };

                await foreach (StreamingResponseUpdate update in responsesClient.CreateResponseStreamingAsync(inputItems, cancellationToken: timeout.Token))
                {
                    switch (update)
                    {
                        case StreamingResponseOutputTextDeltaUpdate textDelta when !string.IsNullOrEmpty(textDelta.Delta):
                            answer.Append(textDelta.Delta);
                            break;

                        case StreamingResponseFailedUpdate failed:
                            throw new InvalidOperationException(failed.Response?.Error?.Message ?? "Agent run failed.");

                        case StreamingResponseErrorUpdate error:
                            throw new InvalidOperationException(error.Message ?? "Agent stream error.");
                    }
                }

                return answer.Length == 0 ? null : answer.ToString();
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning("The custom plan assistant agent did not answer in {Seconds} s.", _settings.AgentTimeoutSeconds);
                return null;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // The message only: the input holds the user's text, which is not logged.
                _logger.LogError("The custom plan assistant agent failed: {Error}", ex.Message);
                return null;
            }
        }
    }
}
