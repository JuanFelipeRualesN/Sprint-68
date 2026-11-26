using System.Globalization;
using System.Text;
using GSWebAPI.GSWebAPI.Domain.Models.CustomPlans;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;

namespace GSWebAPI.GSWebAPI.Services.CustomPlans.Assistant
{
    /// <summary>A lesson ready to be scheduled: catalog ids only (never produced by the model).</summary>
    /// <param name="LessonId">Athenas lesson id.</param>
    /// <param name="SubjectId">Athenas subject id of the plan subject it belongs to.</param>
    /// <param name="Title">Lesson title (preview).</param>
    /// <param name="StandardId">Athenas standard id (only with a framework).</param>
    /// <param name="PracticeIds">Practices of the lesson in Athenas (IsExam 0), in order.</param>
    /// <param name="ExamIds">Exams of the lesson (IsExam 1).</param>
    /// <param name="PreTestId">Pre-test of the lesson (IsExam 2), if any.</param>
    /// <param name="PostTestId">Post-test of the lesson (IsExam 3), if any.</param>
    public sealed record PlanBuilderLesson(
        ulong LessonId,
        ulong SubjectId,
        string Title,
        ulong? StandardId,
        IReadOnlyList<ulong> PracticeIds,
        IReadOnlyList<ulong> ExamIds,
        ulong? PreTestId,
        ulong? PostTestId);

    /// <summary>A catalog lesson chosen for a subject, with the standard it is aligned to.</summary>
    /// <param name="LessonId">Athenas lesson id.</param>
    /// <param name="Title">Lesson title.</param>
    /// <param name="StandardId">First standard of the lesson when the plan uses a framework; otherwise null.</param>
    public sealed record PlanCandidateLesson(ulong LessonId, string Title, ulong? StandardId);

    /// <summary>Schedule settings of a plan.</summary>
    public sealed class PlanBuilderSchedule
    {
        /// <summary>First day.</summary>
        public DateOnly StartDate { get; init; }

        /// <summary>Length in weeks (at least 1).</summary>
        public ushort Weeks { get; init; }

        /// <summary>Study days, ISO 8601 (1 = Monday ... 7 = Sunday).</summary>
        public IReadOnlyList<byte> Days { get; init; } = Array.Empty<byte>();

        /// <summary>Session start time (local).</summary>
        public TimeOnly StartTime { get; init; }

        /// <summary>Minutes per session.</summary>
        public ushort DurationMinutes { get; init; }

        /// <summary>Time zone (IANA or Windows id).</summary>
        public string Timezone { get; init; } = null!;
    }

    /// <summary>The plan structure and the numbers behind it.</summary>
    /// <param name="Structure">Subjects, days and lessons, ready for POST custom-plans.</param>
    /// <param name="Sessions">Every session date of the plan.</param>
    /// <param name="SessionsPerLesson">Sessions one lesson takes.</param>
    /// <param name="Capacity">Lessons that fit in the plan.</param>
    /// <param name="EndDate">The plan's end date as the API computes it (StartDate + weeks).</param>
    public sealed record PlanBuildResult(CustomPlanStructureRequestModel Structure, IReadOnlyList<DateOnly> Sessions, int SessionsPerLesson, int Capacity, DateOnly EndDate);

    /// <summary>
    /// Builds the plan deterministically: sessions from the days and weeks, how many lessons fit, which lessons (round
    /// robin across subjects) and every date and deadline. No I/O and no model: the same input gives the same plan.
    /// </summary>
    public static class CustomPlanBuilder
    {
        /// <summary>Default days for a number of days a week (3 = Mon/Wed/Fri, 2 = Tue/Thu...).</summary>
        public static IReadOnlyList<byte> DefaultDays(int daysPerWeek) => daysPerWeek switch
        {
            1 => new byte[] { 1 },
            2 => new byte[] { 2, 4 },
            3 => new byte[] { 1, 3, 5 },
            4 => new byte[] { 1, 2, 4, 5 },
            5 => new byte[] { 1, 2, 3, 4, 5 },
            6 => new byte[] { 1, 2, 3, 4, 5, 6 },
            7 => new byte[] { 1, 2, 3, 4, 5, 6, 7 },
            _ => Array.Empty<byte>(),
        };

