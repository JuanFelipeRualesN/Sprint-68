using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace GSAIServicesAPI.Services.Tutor
{
    /// <summary>
    /// Small helper that writes the unified tutor SSE frames to an
    /// <see cref="HttpResponse"/> and flushes each one immediately so the client receives tokens as they are produced. 
    /// Keeps the frame format in one place so every engine stays consistent.
    /// </summary>
    public static class SseWriter
    {
        /// <summary>
        /// Writes a single token.
        /// </summary>
        public static async Task WriteDeltaAsync(HttpResponse response, string text, CancellationToken cancellationToken)
        {
            var json = JsonSerializer.Serialize(new { delta = text });
            await response.WriteAsync($"data: {json}\n\n", cancellationToken);
            await response.Body.FlushAsync(cancellationToken);
        }

        /// <summary>
        /// Writes the conversation id so the client can send it back on the next
        /// request and keep the RAG thread's context.
        /// </summary>
        public static async Task WriteConversationIdAsync(HttpResponse response, string conversationId, CancellationToken cancellationToken)
        {
            var json = JsonSerializer.Serialize(new { conversationId });
            await response.WriteAsync($"data: {json}\n\n", cancellationToken);
            await response.Body.FlushAsync(cancellationToken);
        }

        /// <summary>
        /// Writes the terminal sentinel.
        /// </summary>
        public static async Task WriteDoneAsync(HttpResponse response, CancellationToken cancellationToken)
        {
            await response.WriteAsync("data: [DONE]\n\n", cancellationToken);
            await response.Body.FlushAsync(cancellationToken);
        }

        /// <summary>
        /// Writes an error frame.
        /// </summary>
        public static async Task WriteErrorAsync(HttpResponse response, string message, CancellationToken cancellationToken)
        {
            var json = JsonSerializer.Serialize(new { error = message });
            await response.WriteAsync($"data: {json}\n\n", cancellationToken);
            await response.Body.FlushAsync(cancellationToken);
        }
    }
}
