using System.Globalization;
using System.Text.RegularExpressions;

namespace GSAIServicesAPI.Services.DataAssistant
{
    /// <summary>
    /// Anti-invention check: every figure of the answer must come from what the queries returned (or from the question, the context, today's date
    /// or the previous turns of the conversation). It does not judge the text: it only finds figures that nobody gave to the model.
    /// </summary>
    public static class AnswerFigureVerifier
    {
        private static readonly Regex IsoDate = new(@"(\d{4})-(\d{2})-(\d{2})(T[\d:.]+Z?)?", RegexOptions.Compiled);
        private static readonly Regex Number = new(@"\d+(?:[.,]\d+)*", RegexOptions.Compiled);
        private static readonly Regex ListNumbering = new(@"(?m)^\s*\d+[.)]\s+", RegexOptions.Compiled);

        // "5 of January" / "January 5" / the same in Spanish: a day of a month written in words (Spanish or English month names),
        // used when the model only interprets dates. A short word between the day and the month (of, de) is allowed.
        private static readonly Regex DayOfMonth = new(
            @"\b\d{1,2}\s+(?:\p{L}{1,3}\s+)?(?:" + string.Join("|", BuildMonthNames()) + @")\b|\b(?:" + string.Join("|", BuildMonthNames()) + @")\s+\d{1,2}\b",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static IEnumerable<string> BuildMonthNames()
        {
            var names = new List<string>();
            foreach (var code in new[] { "es", "en" })
            {
                try
                {
                    names.AddRange(CultureInfo.GetCultureInfo(code).DateTimeFormat.MonthNames.Where(name => name.Length > 0));
                }
                catch (CultureNotFoundException)
                {
                    // Invariant globalization: the culture data is not available, so only the other language is used.
                }
            }

            names.AddRange(CultureInfo.InvariantCulture.DateTimeFormat.MonthNames.Where(name => name.Length > 0));
            return names.Select(Regex.Escape).Distinct();
        }

        /// <summary>
        /// Normalizes a number to its digits, so 1,234 and 1.234 and 01234 are the same figure.
        /// </summary>
        /// <param name="text">The number as written.</param>
        /// <returns>The digits without leading zeros ("0" when all were zeros).</returns>
        public static string Normalize(string text)
        {
            var digits = new string(text.Where(char.IsDigit).ToArray()).TrimStart('0');
            return digits.Length == 0 ? "0" : digits;
        }

        /// <summary>
        /// Gets the normalized figures of a text, leaving out ISO dates (their parts are taken apart by <see cref="GetDateParts"/>).
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The set of figures.</returns>
        public static HashSet<string> GetFigures(string text) =>
            Number.Matches(IsoDate.Replace(text, " ")).Select(match => Normalize(match.Value)).ToHashSet();

        /// <summary>
        /// Gets the year, month and day of the ISO dates of a text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The set of date parts.</returns>
        public static HashSet<string> GetDateParts(string text) =>
            IsoDate.Matches(text).SelectMany(match => new[] { match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value }).Select(Normalize).ToHashSet();

        /// <summary>
        /// Accumulates the figures a conversation has seen so far with the ones of the current turn.
        /// </summary>
        /// <param name="previous">Figures of the previous turns.</param>
        /// <param name="question">The current question.</param>
        /// <param name="toolOutputs">What the queries returned.</param>
        /// <param name="toolArguments">The arguments the model sent.</param>
        /// <returns>The accumulated set.</returns>
        public static HashSet<string> Accumulate(IReadOnlySet<string>? previous, string question, IEnumerable<string> toolOutputs, IEnumerable<string> toolArguments)
        {
            var figures = previous is null ? new HashSet<string>() : new HashSet<string>(previous);
            figures.UnionWith(GetFigures(question));
            foreach (var text in toolOutputs.Concat(toolArguments))
            {
                figures.UnionWith(GetFigures(text));
                figures.UnionWith(GetDateParts(text));
            }

            return figures;
        }

        /// <summary>
        /// Finds the figures of the answer that do not come from the queries.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <param name="context">The dashboard context text.</param>
        /// <param name="answer">The visible answer.</param>
        /// <param name="toolOutputs">What the queries returned.</param>
        /// <param name="toolArguments">The arguments the model sent.</param>
        /// <param name="today">Today's date as yyyy-MM-dd.</param>
        /// <param name="previousFigures">Figures of the previous turns of the conversation.</param>
        /// <returns>The unexplained figures (at most 8, shortest first); empty when the answer is clean.</returns>
        public static IReadOnlyList<string> Verify(string question, string context, string answer, IReadOnlyCollection<string> toolOutputs,
            IEnumerable<string> toolArguments, string today, IReadOnlySet<string>? previousFigures = null)
        {
            var body = ListNumbering.Replace(answer, string.Empty);

            // Without queries the model only interprets dates ("year to date 2026" = January 1 to today): those "N of month" are not data.
            if (toolOutputs.Count == 0)
            {
                body = DayOfMonth.Replace(body, " ");
            }

            var known = GetFigures($"{question} {context} {today}");
            known.UnionWith(GetDateParts($"{question} {context} {today}"));
            if (previousFigures is not null)
            {
                known.UnionWith(previousFigures);
            }

            foreach (var text in toolOutputs.Concat(toolArguments))
            {
                known.UnionWith(GetFigures(text));
                known.UnionWith(GetDateParts(text));
            }

            return GetFigures(body).Except(known).OrderBy(figure => figure.Length).ThenBy(figure => figure, StringComparer.Ordinal).Take(8).ToList();
        }
    }
}