        /// <summary>ISO 8601 day of week (1 = Monday ... 7 = Sunday).</summary>
        public static byte IsoDay(DateOnly date) => date.DayOfWeek == DayOfWeek.Sunday ? (byte)7 : (byte)date.DayOfWeek;

        /// <summary>The next Monday after <paramref name="today"/> (a week later when today is Monday).</summary>
        public static DateOnly NextMonday(DateOnly today) => today.AddDays(8 - IsoDay(today));

        /// <summary>The API's end date for a plan: StartDate + weeks (inclusive).</summary>
        public static DateOnly EndDate(DateOnly start, ushort weeks) => start.AddDays(7 * weeks);

        /// <summary>Every date from <paramref name="start"/> for <paramref name="weeks"/> weeks that falls on one of the days.</summary>
        public static IReadOnlyList<DateOnly> SessionDates(DateOnly start, ushort weeks, IEnumerable<byte> days)
        {
            var set = days.ToHashSet();
            return Enumerable.Range(0, 7 * weeks).Select(start.AddDays).Where(d => set.Contains(IsoDay(d))).ToList();
        }

        /// <summary>Sessions a lesson takes: ceil(minutes per lesson / session minutes), at least 1.</summary>
        public static int SessionsPerLesson(int minutesPerLesson, int sessionMinutes) =>
            sessionMinutes <= 0 ? 1 : Math.Max(1, (int)Math.Ceiling(minutesPerLesson / (double)sessionMinutes));

        /// <summary>Lessons that fit: whole lessons in the sessions; one lesson when there are fewer sessions than one lesson needs.</summary>
        public static int Capacity(int sessions, int sessionsPerLesson) => sessions == 0 ? 0 : Math.Max(1, sessions / sessionsPerLesson);

