using System.Globalization;
using GSWebAPI.GSWebAPI.Domain.Models.CustomPlans;
using GSWebAPI.GSWebAPI.Domain.Models.UserQuiz;
using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.CustomPlans;
using GSWebAPI.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using static GSWebAPI.Tests.CustomPlans.CustomPlanServiceTests;

namespace GSWebAPI.Tests.CustomPlans
{
    /// <summary>The quiz submit endpoint applied to the student's custom plan, and practices/exams added to a running plan.</summary>
    [Collection(MySqlCollection.Name)]
    public sealed class CustomPlanQuizEvaluatorTests
    {
        private readonly MySqlDatabaseFixture _fixture;
        private readonly FakeUserDirectory _users = new();
        private readonly FakeAthenasCatalog _athenas = new();

        public CustomPlanQuizEvaluatorTests(MySqlDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        private CustomPlanService Plans(GSWebDbContext db) => new(db, _users, new StandardFrameworkValidator(), new GSWebModuleSettings(), _fixture.Time, _athenas);

        private CustomPlanQuizEvaluator Evaluator(GSWebDbContext db) => new(db, _athenas, _fixture.Time, NullLogger<CustomPlanQuizEvaluator>.Instance);

        private async Task<(RequestUser Client, ulong StudentId, CustomPlanResponseModel Plan)> NewActivePlanAsync(Action<CreateCustomPlanRequestModel>? change = null)
        {
            var client = TestUsers.Client(TestUsers.NewId());
            var studentId = TestUsers.NewId();
            _users.AddStudent(client.UserId, studentId);
            await using var db = _fixture.CreateContext();
            var request = NewRequest(studentId);
            change?.Invoke(request);
            var plan = (await Plans(db).CreateAsync(client, request)).Data!;
            Assert.True((await Plans(db).ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active)).Valid);
            return (client, studentId, plan);
        }

        private async Task<int> InsertResultAsync(ulong studentId, ulong quizId, int isExam, decimal percent)
        {
            await _fixture.ExecuteSqlAsync(FormattableString.Invariant(
                $"INSERT INTO usqui_user_quiz_results (user_id, quiz_id, is_exam, lesson_id, total_percent, total_correct, total_questions) VALUES ({studentId}, {quizId}, {isExam}, 501, {percent}, 1, 2)"));
            return Convert.ToInt32(await _fixture.ScalarAsync($"SELECT MAX(id) FROM usqui_user_quiz_results WHERE user_id = {studentId} AND quiz_id = {quizId}"));
        }

        private async Task<CustomPlanQuizEvaluation> SubmitAsync(ulong studentId, ulong quizId, int isExam, int? resultId = null, decimal? score = 90)
        {
            await using var db = _fixture.CreateContext();
            return await Evaluator(db).EvaluateAsync(new CustomPlanQuizSubmission(studentId, quizId == 900 ? 501 : quizId / 10, quizId, isExam, resultId, score, 9, 10));
        }

        private async Task<CustomPlanClientLesson> LessonAsync(ulong planLessonId)
        {
            await using var db = _fixture.CreateContext();
            return await db.CustomPlanClientLessons.AsNoTracking()
                .Include(l => l.Practices).Include(l => l.ExamDates).Include(l => l.Tests)
                .SingleAsync(l => l.Id == planLessonId);
        }

        [Fact]
        public async Task Regular_students_without_a_custom_plan_are_not_affected()
        {
            Assert.Equal(CustomPlanQuizEvaluation.Nothing, await SubmitAsync(TestUsers.NewId(), 5011, 0));
        }

        [Fact]
        public async Task Plans_that_are_not_active_are_not_affected()
        {
            var client = TestUsers.Client(TestUsers.NewId());
            var studentId = TestUsers.NewId();
            _users.AddStudent(client.UserId, studentId);
            await using (var db = _fixture.CreateContext())
            {
                Assert.True((await Plans(db).CreateAsync(client, NewRequest(studentId))).Valid); // stays in draft
            }

            Assert.Equal(CustomPlanQuizEvaluation.Nothing, await SubmitAsync(studentId, 5011, 0));
        }

