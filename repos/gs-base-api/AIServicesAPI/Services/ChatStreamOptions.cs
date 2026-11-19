using OpenAI.Chat;
using System.Reflection;

namespace GSAIServicesAPI.Services
{
    /// <summary>
    /// Builds chat options that ask Azure OpenAI to report token usage while streaming.
    /// The current SDK (v2.1.0) does not expose stream_options.include_usage publicly, so the
    /// internal property is set by reflection. Without it, streamed responses omit the usage
    /// report and the quota can only ever settle against an estimate.
    /// </summary>
    public static class ChatStreamOptions
    {
        // Guards so the quota system doesn't fall back to estimates if the SDK layout changes.
        private static readonly Type StreamOptionsType = typeof(ChatClient).Assembly.GetType("OpenAI.Chat.InternalChatCompletionStreamOptions") ?? throw new InvalidOperationException("OpenAI SDK layout changed: InternalChatCompletionStreamOptions not found.");

        private static readonly PropertyInfo IncludeUsageProperty = StreamOptionsType.GetProperty("IncludeUsage") ?? throw new InvalidOperationException("OpenAI SDK layout changed: IncludeUsage not found.");

        private static readonly PropertyInfo StreamOptionsProperty = typeof(ChatCompletionOptions).GetProperty("StreamOptions", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException("OpenAI SDK layout changed: ChatCompletionOptions.StreamOptions not found.");

        /// <summary>
        /// Chat options with usage reporting enabled, so the final streamed chunk carries
        /// the real input+output token count used to settle the quota.
        /// </summary>
        public static ChatCompletionOptions WithUsage()
        {
            var options = new ChatCompletionOptions();
            var streamOptions = Activator.CreateInstance(StreamOptionsType)!;
            IncludeUsageProperty.SetValue(streamOptions, true);
            StreamOptionsProperty.SetValue(options, streamOptions);
            return options;
        }
    }
}
