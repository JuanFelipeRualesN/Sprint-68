using System.Collections.Concurrent;
using System.Globalization;
using GSWebAPI.GSWebAPI.Domain.Models.CustomPlans;
using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.External;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using Microsoft.EntityFrameworkCore;

namespace GSWebAPI.GSWebAPI.Services.CustomPlans
{
    /// <summary>Read model of the plans of a family, for calendars, dashboards and reports.</summary>
    public interface ICustomPlanOverviewService
    {
        /// <summary>
        /// Student → plan → subject → lesson overview. A client gets the plans they own (optionally only some students);
        /// staff must name the students and get all their plans.
        /// </summary>
        /// <param name="user">The calling user.</param>
        /// <param name="filter">Optional students, plan statuses, date window [DateFrom, DateTo] and lesson score range (0-100).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// One entry per student (for a client, only students they own plans for), with lesson completion, scores,
        /// Athenas practices/exams missing from the plan, whether each lesson can be completed (null when Athenas
        /// cannot be reached) and evidences. Validation errors: <see cref="ServiceErrorCodes.PlanOverviewDatesInvalid"/>,
        /// <see cref="ServiceErrorCodes.PlanOverviewPlanDatesInvalid"/>,
        /// <see cref="ServiceErrorCodes.PlanOverviewScoresInvalid"/>, <see cref="ServiceErrorCodes.PlanOverviewStudentsRequired"/> (staff).
        /// </returns>
        Task<ServiceResult<List<CustomPlanStudentOverviewModel>>> GetOverviewAsync(RequestUser user, CustomPlanOverviewFilterModel filter, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Builds student → plan → subject → lesson, with every area (learning, each practice, exams, pre/post tests), its dates, time zones,
    /// completion and scores, the framework and standard, the Athenas titles and names, and the evidences.
    /// <list type="bullet">
    /// <item>A client sees the plans they own (one or many children). Staff must name the students.</item>
    /// <item>Dates: a lesson is kept when any of its areas or session dates overlaps [DateFrom, DateTo] (practice, exam
    /// and test deadlines read in each activity's time zone).</item>
    /// <item>Scores: lesson score = average of its linked exam results (usqui total_percent). With a score filter,
    /// lessons without a score are left out.</item>
    /// <item>When a date or score filter is set, subjects and plans with no matching lesson are left out.</item>
    /// <item>Athenas and the Users API are display data: if they fail, titles and names are null.</item>
    /// <item>Performance: the filters run on the database data first, Athenas is only asked about the lessons that are
    /// returned, and all the remote reads (names, lessons, subjects, levels, standards, lesson quizzes) run at the same time.
    /// The response waits for them at most <see cref="DisplayDataWait"/>; what is not ready is left empty and fills the
    /// cache for the next request.</item>
    /// </list>
    /// </summary>
    public sealed class CustomPlanOverviewService : ICustomPlanOverviewService
    {
        /// <summary>Time zones by id (the activity time zones are a handful of IANA ids).</summary>
        private static readonly ConcurrentDictionary<string, TimeZoneInfo> Zones = new();

        /// <summary>How long the overview waits for the display data (Users API names and Athenas) before answering without it.</summary>
        public static readonly TimeSpan DisplayDataWait = TimeSpan.FromSeconds(3);

        // Open ends of the lesson window in the database query (dates any plan can have, safe for MySQL DATE/DATETIME).
        private static readonly DateOnly WindowStart = new(1900, 1, 2);
        private static readonly DateOnly WindowEnd = new(9999, 12, 31);

        private readonly GSWebDbContext _db;
        private readonly IUserDirectoryClient _users;
        private readonly IAthenasCatalogClient _athenas;
        private readonly IBlobFileStorage _storage;
        private readonly TimeProvider _timeProvider;

        /// <summary>Creates the service.</summary>
        /// <param name="db">GSWeb database context.</param>
        /// <param name="users">Users API client, for student names.</param>
        /// <param name="athenas">Athenas client, for lesson, subject, level, standard and quiz details.</param>
        /// <param name="storage">Evidence storage, to sign read links of uploaded files.</param>
        /// <param name="timeProvider">Clock for overdue flags.</param>
        public CustomPlanOverviewService(GSWebDbContext db, IUserDirectoryClient users, IAthenasCatalogClient athenas, IBlobFileStorage storage, TimeProvider timeProvider)
        {
            _db = db;
            _users = users;
            _athenas = athenas;
            _storage = storage;
            _timeProvider = timeProvider;
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<List<CustomPlanStudentOverviewModel>>> GetOverviewAsync(RequestUser user, CustomPlanOverviewFilterModel filter, CancellationToken cancellationToken = default)
        {
            filter ??= new CustomPlanOverviewFilterModel();
            var studentIds = (filter.StudentIds ?? new List<ulong>())
                .Concat(filter.StudentId is { } single ? new[] { single } : Array.Empty<ulong>())
                .Where(id => id != 0)
                .Distinct()
                .ToList();
            var statuses = (filter.Statuses ?? new List<CustomPlanStatus>()).Distinct().ToList();

            var errors = new List<string>();
            if (!string.IsNullOrWhiteSpace(filter.Month))
            {
                // A calendar month is the lesson window from its first to its last day.
                if (filter.DateFrom.HasValue || filter.DateTo.HasValue
                    || !DateOnly.TryParseExact(filter.Month.Trim(), "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out var firstDay))
                {
                    errors.Add(ServiceErrorCodes.PlanOverviewMonthInvalid);
                }
                else
                {
                    filter = WithWindow(filter, firstDay, firstDay.AddMonths(1).AddDays(-1));
                }
            }

            if (filter.DateFrom.HasValue && filter.DateTo.HasValue && filter.DateTo < filter.DateFrom)
            {
                errors.Add(ServiceErrorCodes.PlanOverviewDatesInvalid);
            }

            if (filter.PlanDateFrom.HasValue && filter.PlanDateTo.HasValue && filter.PlanDateTo < filter.PlanDateFrom)
            {
                errors.Add(ServiceErrorCodes.PlanOverviewPlanDatesInvalid);
            }

            if (filter.MinScore is < 0 or > 100 || filter.MaxScore is < 0 or > 100
                || (filter.MinScore.HasValue && filter.MaxScore.HasValue && filter.MaxScore < filter.MinScore))
            {
                errors.Add(ServiceErrorCodes.PlanOverviewScoresInvalid);
            }

            if (user.IsStaff && studentIds.Count == 0)
            {
                errors.Add(ServiceErrorCodes.PlanOverviewStudentsRequired);
            }

            if (errors.Count > 0)
            {
                return ServiceResult<List<CustomPlanStudentOverviewModel>>.Fail(ServiceErrorKind.Validation, errors);
            }

            // Which plans: ownership, students, statuses and the plan window (in the database, so old plans and their
            // lessons are never loaded).
            var visible = _db.CustomPlanClients.AsNoTracking();
            visible = user.IsStaff ? visible : visible.Where(p => p.ClientId == user.UserId);
            if (studentIds.Count > 0)
            {
                visible = visible.Where(p => studentIds.Contains(p.StudentId));
            }

            if (statuses.Count > 0)
            {
                visible = visible.Where(p => statuses.Contains(p.Status));
            }

            if (filter.PlanDateTo is { } planDateTo)
            {
                visible = visible.Where(p => p.StartDate <= planDateTo);
            }

            if (filter.PlanDateFrom is { } planDateFrom)
            {
                visible = visible.Where(p => p.EndDate == null || p.EndDate >= planDateFrom);
            }

            var lessonWindowSet = filter.DateFrom.HasValue || filter.DateTo.HasValue;

            // With a lesson window, plans with nothing in it are not loaded; a client's children are read apart (ids
            // only) so a child with nothing in the window is still listed, with no plans.
            List<ulong>? childrenOfClient = lessonWindowSet && !user.IsStaff
                ? await visible.Select(p => p.StudentId).Distinct().ToListAsync(cancellationToken)
                : null;

            IQueryable<CustomPlanClient> query = visible
                .Include(p => p.CustomPlanType)
                .Include(p => p.CurriculumFramework)
                .Include(p => p.Subjects)
                .Include(p => p.Evidences);
            if (lessonWindowSet)
            {
                // Lesson window (dateFrom/dateTo or month): only the lessons that can overlap it are loaded, and only the
                // plans that have one, so a month calendar does not load every lesson of every plan. Activity deadlines
                // are UTC and the window is in local dates, so the database check has one day of margin; the exact
                // check (in each activity's time zone) runs after loading (OverlapsWindow).
                var from = filter.DateFrom ?? WindowStart;
                var to = filter.DateTo ?? WindowEnd;
                var fromUtc = from.AddDays(-1).ToDateTime(TimeOnly.MinValue);
                query = query
                    .Where(p => p.Lessons.Any(l =>
                        (l.LearningAssignedDate <= to && l.LearningDueDate >= from)
                        || l.Dates.Any(d => d.AssignedDate >= from && d.AssignedDate <= to)
                        || l.Practices.Any(a => a.AssignedDate <= to && a.DueDateTime >= fromUtc)
                        || l.ExamDates.Any(a => a.AssignedDate <= to && a.DueDateTime >= fromUtc)
                        || l.Tests.Any(a => a.AssignedDate <= to && a.DueDateTime >= fromUtc)))
                    .Include(p => p.Lessons.Where(l =>
                        (l.LearningAssignedDate <= to && l.LearningDueDate >= from)
                        || l.Dates.Any(d => d.AssignedDate >= from && d.AssignedDate <= to)
                        || l.Practices.Any(a => a.AssignedDate <= to && a.DueDateTime >= fromUtc)
                        || l.ExamDates.Any(a => a.AssignedDate <= to && a.DueDateTime >= fromUtc)
                        || l.Tests.Any(a => a.AssignedDate <= to && a.DueDateTime >= fromUtc)))
                    .ThenInclude(l => l.Dates);
            }
            else
            {
                query = query.Include(p => p.Lessons).ThenInclude(l => l.Dates);
            }

            // EF applies the lesson filter above to these includes of the same navigation too.
            query = query
                .Include(p => p.Lessons).ThenInclude(l => l.ExamDates)
                .Include(p => p.Lessons).ThenInclude(l => l.Practices)
                .Include(p => p.Lessons).ThenInclude(l => l.Tests)
                .AsSplitQuery();

            var plans = await query.OrderBy(p => p.StudentId).ThenByDescending(p => p.StartDate).ThenByDescending(p => p.Id).ToListAsync(cancellationToken);
            var results = await LoadExamResultsAsync(plans.SelectMany(p => p.Lessons).ToList(), cancellationToken);

            // The date and score filters only need the database, so they run before any remote call: Athenas is only
            // asked about the lessons that are returned, not about every lesson the family ever had.
            var lessonFilterSet = filter.DateFrom.HasValue || filter.DateTo.HasValue || filter.MinScore.HasValue || filter.MaxScore.HasValue;
            var keptLessons = plans.ToDictionary(
                p => p.Id,
                p => p.Lessons.Where(l => PassesFilter(l, filter, results)).OrderBy(l => l.Sequence).ThenBy(l => l.Id).ToList());
            var shownPlans = plans.Where(p => !lessonFilterSet || keptLessons[p.Id].Count > 0).ToList();
            var lessons = shownPlans.SelectMany(p => keptLessons[p.Id]).ToList();
            var lessonIds = lessons.Select(l => l.LessonId).Distinct().ToList();

            // Staff get every student they asked for. A client only gets children they own plans for, so an id that is
            // not theirs reveals nothing (not even the name).
            var plansByStudent = plans.ToLookup(p => p.StudentId);
            var ownedStudents = childrenOfClient?.ToHashSet() ?? plansByStudent.Select(g => g.Key).ToHashSet();
            var studentOrder = (studentIds.Count > 0 ? studentIds : ownedStudents.Order().ToList())
                .Where(id => user.IsStaff || ownedStudents.Contains(id))
                .ToList();

            // The remote reads don't depend on each other: they run at the same time, so the response waits for the
            // slowest one instead of the sum of all of them.
            IReadOnlyDictionary<ulong, AthenasCatalogEntry> noCatalog = new Dictionary<ulong, AthenasCatalogEntry>();
            var namesTask = _users.GetFullNamesAsync(user, studentOrder, cancellationToken);
            var athenasLessonsTask = _athenas.GetLessonsAsync(lessonIds, cancellationToken);
            var subjectsTask = shownPlans.Count > 0 ? _athenas.GetSubjectsAsync(user.LangCode, cancellationToken) : Task.FromResult(noCatalog);
            var levelsTask = shownPlans.Count > 0 ? _athenas.GetLevelsAsync(user.LangCode, cancellationToken) : Task.FromResult(noCatalog);
            var standardsTask = _athenas.GetStandardsAsync(lessons.Where(l => l.StandardId.HasValue).Select(l => l.StandardId!.Value), user.LangCode, cancellationToken);
            // Per lesson, so the overview can tell which Athenas practices/exams are missing from the plan.
            var lessonQuizzesTask = _athenas.TryGetLessonQuizzesAsync(lessonIds, cancellationToken);

            // Display data never holds the response for long: what is not ready after the wait is left empty (null
            // names, titles, standards; CanBeCompleted null), and the pending reads still complete and fill the
            // Athenas cache for the next request.
            var all = Task.WhenAll(namesTask, athenasLessonsTask, subjectsTask, levelsTask, standardsTask, lessonQuizzesTask);
            await Task.WhenAny(all, Task.Delay(DisplayDataWait, cancellationToken));
            cancellationToken.ThrowIfCancellationRequested();
            ObserveFailures(all);

            var names = ResultOr(namesTask, new Dictionary<ulong, string>());
            var levels = ResultOr(levelsTask, noCatalog);
            var lessonQuizzes = ResultOr<IReadOnlyDictionary<ulong, IReadOnlyDictionary<ulong, AthenasQuizInfo>?>>(lessonQuizzesTask, new Dictionary<ulong, IReadOnlyDictionary<ulong, AthenasQuizInfo>?>());
            var quizzes = lessonQuizzes.Values.Where(q => q != null).SelectMany(q => q!)
                .GroupBy(q => q.Key).ToDictionary(g => g.Key, g => g.First().Value);

            var context = new BuildContext(filter, results,
                ResultOr<IReadOnlyDictionary<ulong, AthenasLessonInfo>>(athenasLessonsTask, new Dictionary<ulong, AthenasLessonInfo>()),
                ResultOr(subjectsTask, noCatalog), ResultOr(standardsTask, noCatalog),
                quizzes, lessonQuizzes, _timeProvider.GetUtcNow().UtcDateTime);

            var students = new List<CustomPlanStudentOverviewModel>();
            foreach (var studentId in studentOrder)
            {
                var model = new CustomPlanStudentOverviewModel { StudentId = studentId, StudentName = names.GetValueOrDefault(studentId) };
                foreach (var plan in plansByStudent[studentId])
                {
                    if (!lessonFilterSet || keptLessons[plan.Id].Count > 0)
                    {
                        model.Plans.Add(BuildPlan(plan, keptLessons[plan.Id], levels, context, lessonFilterSet));
                    }
                }

                students.Add(model);
            }

            return ServiceResult<List<CustomPlanStudentOverviewModel>>.Ok(students);
        }

        /// <summary>A copy of the filter with the lesson window set (the caller's object is not changed).</summary>
        private static CustomPlanOverviewFilterModel WithWindow(CustomPlanOverviewFilterModel filter, DateOnly from, DateOnly to) => new()
        {
            StudentIds = filter.StudentIds,
            StudentId = filter.StudentId,
            DateFrom = from,
            DateTo = to,
            PlanDateFrom = filter.PlanDateFrom,
            PlanDateTo = filter.PlanDateTo,
            MinScore = filter.MinScore,
            MaxScore = filter.MaxScore,
            Statuses = filter.Statuses,
        };

        /// <summary>The result of a remote read that finished successfully, or <paramref name="fallback"/> (still running or failed).</summary>
        private static T ResultOr<T>(Task<T> task, T fallback) => task.IsCompletedSuccessfully ? task.Result : fallback;

        /// <summary>Observes a failure of reads that finish after the response, so it is not an unobserved task exception.</summary>
        private static void ObserveFailures(Task task) =>
            task.ContinueWith(t => _ = t.Exception, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

        /// <summary>True when the lesson passes the score and date filters (each one only when it is set).</summary>
        private static bool PassesFilter(CustomPlanClientLesson lesson, CustomPlanOverviewFilterModel filter, IReadOnlyDictionary<int, UserQuizResultRef> results)
        {
            if (filter.MinScore.HasValue || filter.MaxScore.HasValue)
            {
                var score = LessonScore(lesson, results);
                if (score == null || score < filter.MinScore || score > filter.MaxScore)
                {
                    return false;
                }
            }

            return (!filter.DateFrom.HasValue && !filter.DateTo.HasValue) || OverlapsWindow(lesson, filter);
        }

        /// <summary>Lesson score: average of its linked exam results (usqui total_percent), null when there are none.</summary>
        private static double? LessonScore(CustomPlanClientLesson lesson, IReadOnlyDictionary<int, UserQuizResultRef> results) =>
            Average(lesson.ExamDates.Select(e => e.GwpUserResultId is { } id && results.TryGetValue(id, out var result) ? Percent(result.TotalPercent) : null));

        /// <summary>The linked results of exams and pre/post tests.</summary>
        private async Task<Dictionary<int, UserQuizResultRef>> LoadExamResultsAsync(List<CustomPlanClientLesson> lessons, CancellationToken cancellationToken)
        {
            var ids = lessons.SelectMany(l => l.ExamDates).Select(e => e.GwpUserResultId)
                .Concat(lessons.SelectMany(l => l.Tests).Select(t => t.GwpUserResultId))
                .Where(id => id.HasValue).Select(id => id!.Value).Distinct().ToList();
            if (ids.Count == 0)
            {
                return new Dictionary<int, UserQuizResultRef>();
            }

            return await _db.UserQuizResults.AsNoTracking().Where(r => ids.Contains(r.Id)).ToDictionaryAsync(r => r.Id, cancellationToken);
        }

        /// <summary>One plan with the lessons that passed the filters (already in sequence order).</summary>
        private CustomPlanOverviewModel BuildPlan(CustomPlanClient plan, List<CustomPlanClientLesson> keptLessons, IReadOnlyDictionary<ulong, AthenasCatalogEntry> levels, BuildContext context, bool lessonFilterSet)
        {
            // Plan evidences under the null key, lesson evidences under the lesson id.
            var evidences = plan.Evidences.OrderByDescending(e => e.CreatedAt).ToLookup(e => e.CustomPlanClientLessonId);
            var built = keptLessons.Select(l => (l.SubjectId, Model: BuildLesson(l, evidences[l.Id], context))).ToList();
            var lessons = built.Select(l => l.Model).ToList();

            var level = levels.GetValueOrDefault(plan.LevelId);
            var model = new CustomPlanOverviewModel
            {
                Id = plan.Id,
                CustomPlanTypeCode = plan.CustomPlanType?.Code,
                Status = plan.Status,
                TotalStudyMinutes = plan.TotalStudyMinutes,
                StartDate = plan.StartDate,
                EndDate = plan.EndDate,
                PlanDurationWeeks = plan.PlanDurationWeeks,
                Level = new CustomPlanCatalogItemModel { Id = plan.LevelId, Code = level?.Code, Name = level?.Name },
                UsesFramework = plan.CurriculumFramework != null,
                Framework = plan.CurriculumFramework is { } framework
                    ? new CustomPlanFrameworkModel { Id = framework.Id, Code = framework.Code, Name = framework.Name, CountryCode = framework.CountryCode, Region = framework.Region }
                    : null,
                Evidences = evidences[null].Select(e => CustomPlanEvidenceService.Map(e, _storage)).ToList(),
            };

            // The plan's subjects by subject id, then any subject only referenced by a lesson.
            var subjectIds = plan.Subjects.Select(s => s.SubjectId).OrderBy(id => id).ToList();
            var listed = subjectIds.ToHashSet();
            subjectIds.AddRange(plan.Lessons.Select(l => l.SubjectId).Where(listed.Add).ToList());
            var lessonsBySubject = built.ToLookup(l => l.SubjectId, l => l.Model);
            foreach (var subjectId in subjectIds)
            {
                var subjectLessons = lessonsBySubject[subjectId].ToList();
                if (lessonFilterSet && subjectLessons.Count == 0)
                {
                    continue;
                }

                var subject = context.Subjects.GetValueOrDefault(subjectId);
                model.Subjects.Add(new CustomPlanSubjectOverviewModel
                {
                    Subject = new CustomPlanCatalogItemModel { Id = subjectId, Code = subject?.Code, Name = subject?.Name },
                    AverageScore = Average(subjectLessons.Select(l => l.Score)),
                    Lessons = subjectLessons,
                });
            }

            model.AverageScore = Average(lessons.Select(l => l.Score));
            return model;
        }

        /// <summary>One lesson that passed the filters, with its evidences (newest first).</summary>
        private CustomPlanLessonOverviewModel BuildLesson(CustomPlanClientLesson lesson, IEnumerable<CustomPlanEvidence> evidences, BuildContext context)
        {
            var exams = lesson.ExamDates.OrderBy(e => e.DueDateTime).ThenBy(e => e.Id).Select(e => BuildExam(e, context)).ToList();
            var tests = lesson.Tests.OrderBy(t => t.TestType).ThenBy(t => t.DueDateTime).Select(t => BuildTest(t, context)).ToList();
            var score = Average(exams.Select(e => e.Result?.Score));
            var practices = lesson.Practices.OrderBy(p => p.Sequence).ThenBy(p => p.Id)
                .Select(p => BuildPractice(p, context)).ToList();
            var athenas = context.Lessons.GetValueOrDefault(lesson.LessonId);
            var standard = lesson.StandardId is { } standardId ? context.Standards.GetValueOrDefault(standardId) : null;
            var quizzesKnown = context.LessonQuizzes.GetValueOrDefault(lesson.LessonId) != null;
            var missingPractices = Missing(lesson, context, AthenasQuizInfo.Practice, lesson.Practices.Select(p => p.PracticeId));
            var missingExams = Missing(lesson, context, AthenasQuizInfo.Exam, lesson.ExamDates.Select(e => e.ExamId));

            return new CustomPlanLessonOverviewModel
            {
                Id = lesson.Id,
                LessonId = lesson.LessonId,
                Title = athenas?.Title,
                LessonNo = athenas?.LessonNo,
                SubjectCode = athenas?.SubjectCode,
                LevelCode = athenas?.LevelCode,
                Standard = lesson.StandardId is { } id ? new CustomPlanStandardModel { Id = id, Code = standard?.Code, Description = standard?.Name } : null,
                Sequence = lesson.Sequence,
                LessonStatus = lesson.LessonStatus,
                CompletedAt = lesson.CompletedAt,
                TotalStudyMinutes = lesson.TotalStudyMinutes,
                Score = score,
                Learning = new CustomPlanLearningAreaModel
                {
                    AssignedDate = lesson.LearningAssignedDate,
                    DueDate = lesson.LearningDueDate,
                    Timezone = lesson.LearningTimezone,
                    ConceptCompleted = lesson.ConceptCompleted,
                    DefinitionsCompleted = lesson.DefinitionsCompleted,
                    ExamplesCompleted = lesson.ExamplesCompleted,
                    IsCompleted = lesson.LearningCompletedAt != null,
                    CompletedAt = lesson.LearningCompletedAt,
                    IsOverdue = lesson.LearningCompletedAt == null && LocalToday(lesson.LearningTimezone, context.UtcNow) > lesson.LearningDueDate,
                },
                Practices = new CustomPlanPracticesSummaryModel
                {
                    Total = practices.Count,
                    Completed = practices.Count(p => p.IsCompleted),
                    IsCompleted = practices.Count > 0 && practices.All(p => p.IsCompleted),
                    CompletedAt = practices.Count > 0 && practices.All(p => p.IsCompleted) ? practices.Max(p => p.CompletedAt) : null,
                    Overdue = practices.Count(p => p.IsOverdue),
                    AverageScore = Average(practices.Select(p => p.Score)),
                    Items = practices,
                },
                Exams = exams,
                Tests = tests,
                MissingPracticeIds = missingPractices,
                MissingExamIds = missingExams,
                CanBeCompleted = quizzesKnown
                    ? practices.All(p => p.IsCompleted) && exams.All(e => e.IsCompleted) && missingPractices.Count == 0 && missingExams.Count == 0
                    : null,
                PreCompleted = lesson.PreCompleted,
                TestCompleted = lesson.TestCompleted,
                PostCompleted = lesson.PostCompleted,
                SessionDates = lesson.Dates.Select(d => d.AssignedDate).OrderBy(d => d).ToList(),
                Evidences = evidences.Select(e => CustomPlanEvidenceService.Map(e, _storage)).ToList(),
            };
        }

        private static CustomPlanPracticeOverviewModel BuildPractice(CustomPlanClientLessonPractice practice, BuildContext context)
        {
            var quiz = context.Quizzes.GetValueOrDefault(practice.PracticeId);
            return Fill(new CustomPlanPracticeOverviewModel
            {
                PracticeId = practice.PracticeId,
                LevelOfKnowledge = quiz?.LevelOfKnowledge,
                Sequence = practice.Sequence,
                Score = practice.Score.HasValue ? (double)practice.Score.Value : null,
                BestScore = practice.BestScore.HasValue ? (double)practice.BestScore.Value : null,
                TotalCorrect = practice.TotalCorrect,
                TotalQuestions = practice.TotalQuestions,
                Attempts = practice.Attempts,
                LastAttemptAt = practice.LastAttemptAt,
            }, practice, practice.Id, quiz?.Title, context);
        }

        private static CustomPlanTestOverviewModel BuildTest(CustomPlanClientLessonTest test, BuildContext context)
        {
            return Fill(new CustomPlanTestOverviewModel
            {
                TestType = test.TestType,
                TestId = test.TestId,
                DurationMinutes = test.DurationMinutes,
                Result = MapResult(test.GwpUserResultId, context),
            }, test, test.Id, context.Quizzes.GetValueOrDefault(test.TestId)?.Title, context);
        }

        /// <summary>Fills the fields every activity has: schedule, status, completion and overdue.</summary>
        private static TModel Fill<TModel>(TModel model, IPlanActivity activity, ulong id, string? title, BuildContext context)
            where TModel : CustomPlanActivityOverviewModel
        {
            var completed = activity.Status == PlanActivityStatus.Completed;
            model.Id = id;
            model.Title = title;
            model.AssignedDate = activity.AssignedDate;
            model.DueDateTime = activity.DueDateTime;
            model.Timezone = activity.Timezone;
            model.Status = activity.Status;
            model.IsCompleted = completed;
            model.CompletedAt = activity.CompletedAt;
            model.IsOverdue = !completed && context.UtcNow > activity.DueDateTime;
            return model;
        }

        /// <summary>Quizzes of that kind the lesson has in Athenas that the plan does not schedule.</summary>
        private static List<ulong> Missing(CustomPlanClientLesson lesson, BuildContext context, string kind, IEnumerable<ulong> scheduled)
        {
            var quizzes = context.LessonQuizzes.GetValueOrDefault(lesson.LessonId);
            if (quizzes == null)
            {
                return new List<ulong>();
            }

            var inPlan = scheduled.ToHashSet();
            return quizzes.Where(q => q.Value.IsExam == kind && !inPlan.Contains(q.Key)).Select(q => q.Key).OrderBy(id => id).ToList();
        }

        private static CustomPlanExamResultModel? MapResult(int? resultId, BuildContext context)
        {
            var result = resultId is { } id ? context.Results.GetValueOrDefault(id) : null;
            return result == null ? null : new CustomPlanExamResultModel
            {
                ResultId = result.Id,
                TryNo = result.TryNo,
                TotalCorrect = result.TotalCorrect,
                TotalQuestions = result.TotalQuestions,
                Score = Percent(result.TotalPercent),
                TakenAt = result.CreatedAt,
            };
        }

        private static CustomPlanExamOverviewModel BuildExam(CustomPlanClientLessonExamDate exam, BuildContext context)
        {
            return Fill(new CustomPlanExamOverviewModel
            {
                ExamId = exam.ExamId,
                DurationMinutes = exam.DurationMinutes,
                Result = MapResult(exam.GwpUserResultId, context),
            }, exam, exam.Id, context.Quizzes.GetValueOrDefault(exam.ExamId)?.Title, context);
        }

        /// <summary>True when any area of the lesson overlaps the filter window (open ends allowed).</summary>
        /// <param name="lesson">The plan lesson, with its dates, practices, exam dates and tests loaded.</param>
        /// <param name="filter">Filter with the optional DateFrom / DateTo.</param>
        /// <returns>True when the learning range, a session date, or any activity's assigned-to-due range overlaps the window.</returns>
        internal static bool OverlapsWindow(CustomPlanClientLesson lesson, CustomPlanOverviewFilterModel filter)
        {
            var from = filter.DateFrom ?? DateOnly.MinValue;
            var to = filter.DateTo ?? DateOnly.MaxValue;
            bool Overlaps(DateOnly start, DateOnly end) => start <= to && end >= from;

            return Overlaps(lesson.LearningAssignedDate, lesson.LearningDueDate)
                || lesson.Practices.Any(p => Overlaps(p.AssignedDate, LocalDate(p.DueDateTime, p.Timezone)))
                || lesson.Tests.Any(t => Overlaps(t.AssignedDate, LocalDate(t.DueDateTime, t.Timezone)))
                || lesson.Dates.Any(d => Overlaps(d.AssignedDate, d.AssignedDate))
                || lesson.ExamDates.Any(e => Overlaps(e.AssignedDate, LocalDate(e.DueDateTime, e.Timezone)));
        }

        private static DateOnly LocalToday(string timezone, DateTime utcNow) => LocalDate(utcNow, timezone);

        private static DateOnly LocalDate(DateTime utc, string timezone)
        {
            // An unknown id is not cached by TimeZoneInfo (it is looked up on disk again every time); cache both outcomes.
            var zone = Zones.GetOrAdd(timezone ?? string.Empty, id => TimeZoneInfo.TryFindSystemTimeZoneById(id, out var found) ? found : TimeZoneInfo.Utc);
            return DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), zone));
        }

        private static double? Percent(float? value) => value.HasValue ? Math.Round(value.Value, 2) : null;

        private static double? Average(IEnumerable<double?> values)
        {
            var list = values.Where(v => v.HasValue).Select(v => v!.Value).ToList();
            return list.Count == 0 ? null : Math.Round(list.Average(), 2);
        }

        private sealed record BuildContext(
            CustomPlanOverviewFilterModel Filter,
            IReadOnlyDictionary<int, UserQuizResultRef> Results,
            IReadOnlyDictionary<ulong, AthenasLessonInfo> Lessons,
            IReadOnlyDictionary<ulong, AthenasCatalogEntry> Subjects,
            IReadOnlyDictionary<ulong, AthenasCatalogEntry> Standards,
            IReadOnlyDictionary<ulong, AthenasQuizInfo> Quizzes,
            IReadOnlyDictionary<ulong, IReadOnlyDictionary<ulong, AthenasQuizInfo>?> LessonQuizzes,
            DateTime UtcNow);
    }
}