        [Fact]
        public async Task Submitting_every_practice_and_exam_completes_the_lesson_and_its_flags()
        {
            var (_, studentId, plan) = await NewActivePlanAsync();
            var lessonId = plan.Lessons.Single(l => l.LessonId == 501).Id;

            // Practices: the score comes from the correction (practice results are not saved in usqui).
            Assert.Equal(new CustomPlanQuizEvaluation(1, 0), await SubmitAsync(studentId, 5011, 0, score: 70));
            Assert.Equal(new CustomPlanQuizEvaluation(1, 0), await SubmitAsync(studentId, 5011, 0, score: 95));
            var practice = (await LessonAsync(lessonId)).Practices.Single(p => p.PracticeId == 5011);
            Assert.Equal(PlanActivityStatus.Completed, practice.Status);
            Assert.Equal(95m, practice.Score);
            Assert.Equal(95m, practice.BestScore);
            Assert.Equal((ushort)2, practice.Attempts);
            Assert.Equal((ushort)9, practice.TotalCorrect);

            await SubmitAsync(studentId, 5012, 0);
            await SubmitAsync(studentId, 5013, 0);
            var afterPractices = await LessonAsync(lessonId);
            Assert.True(afterPractices.PracticesCompleted);
            Assert.False(afterPractices.TestCompleted);
            Assert.NotEqual(PlanLessonStatus.Completed, afterPractices.LessonStatus); // the exam is still pending

            // The exam (result in usqui) is the last one: the lesson is completed. Pre/post tests are optional.
            var examResult = await InsertResultAsync(studentId, 900, 1, 88);
            Assert.Equal(new CustomPlanQuizEvaluation(1, 1), await SubmitAsync(studentId, 900, 1, examResult));
            var done = await LessonAsync(lessonId);
            Assert.True(done.TestCompleted);
            Assert.Equal(examResult, done.ExamDates.Single().GwpUserResultId);
            Assert.Equal(PlanActivityStatus.Completed, done.ExamDates.Single().Status);
            Assert.Equal(PlanLessonStatus.Completed, done.LessonStatus);
            Assert.Equal(_fixture.Time.GetUtcNow().UtcDateTime, done.CompletedAt);
            Assert.False(done.PreCompleted);
        }

        [Fact]
        public async Task Pre_and_post_tests_set_their_lesson_flags()
        {
            var (_, studentId, plan) = await NewActivePlanAsync();
            var lessonId = plan.Lessons.Single(l => l.LessonId == 501).Id;

            var pre = await InsertResultAsync(studentId, 5018, 2, 40);
            await SubmitAsync(studentId, 5018, 2, pre);
            var afterPre = await LessonAsync(lessonId);
            Assert.True(afterPre.PreCompleted);
            Assert.False(afterPre.PostCompleted);
            Assert.Equal(pre, afterPre.Tests.Single(t => t.TestType == PlanTestType.PreTest).GwpUserResultId);

            var post = await InsertResultAsync(studentId, 5019, 3, 90);
            await SubmitAsync(studentId, 5019, 3, post);
            Assert.True((await LessonAsync(lessonId)).PostCompleted);

            // A quiz the plan does not schedule changes nothing.
            Assert.Equal(CustomPlanQuizEvaluation.Nothing, await SubmitAsync(studentId, 5029, 0));
        }

