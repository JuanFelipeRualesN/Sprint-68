using System.Globalization;
using System.Text;

namespace GSWebAPI.GSWebAPI.Services.Common
{
    /// <summary>Detects foul language in user text. The app works in English and Spanish, so both lists apply to every text.</summary>
    public interface IProfanityFilter
    {
        /// <summary>Checks a user text against the English and Spanish word lists.</summary>
        /// <param name="text">Text to check; null, empty or whitespace is clean.</param>
        /// <returns>True when any word (or run of single letters joined together) matches a blocked entry.</returns>
        bool ContainsProfanity(string? text);
    }

    /// <summary>
    /// Word-list filter for English and Spanish.
    /// <list type="bullet">
    /// <item>Matching is per word, so innocent words that contain a foul one ("class", "assist", "putativo") pass.
    /// Entries ending in <c>*</c> also match words that start with them, and entries wrapped in <c>*</c> match
    /// anywhere inside a word ("*fuck*" catches "motherfucker"); only roots that never appear in normal words use them.</item>
    /// <item>Before matching, text is lower-cased, accents are removed (ñ is kept: "coño" is blocked, "cono" is not),
    /// common leetspeak is undone (f0ck, $hit, put@), letters stretched three or more times are collapsed
    /// ("fuuuck", "pendejooo") and letters split by spaces or symbols are joined ("f u c k", "m.i.e.r.d.a").</item>
    /// </list>
    /// A list cannot catch everything (new slang, deliberate misspellings); it is the first line of defense, with
    /// moderators deleting what slips through.
    /// </summary>
    public sealed class ProfanityFilter : IProfanityFilter
    {
        private static readonly string[] English =
        {
            "*fuck*", "fck", "fcking", "fuk", "fuking", "fukin", "wtf", "stfu", "gtfo",
            "shit", "shits", "shitty", "shitting", "shithead*", "*bullshit*", "horseshit", "dipshit",
            "*bitch*", "bastard", "bastards", "*asshole*", "arsehole*", "ass", "asses", "jackass", "dumbass", "dumbasses",
            "cunt", "cunts", "dickhead*", "*cocksucker*", "pussy", "pussies",
            "whore", "whores", "slut", "sluts", "slutty", "twat", "twats", "wanker", "wankers",
            "motherfucker*",
            "nigger*", "nigga*", "faggot*", "fag", "fags", "retard", "retards", "retarded",
            "porn", "porno", "dildo*", "blowjob*", "handjob*", "jerkoff",
        };

        private static readonly string[] Spanish =
        {
            "puta", "putas", "*putamadr*", "puto", "putos", "putita", "putitas", "putazo", "putada", "putadas",
            "*hijueput*", "*hijoput*", "*hijodeput*", "hdp", "ptm", "ctm", "ctmr", "csm", "mrd",
            "*mierd*", "*pendej*", "*cabron*", "*verga*", "*chingad*", "*chingar*", "chinga", "chingas", "chingon", "chingona",
            "*gilipoll*", "*malparid*", "*maricon*", "marica", "maricas", "joto", "jotos",
            "*culero*", "*culera*", "culiao", "culiado", "culo", "culos",
            "coño", "joder", "jodido", "jodida", "jodete", "carajo", "carajos",
            "pinche", "pinches", "mamon", "mamona", "mamones", "mamada", "mamadas", "cojones", "cojudo", "cojuda",
            "huevon", "huevona", "huevones", "weon", "weona", "boludo", "boluda", "pelotudo", "pelotuda",
            "follar", "follando",
            "porno", "pajero", "pajera",
        };

        private static readonly Dictionary<char, char> Leet = new()
        {
            ['0'] = 'o', ['1'] = 'i', ['!'] = 'i', ['3'] = 'e', ['4'] = 'a', ['@'] = 'a', ['5'] = 's', ['$'] = 's', ['7'] = 't',
        };

        private readonly HashSet<string> _exact = new(StringComparer.Ordinal);
        private readonly HashSet<string> _collapsedExact = new(StringComparer.Ordinal);
        private readonly List<string> _prefixes = new();
        private readonly List<string> _contains = new();

        /// <summary>Creates the filter with only the built-in English and Spanish lists.</summary>
        public ProfanityFilter()
            : this(Enumerable.Empty<string>())
        {
        }

