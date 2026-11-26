using GSWebAPI.GSWebAPI.Domain.Models.General;
using System.Text;
using GSWebAPI.GSWebAPI.Domain.Models.CustomPlans;
using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.CustomPlans;
using GSWebAPI.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;
using static GSWebAPI.Tests.CustomPlans.CustomPlanServiceTests;

namespace GSWebAPI.Tests.CustomPlans
{
    [Collection(MySqlCollection.Name)]
    public sealed class CustomPlanOverviewAndEvidenceTests
    {
        private readonly MySqlDatabaseFixture _fixture;
        private readonly FakeUserDirectory _users = new();
        private readonly FakeAthenasCatalog _athenas = new();
        private readonly FakeBlobStorage _storage = new();

        public CustomPlanOverviewAndEvidenceTests(MySqlDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        private CustomPlanService Plans(GSWebDbContext db) => new(db, _users, new StandardFrameworkValidator(), new GSWebModuleSettings(), _fixture.Time, _athenas);

        private CustomPlanActivityService Activities(GSWebDbContext db) => new(db, _fixture.Time);

        private CustomPlanOverviewService Overview(GSWebDbContext db) => new(db, _users, _athenas, _storage, _fixture.Time);

        private CustomPlanEvidenceService Evidences(GSWebDbContext db) => new(db, _storage);

        /// <summary>A client with two children. Child 1 has an active DEPR plan with progress, scores and evidences; child 2 a draft plan.</summary>
        private async Task<Family> NewFamilyAsync()
        {
            var client = TestUsers.Client(TestUsers.NewId());
            var child1 = TestUsers.NewId();
            var child2 = TestUsers.NewId();
            _users.AddStudent(client.UserId, child1);
            _users.AddStudent(client.UserId, child2);

            await using var db = _fixture.CreateContext();
            var plans = Plans(db);
            var plan1 = (await plans.CreateAsync(client, NewRequest(child1, frameworkId: 1, standardId: 77))).Data!;
            var plan2 = (await plans.CreateAsync(client, NewRequest(child2))).Data!;
            Assert.True((await plans.ChangeStatusAsync(client, plan1.Id, CustomPlanStatus.Active)).Valid);

            var lesson501 = plan1.Lessons.Single(l => l.LessonId == 501);
            await plans.UpdateLessonProgressAsync(client, plan1.Id, lesson501.Id, new CustomPlanLessonProgressRequestModel
            {
                ConceptCompleted = true, DefinitionsCompleted = true, ExamplesCompleted = true,
            });

            // Exam and pre-test taken (results in usqui). Practice 5011: two attempts (60%, then 80%), kept by the plan.
            // Practice 5012: in progress. Practice 5013: not started.
            await _fixture.ExecuteSqlAsync($@"
                INSERT INTO usqui_user_quiz_results (user_id, quiz_id, is_exam, lesson_id, try_no, total_correct, total_questions, total_percent, created_at) VALUES
                ({child1}, 900, 1, 501, 1, 17, 20, 85.5, '2026-11-13 20:00:00'),
                ({child1}, 5018, 2, 501, 1, 4, 10, 40, '2026-11-09 13:00:00');");
            var activities = Activities(db);
            var resultId = Convert.ToInt32(await _fixture.ScalarAsync($"SELECT id FROM usqui_user_quiz_results WHERE user_id = {child1} AND is_exam = 1"));
            Assert.True((await activities.LinkExamResultAsync(client, plan1.Id, lesson501.Exams[0].Id, resultId)).Valid);
            var preTestResultId = Convert.ToInt32(await _fixture.ScalarAsync($"SELECT id FROM usqui_user_quiz_results WHERE user_id = {child1} AND is_exam = 2"));
            Assert.True((await activities.LinkTestResultAsync(client, plan1.Id, lesson501.Tests.Single(t => t.TestType == PlanTestType.PreTest).Id, preTestResultId)).Valid);
            await activities.RecordPracticeResultAsync(client, plan1.Id, lesson501.Practices[0].Id, new CustomPlanPracticeResultRequestModel { Score = 60, TotalCorrect = 6, TotalQuestions = 10 });
            _fixture.Time.Advance(TimeSpan.FromMinutes(1));
            Assert.True((await activities.RecordPracticeResultAsync(client, plan1.Id, lesson501.Practices[0].Id, new CustomPlanPracticeResultRequestModel { Score = 80, TotalCorrect = 8, TotalQuestions = 10 })).Valid);
            Assert.True((await activities.UpdatePracticeStatusAsync(client, plan1.Id, lesson501.Practices[1].Id, PlanActivityStatus.InProgress)).Valid);

            var evidences = Evidences(db);
            var lessonEvidence = await evidences.CreateAsync(client, plan1.Id,
                new CustomPlanEvidenceRequestModel { Title = "Fractions worksheet", PlanLessonId = lesson501.Id }, File("worksheet.PDF", "pdf bytes"));
            Assert.True(lessonEvidence.Valid, string.Join(",", lessonEvidence.Messages));
            Assert.True((await evidences.CreateAsync(client, plan1.Id,
                new CustomPlanEvidenceRequestModel { Title = "Volcano project", EvidenceType = CustomPlanEvidenceType.Project, Url = "https://youtu.be/volcano" }, null)).Valid);

            return new Family(client, child1, child2, plan1, plan2);
        }

        private static UploadedFile File(string name, string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            return new UploadedFile(new MemoryStream(bytes), name, "application/octet-stream", bytes.Length);
        }

        private async Task<List<CustomPlanStudentOverviewModel>> GetAsync(RequestUser user, CustomPlanOverviewFilterModel filter)
        {
            await using var db = _fixture.CreateContext();
            var result = await Overview(db).GetOverviewAsync(user, filter);
            Assert.True(result.Valid, string.Join(",", result.Messages));
            return result.Data!;
        }

        [Fact]
        public async Task Overview_returns_each_child_with_plans_by_subject_and_full_lesson_details()
        {
            var family = await NewFamilyAsync();

            var students = await GetAsync(family.Client, new CustomPlanOverviewFilterModel());

            Assert.Equal(new[] { family.Child1, family.Child2 }.OrderBy(id => id), students.Select(s => s.StudentId));
            var child1 = students.Single(s => s.StudentId == family.Child1);
            Assert.Equal($"User {family.Child1}", child1.StudentName);
            var plan = Assert.Single(child1.Plans);
            Assert.Equal(CustomPlanStatus.Active, plan.Status);
            Assert.Equal("Third grade", plan.Level.Name);
            Assert.True(plan.UsesFramework);
            Assert.Equal("DEPR", plan.Framework!.Code);
            Assert.Equal("PR", plan.Framework.CountryCode);

            // Plan by subject: Math has both lessons, Science (in the plan, no lessons yet) is listed empty.
            Assert.Equal(new[] { "Math", "Science" }, plan.Subjects.Select(s => s.Subject.Name));
            var math = plan.Subjects[0];
            Assert.Equal(new[] { 501UL, 502UL }, math.Lessons.Select(l => l.LessonId));
            Assert.Empty(plan.Subjects[1].Lessons);

            var lesson = math.Lessons[0];
            Assert.Equal("Lesson 501", lesson.Title);
            Assert.Equal("MAT-EN", lesson.SubjectCode);
            Assert.Equal("3", lesson.LevelCode);
            Assert.Equal(77UL, lesson.Standard!.Id);
            Assert.Equal("STD-77", lesson.Standard.Code);
            Assert.Equal(85.5, lesson.Score);
            Assert.Equal(85.5, math.AverageScore);
            Assert.Equal(85.5, plan.AverageScore);

            Assert.True(lesson.Learning.IsCompleted);
            Assert.NotNull(lesson.Learning.CompletedAt);
            Assert.Equal(new DateOnly(2026, 11, 9), lesson.Learning.AssignedDate);
            Assert.Equal(new DateOnly(2026, 11, 13), lesson.Learning.DueDate);
            Assert.Equal(PuertoRico, lesson.Learning.Timezone);
            Assert.False(lesson.Learning.IsOverdue);

            // Practices: one item per practice, each with its own dates, status and score.
            Assert.Equal(3, lesson.Practices.Total);
            Assert.Equal(1, lesson.Practices.Completed);
            Assert.False(lesson.Practices.IsCompleted);
            Assert.Equal(80, lesson.Practices.AverageScore);
            var practice = lesson.Practices.Items[0];
            Assert.Equal(5011UL, practice.PracticeId);
            Assert.Equal("Quiz 5011", practice.Title);
            Assert.Equal((byte)1, practice.Sequence);
            Assert.Equal(new DateOnly(2026, 11, 9), practice.AssignedDate);
            Assert.Equal(new DateTime(2026, 11, 11, 0, 0, 0, DateTimeKind.Utc), practice.DueDateTime); // Tue 10, 20:00 PR
            Assert.Equal(PuertoRico, practice.Timezone);
            Assert.Equal(PlanActivityStatus.Completed, practice.Status);
            Assert.NotNull(practice.CompletedAt);
            Assert.Equal(80, practice.Score);
            Assert.Equal(80, practice.BestScore);
            Assert.Equal(8, practice.TotalCorrect);
            Assert.Equal(10, practice.TotalQuestions);
            Assert.Equal(2, practice.Attempts);
            Assert.Equal(_fixture.Time.GetUtcNow().UtcDateTime, practice.LastAttemptAt);
            Assert.Equal(PlanActivityStatus.InProgress, lesson.Practices.Items[1].Status);
            Assert.Equal(PlanActivityStatus.NotStarted, lesson.Practices.Items[2].Status);
            Assert.Null(lesson.Practices.Items[2].Score);
            Assert.Equal(0, lesson.Practices.Items[2].Attempts);

            // Pre-test (result from usqui) and post-test.
            Assert.Equal(new[] { PlanTestType.PreTest, PlanTestType.PostTest }, lesson.Tests.Select(t => t.TestType));
            Assert.Equal("Quiz 5018", lesson.Tests[0].Title);
            Assert.True(lesson.Tests[0].IsCompleted);
            Assert.Equal(40, lesson.Tests[0].Result!.Score);
            Assert.Equal((ushort)30, lesson.Tests[0].DurationMinutes);
            Assert.Equal(PlanActivityStatus.NotStarted, lesson.Tests[1].Status);

            var exam = Assert.Single(lesson.Exams);
            Assert.True(exam.IsCompleted);
            Assert.Equal(PlanActivityStatus.Completed, exam.Status);
            Assert.NotNull(exam.CompletedAt);
            Assert.Equal(new DateTime(2026, 11, 14, 3, 0, 0, DateTimeKind.Utc), exam.DueDateTime);
            Assert.Equal(PuertoRico, exam.Timezone);
            Assert.Equal((ushort)60, exam.DurationMinutes);
            Assert.Equal("Quiz 900", exam.Title);
            Assert.Equal(17, exam.Result!.TotalCorrect);
            Assert.Equal(20, exam.Result.TotalQuestions);
            Assert.Equal(85.5, exam.Result.Score);

            // Evidences: the file is on the lesson (fresh read link), the link is on the plan.
            var fileEvidence = Assert.Single(lesson.Evidences);
            Assert.True(fileEvidence.IsUploadedFile);
            Assert.EndsWith("?sas=read", fileEvidence.Url);
            Assert.Equal("application/pdf", fileEvidence.ContentType);
            var planEvidence = Assert.Single(plan.Evidences);
            Assert.Equal(CustomPlanEvidenceType.Project, planEvidence.EvidenceType);
            Assert.Equal("https://youtu.be/volcano", planEvidence.Url);

            var other = math.Lessons[1];
            Assert.Null(other.Score);
            Assert.Equal(77UL, other.Standard!.Id);
            Assert.False(other.Learning.IsCompleted);
            Assert.Equal(0, other.Practices.Completed);
            Assert.Equal(3, other.Practices.Total);
        }

        [Fact]
        public async Task Overview_filters_by_children_dates_scores_and_status()
        {
            var family = await NewFamilyAsync();

            var onlyChild2 = await GetAsync(family.Client, new CustomPlanOverviewFilterModel { StudentIds = { family.Child2 } });
            Assert.Equal(family.Child2, Assert.Single(onlyChild2).StudentId);

            // ?studentId=... (one student) filters the same way, and is combined with studentIds.
            var singleChild1 = await GetAsync(family.Client, new CustomPlanOverviewFilterModel { StudentId = family.Child1 });
            Assert.Equal(family.Child1, Assert.Single(singleChild1).StudentId);
            var both = await GetAsync(family.Client, new CustomPlanOverviewFilterModel { StudentId = family.Child1, StudentIds = { family.Child2 } });
            Assert.Equal(new[] { family.Child1, family.Child2 }.OrderBy(id => id), both.Select(s => s.StudentId).OrderBy(id => id));

            // Score >= 80: only lesson 501 of child 1 has a score. Subjects and plans with no match are left out.
            var scored = await GetAsync(family.Client, new CustomPlanOverviewFilterModel { MinScore = 80 });
            var scoredPlan = Assert.Single(scored.Single(s => s.StudentId == family.Child1).Plans);
            Assert.Equal(501UL, Assert.Single(Assert.Single(scoredPlan.Subjects).Lessons).LessonId);
            Assert.Empty(scored.Single(s => s.StudentId == family.Child2).Plans);
            Assert.All(await GetAsync(family.Client, new CustomPlanOverviewFilterModel { MaxScore = 50 }), s => Assert.Empty(s.Plans));

            // Dates: every area is between 9 and 13 Nov 2026 (the exam deadline is still the 13th in Puerto Rico).
            Assert.All(await GetAsync(family.Client, new CustomPlanOverviewFilterModel { DateFrom = new DateOnly(2026, 11, 14) }), s => Assert.Empty(s.Plans));
            Assert.All(await GetAsync(family.Client, new CustomPlanOverviewFilterModel { DateTo = new DateOnly(2026, 11, 8) }), s => Assert.Empty(s.Plans));
            var friday = await GetAsync(family.Client, new CustomPlanOverviewFilterModel { DateFrom = new DateOnly(2026, 11, 13), DateTo = new DateOnly(2026, 11, 13) });
            Assert.All(friday, s => Assert.Equal(2, Assert.Single(s.Plans).Subjects.Single().Lessons.Count));

            var drafts = await GetAsync(family.Client, new CustomPlanOverviewFilterModel { Statuses = { CustomPlanStatus.Draft } });
            Assert.Equal(family.Plan2.Id, Assert.Single(Assert.Single(drafts).Plans).Id);
        }

        [Fact]
        public async Task Overview_only_shows_the_clients_own_children()
        {
            var family = await NewFamilyAsync();
            var otherClient = TestUsers.Client(TestUsers.NewId());

            Assert.Empty(await GetAsync(otherClient, new CustomPlanOverviewFilterModel { StudentIds = { family.Child1 } }));
            Assert.Empty(await GetAsync(otherClient, new CustomPlanOverviewFilterModel()));

            // Staff name the students they want to see.
            var staff = TestUsers.User(TestUsers.NewId(), isStaff: true);
            var forStaff = await GetAsync(staff, new CustomPlanOverviewFilterModel { StudentIds = { family.Child1 } });
            Assert.Equal(family.Plan1.Id, Assert.Single(Assert.Single(forStaff).Plans).Id);
        }

        [Fact]
        public async Task Overview_validates_its_filters()
        {
            await using var db = _fixture.CreateContext();
            var client = TestUsers.Client(TestUsers.NewId());

            var result = await Overview(db).GetOverviewAsync(client, new CustomPlanOverviewFilterModel
            {
                DateFrom = new DateOnly(2026, 11, 20), DateTo = new DateOnly(2026, 11, 1), MinScore = 120,
            });
            Assert.Equal(ServiceErrorKind.Validation, result.ErrorKind);
            Assert.Contains(ServiceErrorCodes.PlanOverviewDatesInvalid, result.Messages);
            Assert.Contains(ServiceErrorCodes.PlanOverviewScoresInvalid, result.Messages);
            Assert.Contains(ServiceErrorCodes.PlanOverviewScoresInvalid, (await Overview(db).GetOverviewAsync(client, new CustomPlanOverviewFilterModel { MinScore = 90, MaxScore = 10 })).Messages);
            Assert.Contains(ServiceErrorCodes.PlanOverviewStudentsRequired,
                (await Overview(db).GetOverviewAsync(TestUsers.User(TestUsers.NewId(), isStaff: true), new CustomPlanOverviewFilterModel())).Messages);
        }

        [Fact]
        public async Task Overview_filters_plans_by_their_start_and_end_dates()
        {
            var client = TestUsers.Client(TestUsers.NewId());
            var child = TestUsers.NewId();
            _users.AddStudent(client.UserId, child);
            await using var db = _fixture.CreateContext();

            // Old plan: 5 Oct - 2 Nov 2026. Current plan: 9 Nov - 7 Dec 2026 (4 weeks each).
            var oldRequest = NewRequest(child);
            ShiftDays(oldRequest, -35);
            var old = await Plans(db).CreateAsync(client, oldRequest);
            Assert.True(old.Valid, string.Join(",", old.Messages));
            var current = await Plans(db).CreateAsync(client, NewRequest(child));
            Assert.True(current.Valid, string.Join(",", current.Messages));

            async Task<List<ulong>> PlanIds(DateOnly? from, DateOnly? to) =>
                (await GetAsync(client, new CustomPlanOverviewFilterModel { PlanDateFrom = from, PlanDateTo = to }))
                    .SelectMany(s => s.Plans).Select(p => p.Id).OrderBy(id => id).ToList();

            var all = await PlanIds(null, null);
            Assert.Equal(new[] { old.Data!.Id, current.Data!.Id }.OrderBy(id => id), all);
            Assert.Equal(new[] { current.Data.Id }, await PlanIds(new DateOnly(2026, 11, 3), null)); // the old plan ended on the 2nd
            Assert.Equal(new[] { old.Data.Id }, await PlanIds(null, new DateOnly(2026, 11, 8)));    // the current plan starts on the 9th
            Assert.Equal(all, await PlanIds(new DateOnly(2026, 11, 2), new DateOnly(2026, 11, 9))); // both ends are inclusive
            Assert.Empty(await PlanIds(new DateOnly(2027, 1, 1), null));

            // Whole plans: the lessons of a plan returned are not filtered by the plan window.
            var currentPlan = (await GetAsync(client, new CustomPlanOverviewFilterModel { PlanDateFrom = new DateOnly(2026, 12, 7) })).Single().Plans.Single();
            Assert.Equal(current.Data.Lessons.Count, currentPlan.Subjects.Sum(s => s.Lessons.Count));

            var invalid = await Overview(db).GetOverviewAsync(client, new CustomPlanOverviewFilterModel { PlanDateFrom = new DateOnly(2026, 11, 9), PlanDateTo = new DateOnly(2026, 11, 1) });
            Assert.Equal(ServiceErrorKind.Validation, invalid.ErrorKind);
            Assert.Contains(ServiceErrorCodes.PlanOverviewPlanDatesInvalid, invalid.Messages);
        }

        [Fact]
        public async Task Overview_by_month_returns_the_lessons_of_that_month_from_any_plan()
        {
            var client = TestUsers.Client(TestUsers.NewId());
            var child = TestUsers.NewId();
            var idle = TestUsers.NewId();
            _users.AddStudent(client.UserId, child);
            _users.AddStudent(client.UserId, idle);
            await using var db = _fixture.CreateContext();

            // Old plan: every area between 5 and 9 Oct 2026. Current plan: every area between 9 and 13 Nov 2026.
            var oldRequest = NewRequest(child);
            ShiftDays(oldRequest, -35);
            var old = (await Plans(db).CreateAsync(client, oldRequest)).Data!;
            var current = (await Plans(db).CreateAsync(client, NewRequest(child))).Data!;
            // A child whose only plan is in November: listed with no plans in October.
            await Plans(db).CreateAsync(client, NewRequest(idle));

            async Task<List<CustomPlanStudentOverviewModel>> Month(string month) =>
                await GetAsync(client, new CustomPlanOverviewFilterModel { Month = month });

            var october = await Month("2026-10");
            var octoberPlan = Assert.Single(october.Single(s => s.StudentId == child).Plans);
            Assert.Equal(old.Id, octoberPlan.Id); // the old plan, though it is past
            Assert.Equal(old.Lessons.Count, octoberPlan.Subjects.Sum(s => s.Lessons.Count));
            Assert.Empty(october.Single(s => s.StudentId == idle).Plans);

            var november = await Month("2026-11");
            Assert.Equal(current.Id, Assert.Single(november.Single(s => s.StudentId == child).Plans).Id);
            Assert.Single(november.Single(s => s.StudentId == idle).Plans);

            // Same as dateFrom/dateTo on the first and last day of the month.
            var sameWindow = await GetAsync(client, new CustomPlanOverviewFilterModel { DateFrom = new DateOnly(2026, 11, 1), DateTo = new DateOnly(2026, 11, 30) });
            Assert.Equal(november.SelectMany(s => s.Plans).Select(p => p.Id), sameWindow.SelectMany(s => s.Plans).Select(p => p.Id));

            Assert.All(await Month("2026-12"), s => Assert.Empty(s.Plans));
            Assert.Equal(2, (await Month("2026-12")).Count); // both children still listed

            // Combined with the plan filters and the student filter.
            Assert.Empty((await GetAsync(client, new CustomPlanOverviewFilterModel { Month = "2026-10", PlanDateFrom = new DateOnly(2026, 11, 3) }))
                .SelectMany(s => s.Plans));
            Assert.Equal(child, Assert.Single(await GetAsync(client, new CustomPlanOverviewFilterModel { Month = "2026-10", StudentId = child })).StudentId);

            foreach (var invalid in new[]
            {
                new CustomPlanOverviewFilterModel { Month = "2026-13" },
                new CustomPlanOverviewFilterModel { Month = "11-2026" },
                new CustomPlanOverviewFilterModel { Month = "2026-11", DateFrom = new DateOnly(2026, 11, 1) },
            })
            {
                var result = await Overview(db).GetOverviewAsync(client, invalid);
                Assert.Equal(ServiceErrorKind.Validation, result.ErrorKind);
                Assert.Contains(ServiceErrorCodes.PlanOverviewMonthInvalid, result.Messages);
            }
        }

        [Fact]
        public async Task Overdue_areas_are_flagged_in_their_time_zone()
        {
            var client = TestUsers.Client(TestUsers.NewId());
            var child = TestUsers.NewId();
            _users.AddStudent(client.UserId, child);
            await using var db = _fixture.CreateContext();

            // Same plan, five weeks earlier: everything was due by 9 Oct 2026, and "now" is November.
            var request = NewRequest(child);
            ShiftDays(request, -35);
            var created = await Plans(db).CreateAsync(client, request);
            Assert.True(created.Valid, string.Join(",", created.Messages));

            var lesson = (await GetAsync(client, new CustomPlanOverviewFilterModel())).Single().Plans.Single().Subjects[0].Lessons[0];
            Assert.True(lesson.Learning.IsOverdue);
            Assert.Equal(3, lesson.Practices.Overdue);
            Assert.All(lesson.Practices.Items, p => Assert.True(p.IsOverdue));
            Assert.All(lesson.Tests, t => Assert.True(t.IsOverdue));
            Assert.True(lesson.Exams.Single().IsOverdue);
        }

        [Fact]
        public async Task Overview_still_answers_when_Athenas_is_down()
        {
            var family = await NewFamilyAsync();
            _athenas.Available = false;

            var lesson = (await GetAsync(family.Client, new CustomPlanOverviewFilterModel { StudentIds = { family.Child1 } }))
                .Single().Plans.Single().Subjects[0].Lessons[0];

            Assert.Null(lesson.Title);
            Assert.Null(lesson.Standard!.Code);
            Assert.Equal(85.5, lesson.Score); // data from our own database is still there
        }

        [Fact]
        public async Task Uploaded_evidence_is_stored_in_blob_storage_with_a_safe_content_type()
        {
            var family = await NewFamilyAsync();

            var (blobName, blob) = Assert.Single(_storage.Blobs);
            Assert.StartsWith($"custom-plans/{family.Plan1.Id}/evidences/", blobName);
            Assert.EndsWith(".pdf", blobName);
            Assert.Equal("application/pdf", blob.ContentType); // from the extension, not the client's octet-stream
            Assert.Equal("pdf bytes", Encoding.UTF8.GetString(blob.Bytes));

            await using var check = _fixture.CreateContext();
            var stored = await check.CustomPlanEvidences.AsNoTracking().SingleAsync(e => e.BlobName == blobName);
            Assert.Equal($"https://storage.test/gsweb-api/{blobName}", stored.Url); // no SAS in the database
            Assert.Equal("worksheet.PDF", stored.FileName);
            Assert.Equal(9UL, stored.SizeBytes);
            Assert.Equal(family.Client.UserId, stored.UploadedByUserId);
        }

        [Fact]
        public async Task Evidence_requests_are_validated()
        {
            var family = await NewFamilyAsync();
            await using var db = _fixture.CreateContext();
            var service = Evidences(db);
            var planId = family.Plan1.Id;

            async Task<List<string>> Errors(CustomPlanEvidenceRequestModel request, UploadedFile? file = null) =>
                (await service.CreateAsync(family.Client, planId, request, file)).Messages;

            Assert.Contains(ServiceErrorCodes.EvidenceTitleRequired, await Errors(new() { Title = " ", Url = "https://x.test" }));
            Assert.Contains(ServiceErrorCodes.EvidenceSourceRequired, await Errors(new() { Title = "T" }));
            Assert.Contains(ServiceErrorCodes.EvidenceUrlInvalid, await Errors(new() { Title = "T", Url = "javascript:alert(1)" }));
            Assert.Contains(ServiceErrorCodes.EvidenceUrlInvalid, await Errors(new() { Title = "T", Url = "not a url" }));
            Assert.Contains(ServiceErrorCodes.EvidenceFileTypeNotAllowed, await Errors(new() { Title = "T" }, File("virus.exe", "MZ")));
            Assert.Contains(ServiceErrorCodes.EvidenceFileTooLarge,
                await Errors(new() { Title = "T" }, new UploadedFile(new MemoryStream(), "big.pdf", null, CustomPlanEvidenceService.MaxFileBytes + 1)));
            Assert.Contains(ServiceErrorCodes.EvidenceLessonNotInPlan,
                await Errors(new() { Title = "T", Url = "https://x.test", PlanLessonId = family.Plan2.Lessons[0].Id }));

            _storage.Available = false;
            Assert.Equal(ServiceErrorKind.Unavailable, (await service.CreateAsync(family.Client, planId, new() { Title = "T" }, File("a.png", "png"))).ErrorKind);
            _storage.Available = true;

            var stranger = TestUsers.Client(TestUsers.NewId());
            Assert.Equal(ServiceErrorKind.NotFound, (await service.CreateAsync(stranger, planId, new() { Title = "T", Url = "https://x.test" }, null)).ErrorKind);
            Assert.Equal(ServiceErrorKind.NotFound, (await service.ListAsync(stranger, planId)).ErrorKind);

            Assert.True((await Plans(db).ChangeStatusAsync(family.Client, planId, CustomPlanStatus.Cancelled)).Valid);
            Assert.Contains(ServiceErrorCodes.EvidencePlanClosed, await Errors(new() { Title = "T", Url = "https://x.test" }));
        }

        [Fact]
        public async Task Deleted_evidence_is_hidden_but_kept()
        {
            var family = await NewFamilyAsync();
            await using var db = _fixture.CreateContext();
            var service = Evidences(db);
            var listed = (await service.ListAsync(family.Client, family.Plan1.Id)).Data!;
            Assert.Equal(2, listed.Count);

            Assert.Equal(ServiceErrorKind.NotFound, (await service.DeleteAsync(TestUsers.Client(TestUsers.NewId()), family.Plan1.Id, listed[0].Id)).ErrorKind);
            Assert.True((await service.DeleteAsync(family.Client, family.Plan1.Id, listed[0].Id)).Valid);

            Assert.Single((await service.ListAsync(family.Client, family.Plan1.Id)).Data!);
            await using var check = _fixture.CreateContext();
            Assert.NotNull((await check.CustomPlanEvidences.IgnoreQueryFilters().SingleAsync(e => e.Id == listed[0].Id)).DeletedAt);
        }

        private static void ShiftDays(CreateCustomPlanRequestModel request, int days)
        {
            request.StartDate = request.StartDate.AddDays(days);
            foreach (var lesson in request.Structure.Lessons)
            {
                lesson.LearningAssignedDate = lesson.LearningAssignedDate!.Value.AddDays(days);
                lesson.LearningDueDate = lesson.LearningDueDate!.Value.AddDays(days);
                foreach (var practice in lesson.Practices)
                {
                    practice.AssignedDate = practice.AssignedDate.AddDays(days);
                    practice.DueDateTime = practice.DueDateTime!.Value.AddDays(days);
                }

                foreach (var test in lesson.Tests)
                {
                    test.AssignedDate = test.AssignedDate.AddDays(days);
                    test.DueDateTime = test.DueDateTime!.Value.AddDays(days);
                }

                lesson.AssignedDates = lesson.AssignedDates.Select(d => d.AddDays(days)).ToList();
                foreach (var exam in lesson.Exams)
                {
                    exam.AssignedDate = exam.AssignedDate.AddDays(days);
                    exam.DueDateTime = exam.DueDateTime!.Value.AddDays(days);
                }
            }
        }

        private sealed record Family(RequestUser Client, ulong Child1, ulong Child2, CustomPlanResponseModel Plan1, CustomPlanResponseModel Plan2);
    }
}
