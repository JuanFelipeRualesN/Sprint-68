using GSWebAPI.GSWebAPI.Domain.Models.CustomPlans;
using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using Microsoft.EntityFrameworkCore;

namespace GSWebAPI.GSWebAPI.Services.CustomPlans
{
    /// <summary>Custom study plans: one plan per student, owned by the paying client.</summary>
    public interface ICustomPlanService
    {
        /// <summary>
        /// Creates a plan in draft for one of the client's students, with its header (type, level, framework, duration,
        /// dates) and structure (subjects, days and times, lessons with learning window, session dates, practices, exams
        /// and pre/post tests). Only users with the client role; the client is the caller and cannot be the student.
        /// </summary>
        /// <param name="user">The calling user (the client).</param>
        /// <param name="request">Student, header and optional structure.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The created plan. Errors: <see cref="ServiceErrorCodes.PlanOnlyClients"/>, <see cref="ServiceErrorCodes.PlanClientIsStudent"/>,
        /// <see cref="ServiceErrorCodes.PlanStudentNotOfClient"/>, the header/structure/standard validation codes,
        /// Athenas quiz codes (e.g. <see cref="ServiceErrorCodes.PlanPracticeNotInLesson"/>),
        /// <see cref="ServiceErrorCodes.AthenasUnavailable"/> and <see cref="ServiceErrorCodes.UsersApiUnavailable"/>.
        /// </returns>
        Task<ServiceResult<CustomPlanResponseModel>> CreateAsync(RequestUser user, CreateCustomPlanRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists the custom plan types (catalog) ordered by id, with the name in the session's language (Accept-Language:
        /// <c>es</c>, <c>es-PR</c>... use the Spanish name). A language with no translation gets the English name, and a type with
        /// no translation at all its stored name. Any signed-in user.
        /// </summary>
        /// <param name="langCode">Language of the session (Accept-Language); null = English.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The plan types. It does not fail.</returns>
        Task<ServiceResult<List<CustomPlanTypeResponseModel>>> ListTypesAsync(string? langCode, CancellationToken cancellationToken = default);

        /// <summary>Gets a plan with its full structure and the client and student names. The plan's client or staff.</summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The plan, or <see cref="ServiceErrorCodes.PlanNotFound"/> when it does not exist or the caller cannot see it.</returns>
        Task<ServiceResult<CustomPlanResponseModel>> GetAsync(RequestUser user, ulong planId, CancellationToken cancellationToken = default);

        /// <summary>Lists the plans the caller owns as client, newest first.</summary>
        /// <param name="user">The calling user.</param>
        /// <param name="studentId">Optional student filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Plan summaries with client and student names (empty when the caller owns no plans).</returns>
        Task<ServiceResult<List<CustomPlanSummaryModel>>> ListAsync(RequestUser user, ulong? studentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Replaces the header and structure of a draft plan (same rules as <see cref="CreateAsync"/>). Rows whose natural
        /// key is still requested keep their id; the rest are removed. Only the plan's client.
        /// </summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="request">New header and structure.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The updated plan. Errors: <see cref="ServiceErrorCodes.PlanNotFound"/>, <see cref="ServiceErrorCodes.PlanNotDraft"/>,
        /// the validation codes of <see cref="CreateAsync"/> and <see cref="ServiceErrorCodes.AthenasUnavailable"/>.
        /// </returns>
        Task<ServiceResult<CustomPlanResponseModel>> UpdateDraftAsync(RequestUser user, ulong planId, UpdateCustomPlanDraftRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Moves the plan to another status. Only the plan's client. Allowed: draft -> active (needs at least one lesson) |
        /// cancelled; active -> paused | completed | cancelled; paused -> active | cancelled. The same status is a no-op.
        /// </summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="status">Target status.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The plan. Errors: <see cref="ServiceErrorCodes.PlanNotFound"/>, <see cref="ServiceErrorCodes.PlanStatusTransitionInvalid"/>,
        /// <see cref="ServiceErrorCodes.PlanNeedsLessons"/>.
        /// </returns>
        Task<ServiceResult<CustomPlanResponseModel>> ChangeStatusAsync(RequestUser user, ulong planId, CustomPlanStatus status, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the progress flags and status of a plan lesson. Only the plan's client, while the plan is active.
        /// <list type="bullet">
        /// <item>Null fields are left as they are; learning_completed_at follows concept, definitions and examples.</item>
        /// <item>practices_completed cannot be set when the lesson has scheduled practices (it follows them).</item>
        /// <item>Completed requires every practice and exam the lesson has in Athenas to be scheduled and completed;
        /// Athenas is checked live and the request fails closed when it cannot be reached.</item>
        /// </list>
        /// </summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="planLessonId">Id of the plan lesson (not the Athenas lesson id).</param>
        /// <param name="request">Flags and lesson status to change.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The updated lesson. Errors: <see cref="ServiceErrorCodes.PlanNotFound"/>, <see cref="ServiceErrorCodes.PlanLessonNotFound"/>,
        /// <see cref="ServiceErrorCodes.PlanNotActive"/>, <see cref="ServiceErrorCodes.PlanPracticesFlagFollowsPractices"/>,
        /// <see cref="ServiceErrorCodes.PlanLessonPracticesPending"/> / <see cref="ServiceErrorCodes.PlanLessonExamsPending"/>,
        /// <see cref="ServiceErrorCodes.AthenasUnavailable"/>.
        /// </returns>
        Task<ServiceResult<CustomPlanLessonResponseModel>> UpdateLessonProgressAsync(RequestUser user, ulong planId, ulong planLessonId, CustomPlanLessonProgressRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds practices, exams and/or pre/post tests to a lesson of a plan that is not completed or cancelled (the rest
        /// of the structure is locked after draft). Only the plan's client. Same scheduling and Athenas rules as the plan
        /// structure, and no duplicates with what the lesson already has. Adding a practice or exam to a completed lesson
        /// reopens it (in_progress).
        /// </summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="planLessonId">Id of the plan lesson.</param>
        /// <param name="request">Practices, exams and tests to add (at least one).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The updated lesson. Errors: <see cref="ServiceErrorCodes.PlanNotFound"/>, <see cref="ServiceErrorCodes.PlanClosed"/>,
        /// <see cref="ServiceErrorCodes.PlanLessonNotFound"/>, <see cref="ServiceErrorCodes.PlanActivitiesRequired"/>, scheduling,
        /// duplicate and Athenas validation codes, <see cref="ServiceErrorCodes.AthenasUnavailable"/>.
        /// </returns>
        Task<ServiceResult<CustomPlanLessonResponseModel>> AddLessonActivitiesAsync(RequestUser user, ulong planId, ulong planLessonId, CustomPlanLessonActivitiesRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Replaces the weekly study days and time slots (how many days a week, start times and durations) of a plan that
        /// is not completed or cancelled. Only the plan's client. Same rules as the plan structure; lessons are not changed.
        /// </summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="request">The new days and time slots.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The plan. Errors: <see cref="ServiceErrorCodes.PlanNotFound"/>, <see cref="ServiceErrorCodes.PlanClosed"/>,
        /// <see cref="ServiceErrorCodes.PlanDayInvalid"/>, <see cref="ServiceErrorCodes.PlanDayDuplicated"/>,
        /// <see cref="ServiceErrorCodes.PlanTimeDuplicated"/>, <see cref="ServiceErrorCodes.PlanTimeDurationInvalid"/>,
        /// <see cref="ServiceErrorCodes.PlanTimezoneInvalid"/>.
        /// </returns>
        Task<ServiceResult<CustomPlanResponseModel>> UpdateScheduleAsync(RequestUser user, ulong planId, CustomPlanScheduleRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds one selected Athenas lesson to a plan that is not completed or cancelled, with everything the user fills:
        /// the learning window (concept, definitions, examples), session dates, and the practices, exams and pre/post
        /// tests with their dates. Only the plan's client. Same rules as a lesson of the plan structure: the subject must
        /// be one of the plan's, the lesson and its sequence must not already be in the plan, the dates must be inside the
        /// plan, the standard must belong to the plan's framework, and every quiz must be one the lesson has in Athenas.
        /// </summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="request">The lesson and its schedule.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The new plan lesson. Errors: <see cref="ServiceErrorCodes.PlanNotFound"/>, <see cref="ServiceErrorCodes.PlanClosed"/>,
        /// <see cref="ServiceErrorCodes.PlanLessonDuplicated"/>, <see cref="ServiceErrorCodes.PlanLessonSequenceDuplicated"/>,
        /// <see cref="ServiceErrorCodes.PlanLessonSubjectNotInPlan"/>, the learning window, scheduling, standard and Athenas
        /// validation codes, <see cref="ServiceErrorCodes.AthenasUnavailable"/>.
        /// </returns>
        Task<ServiceResult<CustomPlanLessonResponseModel>> AddLessonAsync(RequestUser user, ulong planId, CustomPlanLessonRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>Adds a subject to a plan that is not completed or cancelled. Only the plan's client.</summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="subjectId">Athenas subject id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The plan. Errors: <see cref="ServiceErrorCodes.PlanNotFound"/>, <see cref="ServiceErrorCodes.PlanClosed"/>,
        /// <see cref="ServiceErrorCodes.PlanSubjectInvalid"/>, <see cref="ServiceErrorCodes.PlanSubjectDuplicated"/>.
        /// </returns>
        Task<ServiceResult<CustomPlanResponseModel>> AddSubjectAsync(RequestUser user, ulong planId, ulong subjectId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a subject from a plan that is not completed or cancelled. Only the plan's client. A subject that still
        /// has lessons in the plan is removed only with <paramref name="removeLessons"/>, and then only when none of those
        /// lessons has progress; the lessons and their dates, practices, exams and tests are deleted with it.
        /// </summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="subjectId">Athenas subject id.</param>
        /// <param name="removeLessons">Also remove the subject's lessons (none may have progress).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The plan. Errors: <see cref="ServiceErrorCodes.PlanNotFound"/>, <see cref="ServiceErrorCodes.PlanClosed"/>,
        /// <see cref="ServiceErrorCodes.PlanSubjectNotFound"/>, <see cref="ServiceErrorCodes.PlanSubjectHasLessons"/>,
        /// <see cref="ServiceErrorCodes.PlanSubjectHasProgress"/>.
        /// </returns>
        Task<ServiceResult<CustomPlanResponseModel>> RemoveSubjectAsync(RequestUser user, ulong planId, ulong subjectId, bool removeLessons, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds study time reported by the app to a lesson and to its plan (<c>total_study_minutes</c> on both), in one
        /// transaction with atomic increments, so simultaneous reports are never lost. The plan's client or its student, while
        /// the plan is active.
        /// </summary>
        /// <param name="user">The calling user (the plan's client or student).</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="planLessonId">Id of the plan lesson.</param>
        /// <param name="minutes">Minutes studied in this session (1 to 600).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The new lesson and plan totals. Errors: <see cref="ServiceErrorCodes.PlanStudyMinutesInvalid"/>,
        /// <see cref="ServiceErrorCodes.PlanNotFound"/>, <see cref="ServiceErrorCodes.PlanLessonNotFound"/>,
        /// <see cref="ServiceErrorCodes.PlanNotActive"/>.
        /// </returns>
        Task<ServiceResult<CustomPlanStudyTimeResponseModel>> AddStudyTimeAsync(RequestUser user, ulong planId, ulong planLessonId, ushort minutes, CancellationToken cancellationToken = default);

        /// <summary>Soft-deletes the plan (its children are hidden through the plan). Only the plan's client, in any status.</summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True when deleted, or <see cref="ServiceErrorCodes.PlanNotFound"/>.</returns>
        Task<ServiceResult<bool>> DeleteAsync(RequestUser user, ulong planId, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Business rules:
    /// <list type="bullet">
    /// <item>Only clients create plans; the client is the user in session and is never the student; the student must
    /// be a student profile the client can manage (Users API).</item>
    /// <item>No framework: every standard_id must be null. With a framework: the standards must belong to it.</item>
    /// <item>end_date defaults to start_date + plan_duration_weeks weeks.</item>
    /// <item>Only draft plans can replace their header and structure. Lesson progress and exam results change only
    /// while the plan is active. The weekly schedule (days and time slots) and the subjects can change in any status but
    /// completed and cancelled (<see cref="UpdateScheduleAsync"/>, <see cref="AddSubjectAsync"/>,
    /// <see cref="RemoveSubjectAsync"/>); a subject's lessons are removed with it only when none has progress.</item>
    /// <item>Status: draft -> active (needs at least one lesson) | cancelled; active -> paused | completed | cancelled;
    /// paused -> active | cancelled. Completed and cancelled are final.</item>
    /// <item>lesson_status = completed sets completed_at (UTC now); moving away from completed clears it.</item>
    /// <item>Study time: the app reports minutes per lesson (<see cref="AddStudyTimeAsync"/>, the plan's client or student,
    /// active plans only, 1-600 per report). The lesson and the plan totals grow together in one transaction; the plan
    /// total is the sum of its lessons. A lesson with study time counts as having progress.</item>
    /// <item>Every lesson has a learning window (concept, definitions, examples): assigned date &lt;= due date, both
    /// inside the plan dates, with a valid time zone. Session dates must fall inside that window. A lesson's subject
    /// must be one of the plan's subjects.</item>
    /// <item>Practices, exams and pre/post tests are scheduled the same way: an open day, a due date-time (stored in
    /// UTC) and a time zone; the deadline is not before the day it opens and both are inside the plan dates (read in
    /// the activity's time zone). Practices and exams open on or after the learning window starts. Exams and tests also
    /// have a duration (1-480 minutes); practices do not.</item>
    /// <item>Each practice, exam and test must be one the lesson has in Athenas, of the right kind (IsExam 0 practice,
    /// 1 exam, 2 pre-test, 3 post-test). There is no minimum: the lesson's quizzes in Athenas are the reference.</item>
    /// <item>A lesson can be marked completed only when every practice and exam it has in Athenas is scheduled in the
    /// plan and completed (and every scheduled one is completed). Athenas down = the request fails (503).</item>
    /// <item>learning_completed_at is set when concept, definitions and examples are all done. Undoing one clears it.
    /// When a lesson has practices, practices_completed follows them (true once all are completed).</item>
    /// <item>Statuses and results of practices, exams and tests change in <see cref="CustomPlanActivityService"/>, and
    /// automatically when the student submits the quiz (<see cref="CustomPlanQuizEvaluator"/>).</item>
    /// <item>The structure is locked after draft, but a lesson can still be added to a running plan
    /// (<see cref="AddLessonAsync"/>, same rules as the structure), and practices, exams and tests can still be added to
    /// a lesson (<see cref="AddLessonActivitiesAsync"/>), so a quiz Athenas adds later never leaves a lesson stuck.
    /// Adding a practice or exam to a completed lesson reopens it (in_progress).</item>
    /// </list>
    /// </summary>
    public sealed class CustomPlanService : ICustomPlanService
    {
        private static readonly Dictionary<CustomPlanStatus, CustomPlanStatus[]> AllowedTransitions = new()
        {
            [CustomPlanStatus.Draft] = new[] { CustomPlanStatus.Active, CustomPlanStatus.Cancelled },
            [CustomPlanStatus.Active] = new[] { CustomPlanStatus.Paused, CustomPlanStatus.Completed, CustomPlanStatus.Cancelled },
            [CustomPlanStatus.Paused] = new[] { CustomPlanStatus.Active, CustomPlanStatus.Cancelled },
            [CustomPlanStatus.Completed] = Array.Empty<CustomPlanStatus>(),
            [CustomPlanStatus.Cancelled] = Array.Empty<CustomPlanStatus>(),
        };

        private const ushort ExamMaxDurationMinutes = 480;

        /// <summary>Most minutes one study-time report can add (10 hours).</summary>
        public const ushort MaxStudyMinutesPerReport = 600;

        private readonly GSWebDbContext _db;
        private readonly IUserDirectoryClient _users;
        private readonly IStandardFrameworkValidator _standardValidator;
        private readonly GSWebModuleSettings _settings;
        private readonly TimeProvider _timeProvider;
        private readonly IAthenasCatalogClient _athenas;

        /// <summary>Creates the service.</summary>
        /// <param name="db">GSWeb database context.</param>
        /// <param name="users">Users API client (student ownership and names).</param>
        /// <param name="standardValidator">Checks that the lesson standards belong to the plan framework.</param>
        /// <param name="settings">Module settings (client role id).</param>
        /// <param name="timeProvider">Clock for completion dates.</param>
        /// <param name="athenas">Athenas client, to validate quizzes and lesson completion.</param>
        public CustomPlanService(
            GSWebDbContext db,
            IUserDirectoryClient users,
            IStandardFrameworkValidator standardValidator,
            GSWebModuleSettings settings,
            TimeProvider timeProvider,
            IAthenasCatalogClient athenas)
        {
            _db = db;
            _users = users;
            _standardValidator = standardValidator;
            _settings = settings;
            _timeProvider = timeProvider;
            _athenas = athenas;
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanResponseModel>> CreateAsync(RequestUser user, CreateCustomPlanRequestModel request, CancellationToken cancellationToken = default)
        {
            if (user.RoleId != _settings.ClientRoleId)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.PlanOnlyClients);
            }

            if (request.StudentId == 0)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.PlanStudentNotOfClient);
            }

            if (request.StudentId == user.UserId)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.PlanClientIsStudent);
            }

            var header = new PlanHeader(request.CustomPlanTypeId, request.LevelId, request.CurriculumFrameworkId, request.PlanDurationWeeks, request.StartDate, request.EndDate);
            var structure = request.Structure ?? new CustomPlanStructureRequestModel();
            var (errors, framework) = await ValidateHeaderAsync(header, cancellationToken);
            errors.AddRange(ValidateStructure(structure, header));
            errors.AddRange(await ValidateStandardsAsync(framework, structure, cancellationToken));
            if (errors.Count > 0)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Validation, errors.Distinct());
            }

            var athenasErrors = await ValidateAgainstAthenasAsync(structure, cancellationToken);
            if (athenasErrors == null)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Unavailable, ServiceErrorCodes.AthenasUnavailable);
            }

            if (athenasErrors.Count > 0)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Validation, athenasErrors.Distinct());
            }

            // Checked last: it is the only rule that needs the Users API.
            var isStudentOfClient = await _users.IsStudentOfClientAsync(user, request.StudentId, cancellationToken);
            if (isStudentOfClient == null)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Unavailable, ServiceErrorCodes.UsersApiUnavailable);
            }

            if (isStudentOfClient == false)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.PlanStudentNotOfClient);
            }

