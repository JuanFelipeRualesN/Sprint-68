namespace GSWebAPI.GSWebAPI.Services.Common
{
    /// <summary>Language of the translated catalogs (forum categories, custom plan types), from the session's Accept-Language.</summary>
    internal static class RequestLanguage
    {
        /// <summary>English: the default, and the fallback when a catalog has no translation for the requested language.</summary>
        public const string English = "en";

        /// <summary>
        /// The primary language tag in lower case: <c>es-PR</c>, <c>es_PR</c> or <c>es-PR,es;q=0.9</c> become <c>es</c>.
        /// Null or blank means English.
        /// </summary>
        public static string Primary(string? langCode)
        {
            if (string.IsNullOrWhiteSpace(langCode))
            {
                return English;
            }

            var first = langCode.Split(',')[0].Split(';')[0].Trim();
            var primary = first.Split('-', '_')[0].Trim().ToLowerInvariant();
            return primary.Length is > 0 and <= 5 ? primary : English;
        }
    }
}
