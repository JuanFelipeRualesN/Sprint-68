using System.Globalization;
using GSWebAPI.GSWebAPI.Services.Common;
using Newtonsoft.Json;

namespace GSWebAPI.GSWebAPI.Services.CustomPlans.Assistant
{
    /// <summary>
    /// Static texts of the assistant (questions and option labels) in Spanish and English, from the embedded resource
    /// <c>CustomPlanAssistantTexts.json</c>. Spanish is the default language.
    /// </summary>
    public static class CustomPlanAssistantTexts
    {
        /// <summary>Spanish, the assistant's default language.</summary>
        public const string Spanish = "es";

        private static readonly Lazy<IReadOnlyDictionary<string, Dictionary<string, string>>> Texts = new(Load);

        /// <summary>The assistant language: en when the session asks for English, es otherwise (also when there is no header).</summary>
        /// <param name="langCode">Accept-Language of the session.</param>
        /// <returns>es or en.</returns>
        public static string Language(string? langCode) =>
            !string.IsNullOrWhiteSpace(langCode) && RequestLanguage.Primary(langCode) == RequestLanguage.English ? RequestLanguage.English : Spanish;

        /// <summary>The text of a key in the language, formatted with the arguments; the key itself when it is missing.</summary>
        /// <param name="key">Text key.</param>
        /// <param name="lang">es or en.</param>
        /// <param name="args">Values of {0}, {1}...</param>
        /// <returns>The text.</returns>
        public static string Get(string key, string lang, params object?[] args)
        {
            if (!Texts.Value.TryGetValue(key, out var byLang))
            {
                return key;
            }

            var text = byLang.TryGetValue(lang, out var found) ? found : byLang.GetValueOrDefault(Spanish) ?? key;
            return args.Length == 0 ? text : string.Format(Culture(lang), text, args);
        }

        /// <summary>A date the way the user reads it: "7 de diciembre de 2026" or "December 7, 2026".</summary>
        /// <param name="date">The date.</param>
        /// <param name="lang">es or en.</param>
        /// <returns>The formatted date.</returns>
        public static string Date(DateOnly date, string lang) =>
            lang == RequestLanguage.English
                ? date.ToString("MMMM d, yyyy", Culture(lang))
                : date.ToString("d 'de' MMMM 'de' yyyy", Culture(lang));

        /// <summary>A time as HH:mm (es) or h:mm tt (en).</summary>
        /// <param name="time">The time.</param>
        /// <param name="lang">es or en.</param>
        /// <returns>The formatted time.</returns>
        public static string Time(TimeOnly time, string lang) =>
            time.ToString(lang == RequestLanguage.English ? "h:mm tt" : "HH:mm", Culture(lang));

        private static CultureInfo Culture(string lang) => CultureInfo.GetCultureInfo(lang == RequestLanguage.English ? "en-US" : "es-PR");

        private static IReadOnlyDictionary<string, Dictionary<string, string>> Load()
        {
            var assembly = typeof(CustomPlanAssistantTexts).Assembly;
            var name = assembly.GetManifestResourceNames().Single(n => n.EndsWith("CustomPlanAssistantTexts.json", StringComparison.Ordinal));
            using var stream = assembly.GetManifestResourceStream(name)!;
            using var reader = new StreamReader(stream);
            return JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(reader.ReadToEnd())
                ?? new Dictionary<string, Dictionary<string, string>>();
        }
    }
}
