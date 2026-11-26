using GSWebAPI.GSWebAPI.Domain.Models.CustomPlans;
using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using GSWebAPI.GSWebAPI.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace GSWebAPI.GSWebAPI.Services.CustomPlans
{
    /// <summary>Status and results of the scheduled activities of a plan: practices, exams and pre/post tests.</summary>
    public interface ICustomPlanActivityService
    {
        /// <summary>Changes the status of a scheduled practice. Only the plan's client, while the plan is active.</summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="planPracticeId">Id of the scheduled practice (plan row, not the Athenas quiz id).</param>
        /// <param name="status">New status; completed_at is set on completed and cleared otherwise.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The updated practice (the lesson's practices_completed flag follows). Errors: <see cref="ServiceErrorCodes.PlanNotFound"/>,
        /// <see cref="ServiceErrorCodes.PlanPracticeNotFound"/> (not in this plan), <see cref="ServiceErrorCodes.PlanNotActive"/>.
        /// </returns>
        Task<ServiceResult<CustomPlanPracticeResponseModel>> UpdatePracticeStatusAsync(RequestUser user, ulong planId, ulong planPracticeId, PlanActivityStatus status, CancellationToken cancellationToken = default);

        /// <summary>
        /// Records one practice attempt entered by the client: the score is stored on the practice (never in
        /// usqui_user_quiz_results), attempts +1, last and best score kept, and the practice is completed.
        /// Only the plan's client, while the plan is active.
        /// </summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="planPracticeId">Id of the scheduled practice.</param>
        /// <param name="request">Score (0-100) and optional correct / total questions (correct cannot exceed total).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The updated practice. Errors: <see cref="ServiceErrorCodes.PlanPracticeScoreInvalid"/>, <see cref="ServiceErrorCodes.PlanNotFound"/>,
        /// <see cref="ServiceErrorCodes.PlanPracticeNotFound"/>, <see cref="ServiceErrorCodes.PlanNotActive"/>.
        /// </returns>
        Task<ServiceResult<CustomPlanPracticeResponseModel>> RecordPracticeResultAsync(RequestUser user, ulong planId, ulong planPracticeId, CustomPlanPracticeResultRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>Changes the status of a scheduled exam date. Only the plan's client, while the plan is active.</summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="examDateId">Id of the scheduled exam date.</param>
        /// <param name="status">New status; completed_at is set on completed and cleared otherwise.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The updated exam (the lesson flags follow). Errors: <see cref="ServiceErrorCodes.PlanNotFound"/>,
        /// <see cref="ServiceErrorCodes.PlanExamNotFound"/>, <see cref="ServiceErrorCodes.PlanNotActive"/>.
        /// </returns>
        Task<ServiceResult<CustomPlanExamResponseModel>> UpdateExamStatusAsync(RequestUser user, ulong planId, ulong examDateId, PlanActivityStatus status, CancellationToken cancellationToken = default);

        /// <summary>
        /// Links the student's usqui_user_quiz_results attempt to a scheduled exam date and completes it. The result must
        /// belong to the plan's student and the exam's quiz, with is_exam 1 (or null, older rows).
        /// Only the plan's client, while the plan is active.
        /// </summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="examDateId">Id of the scheduled exam date.</param>
        /// <param name="gwpUserResultId">usqui_user_quiz_results.id of the attempt.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The updated exam. Errors: <see cref="ServiceErrorCodes.PlanNotFound"/>, <see cref="ServiceErrorCodes.PlanExamNotFound"/>,
        /// <see cref="ServiceErrorCodes.PlanNotActive"/>, <see cref="ServiceErrorCodes.PlanExamResultInvalid"/>.
        /// </returns>
        Task<ServiceResult<CustomPlanExamResponseModel>> LinkExamResultAsync(RequestUser user, ulong planId, ulong examDateId, int gwpUserResultId, CancellationToken cancellationToken = default);

        /// <summary>Changes the status of a scheduled pre-test or post-test. Only the plan's client, while the plan is active.</summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="planTestId">Id of the scheduled test.</param>
        /// <param name="status">New status; completed_at is set on completed and cleared otherwise.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The updated test (the lesson flags follow). Errors: <see cref="ServiceErrorCodes.PlanNotFound"/>,
        /// <see cref="ServiceErrorCodes.PlanTestNotFound"/>, <see cref="ServiceErrorCodes.PlanNotActive"/>.
        /// </returns>
        Task<ServiceResult<CustomPlanTestResponseModel>> UpdateTestStatusAsync(RequestUser user, ulong planId, ulong planTestId, PlanActivityStatus status, CancellationToken cancellationToken = default);

        /// <summary>
        /// Links the student's usqui_user_quiz_results attempt to a scheduled pre-test or post-test and completes it. The
        /// result must belong to the plan's student and the test's quiz, with is_exam 2 (pre-test) or 3 (post-test).
        /// Only the plan's client, while the plan is active.
        /// </summary>
        /// <param name="user">The calling user.</param>
        /// <param name="planId">Custom plan id.</param>
        /// <param name="planTestId">Id of the scheduled test.</param>
        /// <param name="gwpUserResultId">usqui_user_quiz_results.id of the attempt.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// The updated test. Errors: <see cref="ServiceErrorCodes.PlanNotFound"/>, <see cref="ServiceErrorCodes.PlanTestNotFound"/>,
        /// <see cref="ServiceErrorCodes.PlanNotActive"/>, <see cref="ServiceErrorCodes.PlanTestResultInvalid"/>.
        /// </returns>
        Task<ServiceResult<CustomPlanTestResponseModel>> LinkTestResultAsync(RequestUser user, ulong planId, ulong planTestId, int gwpUserResultId, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Business rules:
    /// <list type="bullet">
    /// <item>Only the plan's client changes them, and only while the plan is active.</item>
    /// <item>Status: not_started / in_progress / completed. completed_at is set when it becomes completed and cleared
    /// when it moves away.</item>
    /// <item>Practices: the score is stored on the practice (practice results are not saved in usqui_user_quiz_results).
    /// Each recorded attempt counts one more attempt, keeps the last and the best score, and completes the practice.</item>
    /// <item>Exams and tests: their result is the student's attempt in usqui_user_quiz_results (same student, same
    /// quiz, is_exam 1 for exams, 2 for pre-tests, 3 for post-tests). Linking it completes the activity.</item>
    /// <item>The lesson's practices_completed, test_completed (exams), pre_completed and post_completed follow the
    /// scheduled activities of each kind.</item>
    /// </list>
    /// </summary>
    public sealed class CustomPlanActivityService : ICustomPlanActivityService
    {
        private const int IsExamExam = 1;
        private const int IsExamPreTest = 2;
        private const int IsExamPostTest = 3;

        private readonly GSWebDbContext _db;
        private readonly TimeProvider _timeProvider;

        /// <summary>Creates the service.</summary>
        /// <param name="db">GSWeb database context.</param>
        /// <param name="timeProvider">Clock for completion and attempt dates.</param>
        public CustomPlanActivityService(GSWebDbContext db, TimeProvider timeProvider)
        {
            _db = db;
            _timeProvider = timeProvider;
        }

        // ---------- Practices ----------

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanPracticeResponseModel>> UpdatePracticeStatusAsync(RequestUser user, ulong planId, ulong planPracticeId, PlanActivityStatus status, CancellationToken cancellationToken = default)
        {
            var (_, practice, failure) = await LoadAsync(user, planId, ServiceErrorCodes.PlanPracticeNotFound, cancellationToken,
                _db.CustomPlanClientLessonPractices.Include(p => p.Lesson).ThenInclude(l => l.Practices).Where(p => p.Id == planPracticeId));
            if (failure != null)
            {
                return failure.As<CustomPlanPracticeResponseModel>();
            }

            SetStatus(practice!, status, _timeProvider.UtcNowSeconds());
            UpdateLessonFlags(practice!.Lesson);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<CustomPlanPracticeResponseModel>.Ok(CustomPlanService.MapPractice(practice));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanPracticeResponseModel>> RecordPracticeResultAsync(RequestUser user, ulong planId, ulong planPracticeId, CustomPlanPracticeResultRequestModel request, CancellationToken cancellationToken = default)
        {
            if (request.Score is < 0 or > 100
                || (request.TotalCorrect.HasValue && request.TotalQuestions.HasValue && request.TotalCorrect > request.TotalQuestions))
            {
                return ServiceResult<CustomPlanPracticeResponseModel>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.PlanPracticeScoreInvalid);
            }

            var (_, practice, failure) = await LoadAsync(user, planId, ServiceErrorCodes.PlanPracticeNotFound, cancellationToken,
                _db.CustomPlanClientLessonPractices.Include(p => p.Lesson).ThenInclude(l => l.Practices).Where(p => p.Id == planPracticeId));
            if (failure != null)
            {
                return failure.As<CustomPlanPracticeResponseModel>();
            }

            RecordPracticeAttempt(practice!, request.Score, request.TotalCorrect, request.TotalQuestions, _timeProvider.UtcNowSeconds());
            UpdateLessonFlags(practice!.Lesson);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<CustomPlanPracticeResponseModel>.Ok(CustomPlanService.MapPractice(practice));
        }

        // ---------- Exams ----------

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanExamResponseModel>> UpdateExamStatusAsync(RequestUser user, ulong planId, ulong examDateId, PlanActivityStatus status, CancellationToken cancellationToken = default)
        {
            var (_, exam, failure) = await LoadAsync(user, planId, ServiceErrorCodes.PlanExamNotFound, cancellationToken,
                _db.CustomPlanClientLessonExamDates.Include(e => e.Lesson).Where(e => e.Id == examDateId));
            if (failure != null)
            {
                return failure.As<CustomPlanExamResponseModel>();
            }

            SetStatus(exam!, status, _timeProvider.UtcNowSeconds());
            await UpdateLessonFlagsAsync(exam!.CustomPlanClientLessonId, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<CustomPlanExamResponseModel>.Ok(CustomPlanService.MapExam(exam!));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanExamResponseModel>> LinkExamResultAsync(RequestUser user, ulong planId, ulong examDateId, int gwpUserResultId, CancellationToken cancellationToken = default)
        {
            var (plan, exam, failure) = await LoadAsync(user, planId, ServiceErrorCodes.PlanExamNotFound, cancellationToken,
                _db.CustomPlanClientLessonExamDates.Include(e => e.Lesson).Where(e => e.Id == examDateId));
            if (failure != null)
            {
                return failure.As<CustomPlanExamResponseModel>();
            }

            if (!await IsStudentAttemptAsync(gwpUserResultId, plan!.StudentId, exam!.ExamId, IsExamExam, allowLegacyNullType: true, cancellationToken))
            {
                return ServiceResult<CustomPlanExamResponseModel>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.PlanExamResultInvalid);
            }

            LinkResult(exam, gwpUserResultId, _timeProvider.UtcNowSeconds());
            await UpdateLessonFlagsAsync(exam.CustomPlanClientLessonId, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<CustomPlanExamResponseModel>.Ok(CustomPlanService.MapExam(exam));
        }

        // ---------- Pre/post tests ----------

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanTestResponseModel>> UpdateTestStatusAsync(RequestUser user, ulong planId, ulong planTestId, PlanActivityStatus status, CancellationToken cancellationToken = default)
        {
            var (_, test, failure) = await LoadAsync(user, planId, ServiceErrorCodes.PlanTestNotFound, cancellationToken,
                _db.CustomPlanClientLessonTests.Include(t => t.Lesson).Where(t => t.Id == planTestId));
            if (failure != null)
            {
                return failure.As<CustomPlanTestResponseModel>();
            }

            SetStatus(test!, status, _timeProvider.UtcNowSeconds());
            await UpdateLessonFlagsAsync(test!.CustomPlanClientLessonId, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<CustomPlanTestResponseModel>.Ok(CustomPlanService.MapTest(test!));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CustomPlanTestResponseModel>> LinkTestResultAsync(RequestUser user, ulong planId, ulong planTestId, int gwpUserResultId, CancellationToken cancellationToken = default)
        {
            var (plan, test, failure) = await LoadAsync(user, planId, ServiceErrorCodes.PlanTestNotFound, cancellationToken,
                _db.CustomPlanClientLessonTests.Include(t => t.Lesson).Where(t => t.Id == planTestId));
            if (failure != null)
            {
                return failure.As<CustomPlanTestResponseModel>();
            }

            var isExam = test!.TestType == PlanTestType.PreTest ? IsExamPreTest : IsExamPostTest;
            if (!await IsStudentAttemptAsync(gwpUserResultId, plan!.StudentId, test.TestId, isExam, allowLegacyNullType: false, cancellationToken))
            {
                return ServiceResult<CustomPlanTestResponseModel>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.PlanTestResultInvalid);
            }

            LinkResult(test, gwpUserResultId, _timeProvider.UtcNowSeconds());
            await UpdateLessonFlagsAsync(test.CustomPlanClientLessonId, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<CustomPlanTestResponseModel>.Ok(CustomPlanService.MapTest(test));
        }

        // ---------- Shared ----------

        /// <summary>Sets the status and keeps completed_at consistent with it (kept if already completed, cleared otherwise).</summary>
        /// <param name="activity">The practice, exam date or test.</param>
        /// <param name="status">New status.</param>
        /// <param name="utcNow">Current UTC time, used as completed_at when it becomes completed.</param>
        internal static void SetStatus(IPlanActivity activity, PlanActivityStatus status, DateTime utcNow)
        {
            activity.CompletedAt = status == PlanActivityStatus.Completed ? activity.CompletedAt ?? utcNow : null;
            activity.Status = status;
        }

        /// <summary>
        /// The lesson's section flags follow its scheduled activities (only for the kinds the lesson has):
        /// practices_completed = all practices completed; test_completed = all exams completed;
        /// pre_completed / post_completed = the pre-test / post-test completed.
        /// </summary>
        /// <param name="lesson">The plan lesson, with its loaded practices, exam dates and tests (kinds not loaded are left unchanged).</param>
        internal static void UpdateLessonFlags(CustomPlanClientLesson lesson)
        {
            if (lesson.Practices.Count > 0)
            {
                lesson.PracticesCompleted = lesson.Practices.All(p => p.Status == PlanActivityStatus.Completed);
            }

            if (lesson.ExamDates.Count > 0)
            {
                lesson.TestCompleted = lesson.ExamDates.All(e => e.Status == PlanActivityStatus.Completed);
            }

            var pre = lesson.Tests.Where(t => t.TestType == PlanTestType.PreTest).ToList();
            if (pre.Count > 0)
            {
                lesson.PreCompleted = pre.All(t => t.Status == PlanActivityStatus.Completed);
            }

            var post = lesson.Tests.Where(t => t.TestType == PlanTestType.PostTest).ToList();
            if (post.Count > 0)
            {
                lesson.PostCompleted = post.All(t => t.Status == PlanActivityStatus.Completed);
            }
        }

        /// <summary>One more practice attempt: keeps the last score, the best score and the count, and completes it.</summary>
        /// <param name="practice">The scheduled practice.</param>
        /// <param name="score">Score of the attempt; clamped to 0-100 and rounded to 2 decimals.</param>
        /// <param name="totalCorrect">Correct answers of the attempt, if known.</param>
        /// <param name="totalQuestions">Questions of the attempt, if known.</param>
        /// <param name="utcNow">Current UTC time (last attempt and completion date).</param>
        internal static void RecordPracticeAttempt(CustomPlanClientLessonPractice practice, decimal score, ushort? totalCorrect, ushort? totalQuestions, DateTime utcNow)
        {
            var rounded = Math.Round(Math.Clamp(score, 0m, 100m), 2);
            practice.Score = rounded;
            practice.BestScore = practice.BestScore is { } best && best > rounded ? best : rounded;
            practice.TotalCorrect = totalCorrect;
            practice.TotalQuestions = totalQuestions;
            practice.Attempts = (ushort)Math.Min(practice.Attempts + 1, ushort.MaxValue);
            practice.LastAttemptAt = utcNow;
            SetStatus(practice, PlanActivityStatus.Completed, utcNow);
        }

        /// <summary>Links a usqui result to an exam date and completes it (a different attempt is completed again now).</summary>
        /// <param name="exam">The scheduled exam date.</param>
        /// <param name="resultId">usqui_user_quiz_results.id of the attempt.</param>
        /// <param name="utcNow">Current UTC time, the completion date when the result changes.</param>
        internal static void LinkResult(CustomPlanClientLessonExamDate exam, int resultId, DateTime utcNow)
        {
            if (exam.GwpUserResultId != resultId)
            {
                exam.CompletedAt = null;
            }

            exam.GwpUserResultId = resultId;
            SetStatus(exam, PlanActivityStatus.Completed, utcNow);
        }

        /// <summary>Links a usqui result to a pre-test or post-test and completes it (a different attempt is completed again now).</summary>
        /// <param name="test">The scheduled test.</param>
        /// <param name="resultId">usqui_user_quiz_results.id of the attempt.</param>
        /// <param name="utcNow">Current UTC time, the completion date when the result changes.</param>
        internal static void LinkResult(CustomPlanClientLessonTest test, int resultId, DateTime utcNow)
        {
            if (test.GwpUserResultId != resultId)
            {
                test.CompletedAt = null;
            }

            test.GwpUserResultId = resultId;
            SetStatus(test, PlanActivityStatus.Completed, utcNow);
        }

        private async Task UpdateLessonFlagsAsync(ulong lessonId, CancellationToken cancellationToken)
        {
            var lesson = await _db.CustomPlanClientLessons
                .Include(l => l.Practices).Include(l => l.ExamDates).Include(l => l.Tests)
                .FirstAsync(l => l.Id == lessonId, cancellationToken);
            UpdateLessonFlags(lesson);
        }

        /// <summary>Loads an activity of an active plan owned by the client.</summary>
        private async Task<(CustomPlanClient? Plan, TActivity? Activity, ServiceResult<bool>? Failure)> LoadAsync<TActivity>(
            RequestUser user, ulong planId, string notFoundCode, CancellationToken cancellationToken, IQueryable<TActivity> query)
            where TActivity : class, IPlanActivity
        {
            var plan = await _db.CustomPlanClients.AsNoTracking().FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);
            if (plan == null || plan.ClientId != user.UserId)
            {
                return (null, null, ServiceResult<bool>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.PlanNotFound));
            }

            var lessonIds = _db.CustomPlanClientLessons.Where(l => l.CustomPlanClientId == planId).Select(l => l.Id);
            var activity = await query.Where(a => lessonIds.Contains(a.CustomPlanClientLessonId)).FirstOrDefaultAsync(cancellationToken);
            if (activity == null)
            {
                return (plan, null, ServiceResult<bool>.Fail(ServiceErrorKind.NotFound, notFoundCode));
            }

            if (plan.Status != CustomPlanStatus.Active)
            {
                return (plan, activity, ServiceResult<bool>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.PlanNotActive));
            }

            return (plan, activity, null);
        }

        /// <summary>
        /// True when the usqui_user_quiz_results row is the student's attempt at that quiz, of that kind (is_exam).
        /// Exams also accept a null is_exam (older rows), as before this change.
        /// </summary>
        private async Task<bool> IsStudentAttemptAsync(int resultId, ulong studentId, ulong quizId, int isExam, bool allowLegacyNullType, CancellationToken cancellationToken)
        {
            var result = await _db.UserQuizResults.AsNoTracking().FirstOrDefaultAsync(r => r.Id == resultId, cancellationToken);
            return result != null
                && result.UserId is >= 0 && (ulong)result.UserId.Value == studentId
                && result.QuizId is >= 0 && (ulong)result.QuizId.Value == quizId
                && (result.IsExam == isExam || (allowLegacyNullType && result.IsExam == null));
        }
    }
}
