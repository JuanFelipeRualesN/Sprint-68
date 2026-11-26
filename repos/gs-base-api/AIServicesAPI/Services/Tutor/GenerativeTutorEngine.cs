using Azure.AI.OpenAI;
using GSAIServicesAPI.Models;
using GSAIServicesAPI.Services.Quotas;
using Microsoft.AspNetCore.Http;
using OpenAI.Chat;

namespace GSAIServicesAPI.Services.Tutor
{
    /// <summary>
    /// Generative tutor engine. Streams an Azure OpenAI chat completion.
    /// the client only supplies the conversation history and lesson context.
    /// </summary>
    public class GenerativeTutorEngine : ITutorEngine
    {
        /// Max number of messages to preserve the context window to reduce token cost
        private const int MaxHistoryMessages = 10;

        private readonly AzureOpenAIClient _openAIClient;
        private readonly string _model;
        private readonly ILogger<GenerativeTutorEngine> _logger;

        public GenerativeTutorEngine(AzureOpenAIClient client, IConfiguration config, ILogger<GenerativeTutorEngine> logger)
        {
            _openAIClient = client;
            _model = config["AppSettings:azure_openai_deployment"];
            _logger = logger;
        }

        /// <summary>
        /// Builds the system prompt, replays the conversation history,
        /// and streams the Azure OpenAI completion token by token as SSE per token
        /// </summary>
        public async Task<TutorStreamResult> StreamAsync(TutorStreamRequest request, HttpResponse response, CancellationToken cancellationToken)
        {
            // Holds a copy of what is streamed, which both the audit record and the quota settlement
            // read. Appending to it is a memory operation, so it adds no tokens and no perceived latency.
            var result = new TutorStreamResult();

            try
            {
                ChatClient chatClient = _openAIClient.GetChatClient(_model);

                var systemPrompt = TutorPromptBuilder.Build(request.LessonContext, request.LessonLanguage);

                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(systemPrompt)
                };

                // Keep only the most recent messages so an oversized history can't
                // inflate token cost or exceed the context window.
                var history = request.Messages.Count > MaxHistoryMessages
                    ? request.Messages.Skip(request.Messages.Count - MaxHistoryMessages)
                    : request.Messages;

                // Record the prompt actually sent so a broken
                // stream is billed from this rather than the full untrimmed client history.
                var promptTexts = new List<string> { systemPrompt };

                foreach (var message in history)
                {
                    if (string.IsNullOrWhiteSpace(message.Content)) continue;
                    promptTexts.Add(message.Content);
                    if (string.Equals(message.Role, "assistant", StringComparison.OrdinalIgnoreCase))
                        messages.Add(new AssistantChatMessage(message.Content));
                    else
                        messages.Add(new UserChatMessage(message.Content));
                }

                result.PromptTokensSent = TokenEstimator.FromTexts(promptTexts);

                // The completion only answers. A student's prompt is classified separately, off the
                // stream, by CaptureGenerateExchangeAsync — attaching the tool here made the model skip
                // the answer, and streaming a tool call is unreliable. tool_choice stays out of it.
                // The options only ask the provider to report token usage.
                var options = ChatStreamOptions.WithUsage();

                await foreach (StreamingChatCompletionUpdate chunk in
                    chatClient.CompleteChatStreamingAsync(messages, options, cancellationToken))
                {
                    // Usage arrives only on the final update and covers the whole completion
                    if (chunk.Usage is not null)
                    {
                        result.ReportedTokens = chunk.Usage.TotalTokenCount;
                    }

                    foreach (ChatMessageContentPart part in chunk.ContentUpdate)
                    {
                        if (!string.IsNullOrEmpty(part.Text))
                        {
                            result.AppendStreamedText(part.Text);
                            await SseWriter.WriteDeltaAsync(response, part.Text, cancellationToken);
                        }
                    }
                }

                await SseWriter.WriteDoneAsync(response, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Client disconnected. Keep whatever streamed so far for the audit record.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Generative tutor streaming failed");
                await SseWriter.WriteErrorAsync(response, ex.Message, cancellationToken);
            }

            return result;
        }
    }
}