            var plan = new CustomPlanClient
            {
                ClientId = user.UserId,
                StudentId = request.StudentId,
                Status = CustomPlanStatus.Draft,
            };
            ApplyHeader(plan, header);
            ApplyStructure(plan, structure);

            _db.CustomPlanClients.Add(plan);
            await _db.SaveChangesAsync(cancellationToken);

            return await GetAsync(user, plan.Id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<List<CustomPlanTypeResponseModel>>> ListTypesAsync(string? langCode, CancellationToken cancellationToken = default)
        {
            var lang = RequestLanguage.Primary(langCode);
            var types = await _db.CustomPlanTypes.AsNoTracking()
                .OrderBy(t => t.Id)
                .Select(t => new CustomPlanTypeResponseModel
                {
                    Id = t.Id,
                    Code = t.Code,
                    Name = t.Translations.Where(x => x.LangCode == lang).Select(x => x.Name).FirstOrDefault()
                        ?? t.Translations.Where(x => x.LangCode == RequestLanguage.English).Select(x => x.Name).FirstOrDefault()
                        ?? t.Name,
                })
                .ToListAsync(cancellationToken);
            return ServiceResult<List<CustomPlanTypeResponseModel>>.Ok(types);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanResponseModel>> GetAsync(RequestUser user, ulong planId, CancellationToken cancellationToken = default)
        {
            var plan = await QueryWithStructure()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);
            if (plan == null || !CanAccess(user, plan))
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PlanNotFound);
            }

