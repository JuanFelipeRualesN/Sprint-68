using System.Text.RegularExpressions;

namespace GSAIServicesAPI.Services.Plannings
{
    /// <summary>
    /// To match the Athenas units with the RAG plannings: the subject and level whose units are used, and each unit's number
    /// </summary>
    public static class PlanningSubjectMap
    {
        private static readonly Regex UnitNumberPattern = new(@"\.(\d+)", RegexOptions.Compiled);

        /// <summary>
        /// Used for units without a dot (Eg: Unidad 1)
        /// </summary>
        private static readonly Regex NamedUnitNumberPattern = new(@"^\s*(?:unidad|unit)\s*(\d+)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Subjects whose units are taken from another subject, because the units only exist for that one.
        /// Spanish 1 to 3 is planned as language acquisition, and math 7 and 8 as pre-algebra and algebra I.
        /// </summary>
        private static readonly Dictionary<(string Subject, string Level), string> Redirects = new()
        {
            [("sp", "1")] = "adl",
            [("sp", "2")] = "adl",
            [("sp", "3")] = "adl",
            [("mat-sp", "7")] = "pa-sp",
            [("mat-sp", "8")] = "a1-sp",
        };

        /// <summary>
        /// To get the subject and level whose units are used for a subject and level.
        /// </summary>
        /// <param name="subjectCode">The subject's code.</param>
        /// <param name="levelCode">The level's code.</param>
        /// <returns>Returns the subject and level of the units, the same ones when there is no redirect.</returns>
        public static (string SubjectCode, string LevelCode) ResolveUnitsSource(string subjectCode, string levelCode)
        {
            var key = Normalize(subjectCode, levelCode);
            return Redirects.TryGetValue(key, out var target) ? (target, key.Level) : key;
        }

        /// <summary>
        /// Whether a unit's name already says it is a unit ("Unidad 1"), so it is shown without "Unit" before it.
        /// </summary>
        public static bool IsNamedUnit(string? unitName) => NamedUnitNumberPattern.IsMatch(unitName ?? string.Empty);

        /// <summary>
        /// To get the unit number from the unit's name. It goes after the first dot: "4.3", "AL.1" and "12.1 AL";
        /// a name without a dot can say it in words: "Unidad 1".
        /// </summary>
        /// <param name="unitName">The unit's name.</param>
        /// <returns>Returns the unit number, or null when the name has none.</returns>
        public static int? GetUnitNumber(string? unitName)
        {
            var name = unitName ?? string.Empty;
            var match = UnitNumberPattern.Match(name);
            if (!match.Success)
                match = NamedUnitNumberPattern.Match(name);
            return match.Success ? int.Parse(match.Groups[1].Value) : null;
        }

        private static (string Subject, string Level) Normalize(string subjectCode, string levelCode)
            => ((subjectCode ?? string.Empty).Trim().ToLowerInvariant(), (levelCode ?? string.Empty).Trim().ToLowerInvariant());
    }
}
