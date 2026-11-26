using System;
using System.Globalization;
using System.Linq;

namespace DataAnalysisAPI.DataAnalysisAPI.Services.Utils
{
    /// <summary>
    /// Localized texts of the usage insights (notes, notices, errors, titles). They live in the configuration, under Messages:{key}:{es|en}.
    /// </summary>
    public static class UsageInsightsMessages
    {
        private const string FallbackLanguage = "en";

        /// <summary>
        /// Reduces an Accept-Language value (for example "es-PR,es;q=0.9") to a supported language code.
        /// </summary>
        /// <param name="rawLanguage">The raw header value.</param>
        /// <returns>Returns "es" or "en".</returns>
        public static string NormalizeLanguage(string? rawLanguage)
        {
            var code = (rawLanguage ?? string.Empty).Split(',', ';', '-')[0].Trim().ToLowerInvariant();
            return code == "es" ? "es" : FallbackLanguage;
        }

        /// <summary>
        /// Gets a localized text. It falls back to English and, when the key is unknown, to the key itself.
        /// </summary>
        /// <param name="language">The language code.</param>
        /// <param name="key">The message key.</param>
        /// <param name="arguments">Values for the {0}, {1}... placeholders.</param>
        /// <returns>Returns the formatted text.</returns>
        public static string Get(string language, string key, params object?[] arguments)
        {
            var configuration = AppConfiguration.Configuration;
            var template = configuration?[$"Messages:{key}:{language}"] ?? configuration?[$"Messages:{key}:{FallbackLanguage}"] ?? key;
            return arguments.Length == 0 ? template : string.Format(CultureInfo.InvariantCulture, template, arguments.Select(argument => argument ?? string.Empty).ToArray());
        }

        /// <summary>
        /// Joins several optional texts with a space, skipping the empty ones.
        /// </summary>
        /// <param name="parts">The texts.</param>
        /// <returns>Returns the joined text, or null when every part is empty.</returns>
        public static string? Join(params string?[] parts)
        {
            var text = string.Join(" ", parts.Where(part => !string.IsNullOrEmpty(part)));
            return text.Length == 0 ? null : text;
        }
    }
}
