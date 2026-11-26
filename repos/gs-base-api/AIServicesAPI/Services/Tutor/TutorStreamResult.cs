using System.Text;

namespace GSAIServicesAPI.Services.Tutor
{
    /// <summary>
    /// What a tutor engine consumed while answering one turn, so the caller can settle
    /// the user's quota against it.
    /// <br/><br/>
    /// Engines report the provider's own token count in ReportedTokens when they can obtain it.
    /// When they cannot, StreamedText lets the caller count the answer exactly rather than charging nothing.
    /// </summary>
    public class TutorStreamResult
    {
        /// <summary>
        /// Tokens reported by the model provider for the whole completion
        /// </summary>
        public long ReportedTokens { get; set; }

        /// <summary>
        /// Estimated tokens of the prompt the engine actually sent upstream, after trimming the
        /// history. Lets the caller bill a broken stream from what was really submitted instead
        /// of the full client history
        /// </summary>
        public long PromptTokensSent { get; set; }

        private readonly StringBuilder _streamedText = new();

        /// <summary>
        /// Text streamed to the client. Used to count consumption exactly when no provider
        /// count is available, and read back by the engine's capture step as the audited answer.
        /// </summary>
        public string StreamedText => _streamedText.ToString();

        /// <summary>
        /// Appends a streamed chunk to the running answer.
        /// </summary>
        public void AppendStreamedText(string text) => _streamedText.Append(text);
    }
}
