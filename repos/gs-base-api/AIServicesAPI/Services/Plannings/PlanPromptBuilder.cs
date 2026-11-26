using GSAIServicesAPI.Models.DTOs.Plannings;
using System.Text;
using System.Text.Json;

namespace GSAIServicesAPI.Services.Plannings
{
    /// <summary>
    /// Builds the prompts to generate a plan.
    /// With a unit, the plan comes from its plannings in the RAG: one lesson per planned day, and the sections
    /// from what each day lists. Without a unit, it comes from the topic the teacher wrote.
    /// </summary>
    public static class PlanPromptBuilder
    {
        /// <summary>
        /// The maximum length of each text section the teachers front saves
        /// </summary>
        private const int PlanningTextMaxLength = 250;

        /// <summary>
        /// The maximum length of each day section
        /// </summary>
        private const int DaySectionMaxLength = 1200;

        private static readonly JsonSerializerOptions JsonOptions = new() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

        /// <summary>
        /// The day sections of the plannings (their keys in the RAG) and how the prompt names them
        /// </summary>
        private static readonly (string Key, string Label)[] LessonSections =
        [
            ("Objetivos", "Objectives"),
            ("Actividad de inicio", "Introductory activity"),
            ("Actividad de desarrollo", "Development activity"),
            ("Actividad de cierre", "Closing activity")
        ];

        private static readonly (string Key, string Label)[] CotejoSections =
        [
            ("Avalúos", "Assessments"),
            ("Materiales", "Materials"),
            ("Integración con otras materias", "Integration with other subjects"),
            ("Acomodos razonables", "Reasonable accommodations"),
            ("Estrategia de instrucción diferenciada", "Differentiated instruction strategies"),
            ("Iniciativa o proyecto innovador", "Innovative initiative or project")
        ];

        /// <summary>
        /// The free content lessons per plan week
        /// </summary>
        private const int LessonsPerWeek = 3;
        private const int MinLessons = 3;
        private const int MaxLessons = 10;

        /// <summary>
        /// The most Athenas lessons the model recommends, apart from the free content lessons
        /// </summary>
        private const int MaxAthenasLessons = 4;

        /// <summary>
        /// The Athenas code of English, the only subject taught and planned in English
        /// </summary>
        private const string EnglishSubjectCode = "en";

        /// <summary>
        /// The language of the plan's content. It follows the subject.
        /// </summary>
        public static string ContentLanguage(string? subjectCode) => string.Equals(subjectCode?.Trim(), EnglishSubjectCode, StringComparison.OrdinalIgnoreCase) ? "English" : "Spanish";

