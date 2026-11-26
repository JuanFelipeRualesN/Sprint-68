using Microsoft.AspNetCore.Http;

namespace GSAIServicesAPI.Services.Tutor
{
    /// <summary>
    /// A tutor engine streams an answer for a single turn directly to the HTTP response as Server-Sent Events.
    /// <code>
    /// data: {"delta":"token"}\n\n   <- one per token
    /// data: [DONE]\n\n              <- end of stream
    /// data: {"error":"message"}\n\n <- on failure
    /// </code>
    /// </summary>
    public interface ITutorEngine
    {
        /// <summary>
        /// Streams the tutor's answer to the given request back to the client as SSE per token.
        /// Each engine (generative / RAG) produces the answer differently but emits the same SSE shape.
        /// </summary>
        /// <returns>
        /// What the turn consumed, so the caller can charge it against the user's quota.
        /// Always returned, including when the stream failed part-way
        /// </returns>
        Task<TutorStreamResult> StreamAsync(TutorStreamRequest request, HttpResponse response, CancellationToken cancellationToken);
    }
}