        /// <summary>Creates the filter with the built-in lists plus extra entries.</summary>
        /// <param name="extraWords">More entries with the same syntax, added to the built-in English and Spanish lists.</param>
        public ProfanityFilter(IEnumerable<string> extraWords)
        {
            foreach (var raw in English.Concat(Spanish).Concat(extraWords))
            {
                var entry = raw.Trim();
                var startsWild = entry.StartsWith('*');
                var endsWild = entry.EndsWith('*') && entry.Length > 1;
                var word = NormalizeWord(entry.Trim('*'));
                if (word.Length == 0)
                {
                    continue;
                }

                if (startsWild && endsWild)
                {
                    _contains.Add(word);
                }
                else if (endsWild)
                {
                    _prefixes.Add(word);
                }
                else
                {
                    _exact.Add(word);
                    _collapsedExact.Add(CollapseRuns(word, 1));
                }
            }
        }

        /// <inheritdoc/>
        public bool ContainsProfanity(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            var words = Tokenize(text);
            if (words.Any(IsProfane))
            {
                return true;
            }

            // Letters split by separators: "f u c k", "m.i.e.r.d.a". Join each run of single-letter tokens.
            var run = new StringBuilder();
            foreach (var word in words.Append(string.Empty))
            {
                if (word.Length == 1)
                {
                    run.Append(word);
                    continue;
                }

                if (run.Length > 1 && IsProfane(run.ToString()))
                {
                    return true;
                }

                run.Clear();
            }

            return false;
        }

        private bool IsProfane(string word)
        {
            if (Matches(word))
            {
                return true;
            }

            // Stretched letters ("fuuuck", "pendejooo"). Only when a letter repeats 3+ times, so "as" never becomes "ass".
            if (HasRunOf(word, 3))
            {
                var collapsed = CollapseRuns(word, 1);
                return _collapsedExact.Contains(collapsed) || Matches(collapsed) || Matches(CollapseRuns(word, 2));
            }

            return false;
        }

        private bool Matches(string word)
        {
            return _exact.Contains(word)
                || _prefixes.Any(p => word.StartsWith(p, StringComparison.Ordinal))
                || _contains.Any(c => word.Contains(c, StringComparison.Ordinal));
        }

        /// <summary>Splits on anything that is not a letter, digit or leetspeak symbol, then normalizes each word.</summary>
        private static List<string> Tokenize(string text)
        {
            var words = new List<string>();
            var current = new StringBuilder();
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];

                // "!" is a letter only inside a word ("sh!t"); at the end it is punctuation ("ass!").
                var isWordChar = c == '!'
                    ? current.Length > 0 && i + 1 < text.Length && char.IsLetterOrDigit(text[i + 1])
                    : char.IsLetterOrDigit(c) || Leet.ContainsKey(c);
                if (isWordChar)
                {
                    current.Append(c);
                }
                else if (current.Length > 0)
                {
                    words.Add(current.ToString());
                    current.Clear();
                }
            }

            if (current.Length > 0)
            {
                words.Add(current.ToString());
            }

            return words.Select(NormalizeWord).Where(w => w.Length > 0).ToList();
        }

        /// <summary>Lower case, no accents (except ñ), leetspeak undone. Pure numbers are left as they are.</summary>
        private static string NormalizeWord(string word)
        {
            var lower = word.ToLowerInvariant();
            var hasLetter = lower.Any(char.IsLetter);
            var result = new StringBuilder(lower.Length);
            foreach (var c in lower)
            {
                if (c == 'ñ')
                {
                    result.Append(c);
                    continue;
                }

                if (hasLetter && Leet.TryGetValue(c, out var mapped))
                {
                    result.Append(mapped);
                    continue;
                }

                foreach (var d in c.ToString().Normalize(NormalizationForm.FormD))
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(d) != UnicodeCategory.NonSpacingMark && char.IsLetterOrDigit(d))
                    {
                        result.Append(d);
                    }
                }
            }

            return result.ToString();
        }

        private static bool HasRunOf(string word, int length)
        {
            var run = 1;
            for (var i = 1; i < word.Length; i++)
            {
                run = word[i] == word[i - 1] ? run + 1 : 1;
                if (run >= length)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Shortens every run of the same letter to at most <paramref name="max"/> letters.</summary>
        private static string CollapseRuns(string word, int max)
        {
            var result = new StringBuilder(word.Length);
            var run = 0;
            for (var i = 0; i < word.Length; i++)
            {
                run = i > 0 && word[i] == word[i - 1] ? run + 1 : 1;
                if (run <= max)
                {
                    result.Append(word[i]);
                }
            }

            return result.ToString();
        }
    }
}
