using System.Globalization;
using GSWebAPI.GSWebAPI.Domain.Models.UserQuiz;
using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GSWebAPI.GSWebAPI.Services.CustomPlans
{
    /// <summary>A quiz the student just submitted and that was corrected by <c>UserQuizService.SubmitQuizResponse</c>.</summary>
    /// <param name="StudentId">Id of the student (user) who submitted the quiz.</param>
    /// <param name="LessonId">Athenas lesson id of the quiz.</param>
    /// <param name="QuizId">Athenas quiz id.</param>
    /// <param name="IsExam">Athenas IsExam: 0 practice, 1 exam, 2 pre-test, 3 post-test.</param>
    /// <param name="ResultId">usqui_user_quiz_results.id. Null for practices (their results are not saved there).</param>
    /// <param name="Score">Score percentage of the attempt, if it could be read.</param>
    /// <param name="TotalCorrect">Number of correct answers, if it could be read.</param>
    /// <param name="TotalQuestions">Number of questions, if it could be read.</param>
    public sealed record CustomPlanQuizSubmission(
        ulong StudentId, ulong LessonId, ulong QuizId, int IsExam, int? ResultId, decimal? Score, ushort? TotalCorrect, ushort? TotalQuestions)
    {
        /// <summary>Builds the submission from the quiz endpoint's values. Null when they cannot be read (nothing to evaluate).</summary>
        /// <param name="userId">Student id as received by the endpoint.</param>
        /// <param name="lessonId">Lesson id as received by the endpoint.</param>
        /// <param name="quizId">Quiz id as received by the endpoint.</param>
        /// <param name="isExam">Athenas IsExam value as received by the endpoint.</param>
        /// <param name="result">The corrected result returned by the quiz service.</param>
        /// <returns>The submission, or null when the result is missing or invalid or an id cannot be parsed.</returns>
        public static CustomPlanQuizSubmission? From(string? userId, string? lessonId, string? quizId, string? isExam, UserQuizResultModel? result)
        {
            if (result == null || !result.Valid
                || !ulong.TryParse(userId, out var student) || !ulong.TryParse(lessonId, out var lesson)
                || !ulong.TryParse(quizId, out var quiz) || !int.TryParse(isExam, out var type))
            {
                return null;
            }

            return new CustomPlanQuizSubmission(
                student, lesson, quiz, type,
                int.TryParse(result.Id, out var id) ? id : null,
                ParseScore(result.TotalPercent),
                ushort.TryParse(result.TotalCorrect, out var correct) ? correct : null,
                ushort.TryParse(result.TotalQuestions, out var total) ? total : null);
        }

        /// <summary>
        /// UserQuizService writes TotalPercent with decimal.ToString() (server culture), so that culture is tried first;
        /// a plain "85.5" is also accepted. Only decimal separators are allowed, never thousands separators.
        /// </summary>
        /// <param name="value">TotalPercent text of the result.</param>
        /// <returns>The parsed score, or null when the value is blank or not a number.</returns>
        public static decimal? ParseScore(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            const NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;
            return decimal.TryParse(value, styles, CultureInfo.CurrentCulture, out var current) ? current
                : decimal.TryParse(value, styles, CultureInfo.InvariantCulture, out var invariant) ? invariant
                : null;
        }
    }

    /// <summary>What the evaluation changed (for logs and tests).</summary>
    /// <param name="ActivitiesUpdated">Number of plan lessons whose practice, exam or test received the quiz.</param>
    /// <param name="LessonsCompleted">Number of plan lessons auto-completed by this quiz.</param>
    public sealed record CustomPlanQuizEvaluation(int ActivitiesUpdated, int LessonsCompleted)
    {
        /// <summary>Nothing changed (no active plan with the lesson, nothing scheduled for the quiz, or an error).</summary>
        public static readonly CustomPlanQuizEvaluation Nothing = new(0, 0);
    }

    /// <summary>Applies a submitted quiz to the student's custom plans.</summary>
    public interface ICustomPlanQuizEvaluator
    {
        /// <summary>
        /// Applies the quiz to every active custom plan of the student that has the lesson: completes the matching
        /// scheduled practice, exam or pre/post test, updates the lesson flags and auto-completes the lesson when no
        /// Athenas practice or exam is pending. Never throws: the quiz result is already saved and must not be affected
        /// by the custom plan, so errors are logged and <see cref="CustomPlanQuizEvaluation.Nothing"/> is returned
        /// (only cancellation propagates).
        /// </summary>
        /// <param name="submission">The corrected quiz (see <see cref="CustomPlanQuizSubmission.From"/>).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>How many plan lessons were updated and how many were completed.</returns>
        Task<CustomPlanQuizEvaluation> EvaluateAsync(CustomPlanQuizSubmission submission, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Called by the quiz submit endpoint after its normal process, which stays as it is. Only students with an
    /// <b>active</b> custom plan that has this lesson are affected; for everyone else it does nothing.
    /// <list type="bullet">
    /// <item>Practice (IsExam 0): records the attempt on the scheduled practice (last score, best score, attempts) and
    /// completes it. Practice results are not saved in usqui_user_quiz_results, so the score comes from the correction.</item>
    /// <item>Exam (1): links the new usqui result to the scheduled exam and completes it. With several dates for the same
    /// exam, the first one not completed gets it; if all are completed, the latest one gets the new attempt.</item>
    /// <item>Pre-test (2) / post-test (3): links the result to the scheduled test and completes it.</item>
    /// <item>Then the lesson flags follow (practices_completed, test_completed, pre_completed, post_completed), and the
    /// lesson is marked completed when every practice and exam it has in Athenas is completed in the plan (pre/post
    /// tests are optional). If Athenas cannot be reached, the lesson is left as it is and is evaluated on the next quiz.</item>
    /// </list>
    /// </summary>
    public sealed class CustomPlanQuizEvaluator : ICustomPlanQuizEvaluator
    {
        private readonly GSWebDbContext _db;
        private readonly IAthenasCatalogClient _athenas;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<CustomPlanQuizEvaluator> _logger;

        /// <summary>Creates the evaluator.</summary>
        /// <param name="db">GSWeb database context.</param>
        /// <param name="athenas">Athenas client, used to know which practices and exams the lesson has.</param>
        /// <param name="timeProvider">Clock for completion dates.</param>
        /// <param name="logger">Logger for evaluation errors.</param>
        public CustomPlanQuizEvaluator(GSWebDbContext db, IAthenasCatalogClient athenas, TimeProvider timeProvider, ILogger<CustomPlanQuizEvaluator> logger)
        {
            _db = db;
            _athenas = athenas;
            _timeProvider = timeProvider;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<CustomPlanQuizEvaluation> EvaluateAsync(CustomPlanQuizSubmission submission, CancellationToken cancellationToken = default)
        {
            try
            {
                var lessons = await _db.CustomPlanClientLessons
                    .Include(l => l.Practices)
                    .Include(l => l.ExamDates)
                    .Include(l => l.Tests)
                    .Where(l => l.LessonId == submission.LessonId
                        && l.CustomPlanClient.StudentId == submission.StudentId
                        && l.CustomPlanClient.Status == CustomPlanStatus.Active)
                    .AsSplitQuery()
                    .ToListAsync(cancellationToken);
                if (lessons.Count == 0)
                {
                    return CustomPlanQuizEvaluation.Nothing; // regular student, or the lesson is not in an active plan
                }

                var now = _timeProvider.UtcNowSeconds();
                var updated = 0;
                var completed = 0;
                foreach (var lesson in lessons)
                {
                    if (!Apply(lesson, submission, now))
                    {
                        continue;
                    }

                    updated++;
                    CustomPlanActivityService.UpdateLessonFlags(lesson);
                    if (lesson.LessonStatus != PlanLessonStatus.Completed
                        && await CustomPlanService.GetPendingForCompletionAsync(_athenas, lesson, cancellationToken) is { Count: 0 })
                    {
                        CustomPlanService.SetLessonStatus(lesson, PlanLessonStatus.Completed, now);
                        completed++;
                    }
                }

                if (updated > 0)
                {
                    await _db.SaveChangesAsync(cancellationToken);
                }

                return new CustomPlanQuizEvaluation(updated, completed);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Could not apply quiz {QuizId} of student {StudentId} to the custom plans.", submission.QuizId, submission.StudentId);
                return CustomPlanQuizEvaluation.Nothing;
            }
        }

        /// <summary>Applies the quiz to the matching scheduled activity of the lesson. False when the lesson does not schedule it.</summary>
        private static bool Apply(CustomPlanClientLesson lesson, CustomPlanQuizSubmission submission, DateTime now)
        {
            switch (submission.IsExam)
            {
                case 0:
                    var practice = lesson.Practices.FirstOrDefault(p => p.PracticeId == submission.QuizId);
                    if (practice == null || submission.Score == null)
                    {
                        return false;
                    }

                    CustomPlanActivityService.RecordPracticeAttempt(practice, submission.Score.Value, submission.TotalCorrect, submission.TotalQuestions, now);
                    return true;

                case 1:
                    var exams = lesson.ExamDates.Where(e => e.ExamId == submission.QuizId).OrderBy(e => e.AssignedDate).ThenBy(e => e.Id).ToList();
                    var exam = exams.FirstOrDefault(e => e.Status != PlanActivityStatus.Completed) ?? exams.LastOrDefault();
                    if (exam == null || submission.ResultId == null)
                    {
                        return false;
                    }

                    CustomPlanActivityService.LinkResult(exam, submission.ResultId.Value, now);
                    return true;

                case 2:
                case 3:
                    var type = submission.IsExam == 2 ? PlanTestType.PreTest : PlanTestType.PostTest;
                    var test = lesson.Tests.FirstOrDefault(t => t.TestType == type && t.TestId == submission.QuizId);
                    if (test == null || submission.ResultId == null)
                    {
                        return false;
                    }

                    CustomPlanActivityService.LinkResult(test, submission.ResultId.Value, now);
                    return true;

                default:
                    return false;
            }
        }
    }
}