        [Fact]
        public async Task With_Athenas_down_the_activity_is_saved_and_the_lesson_waits()
        {
            var (_, studentId, plan) = await NewActivePlanAsync(r =>
            {
                r.Structure.Lessons[1].Practices.RemoveRange(1, 2); // lesson 502: one practice, no exam
            });
            var lessonId = plan.Lessons.Single(l => l.LessonId == 502).Id;
            _athenas.Available = false;

            await using (var db = _fixture.CreateContext())
            {
                Assert.Equal(new CustomPlanQuizEvaluation(1, 0),
                    await Evaluator(db).EvaluateAsync(new CustomPlanQuizSubmission(studentId, 502, 5021, 0, null, 100, 10, 10)));
            }

            var lesson = await LessonAsync(lessonId);
            Assert.Equal(PlanActivityStatus.Completed, lesson.Practices.Single().Status);
            Assert.NotEqual(PlanLessonStatus.Completed, lesson.LessonStatus);
        }

        [Fact]
        public async Task Missing_Athenas_practices_can_be_added_to_a_running_plan()
        {
            // Lesson 502 has three practices in Athenas; the plan only scheduled two (say Athenas added the third later).
            // No pre/post tests: they are optional.
            var (client, studentId, plan) = await NewActivePlanAsync(r =>
            {
                r.Structure.Lessons[1].Practices.RemoveAt(2);
                r.Structure.Lessons[1].Tests.Clear();
            });
            var planLesson = plan.Lessons.Single(l => l.LessonId == 502);
            await SubmitAsync(studentId, 5021, 0);
            await SubmitAsync(studentId, 5022, 0);
            Assert.NotEqual(PlanLessonStatus.Completed, (await LessonAsync(planLesson.Id)).LessonStatus);

            await using var db = _fixture.CreateContext();
            var service = Plans(db);
            var missing = (await new CustomPlanOverviewService(db, _users, _athenas, new FakeBlobStorage(), _fixture.Time)
                .GetOverviewAsync(client, new CustomPlanOverviewFilterModel())).Data!
                .Single().Plans.Single().Subjects[0].Lessons.Single(l => l.LessonId == 502);
            Assert.Equal(new ulong[] { 5023 }, missing.MissingPracticeIds);
            Assert.False(missing.CanBeCompleted);

            // Validation: duplicates, quizzes from another lesson, empty requests.
            Assert.Contains(ServiceErrorCodes.PlanPracticeDuplicated, (await service.AddLessonActivitiesAsync(client, plan.Id, planLesson.Id,
                new CustomPlanLessonActivitiesRequestModel { Practices = { Practice(502, 1) } })).Messages);
            var foreign = Practice(502, 3);
            foreign.PracticeId = 5011;
            Assert.Contains(ServiceErrorCodes.PlanPracticeNotInLesson, (await service.AddLessonActivitiesAsync(client, plan.Id, planLesson.Id,
                new CustomPlanLessonActivitiesRequestModel { Practices = { foreign } })).Messages);
            Assert.Contains(ServiceErrorCodes.PlanActivitiesRequired,
                (await service.AddLessonActivitiesAsync(client, plan.Id, planLesson.Id, new CustomPlanLessonActivitiesRequestModel())).Messages);
            Assert.Equal(ServiceErrorKind.NotFound, (await service.AddLessonActivitiesAsync(TestUsers.Client(TestUsers.NewId()), plan.Id, planLesson.Id,
                new CustomPlanLessonActivitiesRequestModel { Practices = { Practice(502, 3) } })).ErrorKind);

            var added = await service.AddLessonActivitiesAsync(client, plan.Id, planLesson.Id, new CustomPlanLessonActivitiesRequestModel { Practices = { Practice(502, 3) } });
            Assert.True(added.Valid, string.Join(",", added.Messages));
            Assert.Equal(new ulong[] { 5021, 5022, 5023 }, added.Data!.Practices.Select(p => p.PracticeId));
            Assert.False(added.Data.PracticesCompleted);

            // Doing the new practice completes the lesson.
            Assert.Equal(new CustomPlanQuizEvaluation(1, 1), await SubmitAsync(studentId, 5023, 0));
            Assert.Equal(PlanLessonStatus.Completed, (await LessonAsync(planLesson.Id)).LessonStatus);

            // Adding to a completed lesson reopens it; a closed plan accepts nothing.
            await using var db2 = _fixture.CreateContext();
            var reopened = await Plans(db2).AddLessonActivitiesAsync(client, plan.Id, planLesson.Id,
                new CustomPlanLessonActivitiesRequestModel { Tests = { Test(502, PlanTestType.PreTest, new DateOnly(2026, 11, 10), 12) } });
            Assert.True(reopened.Valid, string.Join(",", reopened.Messages));
            Assert.Equal(PlanLessonStatus.Completed, reopened.Data!.LessonStatus); // a pre-test is optional: still completed
            var exam = Exam();
            exam.ExamId = 900;
            await Plans(db2).ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Cancelled);
            Assert.Contains(ServiceErrorCodes.PlanClosed, (await Plans(db2).AddLessonActivitiesAsync(client, plan.Id, planLesson.Id,
                new CustomPlanLessonActivitiesRequestModel { Exams = { exam } })).Messages);
        }