            var names = await _users.GetFullNamesAsync(user, new[] { plan.ClientId, plan.StudentId }, cancellationToken);
            return ServiceResult<CustomPlanResponseModel>.Ok(MapPlan(plan, names));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<List<CustomPlanSummaryModel>>> ListAsync(RequestUser user, ulong? studentId, CancellationToken cancellationToken = default)
        {
            var query = _db.CustomPlanClients
                .AsNoTracking()
                .Include(p => p.CustomPlanType)
                .Include(p => p.CurriculumFramework)
                .Where(p => p.ClientId == user.UserId);
            if (studentId.HasValue)
            {
                query = query.Where(p => p.StudentId == studentId.Value);
            }

            var plans = await query.OrderByDescending(p => p.CreatedAt).ThenByDescending(p => p.Id).ToListAsync(cancellationToken);
            var names = await _users.GetFullNamesAsync(user, plans.SelectMany(p => new[] { p.ClientId, p.StudentId }), cancellationToken);
            return ServiceResult<List<CustomPlanSummaryModel>>.Ok(plans.Select(p => MapSummary(new CustomPlanSummaryModel(), p, names)).ToList());
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanResponseModel>> UpdateDraftAsync(RequestUser user, ulong planId, UpdateCustomPlanDraftRequestModel request, CancellationToken cancellationToken = default)
        {
            var plan = await QueryWithStructure().FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);
            if (plan == null || plan.ClientId != user.UserId)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PlanNotFound);
            }

            if (plan.Status != CustomPlanStatus.Draft)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.PlanNotDraft);
            }

            var header = new PlanHeader(request.CustomPlanTypeId, request.LevelId, request.CurriculumFrameworkId, request.PlanDurationWeeks, request.StartDate, request.EndDate);
            var structure = request.Structure ?? new CustomPlanStructureRequestModel();
            var (errors, framework) = await ValidateHeaderAsync(header, cancellationToken);
            errors.AddRange(ValidateStructure(structure, header));
            errors.AddRange(await ValidateStandardsAsync(framework, structure, cancellationToken));
            if (errors.Count > 0)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Validation, errors.Distinct());
            }

            var athenasErrors = await ValidateAgainstAthenasAsync(structure, cancellationToken);
            if (athenasErrors == null)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Unavailable, ServiceErrorCodes.AthenasUnavailable);
            }

            if (athenasErrors.Count > 0)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Validation, athenasErrors.Distinct());
            }

            ApplyHeader(plan, header);
            ApplyStructure(plan, structure);
            await _db.SaveChangesAsync(cancellationToken);

            return await GetAsync(user, plan.Id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanResponseModel>> ChangeStatusAsync(RequestUser user, ulong planId, CustomPlanStatus status, CancellationToken cancellationToken = default)
        {
            var plan = await _db.CustomPlanClients.FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);
            if (plan == null || plan.ClientId != user.UserId)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PlanNotFound);
            }

            if (plan.Status != status)
            {
                if (!AllowedTransitions[plan.Status].Contains(status))
                {
                    return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.PlanStatusTransitionInvalid);
                }

                if (plan.Status == CustomPlanStatus.Draft && status == CustomPlanStatus.Active
                    && !await _db.CustomPlanClientLessons.AnyAsync(l => l.CustomPlanClientId == plan.Id, cancellationToken))
                {
                    return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.PlanNeedsLessons);
                }

                plan.Status = status;
                await _db.SaveChangesAsync(cancellationToken);
            }

            return await GetAsync(user, plan.Id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanLessonResponseModel>> UpdateLessonProgressAsync(RequestUser user, ulong planId, ulong planLessonId, CustomPlanLessonProgressRequestModel request, CancellationToken cancellationToken = default)
        {
            var plan = await _db.CustomPlanClients.AsNoTracking().FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);
            if (plan == null || plan.ClientId != user.UserId)
            {
                return ServiceResult<CustomPlanLessonResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PlanNotFound);
            }

            var lesson = await _db.CustomPlanClientLessons
                .Include(l => l.Dates)
                .Include(l => l.ExamDates)
                .Include(l => l.Practices)
                .Include(l => l.Tests)
                .FirstOrDefaultAsync(l => l.Id == planLessonId && l.CustomPlanClientId == planId, cancellationToken);
            if (lesson == null)
            {
                return ServiceResult<CustomPlanLessonResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PlanLessonNotFound);
            }

            if (plan.Status != CustomPlanStatus.Active)
            {
                return ServiceResult<CustomPlanLessonResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.PlanNotActive);
            }

            // With scheduled practices the flag is derived from them (see UpdatePracticeStatusAsync).
            if (request.PracticesCompleted.HasValue && lesson.Practices.Count > 0)
            {
                return ServiceResult<CustomPlanLessonResponseModel>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.PlanPracticesFlagFollowsPractices);
            }

            if (request.LessonStatus == PlanLessonStatus.Completed && lesson.LessonStatus != PlanLessonStatus.Completed)
            {
                var pending = await GetPendingForCompletionAsync(lesson, cancellationToken);
                if (pending == null)
                {
                    return ServiceResult<CustomPlanLessonResponseModel>.Fail(ServiceErrorKind.Unavailable, ServiceErrorCodes.AthenasUnavailable);
                }

                if (pending.Count > 0)
                {
                    return ServiceResult<CustomPlanLessonResponseModel>.Fail(ServiceErrorKind.Conflict, pending);
                }
            }

            lesson.ConceptCompleted = request.ConceptCompleted ?? lesson.ConceptCompleted;
            lesson.DefinitionsCompleted = request.DefinitionsCompleted ?? lesson.DefinitionsCompleted;
            lesson.ExamplesCompleted = request.ExamplesCompleted ?? lesson.ExamplesCompleted;
            lesson.PreCompleted = request.PreCompleted ?? lesson.PreCompleted;
            lesson.PracticesCompleted = request.PracticesCompleted ?? lesson.PracticesCompleted;
            lesson.TestCompleted = request.TestCompleted ?? lesson.TestCompleted;
            lesson.PostCompleted = request.PostCompleted ?? lesson.PostCompleted;
            var now = _timeProvider.UtcNowSeconds();
            SetAreaCompletion(lesson, now);
            if (request.LessonStatus is { } newStatus)
            {
                SetLessonStatus(lesson, newStatus, now);
            }

            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<CustomPlanLessonResponseModel>.Ok(MapLesson(lesson));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanLessonResponseModel>> AddLessonActivitiesAsync(RequestUser user, ulong planId, ulong planLessonId, CustomPlanLessonActivitiesRequestModel request, CancellationToken cancellationToken = default)
        {
            var plan = await _db.CustomPlanClients.AsNoTracking().FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);
            if (plan == null || plan.ClientId != user.UserId)
            {
                return ServiceResult<CustomPlanLessonResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PlanNotFound);
            }

            if (plan.Status is CustomPlanStatus.Completed or CustomPlanStatus.Cancelled)
            {
                return ServiceResult<CustomPlanLessonResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.PlanClosed);
            }

            var lesson = await _db.CustomPlanClientLessons
                .Include(l => l.Dates)
                .Include(l => l.ExamDates)
                .Include(l => l.Practices)
                .Include(l => l.Tests)
                .FirstOrDefaultAsync(l => l.Id == planLessonId && l.CustomPlanClientId == planId, cancellationToken);
            if (lesson == null)
            {
                return ServiceResult<CustomPlanLessonResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PlanLessonNotFound);
            }

            var practices = request.Practices ?? new List<CustomPlanPracticeRequestModel>();
            var exams = request.Exams ?? new List<CustomPlanExamRequestModel>();
            var tests = request.Tests ?? new List<CustomPlanTestRequestModel>();
            if (practices.Count + exams.Count + tests.Count == 0)
            {
                return ServiceResult<CustomPlanLessonResponseModel>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.PlanActivitiesRequired);
            }

            // Same rules as the plan structure, against the lesson's learning window and the plan dates.
            var lessonRequest = new CustomPlanLessonRequestModel
            {
                LessonId = lesson.LessonId,
                LearningAssignedDate = lesson.LearningAssignedDate,
                Practices = practices,
                Exams = exams,
                Tests = tests,
            };
            var planStart = plan.StartDate;
            var planEnd = plan.EndDate ?? plan.StartDate.AddDays(7 * plan.PlanDurationWeeks);
            var errors = new List<string>();
            errors.AddRange(practices.SelectMany(p => ValidatePractice(p, lessonRequest, planStart, planEnd)));
            errors.AddRange(exams.SelectMany(x => ValidateExam(x, lessonRequest, planStart, planEnd)));
            errors.AddRange(tests.SelectMany(t => ValidateTest(t, planStart, planEnd)));

            // Not repeated in the request nor already in the lesson.
            var practiceIds = lesson.Practices.Select(p => p.PracticeId).Concat(practices.Select(p => p.PracticeId)).ToList();
            var practiceSequences = lesson.Practices.Select(p => p.Sequence).Concat(practices.Select(p => p.Sequence)).ToList();
            var examKeys = lesson.ExamDates.Select(e => (e.ExamId, e.AssignedDate)).Concat(exams.Select(x => (x.ExamId, x.AssignedDate))).ToList();
            var testKeys = lesson.Tests.Select(t => (t.TestType, t.TestId)).Concat(tests.Select(t => (t.TestType, t.TestId))).ToList();
            if (practiceIds.Distinct().Count() != practiceIds.Count)
            {
                errors.Add(ServiceErrorCodes.PlanPracticeDuplicated);
            }

            if (practiceSequences.Distinct().Count() != practiceSequences.Count)
            {
                errors.Add(ServiceErrorCodes.PlanPracticeSequenceDuplicated);
            }

            if (examKeys.Distinct().Count() != examKeys.Count)
            {
                errors.Add(ServiceErrorCodes.PlanExamDuplicated);
            }

            if (testKeys.Distinct().Count() != testKeys.Count)
            {
                errors.Add(ServiceErrorCodes.PlanTestDuplicated);
            }

            if (errors.Count > 0)
            {
                return ServiceResult<CustomPlanLessonResponseModel>.Fail(ServiceErrorKind.Validation, errors.Distinct());
            }

            var athenasErrors = await ValidateAgainstAthenasAsync(new CustomPlanStructureRequestModel { Lessons = { lessonRequest } }, cancellationToken);
            if (athenasErrors == null)
            {
                return ServiceResult<CustomPlanLessonResponseModel>.Fail(ServiceErrorKind.Unavailable, ServiceErrorCodes.AthenasUnavailable);
            }

            if (athenasErrors.Count > 0)
            {
                return ServiceResult<CustomPlanLessonResponseModel>.Fail(ServiceErrorKind.Validation, athenasErrors.Distinct());
            }

            foreach (var p in practices)
            {
                lesson.Practices.Add(new CustomPlanClientLessonPractice
                {
                    PracticeId = p.PracticeId,
                    Sequence = p.Sequence,
                    AssignedDate = p.AssignedDate,
                    DueDateTime = TruncateToSeconds(p.DueDateTime!.Value),
                    Timezone = p.Timezone,
                });
            }

            foreach (var x in exams)
            {
                lesson.ExamDates.Add(new CustomPlanClientLessonExamDate
                {
                    ExamId = x.ExamId,
                    AssignedDate = x.AssignedDate,
                    DueDateTime = TruncateToSeconds(x.DueDateTime!.Value),
                    Timezone = x.Timezone,
                    DurationMinutes = x.DurationMinutes,
                });
            }

            foreach (var t in tests)
            {
                lesson.Tests.Add(new CustomPlanClientLessonTest
                {
                    TestType = t.TestType,
                    TestId = t.TestId,
                    AssignedDate = t.AssignedDate,
                    DueDateTime = TruncateToSeconds(t.DueDateTime!.Value),
                    Timezone = t.Timezone,
                    DurationMinutes = t.DurationMinutes,
                });
            }

            // New pending work: the lesson's flags and status follow.
            CustomPlanActivityService.UpdateLessonFlags(lesson);
            if (lesson.LessonStatus == PlanLessonStatus.Completed && (practices.Count > 0 || exams.Count > 0))
            {
                SetLessonStatus(lesson, PlanLessonStatus.InProgress, _timeProvider.UtcNowSeconds());
            }

            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<CustomPlanLessonResponseModel>.Ok(MapLesson(lesson));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanResponseModel>> UpdateScheduleAsync(RequestUser user, ulong planId, CustomPlanScheduleRequestModel request, CancellationToken cancellationToken = default)
        {
            var plan = await _db.CustomPlanClients
                .Include(p => p.Days).ThenInclude(d => d.Times)
                .FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);
            if (CheckOpenPlan(user, plan) is { } failure)
            {
                return failure;
            }

            var errors = ValidateDays(request.Days);
            if (errors.Count > 0)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Validation, errors.Distinct());
            }

            ApplyDays(plan!, request.Days);
            await _db.SaveChangesAsync(cancellationToken);
            return await GetAsync(user, plan!.Id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanResponseModel>> AddSubjectAsync(RequestUser user, ulong planId, ulong subjectId, CancellationToken cancellationToken = default)
        {
            var plan = await _db.CustomPlanClients.Include(p => p.Subjects).FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);
            if (CheckOpenPlan(user, plan) is { } failure)
            {
                return failure;
            }

            if (subjectId == 0)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.PlanSubjectInvalid);
            }

            if (plan!.Subjects.Any(s => s.SubjectId == subjectId))
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.PlanSubjectDuplicated);
            }

            plan.Subjects.Add(new CustomPlanClientSubject { SubjectId = subjectId });
            await _db.SaveChangesAsync(cancellationToken);
            return await GetAsync(user, plan.Id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanResponseModel>> RemoveSubjectAsync(RequestUser user, ulong planId, ulong subjectId, bool removeLessons, CancellationToken cancellationToken = default)
        {
            var plan = await _db.CustomPlanClients
                .Include(p => p.Subjects)
                .Include(p => p.Lessons.Where(l => l.SubjectId == subjectId)).ThenInclude(l => l.Practices)
                .Include(p => p.Lessons).ThenInclude(l => l.ExamDates)
                .Include(p => p.Lessons).ThenInclude(l => l.Tests)
                .Include(p => p.Lessons).ThenInclude(l => l.Dates)
                .AsSplitQuery()
                .FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);
            if (CheckOpenPlan(user, plan) is { } failure)
            {
                return failure;
            }

            var subject = plan!.Subjects.FirstOrDefault(s => s.SubjectId == subjectId);
            if (subject == null)
            {
                return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PlanSubjectNotFound);
            }

            var lessons = plan.Lessons.Where(l => l.SubjectId == subjectId).ToList();
            if (lessons.Count > 0)
            {
                if (!removeLessons)
                {
                    return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.PlanSubjectHasLessons);
                }

                var lessonIds = lessons.Select(l => (ulong?)l.Id).ToList();
                var withEvidence = (await _db.CustomPlanEvidences
                        .Where(e => e.CustomPlanClientId == plan.Id && lessonIds.Contains(e.CustomPlanClientLessonId))
                        .Select(e => e.CustomPlanClientLessonId!.Value)
                        .Distinct()
                        .ToListAsync(cancellationToken))
                    .ToHashSet();
                if (lessons.Any(l => HasProgress(l, withEvidence.Contains(l.Id))))
                {
                    return ServiceResult<CustomPlanResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.PlanSubjectHasProgress);
                }

                // Hard delete, like a lesson removed from a draft: nothing was done on them (checked above).
                plan.Lessons.RemoveAll(l => l.SubjectId == subjectId);
            }

            plan.Subjects.Remove(subject);
            await _db.SaveChangesAsync(cancellationToken);
            return await GetAsync(user, plan.Id, cancellationToken);
        }

        /// <summary>
        /// Null when the plan exists, belongs to the caller and is not completed or cancelled; otherwise the failure
        /// (<see cref="ServiceErrorCodes.PlanNotFound"/> or <see cref="ServiceErrorCodes.PlanClosed"/>).
        /// </summary>
        private static ServiceResult<CustomPlanResponseModel>? CheckOpenPlan(RequestUser user, CustomPlanClient? plan) =>
            CheckOpenPlan<CustomPlanResponseModel>(user, plan);

        /// <inheritdoc cref="CheckOpenPlan(RequestUser, CustomPlanClient?)"/>
        private static ServiceResult<T>? CheckOpenPlan<T>(RequestUser user, CustomPlanClient? plan)
        {
            if (plan == null || plan.ClientId != user.UserId)
            {
                return ServiceResult<T>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PlanNotFound);
            }

            return plan.Status is CustomPlanStatus.Completed or CustomPlanStatus.Cancelled
                ? ServiceResult<T>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.PlanClosed)
                : null;
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanLessonResponseModel>> AddLessonAsync(RequestUser user, ulong planId, CustomPlanLessonRequestModel request, CancellationToken cancellationToken = default)
        {
            var plan = await _db.CustomPlanClients
                .AsNoTracking()
                .Include(p => p.Subjects)
                .Include(p => p.CurriculumFramework)
                .FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);
            if (CheckOpenPlan<CustomPlanLessonResponseModel>(user, plan) is { } failure)
            {
                return failure;
            }

            // Same rules as a lesson of the plan structure, against the plan's dates, subjects and framework.
            var header = new PlanHeader(plan!.CustomPlanTypeId, plan.LevelId, plan.CurriculumFrameworkId, plan.PlanDurationWeeks, plan.StartDate, plan.EndDate);
            var structure = new CustomPlanStructureRequestModel
            {
                SubjectIds = plan.Subjects.Select(s => s.SubjectId).ToList(),
                Lessons = { request },
            };
            var errors = ValidateStructure(structure, header);
            var taken = await _db.CustomPlanClientLessons
                .Where(l => l.CustomPlanClientId == plan.Id && (l.LessonId == request.LessonId || l.Sequence == request.Sequence))
                .Select(l => new { l.LessonId, l.Sequence })
                .ToListAsync(cancellationToken);
            if (taken.Any(l => l.LessonId == request.LessonId))
            {
                errors.Add(ServiceErrorCodes.PlanLessonDuplicated);
            }

            if (taken.Any(l => l.Sequence == request.Sequence))
            {
                errors.Add(ServiceErrorCodes.PlanLessonSequenceDuplicated);
            }

            var framework = plan.CurriculumFramework is { IsActive: true } active ? active : null;
            errors.AddRange(await ValidateStandardsAsync(framework, structure, cancellationToken));
            if (errors.Count > 0)
            {
                return ServiceResult<CustomPlanLessonResponseModel>.Fail(ServiceErrorKind.Validation, errors.Distinct());
            }

            var athenasErrors = await ValidateAgainstAthenasAsync(structure, cancellationToken);
            if (athenasErrors == null)
            {
                return ServiceResult<CustomPlanLessonResponseModel>.Fail(ServiceErrorKind.Unavailable, ServiceErrorCodes.AthenasUnavailable);
            }

            if (athenasErrors.Count > 0)
            {
                return ServiceResult<CustomPlanLessonResponseModel>.Fail(ServiceErrorKind.Validation, athenasErrors.Distinct());
            }

            var lesson = new CustomPlanClientLesson { CustomPlanClientId = plan.Id };
            ApplyLesson(lesson, request);
            _db.CustomPlanClientLessons.Add(lesson);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<CustomPlanLessonResponseModel>.Ok(MapLesson(lesson));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanStudyTimeResponseModel>> AddStudyTimeAsync(RequestUser user, ulong planId, ulong planLessonId, ushort minutes, CancellationToken cancellationToken = default)
        {
            if (minutes is 0 or > MaxStudyMinutesPerReport)
            {
                return ServiceResult<CustomPlanStudyTimeResponseModel>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.PlanStudyMinutesInvalid);
            }

            var plan = await _db.CustomPlanClients.AsNoTracking().FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);
            if (plan == null || (plan.ClientId != user.UserId && plan.StudentId != user.UserId))
            {
                return ServiceResult<CustomPlanStudyTimeResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PlanNotFound);
            }

            if (!await _db.CustomPlanClientLessons.AnyAsync(l => l.Id == planLessonId && l.CustomPlanClientId == planId, cancellationToken))
            {
                return ServiceResult<CustomPlanStudyTimeResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PlanLessonNotFound);
            }

            if (plan.Status != CustomPlanStatus.Active)
            {
                return ServiceResult<CustomPlanStudyTimeResponseModel>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.PlanNotActive);
            }

            // total = total + n in SQL for both rows, in one transaction (wrapped for the retry-on-failure strategy).
            var now = _timeProvider.UtcNowSeconds();
            var strategy = _db.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
                await _db.CustomPlanClientLessons
                    .Where(l => l.Id == planLessonId && l.CustomPlanClientId == planId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(l => l.TotalStudyMinutes, l => l.TotalStudyMinutes + minutes)
                        .SetProperty(l => l.UpdatedAt, now), cancellationToken);
                await _db.CustomPlanClients
                    .Where(p => p.Id == planId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.TotalStudyMinutes, p => p.TotalStudyMinutes + minutes)
                        .SetProperty(p => p.UpdatedAt, now), cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            });

            var lessonTotal = await _db.CustomPlanClientLessons.AsNoTracking()
                .Where(l => l.Id == planLessonId)
                .Select(l => l.TotalStudyMinutes)
                .FirstAsync(cancellationToken);
            var planTotal = await _db.CustomPlanClients.AsNoTracking()
                .Where(p => p.Id == planId)
                .Select(p => p.TotalStudyMinutes)
                .FirstAsync(cancellationToken);

            // ExecuteUpdate skips the change tracker: rows this context already tracks get the new totals, so a later save or
            // check in the same context never works with (or writes back) the old value.
            SyncTracked<CustomPlanClientLesson>(l => l.Id == planLessonId, l => l.TotalStudyMinutes, lessonTotal);
            SyncTracked<CustomPlanClient>(p => p.Id == planId, p => p.TotalStudyMinutes, planTotal);
            return ServiceResult<CustomPlanStudyTimeResponseModel>.Ok(new CustomPlanStudyTimeResponseModel
            {
                PlanId = planId,
                PlanLessonId = planLessonId,
                AddedMinutes = minutes,
                LessonTotalStudyMinutes = lessonTotal,
                PlanTotalStudyMinutes = planTotal,
            });
        }

        /// <summary>Sets a value written by ExecuteUpdate on the matching tracked entities, as current and original value.</summary>
        private void SyncTracked<TEntity>(Func<TEntity, bool> match, System.Linq.Expressions.Expression<Func<TEntity, uint>> property, uint value)
            where TEntity : class
        {
            foreach (var entry in _db.ChangeTracker.Entries<TEntity>().Where(e => match(e.Entity)))
            {
                var prop = entry.Property(property);
                prop.CurrentValue = value;
                prop.OriginalValue = value;
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<bool>> DeleteAsync(RequestUser user, ulong planId, CancellationToken cancellationToken = default)
        {
            var plan = await _db.CustomPlanClients.FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);
            if (plan == null || plan.ClientId != user.UserId)
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PlanNotFound);
            }

            // Soft delete (interceptor). The children have no deleted_at and are hidden through the plan's filter.
            _db.CustomPlanClients.Remove(plan);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<bool>.Ok(true);
        }

        /// <summary>
        /// Keeps learning_completed_at (concept + definitions + examples) consistent with the flags: set the first time
        /// the three are done, cleared if one is undone.
        /// </summary>
        /// <param name="lesson">The plan lesson.</param>
        /// <param name="utcNow">Current UTC time, used when the learning part becomes completed.</param>
        internal static void SetAreaCompletion(CustomPlanClientLesson lesson, DateTime utcNow)
        {
            var learningDone = lesson.ConceptCompleted && lesson.DefinitionsCompleted && lesson.ExamplesCompleted;
            lesson.LearningCompletedAt = learningDone ? lesson.LearningCompletedAt ?? utcNow : null;
        }

        /// <summary>
        /// Errors that block marking the lesson completed: practices or exams that the lesson has in Athenas but are not
        /// scheduled and completed in the plan, or scheduled ones not completed. Null when Athenas cannot be reached.
        /// </summary>
        private Task<List<string>?> GetPendingForCompletionAsync(CustomPlanClientLesson lesson, CancellationToken cancellationToken) =>
            GetPendingForCompletionAsync(_athenas, lesson, cancellationToken);

        /// <summary>
        /// Errors that block marking the lesson completed: practices or exams that the lesson has in Athenas but are not
        /// scheduled and completed in the plan, or scheduled ones not completed. Null when Athenas cannot be reached.
        /// </summary>
        /// <param name="athenas">Athenas client used to read the lesson quizzes.</param>
        /// <param name="lesson">The plan lesson, with its practices and exam dates loaded.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// Empty when nothing is pending; otherwise <see cref="ServiceErrorCodes.PlanLessonPracticesPending"/> and/or
        /// <see cref="ServiceErrorCodes.PlanLessonExamsPending"/>; null when Athenas cannot be reached (callers fail closed).
        /// </returns>
        internal static async Task<List<string>?> GetPendingForCompletionAsync(IAthenasCatalogClient athenas, CustomPlanClientLesson lesson, CancellationToken cancellationToken)
        {
            var quizzes = await athenas.TryGetLessonQuizzesAsync(lesson.LessonId, cancellationToken);
            if (quizzes == null)
            {
                return null;
            }

            var completedPractices = lesson.Practices.Where(p => p.Status == PlanActivityStatus.Completed).Select(p => p.PracticeId).ToHashSet();
            var completedExams = lesson.ExamDates.Where(e => e.Status == PlanActivityStatus.Completed).Select(e => e.ExamId).ToHashSet();
            var pending = new List<string>();

            if (lesson.Practices.Any(p => p.Status != PlanActivityStatus.Completed)
                || quizzes.Any(q => q.Value.IsExam == AthenasQuizInfo.Practice && !completedPractices.Contains(q.Key)))
            {
                pending.Add(ServiceErrorCodes.PlanLessonPracticesPending);
            }

            if (lesson.ExamDates.Any(e => e.Status != PlanActivityStatus.Completed)
                || quizzes.Any(q => q.Value.IsExam == AthenasQuizInfo.Exam && !completedExams.Contains(q.Key)))
            {
                pending.Add(ServiceErrorCodes.PlanLessonExamsPending);
            }

            return pending;
        }

        /// <summary>Sets the lesson status and keeps completed_at consistent with it (set when it becomes completed, cleared otherwise).</summary>
        /// <param name="lesson">The plan lesson.</param>
        /// <param name="newStatus">New lesson status.</param>
        /// <param name="utcNow">Current UTC time, used as completed_at.</param>
        internal static void SetLessonStatus(CustomPlanClientLesson lesson, PlanLessonStatus newStatus, DateTime utcNow)
        {
            if (newStatus == PlanLessonStatus.Completed)
            {
                if (lesson.LessonStatus != PlanLessonStatus.Completed || lesson.CompletedAt == null)
                {
                    lesson.CompletedAt = utcNow;
                }
            }
            else
            {
                lesson.CompletedAt = null;
            }

            lesson.LessonStatus = newStatus;
        }

        private IQueryable<CustomPlanClient> QueryWithStructure()
        {
            return _db.CustomPlanClients
                .Include(p => p.CustomPlanType)
                .Include(p => p.CurriculumFramework)
                .Include(p => p.Subjects)
                .Include(p => p.Days).ThenInclude(d => d.Times)
                .Include(p => p.Lessons).ThenInclude(l => l.Dates)
                .Include(p => p.Lessons).ThenInclude(l => l.ExamDates)
                .Include(p => p.Lessons).ThenInclude(l => l.Practices)
                .Include(p => p.Lessons).ThenInclude(l => l.Tests)
                .AsSplitQuery();
        }

        private static bool CanAccess(RequestUser user, CustomPlanClient plan) => plan.ClientId == user.UserId || user.IsStaff;

        private async Task<(List<string> Errors, CurriculumFramework? Framework)> ValidateHeaderAsync(PlanHeader header, CancellationToken cancellationToken)
        {
            var errors = new List<string>();
            if (!await _db.CustomPlanTypes.AnyAsync(t => t.Id == header.CustomPlanTypeId, cancellationToken))
            {
                errors.Add(ServiceErrorCodes.PlanTypeInvalid);
            }

            if (header.LevelId == 0)
            {
                errors.Add(ServiceErrorCodes.PlanLevelRequired);
            }

            CurriculumFramework? framework = null;
            if (header.CurriculumFrameworkId.HasValue)
            {
                framework = await _db.CurriculumFrameworks.AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Id == header.CurriculumFrameworkId.Value, cancellationToken);
                if (framework == null || !framework.IsActive)
                {
                    errors.Add(ServiceErrorCodes.PlanFrameworkInvalid);
                    framework = null;
                }
            }

            if (header.PlanDurationWeeks == 0)
            {
                errors.Add(ServiceErrorCodes.PlanDurationInvalid);
            }

            if (header.EndDate.HasValue && header.EndDate.Value < header.StartDate)
            {
                errors.Add(ServiceErrorCodes.PlanEndBeforeStart);
            }

            return (errors, framework);
        }

        private static List<string> ValidateStructure(CustomPlanStructureRequestModel structure, PlanHeader header)
        {
            var planStart = header.StartDate;
            var planEnd = EffectiveEndDate(header);
            var errors = new List<string>();
            var subjectIds = structure.SubjectIds ?? new List<ulong>();
            if (subjectIds.Distinct().Count() != subjectIds.Count)
            {
                errors.Add(ServiceErrorCodes.PlanSubjectDuplicated);
            }

            errors.AddRange(ValidateDays(structure.Days));

            var lessons = structure.Lessons ?? new List<CustomPlanLessonRequestModel>();
            if (lessons.Select(l => l.LessonId).Distinct().Count() != lessons.Count)
            {
                errors.Add(ServiceErrorCodes.PlanLessonDuplicated);
            }

            if (lessons.Select(l => l.Sequence).Distinct().Count() != lessons.Count)
            {
                errors.Add(ServiceErrorCodes.PlanLessonSequenceDuplicated);
            }

            foreach (var lesson in lessons)
            {
                if (lesson.LessonId == 0)
                {
                    errors.Add(ServiceErrorCodes.PlanLessonInvalid);
                }

                if (!subjectIds.Contains(lesson.SubjectId))
                {
                    errors.Add(ServiceErrorCodes.PlanLessonSubjectNotInPlan);
                }

                errors.AddRange(ValidateLearningWindow(lesson, planStart, planEnd));
                var practices = lesson.Practices ?? new List<CustomPlanPracticeRequestModel>();
                if (practices.Select(p => p.PracticeId).Distinct().Count() != practices.Count)
                {
                    errors.Add(ServiceErrorCodes.PlanPracticeDuplicated);
                }

                if (practices.Select(p => p.Sequence).Distinct().Count() != practices.Count)
                {
                    errors.Add(ServiceErrorCodes.PlanPracticeSequenceDuplicated);
                }

                foreach (var practice in practices)
                {
                    errors.AddRange(ValidatePractice(practice, lesson, planStart, planEnd));
                }

                var exams = lesson.Exams ?? new List<CustomPlanExamRequestModel>();
                if (exams.Select(x => (x.ExamId, x.AssignedDate)).Distinct().Count() != exams.Count)
                {
                    errors.Add(ServiceErrorCodes.PlanExamDuplicated);
                }

                foreach (var exam in exams)
                {
                    errors.AddRange(ValidateExam(exam, lesson, planStart, planEnd));
                }

                var tests = lesson.Tests ?? new List<CustomPlanTestRequestModel>();
                if (tests.Select(t => (t.TestType, t.TestId)).Distinct().Count() != tests.Count)
                {
                    errors.Add(ServiceErrorCodes.PlanTestDuplicated);
                }

                foreach (var test in tests)
                {
                    errors.AddRange(ValidateTest(test, planStart, planEnd));
                }
            }

            return errors;
        }

        /// <summary>Weekly study days: day of week 1..7, once each; per day, unique start times, a duration and a valid time zone.</summary>
        private static List<string> ValidateDays(List<CustomPlanDayRequestModel>? requested)
        {
            var errors = new List<string>();
            var days = requested ?? new List<CustomPlanDayRequestModel>();
            if (days.Any(d => d.DayOfWeek is < 1 or > 7))
            {
                errors.Add(ServiceErrorCodes.PlanDayInvalid);
            }

            if (days.Select(d => d.DayOfWeek).Distinct().Count() != days.Count)
            {
                errors.Add(ServiceErrorCodes.PlanDayDuplicated);
            }

            foreach (var day in days)
            {
                var times = day.Times ?? new List<CustomPlanDayTimeRequestModel>();
                if (times.Select(t => t.StartTime).Distinct().Count() != times.Count)
                {
                    errors.Add(ServiceErrorCodes.PlanTimeDuplicated);
                }

                if (times.Any(t => t.DurationMinutes == 0))
                {
                    errors.Add(ServiceErrorCodes.PlanTimeDurationInvalid);
                }

                if (times.Any(t => !TimeProviderExtensions.IsValidTimeZone(t.Timezone)))
                {
                    errors.Add(ServiceErrorCodes.PlanTimezoneInvalid);
                }
            }

            return errors;
        }

        /// <summary>Learning part (concept, definitions, examples): one window per lesson, inside the plan dates.</summary>
        private static IEnumerable<string> ValidateLearningWindow(CustomPlanLessonRequestModel lesson, DateOnly planStart, DateOnly planEnd)
        {
            if (!TimeProviderExtensions.IsValidTimeZone(lesson.LearningTimezone))
            {
                yield return ServiceErrorCodes.PlanTimezoneInvalid;
            }

            var dates = lesson.AssignedDates ?? new List<DateOnly>();
            if (dates.Distinct().Count() != dates.Count)
            {
                yield return ServiceErrorCodes.PlanLessonDateDuplicated;
            }

            if (lesson.LearningAssignedDate is not { } assigned || lesson.LearningDueDate is not { } due)
            {
                yield return ServiceErrorCodes.PlanLearningDatesRequired;
                yield break;
            }

            if (due < assigned)
            {
                yield return ServiceErrorCodes.PlanLearningDueBeforeAssigned;
            }

            if (assigned < planStart || due > planEnd)
            {
                yield return ServiceErrorCodes.PlanLearningOutsidePlan;
            }

            if (dates.Any(d => d < assigned || d > due))
            {
                yield return ServiceErrorCodes.PlanLessonDateOutsideLearning;
            }
        }

        /// <summary>One practice: its own window inside the plan dates, not before the learning window starts.</summary>
        private static IEnumerable<string> ValidatePractice(CustomPlanPracticeRequestModel practice, CustomPlanLessonRequestModel lesson, DateOnly planStart, DateOnly planEnd)
        {
            if (practice.PracticeId == 0 || practice.Sequence == 0)
            {
                yield return ServiceErrorCodes.PlanPracticeInvalid;
            }

            foreach (var error in ValidateSchedule(practice.AssignedDate, practice.DueDateTime, practice.Timezone, lesson.LearningAssignedDate, planStart, planEnd, PracticeCodes))
            {
                yield return error;
            }
        }

        /// <summary>A pre-test or post-test: scheduled like an exam (no learning-window rule: a pre-test comes first).</summary>
        private static IEnumerable<string> ValidateTest(CustomPlanTestRequestModel test, DateOnly planStart, DateOnly planEnd)
        {
            if (test.TestId == 0 || !Enum.IsDefined(test.TestType))
            {
                yield return ServiceErrorCodes.PlanTestInvalid;
            }

            if (test.DurationMinutes is 0 or > ExamMaxDurationMinutes)
            {
                yield return ServiceErrorCodes.PlanTestDurationInvalid;
            }

            foreach (var error in ValidateSchedule(test.AssignedDate, test.DueDateTime, test.Timezone, null, planStart, planEnd, TestCodes))
            {
                yield return error;
            }
        }

        private sealed record ScheduleCodes(string DueRequired, string DueBeforeAssigned, string OutsidePlan, string BeforeLearning);

        private static readonly ScheduleCodes ExamCodes = new(ServiceErrorCodes.PlanExamDueRequired, ServiceErrorCodes.PlanExamDueBeforeAssigned, ServiceErrorCodes.PlanExamOutsidePlan, ServiceErrorCodes.PlanExamBeforeLearning);

        private static readonly ScheduleCodes PracticeCodes = new(ServiceErrorCodes.PlanPracticeDueRequired, ServiceErrorCodes.PlanPracticeDueBeforeAssigned, ServiceErrorCodes.PlanPracticeOutsidePlan, ServiceErrorCodes.PlanPracticeBeforeLearning);

        private static readonly ScheduleCodes TestCodes = new(ServiceErrorCodes.PlanTestDueRequired, ServiceErrorCodes.PlanTestDueBeforeAssigned, ServiceErrorCodes.PlanTestOutsidePlan, ServiceErrorCodes.PlanTestOutsidePlan);

        /// <summary>
        /// Shared rules of a scheduled activity: open day and deadline inside the plan, deadline not before the open day,
        /// valid time zone, and (when <paramref name="learningStart"/> is given) not opening before the learning window.
        /// Dates are compared in the activity's time zone, so a deadline of 23:00 in Puerto Rico counts as that day even
        /// though it is already the next day in UTC.
        /// </summary>
        private static IEnumerable<string> ValidateSchedule(DateOnly assigned, DateTime? due, string? timezone, DateOnly? learningStart, DateOnly planStart, DateOnly planEnd, ScheduleCodes codes)
        {
            if (assigned < planStart || assigned > planEnd)
            {
                yield return codes.OutsidePlan;
            }

            if (learningStart is { } start && assigned < start)
            {
                yield return codes.BeforeLearning;
            }

            if (!TimeProviderExtensions.IsValidTimeZone(timezone))
            {
                yield return ServiceErrorCodes.PlanTimezoneInvalid;
                yield break;
            }

            if (due is not { } dueDateTime)
            {
                yield return codes.DueRequired;
                yield break;
            }

            var zone = TimeZoneInfo.FindSystemTimeZoneById(timezone!);
            var dueLocal = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(UtcDateTimeConverter.ToUtc(dueDateTime), zone));
            if (dueLocal < assigned)
            {
                yield return codes.DueBeforeAssigned;
            }

            if (dueLocal > planEnd)
            {
                yield return codes.OutsidePlan;
            }
        }

        /// <summary>
        /// Every practice, exam and test of the request must be a quiz of that lesson in Athenas, of the right kind.
        /// Null when Athenas cannot be reached (the request fails closed).
        /// </summary>
        private async Task<List<string>?> ValidateAgainstAthenasAsync(CustomPlanStructureRequestModel structure, CancellationToken cancellationToken)
        {
            var errors = new List<string>();
            foreach (var lesson in structure.Lessons ?? new List<CustomPlanLessonRequestModel>())
            {
                var practices = lesson.Practices ?? new List<CustomPlanPracticeRequestModel>();
                var exams = lesson.Exams ?? new List<CustomPlanExamRequestModel>();
                var tests = lesson.Tests ?? new List<CustomPlanTestRequestModel>();
                if (practices.Count == 0 && exams.Count == 0 && tests.Count == 0)
                {
                    continue;
                }

                var quizzes = await _athenas.TryGetLessonQuizzesAsync(lesson.LessonId, cancellationToken);
                if (quizzes == null)
                {
                    return null;
                }

                bool Is(ulong quizId, string kind) => quizzes.TryGetValue(quizId, out var quiz) && quiz.IsExam == kind;

                if (practices.Any(p => !Is(p.PracticeId, AthenasQuizInfo.Practice)))
                {
                    errors.Add(ServiceErrorCodes.PlanPracticeNotInLesson);
                }

                if (exams.Any(x => !Is(x.ExamId, AthenasQuizInfo.Exam)))
                {
                    errors.Add(ServiceErrorCodes.PlanExamNotInLesson);
                }

                if (tests.Any(t => !Is(t.TestId, t.TestType == PlanTestType.PreTest ? AthenasQuizInfo.PreTest : AthenasQuizInfo.PostTest)))
                {
                    errors.Add(ServiceErrorCodes.PlanTestNotInLesson);
                }
            }

            return errors;
        }

        /// <summary>
        /// Exam: opens on AssignedDate (local) and closes at DueDateTime. Dates are compared in the exam's time zone, so
        /// a deadline of 23:00 in Puerto Rico counts as that day even though it is already the next day in UTC.
        /// </summary>
        private static IEnumerable<string> ValidateExam(CustomPlanExamRequestModel exam, CustomPlanLessonRequestModel lesson, DateOnly planStart, DateOnly planEnd)
        {
            if (exam.ExamId == 0)
            {
                yield return ServiceErrorCodes.PlanExamInvalid;
            }

            if (exam.DurationMinutes is 0 or > ExamMaxDurationMinutes)
            {
                yield return ServiceErrorCodes.PlanExamDurationInvalid;
            }

            foreach (var error in ValidateSchedule(exam.AssignedDate, exam.DueDateTime, exam.Timezone, lesson.LearningAssignedDate, planStart, planEnd, ExamCodes))
            {
                yield return error;
            }
        }

        private static DateOnly EffectiveEndDate(PlanHeader header) => header.EndDate ?? header.StartDate.AddDays(7 * header.PlanDurationWeeks);

        private static DateTime TruncateToSeconds(DateTime value)
        {
            var utc = UtcDateTimeConverter.ToUtc(value);
            return new DateTime(utc.Ticks - (utc.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc);
        }

        private async Task<List<string>> ValidateStandardsAsync(CurriculumFramework? framework, CustomPlanStructureRequestModel structure, CancellationToken cancellationToken)
        {
            var standardIds = (structure.Lessons ?? new List<CustomPlanLessonRequestModel>())
                .Where(l => l.StandardId.HasValue)
                .Select(l => l.StandardId!.Value)
                .Distinct()
                .ToList();
            if (standardIds.Count == 0)
            {
                return new List<string>();
            }

            // A plan with no (valid) framework has no standards alignment.
            if (framework == null)
            {
                return new List<string> { ServiceErrorCodes.PlanStandardWithoutFramework };
            }

            return await _standardValidator.AreStandardsInFrameworkAsync(framework, standardIds, cancellationToken)
                ? new List<string>()
                : new List<string> { ServiceErrorCodes.PlanStandardNotInFramework };
        }

        private static void ApplyHeader(CustomPlanClient plan, PlanHeader header)
        {
            plan.CustomPlanTypeId = header.CustomPlanTypeId;
            plan.LevelId = header.LevelId;
            plan.CurriculumFrameworkId = header.CurriculumFrameworkId;
            plan.PlanDurationWeeks = header.PlanDurationWeeks;
            plan.StartDate = header.StartDate;
            plan.EndDate = EffectiveEndDate(header);
        }

        /// <summary>
        /// Makes the plan structure match the request. Existing rows are kept (same id) when their natural key is still
        /// requested; rows no longer requested are removed (hard delete, they are draft structure).
        /// </summary>
        private static void ApplyStructure(CustomPlanClient plan, CustomPlanStructureRequestModel structure)
        {
            Sync(plan.Subjects, (structure.SubjectIds ?? new List<ulong>()).Distinct(),
                e => e.SubjectId, id => id,
                () => new CustomPlanClientSubject(),
                (e, id) => e.SubjectId = id);

            ApplyDays(plan, structure.Days);

            Sync(plan.Lessons, structure.Lessons ?? new List<CustomPlanLessonRequestModel>(),
                e => e.LessonId, l => l.LessonId,
                () => new CustomPlanClientLesson(),
                ApplyLesson);
        }

        /// <summary>
        /// Makes one plan lesson match the request: learning window, session dates, practices, exams and tests (rows whose
        /// natural key is still requested keep their id; the rest are removed).
        /// </summary>
        private static void ApplyLesson(CustomPlanClientLesson lesson, CustomPlanLessonRequestModel request)
        {
            lesson.LessonId = request.LessonId;
            lesson.StandardId = request.StandardId;
            lesson.Sequence = request.Sequence;
            lesson.LearningAssignedDate = request.LearningAssignedDate!.Value;
            lesson.LearningDueDate = request.LearningDueDate!.Value;
            lesson.LearningTimezone = request.LearningTimezone;
            lesson.SubjectId = request.SubjectId;
            Sync(lesson.Practices, request.Practices ?? new List<CustomPlanPracticeRequestModel>(),
                e => e.PracticeId, p => p.PracticeId,
                () => new CustomPlanClientLessonPractice(),
                (practice, p) =>
                {
                    practice.PracticeId = p.PracticeId;
                    practice.Sequence = p.Sequence;
                    practice.AssignedDate = p.AssignedDate;
                    practice.DueDateTime = TruncateToSeconds(p.DueDateTime!.Value);
                    practice.Timezone = p.Timezone;
                });
            Sync(lesson.Tests, request.Tests ?? new List<CustomPlanTestRequestModel>(),
                e => (e.TestType, e.TestId), t => (t.TestType, t.TestId),
                () => new CustomPlanClientLessonTest(),
                (test, t) =>
                {
                    test.TestType = t.TestType;
                    test.TestId = t.TestId;
                    test.AssignedDate = t.AssignedDate;
                    test.DueDateTime = TruncateToSeconds(t.DueDateTime!.Value);
                    test.Timezone = t.Timezone;
                    test.DurationMinutes = t.DurationMinutes;
                });
            Sync(lesson.Dates, (request.AssignedDates ?? new List<DateOnly>()).Distinct(),
                e => e.AssignedDate, d => d,
                () => new CustomPlanClientLessonDate(),
                (date, d) => date.AssignedDate = d);
            Sync(lesson.ExamDates, (request.Exams ?? new List<CustomPlanExamRequestModel>()).DistinctBy(x => (x.ExamId, x.AssignedDate)),
                e => (e.ExamId, e.AssignedDate), x => (x.ExamId, x.AssignedDate),
                () => new CustomPlanClientLessonExamDate(),
                (exam, x) =>
                {
                    exam.ExamId = x.ExamId;
                    exam.AssignedDate = x.AssignedDate;
                    exam.DueDateTime = TruncateToSeconds(x.DueDateTime!.Value);
                    exam.Timezone = x.Timezone;
                    exam.DurationMinutes = x.DurationMinutes;
                });
        }

        /// <summary>Makes the weekly days and time slots match the request (same day / start time keeps its row id).</summary>
        private static void ApplyDays(CustomPlanClient plan, List<CustomPlanDayRequestModel>? days)
        {
            Sync(plan.Days, days ?? new List<CustomPlanDayRequestModel>(),
                e => e.DayOfWeek, d => d.DayOfWeek,
                () => new CustomPlanClientDay(),
                (day, request) =>
                {
                    day.DayOfWeek = request.DayOfWeek;
                    Sync(day.Times, request.Times ?? new List<CustomPlanDayTimeRequestModel>(),
                        e => e.StartTime, t => t.StartTime,
                        () => new CustomPlanClientDayTime(),
                        (time, t) =>
                        {
                            time.StartTime = t.StartTime;
                            time.DurationMinutes = t.DurationMinutes;
                            time.Timezone = t.Timezone;
                        });
                });
        }

        /// <summary>
        /// True when the lesson has any progress: a status other than not started, a completed area, an activity that is
        /// not "not started", study time, or an evidence attached.
        /// </summary>
        private static bool HasProgress(CustomPlanClientLesson lesson, bool hasEvidence) =>
            hasEvidence
            || lesson.TotalStudyMinutes > 0
            || lesson.LessonStatus != PlanLessonStatus.NotStarted
            || lesson.ConceptCompleted || lesson.DefinitionsCompleted || lesson.ExamplesCompleted
            || lesson.PreCompleted || lesson.PracticesCompleted || lesson.TestCompleted || lesson.PostCompleted
            || lesson.Practices.Any(p => p.Status != PlanActivityStatus.NotStarted)
            || lesson.ExamDates.Any(e => e.Status != PlanActivityStatus.NotStarted)
            || lesson.Tests.Any(t => t.Status != PlanActivityStatus.NotStarted);

        private static void Sync<TEntity, TItem, TKey>(
            List<TEntity> current,
            IEnumerable<TItem> desired,
            Func<TEntity, TKey> entityKey,
            Func<TItem, TKey> itemKey,
            Func<TEntity> create,
            Action<TEntity, TItem> update)
            where TKey : notnull
        {
            var desiredItems = desired.ToList();
            var desiredKeys = desiredItems.Select(itemKey).ToHashSet();
            current.RemoveAll(e => !desiredKeys.Contains(entityKey(e)));

            foreach (var item in desiredItems)
            {
                var key = itemKey(item);
                var entity = current.FirstOrDefault(e => EqualityComparer<TKey>.Default.Equals(entityKey(e), key));
                if (entity == null)
                {
                    entity = create();
                    current.Add(entity);
                }

                update(entity, item);
            }
        }

        private static CustomPlanResponseModel MapPlan(CustomPlanClient plan, IReadOnlyDictionary<ulong, string> names)
        {
            var model = MapSummary(new CustomPlanResponseModel(), plan, names);
            model.SubjectIds = plan.Subjects.Select(s => s.SubjectId).OrderBy(id => id).ToList();
            model.Days = plan.Days.OrderBy(d => d.DayOfWeek).Select(d => new CustomPlanDayResponseModel
            {
                Id = d.Id,
                DayOfWeek = d.DayOfWeek,
                Times = d.Times.OrderBy(t => t.StartTime).Select(t => new CustomPlanDayTimeResponseModel
                {
                    Id = t.Id,
                    StartTime = t.StartTime,
                    DurationMinutes = t.DurationMinutes,
                    Timezone = t.Timezone,
                }).ToList(),
            }).ToList();
            model.Lessons = plan.Lessons.OrderBy(l => l.Sequence).ThenBy(l => l.Id).Select(MapLesson).ToList();
            return model;
        }

        private static TModel MapSummary<TModel>(TModel model, CustomPlanClient plan, IReadOnlyDictionary<ulong, string> names)
            where TModel : CustomPlanSummaryModel
        {
            model.Id = plan.Id;
            model.CustomPlanTypeId = plan.CustomPlanTypeId;
            model.CustomPlanTypeCode = plan.CustomPlanType?.Code;
            model.ClientId = plan.ClientId;
            model.ClientName = names.GetValueOrDefault(plan.ClientId);
            model.StudentId = plan.StudentId;
            model.StudentName = names.GetValueOrDefault(plan.StudentId);
            model.LevelId = plan.LevelId;
            model.CurriculumFrameworkId = plan.CurriculumFrameworkId;
            model.CurriculumFrameworkCode = plan.CurriculumFramework?.Code;
            model.PlanDurationWeeks = plan.PlanDurationWeeks;
            model.StartDate = plan.StartDate;
            model.EndDate = plan.EndDate;
            model.Status = plan.Status;
            model.TotalStudyMinutes = plan.TotalStudyMinutes;
            model.CreatedAt = plan.CreatedAt;
            model.UpdatedAt = plan.UpdatedAt;
            return model;
        }

        /// <summary>Maps a plan lesson with its session dates, exams, practices and tests.</summary>
        /// <param name="lesson">The plan lesson, with its children loaded.</param>
        /// <returns>The response model.</returns>
        internal static CustomPlanLessonResponseModel MapLesson(CustomPlanClientLesson lesson)
        {
            return new CustomPlanLessonResponseModel
            {
                Id = lesson.Id,
                LessonId = lesson.LessonId,
                StandardId = lesson.StandardId,
                Sequence = lesson.Sequence,
                LearningAssignedDate = lesson.LearningAssignedDate,
                LearningDueDate = lesson.LearningDueDate,
                LearningTimezone = lesson.LearningTimezone,
                LearningCompletedAt = lesson.LearningCompletedAt,
                SubjectId = lesson.SubjectId,
                LessonStatus = lesson.LessonStatus,
                ConceptCompleted = lesson.ConceptCompleted,
                DefinitionsCompleted = lesson.DefinitionsCompleted,
                ExamplesCompleted = lesson.ExamplesCompleted,
                PreCompleted = lesson.PreCompleted,
                PracticesCompleted = lesson.PracticesCompleted,
                TestCompleted = lesson.TestCompleted,
                PostCompleted = lesson.PostCompleted,
                CompletedAt = lesson.CompletedAt,
                TotalStudyMinutes = lesson.TotalStudyMinutes,
                AssignedDates = lesson.Dates.Select(d => d.AssignedDate).OrderBy(d => d).ToList(),
                Exams = lesson.ExamDates.OrderBy(e => e.AssignedDate).ThenBy(e => e.Id).Select(MapExam).ToList(),
                Practices = lesson.Practices.OrderBy(p => p.Sequence).ThenBy(p => p.Id).Select(MapPractice).ToList(),
                Tests = lesson.Tests.OrderBy(t => t.TestType).ThenBy(t => t.Id).Select(MapTest).ToList(),
            };
        }

        /// <summary>Maps a scheduled pre-test or post-test.</summary>
        /// <param name="test">The scheduled test.</param>
        /// <returns>The response model.</returns>
        internal static CustomPlanTestResponseModel MapTest(CustomPlanClientLessonTest test)
        {
            return new CustomPlanTestResponseModel
            {
                Id = test.Id,
                TestType = test.TestType,
                TestId = test.TestId,
                AssignedDate = test.AssignedDate,
                DueDateTime = test.DueDateTime,
                Timezone = test.Timezone,
                DurationMinutes = test.DurationMinutes,
                Status = test.Status,
                GwpUserResultId = test.GwpUserResultId,
                CompletedAt = test.CompletedAt,
            };
        }

        /// <summary>Maps a scheduled practice with its stored score and attempts.</summary>
        /// <param name="practice">The scheduled practice.</param>
        /// <returns>The response model.</returns>
        internal static CustomPlanPracticeResponseModel MapPractice(CustomPlanClientLessonPractice practice)
        {
            return new CustomPlanPracticeResponseModel
            {
                Id = practice.Id,
                PracticeId = practice.PracticeId,
                Sequence = practice.Sequence,
                AssignedDate = practice.AssignedDate,
                DueDateTime = practice.DueDateTime,
                Timezone = practice.Timezone,
                Status = practice.Status,
                Score = practice.Score,
                BestScore = practice.BestScore,
                TotalCorrect = practice.TotalCorrect,
                TotalQuestions = practice.TotalQuestions,
                Attempts = practice.Attempts,
                LastAttemptAt = practice.LastAttemptAt,
                CompletedAt = practice.CompletedAt,
            };
        }

        /// <summary>Maps a scheduled exam date.</summary>
        /// <param name="exam">The scheduled exam date.</param>
        /// <returns>The response model.</returns>
        internal static CustomPlanExamResponseModel MapExam(CustomPlanClientLessonExamDate exam)
        {
            return new CustomPlanExamResponseModel
            {
                Id = exam.Id,
                ExamId = exam.ExamId,
                AssignedDate = exam.AssignedDate,
                DueDateTime = exam.DueDateTime,
                Timezone = exam.Timezone,
                DurationMinutes = exam.DurationMinutes,
                GwpUserResultId = exam.GwpUserResultId,
                Status = exam.Status,
                CompletedAt = exam.CompletedAt,
            };
        }

        private sealed record PlanHeader(
            byte CustomPlanTypeId,
            ulong LevelId,
            ushort? CurriculumFrameworkId,
            ushort PlanDurationWeeks,
            DateOnly StartDate,
            DateOnly? EndDate);
    }
}
