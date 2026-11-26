using GSAIServicesAPI.FeignAPIs;
using GSAIServicesAPI.FeignAPIs.Models;
using GSAIServicesAPI.Models.DTOs.Plannings;
using GSAIServicesAPI.Utils;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;

namespace GSAIServicesAPI.Services.Plannings
{
    /// <summary>
    /// Reads the official plannings of the RAG for the Athenas units: which units have plannings, their weeks,
    /// and the summary and days of those weeks. The index keeps the Athenas codes of each planning
    /// (subject_codes and grade_code), so the plannings are found by the plan's subject and level.
    /// </summary>
    public class PlanningsService
    {
        /// <summary>
        /// The fields read from a week's summary chunk and from a day chunk.
        /// </summary>
        private const string SummarySelect = "planning_id,week,unit,content,standards,expectations";
        private const string DaySelect = "planning_id,week,day,day_order,lesson_title,content";

        /// <summary>
        /// How long the week codes of a subject and level are kept. The plannings only change when they are indexed again.
        /// </summary>
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        /// <summary>
        /// A week code: the grades, the unit number and the week number, like "4.3.1"
        /// </summary>
        private static readonly Regex WeekCodePattern = new(@"^(.+)\.(\d+)\.(\d+)\.?$", RegexOptions.Compiled);

        private readonly AthenasFeignAPI _athenasFeignAPI;
        private readonly PlanningsSearchClient _searchClient;
        private readonly IMemoryCache _cache;

        public PlanningsService(AthenasFeignAPI athenasFeignAPI, PlanningsSearchClient searchClient, IMemoryCache cache)
        {
            _athenasFeignAPI = athenasFeignAPI;
            _searchClient = searchClient;
            _cache = cache;
        }

        /// <summary>
        /// To get the units of a subject and level that have plannings, in the unit number's order.
        /// Applies the subject redirects, and keeps only the units whose name has a unit number with plannings in the RAG,
        /// so those without a number, are left out.
        /// </summary>
        /// <param name="subjectCode">The subject's code of the plan.</param>
        /// <param name="levelCode">The level's code of the plan.</param>
        /// <param name="langCode">The language code.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the units with their weeks.</returns>
        public async Task<List<CurriculumUnitDto>> GetUnitsAsync(string subjectCode, string levelCode, string langCode, CancellationToken cancellationToken = default)
        {
            var (unitsSubject, unitsLevel) = PlanningSubjectMap.ResolveUnitsSource(subjectCode, levelCode);
            var weeksByUnit = await GetWeeksByUnitAsync(unitsSubject, unitsLevel, cancellationToken);
            if (weeksByUnit.Count == 0)
                return [];

            var units = await _athenasFeignAPI.GetUnits(unitsSubject, unitsLevel, langCode);

            return units
                .Select(unit => (Unit: unit, Number: PlanningSubjectMap.GetUnitNumber(unit.Name)))
                .Where(item => item.Number is int number && weeksByUnit.ContainsKey(number))
                .OrderBy(item => item.Number)
                .ThenBy(item => item.Unit.Name)
                .Select(item => new CurriculumUnitDto
                {
                    Id = item.Unit.Id,
                    Name = item.Unit.Name,
                    Desc = item.Unit.Desc,
                    SubjectCode = unitsSubject,
                    LevelCode = unitsLevel,
                    Duration = item.Unit.Duration,
                    Weeks = weeksByUnit[item.Number!.Value].Keys.ToList()
                })
                .ToList();
        }

        /// <summary>
        /// To get the preview of a unit's plannings, of one week or of the whole unit, as the teacher sees it
        /// before generating. It comes from the same reading the plan is generated from.
        /// </summary>
        /// <param name="subjectCode">The subject's code of the plan.</param>
        /// <param name="levelCode">The level's code of the plan.</param>
        /// <param name="unitId">The unit's id in Athenas.</param>
        /// <param name="weekNumber">The week number, or null for the whole unit.</param>
        /// <param name="langCode">The language code.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the preview, or null when the unit or the week have no plannings.</returns>
        public async Task<UnitPreviewDto?> GetUnitPreviewAsync(string subjectCode, string levelCode, string unitId, int? weekNumber, string langCode, CancellationToken cancellationToken = default)
        {
            var planning = await GetUnitPlanningAsync(subjectCode, levelCode, unitId, weekNumber, langCode, cancellationToken);
            return planning?.Preview;
        }

