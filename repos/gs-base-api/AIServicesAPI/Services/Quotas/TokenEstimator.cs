using SharpToken;

namespace GSAIServicesAPI.Services.Quotas
{
    /// <summary>
    /// Token counts used to decide how much quota to hold before an AI call runs. 
    /// The real consumption is always taken from the provider's usage report and settled after.
    /// </summary>
    public static class TokenEstimator
    {
        /// <summary>
        /// Encoding used to count tokens
        /// </summary>
        private static readonly Lazy<GptEncoding> TokenEncoding = new(() =>
            GptEncoding.GetEncoding(AppConfiguration.Configuration["AppSettings:token_encoding"] ?? "o200k_base"));

        /// <summary>
        /// Special tokens are counted as ordinary text
        /// </summary>
        private static readonly HashSet<string> NoDisallowedSpecial = new();

        /// <summary>
        /// Tokens assumed for a response whose length is not known in advance. 
        /// Held up-front and refunded on commit if the answer turns out shorter.
        /// </summary>
        public const long AssumedOutputTokens = 1500;

        /// <summary>
        /// Counts the tokens in a piece of text.
        /// </summary>
        public static long FromText(string? text)
        {
            return string.IsNullOrEmpty(text)
                ? 0
                : TokenEncoding.Value.Encode(text, allowedSpecial: null, disallowedSpecial: NoDisallowedSpecial).Count;
        }

        /// <summary>
        /// Counts the tokens across several pieces of text.
        /// </summary>
        public static long FromTexts(IEnumerable<string?> texts)
        {
            return texts.Sum(FromText);
        }
    }
}