        /// <summary>
        /// To build the system and user prompts.
        /// </summary>
        /// <param name="request">The teacher's request.</param>
        /// <param name="planning">The unit's plannings, or null when the teacher wrote a topic.</param>
        /// <param name="athenasLessons">The Athenas lessons the model can recommend.</param>
        /// <returns>Returns the system and the user prompts.</returns>
        public static (string System, string User) Build(PlanGenerationRequest request, UnitPlanning? planning, IReadOnlyList<AthenasLessonCandidate> athenasLessons)
        {
            // Only the weeks to cover, the front sends their days already.
            var planWeeks = PlanWeeks(request.PlanDays).Take(Math.Max(request.Weeks, 1)).ToList();
            var planDays = planWeeks.SelectMany(days => days).ToList();
            var singleWeek = planning is not null && planning.Preview.Weeks.Count == 1 && planning.Days.Count > 0;

            // Some plannings end a standard with a period, so the same one would appear twice
            var standards = Unique(planning?.Preview.Weeks.SelectMany(week => week.Standards).Select(standard => standard.Trim().TrimEnd('.')));
            // Some plannings keep two codes in one entry, which the model must copy one by one
            var expectations = ForLevel(Unique(planning?.Preview.Weeks
                .SelectMany(week => week.Expectations)
                .SelectMany(code => code.Split([' ', ',', ';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))), request.LevelCode);

            // Around three lessons per plan week. A single planned week keeps at least one lesson per planned day,
            // and stretched over more plan weeks its days get more lessons, in order
            var weekCount = planWeeks.Count > 0 ? planWeeks.Count : Math.Max(request.Weeks, 1);
            var lessonsByWeeks = Math.Clamp(weekCount * LessonsPerWeek, MinLessons, MaxLessons);
            var lessonCount = singleWeek ? Math.Max(planning!.Days.Count, lessonsByWeeks) : lessonsByWeeks;
            var plannedDays = singleWeek ? planning!.Days : null;
            var stretched = singleWeek && lessonCount > planning!.Days.Count;

            // With several unit weeks, each lesson covers some of them, in order, so the whole unit fits the plan
            var unitWeeks = planning is not null && !singleWeek ? planning.Preview.Weeks : null;

            // Without assessment types there is nothing to save, so the prompt does not mention the section
            var evaluations = request.Catalogs.Evaluations;
            var hasEvaluations = evaluations.Diagnostic.Count + evaluations.Formative.Count + evaluations.Summative.Count > 0;

            var sectionRules = SectionRules(request.Catalogs, request.SchoolImprovementPlan);
            var system = new StringBuilder()
                .AppendLine("You are an expert instructional planning assistant for teachers in Puerto Rico.")
                .AppendLine(planning is null
                    ? "Your task: generate a complete plan in the \"Cotejo de Planificación\" format, ready to submit."
                    : "Your task: generate a complete plan in the \"Cotejo de Planificación\" format, ready to submit, using the unit's official planning as reference. Cover its topics and objectives without copying its text.")
                .AppendLine($"Write all the content (titles, objectives, activities, descriptions) in {ContentLanguage(request.SubjectCode)}. Copy the list options exactly as written, whatever their language.")
                .AppendLine()
                .AppendLine($"## Standards for {request.SubjectName} {request.LevelCode}")
                .AppendLine(StandardsBlock(standards, expectations))
                .AppendLine()
                .AppendLine("## Response format (plain JSON, no markdown)")
                .AppendLine(ResponseFormat(athenasLessons.Count > 0, SectionKeys(sectionRules)))
                .AppendLine()
                .AppendLine("## \"sections\" keys")
                .AppendLine(sectionRules)
                .AppendLine();

            if (planning is not null)
                system.AppendLine("## How to use the official planning").AppendLine(PlanningRules(singleWeek, stretched, hasEvaluations)).AppendLine();

            if (athenasLessons.Count > 0)
                system.AppendLine("## Athenas lessons").AppendLine(AthenasRules(Math.Min(lessonCount, MaxAthenasLessons))).AppendLine();

            system
                .AppendLine("## Plan calendar")
                .AppendLine(CalendarRules(planDays, planWeeks, lessonCount, hasEvaluations, unitWeeks, stretched ? plannedDays : null))
                .AppendLine()
                .AppendLine("The option lists are closed: copy each option verbatim and do not add others.")
                .AppendLine($"Each \"text\" has at most {PlanningTextMaxLength} characters.")
                .AppendLine("Include only the keys described here, with their names exactly as written.")
                .AppendLine("Fill every key of \"sections\" listed above; omit one only when a rule says so.")
                .AppendLine()
                .Append("Reply ONLY with the JSON. No explanations before or after. No markdown.");

            var user = new StringBuilder()
                .AppendLine("Generate the complete plan:")
                .AppendLine()
                .AppendLine($"Subject: {request.SubjectName}")
                .AppendLine($"Grade: {request.LevelCode}")
                .AppendLine($"Weeks to cover: {(planWeeks.Count > 0 ? planWeeks.Count : request.Weeks)}")
                .AppendLine()
                .AppendLine(planning is null
                    ? $"Unit or topic to cover:\n{request.Topic}"
                    : PlanningBlock(planning, singleWeek, planDays, lessonCount))
                .AppendLine()
                .AppendLine($"The school {(request.SchoolImprovementPlan ? "IS" : "is NOT")} on an improvement plan.")
                .AppendLine();

            if (athenasLessons.Count > 0)
                user.AppendLine("Available Athenas lessons:")
                    .AppendLine(string.Join("\n", athenasLessons.Select(lesson => $"- [{lesson.Id}] {lesson.Title}: {lesson.Description}")))
                    .AppendLine();

            user.Append(!singleWeek
                ? $"In \"lessons\" suggest {lessonCount} free content lessons."
                : stretched
                    ? $"In \"lessons\" create {lessonCount} free content lessons, following the planned days the calendar gives them."
                    : $"In \"lessons\" create {lessonCount} free content lessons, one for each day of the planning.");
            if (athenasLessons.Count > 0)
                user.Append(" Choose \"athenasLessons\" first; then each free content lesson must have a title and a topic different from every Athenas lesson you chose.");
            user.Append(" Fill every section with specific, actionable content.");

            return (system.ToString(), user.ToString());
        }

        private static string AthenasRules(int maxLessons)
            => string.Join("\n",
                $"In \"athenasLessons\" recommend up to {maxLessons} lessons from the Athenas lessons the user lists.",
                "Recommend only the ones whose topic is a topic or objective of the plan, and leave out those about other topics or about general skills the plan does not work on. When one fits, recommend it: it already has its full content and its tests. If none fits, return an empty list.",
                "An Athenas lesson is added to the plan, it does not replace a free content lesson: the free content lessons stay as many as the other rules ask.",
                "Copy \"lessonId\" exactly as it appears between brackets, do not repeat a lesson and do not invent ids.",
                "In \"date\" put the plan day on which it is best to work on it: the date of the lesson that covers its topic.",
                "The free content lesson that shares its date with an Athenas lesson complements it: the same objectives with a different title and a different approach (practice, application or assessment), never a copy of the Athenas lesson.");

        /// <summary>
        /// The plan's weeks and the date of each lesson, so the sections point to lessons and weeks instead of dates
        /// </summary>
        private static string CalendarRules(List<string> planDays, List<List<string>> planWeeks, int lessonCount, bool hasEvaluations,
            List<WeekPreviewDto>? unitWeeks, List<PlanningDay>? stretchedDays)
        {
            var weeks = planWeeks.Select((days, index) => $"- Week {index + 1}: {days[0]} to {days[^1]}");
            var lessons = planDays.Count == 0
                ? []
                : Enumerable.Range(0, lessonCount).Select(index =>
                {
                    var day = planDays[Slot(index, lessonCount, planDays.Count)];
                    var week = planWeeks.FindIndex(days => days.Contains(day)) + 1;
                    return $"- Lesson {index + 1}: {day} (week {week}){UnitWeeksOfLesson(index, lessonCount, unitWeeks)}{PlannedDayOfLesson(index, lessonCount, stretchedDays)}";
                });

            var spread = string.Empty;

            if (unitWeeks is { Count: > 0 })
                spread = $"The unit has {unitWeeks.Count} planned weeks and this plan has {Math.Max(planWeeks.Count, 1)} week(s) with {lessonCount} lessons: cover the whole unit, in order. "
                    + "Each lesson works on the unit weeks the calendar gives it, choosing their essential objectives. When several lessons share a unit week, "
                    + "split its objectives among them; when a lesson has several unit weeks, integrate their main objectives.\n";
            else if (stretchedDays is { Count: > 0 })
                spread = $"The planning has {stretchedDays.Count} days and this plan has {Math.Max(planWeeks.Count, 1)} week(s) with {lessonCount} lessons: the lessons follow the planned days in order, as the calendar gives them. "
                    + "When a planned day has several lessons, the first one develops it and the next ones go deeper or apply it, with their own title and activities.\n";

            return spread + string.Join("\n",
                "Plan weeks:",
                string.Join("\n", weeks),
                "Date of each lesson in \"lessons\", numbered in their order (the first one is 1):",
                string.Join("\n", lessons),
                "- In each lesson's \"date\" put the date this calendar gives it.",
                "- The sections have no dates:",
                "  - \"fl-obs\" and \"fl-reflex\" are weekly: one entry per plan week, with its number in \"week\" and content about that week's lessons.",
                "  - \"gl-forums\", \"fl-plan1\" and \"fl-plan2\" cover the whole plan: a single entry each, without \"week\" or \"lessonNumbers\".",
                $"  - \"fl-mats\", \"fl-integ\", \"fl-acom\", \"fl-strat\" and \"fl-innov\" go with the lessons: in \"lessonNumbers\" put the numbers of the lessons that use them. At most 2 entries per key (per category in \"fl-acom\" and \"fl-strat\"): a general one with the options most lessons use, listing those lessons, and, only if needed, a specific one with the options a few lessons use, listing only those.",
                hasEvaluations
                    ? "  - \"fl-eval\" also goes with the lessons, in at most 3 entries: for example a diagnostic one in the first lesson, the formative ones along the lessons that use them, and a summative one in the last lesson."
                    : string.Empty).TrimEnd();
        }

        /// <summary>
        /// The unit weeks a lesson covers, as the calendar shows them
        /// </summary>
        private static string UnitWeeksOfLesson(int index, int lessonCount, List<WeekPreviewDto>? unitWeeks)
        {
            if (unitWeeks is not { Count: > 0 } || lessonCount <= 0)
                return string.Empty;

            var (first, last) = SourcesOfLesson(index, lessonCount, unitWeeks.Count);
            var covered = unitWeeks.Skip(first).Take(last - first + 1).ToList();

            var numbers = covered.Count == 1 ? $"{covered[0].Number}" : $"{covered[0].Number}-{covered[^1].Number}";
            var themes = string.Join(" / ", covered.Select(week => week.Theme).Where(theme => !string.IsNullOrWhiteSpace(theme)));
            return $" – unit week {numbers}{(themes.Length > 0 ? $": {themes}" : string.Empty)}";
        }

        /// <summary>
        /// The planned day a lesson follows when a single planned week is stretched over more lessons than days
        /// </summary>
        private static string PlannedDayOfLesson(int index, int lessonCount, List<PlanningDay>? days)
        {
            if (days is not { Count: > 0 } || lessonCount <= 0)
                return string.Empty;

            var (first, _) = SourcesOfLesson(index, lessonCount, days.Count);
            return $" – planned day: {Capitalize(days[first].Day)}";
        }

        /// <summary>
        /// The first and last source (unit week or planned day) a lesson takes when the sources are spread over the
        /// lessons in order
        /// </summary>
        private static (int First, int Last) SourcesOfLesson(int index, int lessonCount, int sourceCount)
        {
            int Start(int lesson) => lesson * sourceCount / lessonCount;
            var first = Start(index);
            var last = Math.Max(first, (index + 1 < lessonCount ? Start(index + 1) : sourceCount) - 1);
            return (first, last);
        }

        /// <summary>
        /// The plan's days grouped by calendar week (Monday to Sunday), in order, like the front groups them.
        /// </summary>
        private static List<List<string>> PlanWeeks(List<string> planDays)
            => planDays
                .Select(day => (Day: day, Date: DateOnly.TryParseExact(day, "yyyy-MM-dd", out var date) ? date : (DateOnly?)null))
                .Where(item => item.Date is not null)
                .GroupBy(item => item.Date!.Value.AddDays(-(((int)item.Date!.Value.DayOfWeek + 6) % 7)))
                .OrderBy(group => group.Key)
                .Select(group => group.Select(item => item.Day).ToList())
                .ToList();

        private static string StandardsBlock(List<string> standards, List<string> expectations)
        {
            if (standards.Count == 0 && expectations.Count == 0)
                return "There are no standards loaded for this subject and grade. Leave \"standardCode\" and \"expectations\" empty; never invent codes or expectations.";

            return "Use only the standards and expectations of the unit's planning.\n"
                + $"In \"standardCode\" put the standards that apply to the lesson, comma separated, copied from this list: {Json(standards)}.\n"
                + $"In \"expectations\" put the expectation codes that apply, comma separated, copied from this list: {Json(expectations)}.";
        }

        private static string ResponseFormat(bool hasAthenasLessons, IReadOnlyList<string> sectionKeys)
        {
            var athenas = hasAthenasLessons ? "\n  \"athenasLessons\": [{ \"lessonId\": \"lesson id\", \"date\": \"YYYY-MM-DD\" }]," : string.Empty;
            var sections = string.Join(",\n", sectionKeys.Select(key => $"    \"{key}\": [...]"));
            return $$"""
                {{{athenas}}
                  "lessons": [
                    {
                      "title": "Lesson title",
                      "date": "YYYY-MM-DD of the lesson according to the calendar",
                      "objective": "Objective",
                      "standardCode": "Lesson standards",
                      "expectations": "Lesson expectations",
                      "strategy": "Academic strategy",
                      "startActivity": "Introductory activity",
                      "devActivity": "Development activity",
                      "endActivity": "Closing activity"
                    }
                  ],
                  "sections": {
                {{sections}}
                  }
                }
                """;
        }

        private static List<string> SectionKeys(string sectionRules)
            => SectionKeyPattern.Matches(sectionRules).Select(match => match.Groups[1].Value).Distinct().ToList();

        private static readonly System.Text.RegularExpressions.Regex SectionKeyPattern =
            new(@"^- ""([a-z]+-[a-z0-9]+)"":", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.Compiled);

        private static string SectionRules(PlanCatalogs catalogs, bool schoolImprovementPlan)
        {
            var rules = new List<string>
            {
                "- \"gl-forums\": [{ \"title\": \"...\", \"description\": \"Opening prompt of the forum\" }]",
                "- \"fl-obs\": [{ \"week\": 1, \"text\": \"...\" }]: what the teacher should watch for in that week's lessons (difficulties, progress, students who need support).",
                "- \"fl-reflex\": [{ \"week\": 1, \"includeReflection\": true, \"text\": \"...\" }]: questions or points for the teacher to reflect on their own practice that week."
            };

            void Options(string key, string field, List<string> list)
            {
                if (list.Count > 0)
                    rules.Add($"- \"{key}\": [{{ \"lessonNumbers\": [1, 2], \"{field}\": [...] }}]. Options: {Json(list)}");
            }

            Options("fl-integ", "subjects", catalogs.Subjects);
            Options("fl-innov", "types", catalogs.Projects);
            Options("fl-mats", "materials", catalogs.Materials);

            var evaluations = EvaluationRule(catalogs.Evaluations);
            if (evaluations.Length > 0)
                rules.Add(evaluations);

            var accommodations = Categorized(catalogs.Accommodations);
            if (accommodations.Length > 0)
                rules.Add($"- \"fl-acom\": [{{ \"lessonNumbers\": [1, 2], \"category\": \"...\", \"accommodations\": [...] }}]. Options by category:\n{accommodations}");

            var differentiation = Categorized(catalogs.Differentiation);
            if (differentiation.Length > 0)
                rules.Add($"- \"fl-strat\": [{{ \"lessonNumbers\": [1, 2], \"category\": \"...\", \"types\": [...] }}]. Options by category:\n{differentiation}");

            if (schoolImprovementPlan)
            {
                if (catalogs.ActiveResponses.Count > 0)
                    rules.Add($"- \"fl-plan1\": [{{ \"modes\": [...], \"includesActiveResponse\": true, \"includesLearningCheck\": true }}]. Mode options: {Json(catalogs.ActiveResponses)}");
                rules.Add("- \"fl-plan2\": [{ \"text\": \"...\" }]: a common experience, a 2 to 3 minute activity the whole class shares to start the lessons (a short reading, an image, a video, a question or a demonstration), "
                    + "tied to the plan's topic. Describe what the teacher does and what the students do, in concrete and observable terms.");
            }

            rules.Add("Every \"text\" holds the content itself, in 1 to 3 sentences; never the section's name.");
            return string.Join("\n", rules);
        }

        /// <summary>
        /// The catalog usually offers the same types in every category, so a shared list goes once instead of once per category.
        /// </summary>
        private static string EvaluationRule(EvaluationOptions evaluations)
        {
            var categories = new (string Key, List<string> Options)[]
            {
                ("diagnostic", evaluations.Diagnostic),
                ("formative", evaluations.Formative),
                ("summative", evaluations.Summative)
            }.Where(category => category.Options.Count > 0).ToList();

            if (categories.Count == 0)
                return string.Empty;

            var names = string.Join(", ", categories.Select(category => $"\"{category.Key}\""));
            var shared = categories.All(category => category.Options.SequenceEqual(categories[0].Options));
            var options = shared
                ? $"Types (the same for every category): {Json(categories[0].Options)}"
                : "Types by category:\n" + string.Join("\n", categories.Select(category => $"  - {category.Key}: {Json(category.Options)}"));
            return $"- \"fl-eval\": [{{ \"lessonNumbers\": [1], \"category\": \"{categories[0].Key}\", \"types\": [...] }}]. \"category\" is one of {names}; \"types\" are assessment types copied from the list, never free text. {options}";
        }

        /// <summary>
        /// The rules to use the unit's official planning as a reference: what to take from it and how the lessons follow it.
        /// </summary>
        private static string PlanningRules(bool singleWeek, bool stretched, bool hasEvaluations)
        {
            var lessons = !singleWeek
                ? "- Lessons: each one covers the objectives of the unit weeks the calendar gives it, with your own content: a different title for each lesson, and new introductory, development and closing activities."
                : stretched
                    ? "- Lessons: each one works on the objectives of the planned day the calendar gives it, with its date in \"date\", but with its own content: a different title for each lesson, and new introductory, development and closing activities."
                    : "- Lessons: one for each day of the planning, in the same order, with the given plan day in \"date\". Each one works on its day's objectives, but with its own content: a different title for each lesson, and new introductory, development and closing activities.";

            var rules = new[]
            {
                "The planning is a reference of what must be covered (topics, objectives, standards), not a text to copy. Do not reproduce or summarize its titles or its activities; write new content aligned with it.",
                lessons,
                hasEvaluations
                    ? "- The planning's materials and assessments are not options: they only hint which options of the \"fl-mats\" and \"fl-eval\" lists fit. Answer only with options of those lists, copied verbatim."
                    : "- The planning's materials are not options: they only hint which options of the \"fl-mats\" list fit. Answer only with options of that list, copied verbatim.",
                "- \"fl-mats\": pick the options of its list that match the materials your lessons need.",
                "- \"fl-integ\": pick the catalog subjects your lessons integrate with, using the planning's integrations as reference: a subject the planning integrates on a single day goes only with the lesson that works on that day. Ignore \"n/a\". If there is no integration, omit this key.",
                hasEvaluations ? "- \"fl-eval\": pick the assessment types of its lists that check what your lessons teach." : null,
                "- \"fl-acom\", \"fl-strat\" and \"fl-innov\": propose them for the plan's lessons and objectives; if the planning has any, use them as reference.",
                "- \"gl-forums\", \"fl-obs\", \"fl-reflex\", \"fl-plan1\" and \"fl-plan2\": write them with your own content about the plan's topics and objectives and the lessons you propose; do not copy text from the planning."
            };

            return string.Join("\n", rules.Where(rule => rule is not null));
        }

        /// <summary>
        /// The plannings as text: the summary of each unit week and what its days list.
        /// A single week keeps each day, with its lesson and all its sections, because each one becomes a lesson.
        /// Several weeks keep, per week, the days' lessons and the sections once, without repeating
        /// what every day lists, to keep the prompt bounded. The themes every week shares go once for the unit.
        /// </summary>
        private static string PlanningBlock(UnitPlanning planning, bool singleWeek, List<string> planDays, int lessonCount)
        {
            var preview = planning.Preview;
            var text = new StringBuilder()
                .AppendLine("Official planning of the unit (reference, do not copy):")
                .AppendLine()
                .AppendLine(UnitHeading(preview));

            // Some units repeat the same cross-curricular and generating themes every week
            var sharedTransversal = SharedList(preview.Weeks, week => week.TransversalThemes);
            var sharedGenerating = SharedList(preview.Weeks, week => week.GeneratedThemes);
            text.Append(sharedTransversal ? ListLine("Cross-curricular themes (every week)", preview.Weeks[0].TransversalThemes) : string.Empty)
                .Append(sharedGenerating ? ListLine("Generating themes (every week)", preview.Weeks[0].GeneratedThemes) : string.Empty);

            foreach (var week in preview.Weeks)
            {
                text.AppendLine()
                    .AppendLine($"### Unit week {week.Number} ({week.Code}){(string.IsNullOrWhiteSpace(week.Theme) ? "" : $": {week.Theme}")}")
                    .Append(sharedTransversal ? string.Empty : ListLine("Cross-curricular themes", week.TransversalThemes))
                    .Append(sharedGenerating ? string.Empty : ListLine("Generating themes", week.GeneratedThemes));
                if (week.Objectives.Count > 0)
                    text.AppendLine("Week objectives:").AppendLine(string.Join("\n", week.Objectives.Select(objective => $"- {objective}")));

                var days = planning.Days.Where(day => day.WeekNumber == week.Number).ToList();
                if (singleWeek)
                    AppendDays(text, days, week, planDays, lessonCount);
                else
                    AppendWeekSections(text, days, week);
            }

            return text.ToString().TrimEnd();
        }

        /// <summary>
        /// The unit's name and title
        /// </summary>
        private static string UnitHeading(UnitPreviewDto preview)
        {
            if (!string.IsNullOrWhiteSpace(preview.PlanningUnit) && PlanningSubjectMap.IsNamedUnit(preview.PlanningUnit))
                return preview.PlanningUnit;

            var name = PlanningSubjectMap.IsNamedUnit(preview.Name) ? preview.Name : $"Unit {preview.Name}";
            return $"{name}: {(string.IsNullOrWhiteSpace(preview.PlanningUnit) ? preview.Desc : preview.PlanningUnit)}";
        }

        /// <summary>
        /// Each day of a single week, with where it goes in the plan, its lesson and all its sections.
        /// With one lesson per day, each day goes to its plan day
        /// </summary>
        private static void AppendDays(StringBuilder text, List<PlanningDay> days, WeekPreviewDto week, List<string> planDays, int lessonCount)
        {
            for (var index = 0; index < days.Count; index++)
            {
                var day = days[index];
                var date = planDays.Count == 0
                    ? string.Empty
                    : lessonCount > days.Count
                        ? $" -> {LessonsOfDay(index, days.Count, lessonCount)}"
                        : $" -> plan day {planDays[Slot(index, days.Count, planDays.Count)]}";
                text.AppendLine()
                    .AppendLine($"#### {Capitalize(day.Day)}{date}{LessonTitle(day, week)}");

                foreach (var (key, label) in LessonSections.Concat(CotejoSections))
                {
                    if (day.Sections.TryGetValue(key, out var values) && values.Count > 0)
                        text.AppendLine($"{label}: {Truncate(string.Join("; ", values), DaySectionMaxLength)}");
                }
            }
        }

        /// <summary>
        /// The lessons that follow a planned day when the week is stretched
        /// </summary>
        private static string LessonsOfDay(int dayIndex, int dayCount, int lessonCount)
        {
            var numbers = Enumerable.Range(0, lessonCount)
                .Where(lesson => SourcesOfLesson(lesson, lessonCount, dayCount).First == dayIndex)
                .Select(lesson => lesson + 1)
                .ToList();
            return numbers.Count switch
            {
                0 => "no lesson",
                1 => $"lesson {numbers[0]}",
                _ => $"lessons {numbers[0]}-{numbers[^1]}"
            };
        }

        /// <summary>
        /// The lessons of a week's days and the sections of the open content area they list
        /// </summary>
        private static void AppendWeekSections(StringBuilder text, List<PlanningDay> days, WeekPreviewDto week)
        {
            // Some plannings title every day with the week's theme, which says nothing more
            var lessons = Unique(days.Select(day => day.LessonTitle))
                .Where(title => !string.Equals(title.Trim(), week.Theme?.Trim(), StringComparison.OrdinalIgnoreCase))
                .ToList();
            text.Append(ListLine($"Lessons of its {days.Count} days", lessons));

            foreach (var (key, label) in CotejoSections)
            {
                var values = Unique(days.SelectMany(day => day.Sections.TryGetValue(key, out var list) ? list : []));
                if (values.Count > 0)
                    text.AppendLine($"{label}: {Truncate(string.Join("; ", values), DaySectionMaxLength)}");
            }
        }

        /// <summary>
        /// A day's lesson title for its heading, left out when it only repeats the week's theme.
        /// </summary>
        private static string LessonTitle(PlanningDay day, WeekPreviewDto week)
            => string.IsNullOrWhiteSpace(day.LessonTitle) || string.Equals(day.LessonTitle.Trim(), week.Theme?.Trim(), StringComparison.OrdinalIgnoreCase)
                ? string.Empty
                : $": {day.LessonTitle}";

        /// <summary>
        /// Whether every week has the same list, so it can go once.
        /// </summary>
        private static bool SharedList(List<WeekPreviewDto> weeks, Func<WeekPreviewDto, List<string>> list)
            => weeks.Count > 1 && list(weeks[0]).Count > 0 && weeks.All(week => list(week).SequenceEqual(list(weeks[0]), StringComparer.OrdinalIgnoreCase));

        /// <summary>
        /// The expectations of the plan's grade. A planning shared by several grades lists those of each one
        /// ("4.N.1", "5.N.1"), so the ones of another grade go out; the ones without a grade stay.
        /// Levels that are not grades, like "ns", keep them all.
        /// </summary>
        private static List<string> ForLevel(List<string> expectations, string levelCode)
        {
            var level = (levelCode ?? string.Empty).Trim();

            if (!GradePattern.IsMatch(level))
                return expectations;

            var ofLevel = expectations
                .Where(code =>
                {
                    var prefix = code.Split('.')[0].Trim();
                    return !GradePattern.IsMatch(prefix) || string.Equals(prefix, level, StringComparison.OrdinalIgnoreCase);
                })
                .ToList();
            return ofLevel.Count > 0 ? ofLevel : expectations;
        }

        private static readonly System.Text.RegularExpressions.Regex GradePattern =
            new(@"^(k|pk|\d{1,2})$", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);

        /// <summary>
        /// The plan day of a planned day, spreading the planned days over the plan's days like the front spreads the lessons.
        /// </summary>
        private static int Slot(int index, int total, int planDays)
            => total > 1 ? (int)Math.Round((double)index * (planDays - 1) / (total - 1), MidpointRounding.AwayFromZero) : 0;

        private static string Categorized(List<CategoryOptions> categories)
            => string.Join("\n", categories
                .Where(category => category.Options.Count > 0)
                .Select(category => $"  - {category.Category}: {Json(category.Options)}"));

        private static string ListLine(string label, List<string> values)
            => values.Count > 0 ? $"{label}: {string.Join("; ", values)}\n" : string.Empty;

        private static List<string> Unique(IEnumerable<string>? values)
            => (values ?? []).Where(value => !string.IsNullOrWhiteSpace(value)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

        // Some catalogs repeat an option, the model only needs it once.
        private static string Json(List<string> values) => JsonSerializer.Serialize(values.Distinct(StringComparer.Ordinal), JsonOptions);

        private static string Truncate(string value, int length) => value.Length <= length ? value : $"{value[..length]}...";

        private static string Capitalize(string value) => string.IsNullOrEmpty(value) ? "Day" : char.ToUpperInvariant(value[0]) + value[1..];
    }
}