        /// <summary>
        /// To get the plannings of a unit to generate a plan from: the summary of its weeks and the content of each day.
        /// </summary>
        /// <param name="subjectCode">The subject's code of the plan.</param>
        /// <param name="levelCode">The level's code of the plan.</param>
        /// <param name="unitId">The unit's id in Athenas.</param>
        /// <param name="weekNumber">The week number, or null for the whole unit.</param>
        /// <param name="langCode">The language code.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the plannings, or null when the unit or the week have no plannings.</returns>
        public async Task<UnitPlanning?> GetUnitPlanningAsync(string subjectCode, string levelCode, string unitId, int? weekNumber, string langCode, CancellationToken cancellationToken = default)
        {
            var scope = await ResolveScopeAsync(subjectCode, levelCode, unitId, weekNumber, langCode, cancellationToken);
            if (scope is null)
                return null;

            var summariesTask = _searchClient.SearchAsync(WeeksFilter("summary", scope), SummarySelect, cancellationToken);
            var daysTask = _searchClient.SearchAsync(WeeksFilter("day", scope), DaySelect, cancellationToken);
            await Task.WhenAll(summariesTask, daysTask);

            var days = daysTask.Result
                .Select(chunk => (Chunk: chunk, Code: ParseWeekCode(chunk.Week)))
                .Where(item => item.Code is not null)
                .GroupBy(item => item.Code!.Value.Week)
                // A week with a second plan keeps only the plan with more days, so its days are not repeated
                .SelectMany(group => group
                    .GroupBy(item => item.Chunk.PlanningId)
                    .OrderByDescending(plan => plan.Count())
                    .ThenBy(plan => plan.Key)
                    .First())
                .OrderBy(item => item.Code!.Value.Week)
                .ThenBy(item => item.Chunk.DayOrder ?? int.MaxValue)
                .Select(item => new PlanningDay
                {
                    WeekNumber = item.Code!.Value.Week,
                    Day = item.Chunk.Day ?? string.Empty,
                    LessonTitle = item.Chunk.LessonTitle ?? string.Empty,
                    Sections = ParseSections(item.Chunk.Content)
                })
                .ToList();

            return new UnitPlanning { Preview = BuildPreview(scope, summariesTask.Result, days), Days = days };
        }

        /// <summary>
        /// To find the unit and the week codes to read. Null when the unit or the week have no plannings.
        /// </summary>
        private async Task<UnitScope?> ResolveScopeAsync(string subjectCode, string levelCode, string unitId, int? weekNumber, string langCode, CancellationToken cancellationToken)
        {
            var unit = (await GetUnitsAsync(subjectCode, levelCode, langCode, cancellationToken))
                .FirstOrDefault(item => item.Id == unitId);
            if (unit is null)
                return null;

            var weeks = weekNumber is int week ? unit.Weeks.Where(number => number == week).ToList() : unit.Weeks;
            if (weeks.Count == 0)
                return null;

            // The week codes as the index keeps them, e.g. "6-8.4.3" for a range of grades
            var weeksByUnit = await GetWeeksByUnitAsync(unit.SubjectCode, unit.LevelCode, cancellationToken);
            var unitWeeks = weeksByUnit[PlanningSubjectMap.GetUnitNumber(unit.Name)!.Value];
            return new UnitScope(unit, weeks.ToDictionary(number => number, number => unitWeeks[number]));
        }

        /// <summary>
        /// To build the preview of the unit: its name, the title its plannings give it, and the preview of each week.
        /// </summary>
        private static UnitPreviewDto BuildPreview(UnitScope scope, List<PlanningChunk> summaries, List<PlanningDay> days)
        {
            return new UnitPreviewDto
            {
                UnitId = scope.Unit.Id,
                Name = scope.Unit.Name,
                Desc = scope.Unit.Desc,
                PlanningUnit = summaries.Select(chunk => chunk.Unit).FirstOrDefault(name => !string.IsNullOrWhiteSpace(name)) ?? string.Empty,
                Weeks = summaries
                    .Select(chunk => (Chunk: chunk, Code: ParseWeekCode(chunk.Week)))
                    .Where(item => item.Code is not null)
                    .GroupBy(item => item.Code!.Value.Week)
                    .OrderBy(group => group.Key)
                    .Select(group => ToWeekPreview(group.Key, scope.WeekCodes[group.Key], group.Select(item => item.Chunk),
                        days.Where(day => day.WeekNumber == group.Key)))
                    .ToList()
            };
        }

        /// <summary>
        /// The unit and its weeks to read
        /// </summary>
        private sealed record UnitScope(CurriculumUnitDto Unit, Dictionary<int, string> WeekCodes);

        /// <summary>
        /// To filter the chunks of a type to the scope's weeks. The subject and level are filtered too,
        /// because subjects planned for the same grades share their week codes
        /// </summary>
        private static string WeeksFilter(string chunkType, UnitScope scope)
        {
            var weekCodes = string.Join(",", scope.WeekCodes.Values.Select(PlanningsSearchClient.EscapeLiteral));
            return $"type eq '{chunkType}' and {SubjectFilter(scope.Unit.SubjectCode, scope.Unit.LevelCode)} and search.in(week, '{weekCodes}', ',')";
        }

        /// <summary>
        /// To filter the chunks of a subject and level by their Athenas codes. The index keeps every grade a
        /// planning is for, and the Athenas levels that are not grades, like "ns" (9 to 12) or "p1" (K to 3).
        /// </summary>
        private static string SubjectFilter(string subjectCode, string levelCode)
            => $"subject_codes/any(s: s eq '{PlanningsSearchClient.EscapeLiteral(subjectCode)}') and grade_code/any(g: g eq '{PlanningsSearchClient.EscapeLiteral(levelCode)}')";