        /// <summary>Takes items round robin from the queues (one of each in turn) until <paramref name="max"/> or all are used.</summary>
        public static List<T> Interleave<T>(IReadOnlyList<IReadOnlyList<T>> queues, int max)
        {
            var result = new List<T>();
            for (var round = 0; result.Count < max; round++)
            {
                var took = false;
                foreach (var queue in queues)
                {
                    if (round < queue.Count && result.Count < max)
                    {
                        result.Add(queue[round]);
                        took = true;
                    }
                }

                if (!took)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Orders and filters the catalog of one subject. With a framework, only lessons with a standard are kept (and get
        /// their first standard). Then: lessons the model matched to the user's need first (only those when
        /// <paramref name="onlyMatched"/>), then lessons to reinforce, then lessons that share a standard to reinforce,
        /// then the rest, each group in catalog order.
        /// </summary>
        /// <param name="catalog">Published lessons of the subject and level, in catalog order.</param>
        /// <param name="standardIds">Ids of the Athenas standards (codes of type standard); null when there is no framework.</param>
        /// <param name="reinforceLessonIds">Lessons where the student had low scores or overdue work.</param>
        /// <param name="reinforceStandardIds">Standards of those lessons.</param>
        /// <param name="matchedLessonIds">Lessons matched to the user's skill or need, or null.</param>
        /// <param name="onlyMatched">Keep only the matched lessons (specific task) when some matched.</param>
        /// <returns>The candidates in the order they should be scheduled.</returns>
        public static List<PlanCandidateLesson> OrderCandidates(
            IReadOnlyList<AthenasCatalogLesson> catalog,
            IReadOnlySet<ulong>? standardIds,
            IReadOnlySet<ulong> reinforceLessonIds,
            IReadOnlySet<ulong> reinforceStandardIds,
            IReadOnlySet<ulong>? matchedLessonIds,
            bool onlyMatched)
        {
            var lessons = new List<(AthenasCatalogLesson Lesson, ulong? Standard, int Index)>();
            for (var i = 0; i < catalog.Count; i++)
            {
                var lesson = catalog[i];
                ulong? standard = null;
                if (standardIds != null)
                {
                    standard = lesson.CodeIds.Where(standardIds.Contains).Select(id => (ulong?)id).FirstOrDefault();
                    if (standard == null)
                    {
                        continue;
                    }
                }

                lessons.Add((lesson, standard, i));
            }

            var anyMatched = matchedLessonIds is { Count: > 0 } && lessons.Any(l => matchedLessonIds.Contains(l.Lesson.LessonId));
            if (anyMatched && onlyMatched)
            {
                lessons = lessons.Where(l => matchedLessonIds!.Contains(l.Lesson.LessonId)).ToList();
            }

            int Rank((AthenasCatalogLesson Lesson, ulong? Standard, int Index) l)
            {
                if (anyMatched && matchedLessonIds!.Contains(l.Lesson.LessonId))
                {
                    return 0;
                }

                if (reinforceLessonIds.Contains(l.Lesson.LessonId))
                {
                    return 1;
                }

                return l.Lesson.CodeIds.Any(reinforceStandardIds.Contains) ? 2 : 3;
            }

            return lessons.OrderBy(Rank).ThenBy(l => l.Index).Select(l => new PlanCandidateLesson(l.Lesson.LessonId, l.Lesson.Title, l.Standard)).ToList();
        }

        /// <summary>
        /// The catalog lessons that share words with the text (title words weigh 3, description words 1), best first.
        /// Used to narrow the catalog before the model picks among real lessons.
        /// </summary>
        public static List<AthenasCatalogLesson> MatchByKeywords(IReadOnlyList<AthenasCatalogLesson> catalog, string? text, int max)
        {
            var terms = Words(text).ToHashSet();
            if (terms.Count == 0)
            {
                return new List<AthenasCatalogLesson>();
            }

            return catalog
                .Select((lesson, index) => (Lesson: lesson, Index: index, Score:
                    Words(lesson.Title).Distinct().Count(terms.Contains) * 3 + Words(lesson.Description).Distinct().Count(terms.Contains)))
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .ThenBy(x => x.Index)
                .Take(max)
                .Select(x => x.Lesson)
                .ToList();
        }

        /// <summary>
        /// Schedules the lessons in order: lesson i takes the next <see cref="SessionsPerLesson"/> sessions (the last lesson
        /// also takes the sessions left over). Learning: first to last session. Pre-test on the first session; practices
        /// spread over the sessions, each due the next day; exams and post-test on the last session.
        /// </summary>
        /// <param name="schedule">Days, times and length of the plan.</param>
        /// <param name="subjectIds">Subjects of the plan.</param>
        /// <param name="lessons">Lessons in the order they are studied; only the first <c>Capacity</c> are used.</param>
        /// <param name="settings">Minutes per lesson, durations and deadlines.</param>
        /// <returns>The structure and its numbers.</returns>
        public static PlanBuildResult Build(PlanBuilderSchedule schedule, IReadOnlyList<ulong> subjectIds, IReadOnlyList<PlanBuilderLesson> lessons, CustomPlanAssistantSettings settings)
        {
            var sessions = SessionDates(schedule.StartDate, schedule.Weeks, schedule.Days);
            var perLesson = SessionsPerLesson(settings.MinutesPerLesson, schedule.DurationMinutes);
            var capacity = Capacity(sessions.Count, perLesson);
            var zone = Zone(schedule.Timezone);

            var structure = new CustomPlanStructureRequestModel
            {
                SubjectIds = subjectIds.Distinct().ToList(),
                Days = schedule.Days.Distinct().OrderBy(d => d).Select(d => new CustomPlanDayRequestModel
                {
                    DayOfWeek = d,
                    Times = new List<CustomPlanDayTimeRequestModel>
                    {
                        new() { StartTime = schedule.StartTime, DurationMinutes = schedule.DurationMinutes, Timezone = schedule.Timezone },
                    },
                }).ToList(),
            };

            var chosen = lessons.Take(capacity).ToList();
            for (var i = 0; i < chosen.Count; i++)
            {
                var first = i * perLesson;
                var count = i == chosen.Count - 1 && chosen.Count == capacity ? sessions.Count - first : Math.Min(perLesson, sessions.Count - first);
                var dates = sessions.Skip(first).Take(count).ToList();
                structure.Lessons.Add(ScheduleLesson(chosen[i], (ushort)(i + 1), dates, schedule.Timezone, zone, settings));
            }

            return new PlanBuildResult(structure, sessions, perLesson, capacity, EndDate(schedule.StartDate, schedule.Weeks));
        }

        private static CustomPlanLessonRequestModel ScheduleLesson(PlanBuilderLesson lesson, ushort sequence, List<DateOnly> dates, string timezone, TimeZoneInfo zone, CustomPlanAssistantSettings settings)
        {
            var firstDate = dates[0];
            var lastDate = dates[^1];
            var model = new CustomPlanLessonRequestModel
            {
                LessonId = lesson.LessonId,
                SubjectId = lesson.SubjectId,
                StandardId = lesson.StandardId,
                Sequence = sequence,
                LearningAssignedDate = firstDate,
                LearningDueDate = lastDate,
                LearningTimezone = timezone,
                AssignedDates = dates,
            };

            if (lesson.PreTestId is { } pre)
            {
                model.Tests.Add(new CustomPlanTestRequestModel
                {
                    TestType = PlanTestType.PreTest,
                    TestId = pre,
                    AssignedDate = firstDate,
                    DueDateTime = ToUtc(firstDate, settings.PreTestDueTime, zone),
                    Timezone = timezone,
                    DurationMinutes = settings.TestDurationMinutes,
                });
            }

            var practices = lesson.PracticeIds.Distinct().Take(byte.MaxValue).ToList();
            for (var j = 0; j < practices.Count; j++)
            {
                var date = dates[j * dates.Count / practices.Count];
                model.Practices.Add(new CustomPlanPracticeRequestModel
                {
                    PracticeId = practices[j],
                    Sequence = (byte)(j + 1),
                    AssignedDate = date,
                    DueDateTime = ToUtc(date.AddDays(1), settings.PracticeDueTime, zone),
                    Timezone = timezone,
                });
            }

            foreach (var exam in lesson.ExamIds.Distinct())
            {
                model.Exams.Add(new CustomPlanExamRequestModel
                {
                    ExamId = exam,
                    AssignedDate = lastDate,
                    DueDateTime = ToUtc(lastDate, settings.ExamDueTime, zone),
                    Timezone = timezone,
                    DurationMinutes = settings.ExamDurationMinutes,
                });
            }

            if (lesson.PostTestId is { } post)
            {
                model.Tests.Add(new CustomPlanTestRequestModel
                {
                    TestType = PlanTestType.PostTest,
                    TestId = post,
                    AssignedDate = lastDate,
                    DueDateTime = ToUtc(lastDate, settings.PostTestDueTime, zone),
                    Timezone = timezone,
                    DurationMinutes = settings.TestDurationMinutes,
                });
            }

            return model;
        }

        /// <summary>A local date and time in the zone, as UTC (e.g. 23:00 in Puerto Rico = 03:00Z the next day).</summary>
        public static DateTime ToUtc(DateOnly date, TimeOnly time, TimeZoneInfo zone) =>
            TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(date.ToDateTime(time), DateTimeKind.Unspecified), zone);

        /// <summary>The time zone by IANA or Windows id; UTC when unknown.</summary>
        public static TimeZoneInfo Zone(string? timezone) =>
            !string.IsNullOrWhiteSpace(timezone) && TimeZoneInfo.TryFindSystemTimeZoneById(timezone, out var zone) ? zone : TimeZoneInfo.Utc;

        /// <summary>Lower-case words of more than two letters, without accents.</summary>
        public static IEnumerable<string> Words(string? text)
        {
            var normalized = (text ?? string.Empty).Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder(normalized.Length);
            foreach (var c in normalized)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category == UnicodeCategory.NonSpacingMark)
                {
                    continue;
                }

                builder.Append(char.IsLetterOrDigit(c) ? char.ToLowerInvariant(c) : ' ');
            }

            return builder.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(w => w.Length > 2 && !StopWords.Contains(w));
        }

        private static readonly HashSet<string> StopWords = new(StringComparer.Ordinal)
        {
            "the", "and", "for", "with", "una", "uno", "unos", "unas", "los", "las", "del", "que", "con", "por", "para", "como",
            "mas", "sus", "esta", "este", "quiero", "necesita", "trabajar", "hijo", "hija", "want", "needs", "work", "child",
        };
    }
}