        [Fact]
        public async Task Adding_a_practice_to_a_completed_lesson_reopens_it()
        {
            var (client, studentId, plan) = await NewActivePlanAsync(r => r.Structure.Lessons[1].Practices.RemoveAt(2));
            var planLesson = plan.Lessons.Single(l => l.LessonId == 502);
            await using var db = _fixture.CreateContext();

            // Mark it completed by hand first (as if it had been completed before Athenas added the practice).
            await db.CustomPlanClientLessons.Where(l => l.Id == planLesson.Id)
                .ExecuteUpdateAsync(u => u.SetProperty(l => l.LessonStatus, PlanLessonStatus.Completed).SetProperty(l => l.CompletedAt, DateTime.UtcNow));

            var added = (await Plans(db).AddLessonActivitiesAsync(client, plan.Id, planLesson.Id,
                new CustomPlanLessonActivitiesRequestModel { Practices = { Practice(502, 3) } })).Data!;
            Assert.Equal(PlanLessonStatus.InProgress, added.LessonStatus);
            Assert.Null(added.CompletedAt);
        }

        [Theory]
        [InlineData("85.5", "en-US", 85.5)]
        [InlineData("85,5", "es-ES", 85.5)]
        [InlineData("85.5", "es-ES", 85.5)]
        [InlineData("100", "es-CO", 100.0)]
        [InlineData("1,234", "en-US", null)] // never a thousands separator
        [InlineData("", "en-US", null)]
        public void The_score_from_the_correction_is_read_in_the_server_culture(string value, string culture, double? expected)
        {
            var previous = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo(culture);
                Assert.Equal(expected.HasValue ? (decimal?)expected.Value : null, CustomPlanQuizSubmission.ParseScore(value));
            }
            finally
            {
                CultureInfo.CurrentCulture = previous;
            }
        }

        [Fact]
        public void Submission_is_built_only_from_a_valid_corrected_quiz()
        {
            var result = new UserQuizResultModel { Valid = true, Id = "77", TotalPercent = "90", TotalCorrect = "9", TotalQuestions = "10" };

            var submission = CustomPlanQuizSubmission.From("12", "501", "900", "1", result)!;
            Assert.Equal(new CustomPlanQuizSubmission(12, 501, 900, 1, 77, 90, 9, 10), submission);
            Assert.Null(CustomPlanQuizSubmission.From("12", "501", "5011", "0", new UserQuizResultModel { Valid = true, TotalPercent = "80" })!.ResultId);
            Assert.Null(CustomPlanQuizSubmission.From("12", "501", "900", "1", new UserQuizResultModel { Valid = false }));
            Assert.Null(CustomPlanQuizSubmission.From("12", "501", "900", "1", null));
            Assert.Null(CustomPlanQuizSubmission.From("x", "501", "900", "1", result));
        }
    }
}