        /// <summary>
        /// To get the weeks with plannings of each unit number of a subject and level, with their week codes.
        /// Only the week codes of the summaries are read, and they are cached per subject and level.
        /// </summary>
        private async Task<Dictionary<int, SortedDictionary<int, string>>> GetWeeksByUnitAsync(string subjectCode, string levelCode, CancellationToken cancellationToken)
        {
            // The load is shared by the requests that wait for it
            var summaries = await _cache.GetOrLoadOnceAsync(
                $"plannings_summaries_{subjectCode}_{levelCode}",
                () => _searchClient.SearchAsync($"type eq 'summary' and {SubjectFilter(subjectCode, levelCode)}", "planning_id,week"),
                CacheDuration,
                cancellationToken: cancellationToken);

            var weeksByUnit = new Dictionary<int, SortedDictionary<int, string>>();
            foreach (var chunk in summaries)
            {
                if (ParseWeekCode(chunk.Week) is not { } code)
                    continue;

                if (!weeksByUnit.TryGetValue(code.Unit, out var weeks))
                    weeksByUnit[code.Unit] = weeks = [];
                weeks.TryAdd(code.Week, chunk.Week!.Trim());
            }

            return weeksByUnit;
        }

        /// <summary>
        /// To read the unit and week numbers of a week code
        /// </summary>
        private static (int Unit, int Week)? ParseWeekCode(string? weekCode)
        {
            var match = WeekCodePattern.Match((weekCode ?? string.Empty).Trim());
            return match.Success ? (int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value)) : null;
        }

        /// <summary>
        /// To build a week's preview: themes, objectives, standards and expectations of its summaries,
        /// and the sections and activities of its days
        /// </summary>
        private static WeekPreviewDto ToWeekPreview(int number, string code, IEnumerable<PlanningChunk> chunks, IEnumerable<PlanningDay> days)
        {
            var week = new WeekPreviewDto { Number = number, Code = code };

            foreach (var chunk in chunks)
            {
                var sections = ParseSections(chunk.Content);
                if (string.IsNullOrEmpty(week.Theme) && sections.TryGetValue("Tema", out var theme))
                    week.Theme = theme.FirstOrDefault() ?? string.Empty;

                AddUnique(week.TransversalThemes, sections.GetValueOrDefault("Temas transversales"));
                AddUnique(week.GeneratedThemes, sections.GetValueOrDefault("Temas generadores"));
                AddUnique(week.Objectives, sections.GetValueOrDefault("Objetivos de la semana")?.Where(item => !item.EndsWith(':')));
                AddUnique(week.Standards, chunk.Standards);
                AddUnique(week.Expectations, chunk.Expectations);
            }

            foreach (var day in days)
            {
                AddUnique(week.Materials, day.Sections.GetValueOrDefault("Materiales"));
                AddUnique(week.Integrations, day.Sections.GetValueOrDefault("Integración con otras materias"));
                AddUnique(week.Evaluations, day.Sections.GetValueOrDefault("Avalúos"));
                AddUnique(week.DifferentiatedInstruction, day.Sections.GetValueOrDefault("Estrategia de instrucción diferenciada"));
                AddUnique(week.Initiatives, day.Sections.GetValueOrDefault("Iniciativa o proyecto innovador"));
                AddUnique(week.Accommodations, day.Sections.GetValueOrDefault("Acomodos razonables"));
                week.Days.Add(new DayPreviewDto
                {
                    Day = day.Day,
                    LessonTitle = day.LessonTitle,
                    InitialActivity = day.Sections.GetValueOrDefault("Actividad de inicio") ?? [],
                    DevelopmentActivity = day.Sections.GetValueOrDefault("Actividad de desarrollo") ?? [],
                    ClosingActivity = day.Sections.GetValueOrDefault("Actividad de cierre") ?? []
                });
            }

            return week;
        }

        /// <summary>
        /// To read the sections of a summary or day chunk
        /// </summary>
        private static Dictionary<string, List<string>> ParseSections(IEnumerable<string>? content)
        {
            var sections = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            foreach (var section in content ?? [])
            {
                var separator = section.IndexOf(':');
                if (separator <= 0)
                    continue;

                var label = section[..separator].Trim();
                var value = section[(separator + 1)..];
                var lines = value.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                sections[label] = value.StartsWith('\n') || value.StartsWith("\r\n")
                    ? lines.Select(line => line.StartsWith("- ") ? line[2..].Trim() : line).ToList()
                    : [value.Trim()];
            }

            return sections;
        }

        /// <summary>
        /// To add the values not yet in the list
        /// </summary>
        private static void AddUnique(List<string> target, IEnumerable<string>? values)
        {
            foreach (var value in values ?? [])
            {
                var trimmed = value?.Trim();
                if (!string.IsNullOrEmpty(trimmed) && !target.Contains(trimmed, StringComparer.OrdinalIgnoreCase))
                    target.Add(trimmed);
            }
        }
    }
}
