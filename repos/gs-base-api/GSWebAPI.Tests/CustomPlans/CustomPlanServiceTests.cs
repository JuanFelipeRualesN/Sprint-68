using GSWebAPI.GSWebAPI.Domain.Models.CustomPlans;
using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.CustomPlans;
using GSWebAPI.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GSWebAPI.Tests.CustomPlans
{
    [Collection(MySqlCollection.Name)]
    public sealed class CustomPlanServiceTests
    {
        private readonly MySqlDatabaseFixture _fixture;
        private readonly FakeUserDirectory _users = new();
        private readonly FakeAthenasCatalog _athenas = new();

        public CustomPlanServiceTests(MySqlDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        private CustomPlanService CreateService(GSWebDbContext db) =>
            new(db, _users, new StandardFrameworkValidator(), new GSWebModuleSettings(), _fixture.Time, _athenas);

        private CustomPlanActivityService Activities(GSWebDbContext db) => new(db, _fixture.Time);

        private (RequestUser Client, ulong StudentId) NewClientWithStudent()
        {
            var clientId = TestUsers.NewId();
            var studentId = TestUsers.NewId();
            _users.AddStudent(clientId, studentId);
            return (TestUsers.Client(clientId), studentId);
        }

        internal static CreateCustomPlanRequestModel NewRequest(ulong studentId, ushort? frameworkId = null, ulong? standardId = null) => new()
        {
            CustomPlanTypeId = 1,
            StudentId = studentId,
            LevelId = 3,
            CurriculumFrameworkId = frameworkId,
            PlanDurationWeeks = 4,
            StartDate = new DateOnly(2026, 11, 9),
            Structure = new CustomPlanStructureRequestModel
            {
                SubjectIds = new List<ulong> { 10, 11 },
                Days = new List<CustomPlanDayRequestModel>
                {
                    new()
                    {
                        DayOfWeek = 1,
                        Times = new List<CustomPlanDayTimeRequestModel>
                        {
                            new() { StartTime = new TimeOnly(9, 0), DurationMinutes = 45, Timezone = "America/Puerto_Rico" },
                        },
                    },
                    new() { DayOfWeek = 3 },
                },
                Lessons = new List<CustomPlanLessonRequestModel>
                {
                    Lesson(501, 1, standardId, new DateOnly(2026, 11, 9), new DateOnly(2026, 11, 11)),
                    Lesson(502, 2, standardId),
                },
            },
        };

        internal const string PuertoRico = "America/Puerto_Rico";

        /// <summary>
        /// Subject 10, everything in Puerto Rico time. Learning Mon 9 - Fri 13 Nov 2026; the three practices the lesson has
        /// in Athenas (quiz ids lessonId*10 + 1..3, see FakeAthenasCatalog); a pre-test on Mon 9 and a post-test on
        /// Fri 13. Lesson 501 also gets its exam (900).
        /// </summary>
        internal static CustomPlanLessonRequestModel Lesson(ulong lessonId, ushort sequence, ulong? standardId = null, params DateOnly[] sessionDates) => new()
        {
            LessonId = lessonId,
            StandardId = standardId,
            SubjectId = 10,
            Sequence = sequence,
            LearningAssignedDate = new DateOnly(2026, 11, 9),
            LearningDueDate = new DateOnly(2026, 11, 13),
            LearningTimezone = PuertoRico,
            AssignedDates = sessionDates.ToList(),
            Exams = lessonId == 501 ? new List<CustomPlanExamRequestModel> { Exam() } : new List<CustomPlanExamRequestModel>(),
            Practices = Enumerable.Range(1, 3).Select(n => Practice(lessonId, (byte)n)).ToList(),
            Tests = new List<CustomPlanTestRequestModel>
            {
                Test(lessonId, PlanTestType.PreTest, new DateOnly(2026, 11, 9), 12),
                Test(lessonId, PlanTestType.PostTest, new DateOnly(2026, 11, 13), 22),
            },
        };

        /// <summary>Practice n of a lesson: opens Mon 9 Nov + (n - 1) days, due the next day at 20:00 Puerto Rico.</summary>
        internal static CustomPlanPracticeRequestModel Practice(ulong lessonId, byte n) => new()
        {
            PracticeId = lessonId * 10 + n,
            Sequence = n,
            AssignedDate = new DateOnly(2026, 11, 8 + n),
            DueDateTime = PuertoRicoTime(new DateOnly(2026, 11, 9 + n), 20),
            Timezone = PuertoRico,
        };

        /// <summary>Pre-test (quiz lessonId*10 + 8) or post-test (lessonId*10 + 9), due that day at the given hour.</summary>
        internal static CustomPlanTestRequestModel Test(ulong lessonId, PlanTestType type, DateOnly day, int dueHour) => new()
        {
            TestType = type,
            TestId = lessonId * 10 + (type == PlanTestType.PreTest ? 8UL : 9UL),
            AssignedDate = day,
            DueDateTime = PuertoRicoTime(day, dueHour),
            Timezone = PuertoRico,
            DurationMinutes = 30,
        };

        internal static DateTime PuertoRicoTime(DateOnly day, int hour) =>
            new DateTimeOffset(day.ToDateTime(new TimeOnly(hour, 0)), TimeSpan.FromHours(-4)).UtcDateTime;

        /// <summary>Opens Fri 13 Nov; due that day at 23:00 in Puerto Rico (UTC-4), which is 03:00 UTC on the 14th.</summary>
        internal static CustomPlanExamRequestModel Exam() => new()
        {
            ExamId = 900,
            AssignedDate = new DateOnly(2026, 11, 13),
            DueDateTime = new DateTimeOffset(2026, 11, 13, 23, 0, 0, TimeSpan.FromHours(-4)).UtcDateTime,
            Timezone = PuertoRico,
            DurationMinutes = 60,
        };

        private static CustomPlanDayRequestModel Day(byte dayOfWeek, params (int Hour, int Minute, ushort Duration)[] times) => new()
        {
            DayOfWeek = dayOfWeek,
            Times = times.Select(t => new CustomPlanDayTimeRequestModel { StartTime = new TimeOnly(t.Hour, t.Minute), DurationMinutes = t.Duration, Timezone = PuertoRico }).ToList(),
        };

        [Fact]
        public async Task The_schedule_can_change_while_the_plan_runs_without_touching_the_lessons()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);
            var plan = (await service.CreateAsync(client, NewRequest(studentId))).Data!;
            Assert.True((await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active)).Valid);
            var mondayId = plan.Days.Single(d => d.DayOfWeek == 1).Id;

            // Monday: now 2 slots (60 + 30 minutes). Wednesday dropped. Tuesday and Thursday added.
            var updated = await service.UpdateScheduleAsync(client, plan.Id, new CustomPlanScheduleRequestModel
            {
                Days = { Day(1, (9, 0, 60), (16, 30, 30)), Day(2, (15, 0, 90)), Day(4, (15, 0, 45)) },
            });

            Assert.True(updated.Valid, string.Join(",", updated.Messages));
            Assert.Equal(new byte[] { 1, 2, 4 }, updated.Data!.Days.Select(d => d.DayOfWeek));
            var monday = updated.Data.Days[0];
            Assert.Equal(mondayId, monday.Id); // same day keeps its row
            Assert.Equal(new ushort[] { 60, 30 }, monday.Times.Select(t => t.DurationMinutes));
            Assert.Equal(90, updated.Data.Days[1].Times.Single().DurationMinutes);
            Assert.Equal(plan.Lessons.Select(l => l.Id), updated.Data.Lessons.Select(l => l.Id));
            Assert.Equal(CustomPlanStatus.Active, updated.Data.Status);

            Assert.Empty((await service.UpdateScheduleAsync(client, plan.Id, new CustomPlanScheduleRequestModel())).Data!.Days); // cleared

            async Task<IReadOnlyCollection<string>> Errors(params CustomPlanDayRequestModel[] days) =>
                (await service.UpdateScheduleAsync(client, plan.Id, new CustomPlanScheduleRequestModel { Days = days.ToList() })).Messages;

            Assert.Contains(ServiceErrorCodes.PlanDayInvalid, await Errors(Day(8)));
            Assert.Contains(ServiceErrorCodes.PlanDayDuplicated, await Errors(Day(1), Day(1)));
            Assert.Contains(ServiceErrorCodes.PlanTimeDuplicated, await Errors(Day(1, (9, 0, 30), (9, 0, 60))));
            Assert.Contains(ServiceErrorCodes.PlanTimeDurationInvalid, await Errors(Day(1, (9, 0, 0))));
            var badZone = Day(1, (9, 0, 30));
            badZone.Times[0].Timezone = "Mars/Olympus";
            Assert.Contains(ServiceErrorCodes.PlanTimezoneInvalid, await Errors(badZone));

            Assert.Equal(ServiceErrorKind.NotFound, (await service.UpdateScheduleAsync(TestUsers.Client(TestUsers.NewId()), plan.Id, new CustomPlanScheduleRequestModel())).ErrorKind);
            Assert.True((await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Completed)).Valid);
            var closed = await service.UpdateScheduleAsync(client, plan.Id, new CustomPlanScheduleRequestModel { Days = { Day(5) } });
            Assert.Equal(ServiceErrorKind.Conflict, closed.ErrorKind);
            Assert.Contains(ServiceErrorCodes.PlanClosed, closed.Messages);
        }

        [Fact]
        public async Task Subjects_can_be_added_and_removed_while_the_plan_runs()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);
            var plan = (await service.CreateAsync(client, NewRequest(studentId))).Data!; // subjects 10 (both lessons) and 11
            Assert.True((await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active)).Valid);

            Assert.Equal(new ulong[] { 10, 11, 12 }, (await service.AddSubjectAsync(client, plan.Id, 12)).Data!.SubjectIds);
            Assert.Contains(ServiceErrorCodes.PlanSubjectDuplicated, (await service.AddSubjectAsync(client, plan.Id, 12)).Messages);
            Assert.Contains(ServiceErrorCodes.PlanSubjectInvalid, (await service.AddSubjectAsync(client, plan.Id, 0)).Messages);

            // No lessons: removed right away.
            Assert.Equal(new ulong[] { 10, 12 }, (await service.RemoveSubjectAsync(client, plan.Id, 11, removeLessons: false)).Data!.SubjectIds);
            Assert.Contains(ServiceErrorCodes.PlanSubjectNotFound, (await service.RemoveSubjectAsync(client, plan.Id, 99, false)).Messages);

            // Subject 10 has the two lessons: only with removeLessons, and not once one has progress.
            var hasLessons = await service.RemoveSubjectAsync(client, plan.Id, 10, false);
            Assert.Equal(ServiceErrorKind.Conflict, hasLessons.ErrorKind);
            Assert.Contains(ServiceErrorCodes.PlanSubjectHasLessons, hasLessons.Messages);
            var lesson501 = plan.Lessons.Single(l => l.LessonId == 501);
            Assert.True((await service.UpdateLessonProgressAsync(client, plan.Id, lesson501.Id, new CustomPlanLessonProgressRequestModel { ConceptCompleted = true })).Valid);
            Assert.Contains(ServiceErrorCodes.PlanSubjectHasProgress, (await service.RemoveSubjectAsync(client, plan.Id, 10, true)).Messages);
            Assert.Equal(2, (await service.GetAsync(client, plan.Id)).Data!.Lessons.Count);

            Assert.True((await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Cancelled)).Valid);
            Assert.Contains(ServiceErrorCodes.PlanClosed, (await service.AddSubjectAsync(client, plan.Id, 13)).Messages);
            Assert.Contains(ServiceErrorCodes.PlanClosed, (await service.RemoveSubjectAsync(client, plan.Id, 12, false)).Messages);
            Assert.Equal(ServiceErrorKind.NotFound, (await service.AddSubjectAsync(TestUsers.Client(TestUsers.NewId()), plan.Id, 13)).ErrorKind);
        }

        [Fact]
        public async Task A_selected_lesson_can_be_added_with_its_schedule_while_the_plan_runs()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);
            var plan = (await service.CreateAsync(client, NewRequest(studentId))).Data!; // lessons 501 (seq 1) and 502 (seq 2)
            Assert.True((await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active)).Valid);

            var request = Lesson(503, 3, null, new DateOnly(2026, 11, 10), new DateOnly(2026, 11, 12));
            request.SubjectId = 11;
            var added = await service.AddLessonAsync(client, plan.Id, request);

            Assert.True(added.Valid, string.Join(",", added.Messages));
            var lesson = added.Data!;
            Assert.NotEqual(0UL, lesson.Id);
            Assert.Equal(503UL, lesson.LessonId);
            Assert.Equal(11UL, lesson.SubjectId);
            Assert.Equal(PlanLessonStatus.NotStarted, lesson.LessonStatus);
            Assert.Equal(new DateOnly(2026, 11, 9), lesson.LearningAssignedDate);
            Assert.Equal(new DateOnly(2026, 11, 13), lesson.LearningDueDate);
            Assert.Equal(2, lesson.AssignedDates.Count);
            Assert.Equal(3, lesson.Practices.Count);
            Assert.Equal(2, lesson.Tests.Count);
            Assert.Equal(3, (await service.GetAsync(client, plan.Id)).Data!.Lessons.Count);

            // Same rules as the structure: not twice, sequence free, subject of the plan, dates inside, quizzes of the lesson.
            Assert.Contains(ServiceErrorCodes.PlanLessonDuplicated, (await service.AddLessonAsync(client, plan.Id, Lesson(503, 4))).Messages);
            Assert.Contains(ServiceErrorCodes.PlanLessonSequenceDuplicated, (await service.AddLessonAsync(client, plan.Id, Lesson(504, 1))).Messages);
            var otherSubject = Lesson(504, 4);
            otherSubject.SubjectId = 99;
            Assert.Contains(ServiceErrorCodes.PlanLessonSubjectNotInPlan, (await service.AddLessonAsync(client, plan.Id, otherSubject)).Messages);
            var outside = Lesson(504, 4);
            outside.LearningDueDate = new DateOnly(2027, 1, 15);
            Assert.Contains(ServiceErrorCodes.PlanLearningOutsidePlan, (await service.AddLessonAsync(client, plan.Id, outside)).Messages);
            var withoutWindow = Lesson(504, 4);
            withoutWindow.LearningAssignedDate = null;
            Assert.Contains(ServiceErrorCodes.PlanLearningDatesRequired, (await service.AddLessonAsync(client, plan.Id, withoutWindow)).Messages);
            var wrongQuiz = Lesson(504, 4);
            wrongQuiz.Practices[0].PracticeId = 5011; // a practice of lesson 501
            Assert.Contains(ServiceErrorCodes.PlanPracticeNotInLesson, (await service.AddLessonAsync(client, plan.Id, wrongQuiz)).Messages);
            Assert.Contains(ServiceErrorCodes.PlanStandardWithoutFramework, (await service.AddLessonAsync(client, plan.Id, Lesson(504, 4, standardId: 16))).Messages);

            _athenas.Available = false;
            Assert.Equal(ServiceErrorKind.Unavailable, (await service.AddLessonAsync(client, plan.Id, Lesson(504, 4))).ErrorKind);
            _athenas.Available = true;

            Assert.Equal(ServiceErrorKind.NotFound, (await service.AddLessonAsync(TestUsers.Client(TestUsers.NewId()), plan.Id, Lesson(504, 4))).ErrorKind);
            Assert.True((await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Completed)).Valid);
            Assert.Contains(ServiceErrorCodes.PlanClosed, (await service.AddLessonAsync(client, plan.Id, Lesson(504, 4))).Messages);
        }

        [Theory]
        [InlineData("es", "Educación en el hogar", "Refuerzos")]
        [InlineData("es-PR", "Educación en el hogar", "Refuerzos")]
        [InlineData("es-PR,es;q=0.9,en;q=0.8", "Educación en el hogar", "Refuerzos")]
        [InlineData("en", "Homeschool", "Reinforcements")]
        [InlineData(null, "Homeschool", "Reinforcements")]
        [InlineData("fr", "Homeschool", "Reinforcements")] // no French translation: English
        public async Task Plan_types_follow_the_session_language(string? langCode, string homeschool, string reinforcements)
        {
            await using var db = _fixture.CreateContext();

            var types = (await CreateService(db).ListTypesAsync(langCode)).Data!;

            Assert.Equal(new[] { "homeschool", "reinforcements", "specific_task", "customized" }, types.Select(t => t.Code));
            Assert.Equal(homeschool, types.Single(t => t.Code == "homeschool").Name);
            Assert.Equal(reinforcements, types.Single(t => t.Code == "reinforcements").Name);
            Assert.Equal((byte)1, types.Single(t => t.Code == "homeschool").Id);
        }

        [Fact]
        public async Task Study_time_adds_up_per_lesson_and_per_plan()
        {
            var (client, studentId) = NewClientWithStudent();
            var student = TestUsers.User(studentId);
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);
            var plan = (await service.CreateAsync(client, NewRequest(studentId))).Data!;
            var lesson501 = plan.Lessons.Single(l => l.LessonId == 501).Id;
            var lesson502 = plan.Lessons.Single(l => l.LessonId == 502).Id;
            Assert.Equal(0u, plan.TotalStudyMinutes);

            // Only while the plan is active.
            Assert.Contains(ServiceErrorCodes.PlanNotActive, (await service.AddStudyTimeAsync(client, plan.Id, lesson501, 30)).Messages);
            Assert.True((await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active)).Valid);

            // The client and the student both report; the plan total is the sum of its lessons.
            Assert.True((await service.AddStudyTimeAsync(client, plan.Id, lesson501, 30)).Valid);
            var byStudent = (await service.AddStudyTimeAsync(student, plan.Id, lesson501, 45)).Data!;
            Assert.Equal((75u, 75u), (byStudent.LessonTotalStudyMinutes, byStudent.PlanTotalStudyMinutes));
            var other = (await service.AddStudyTimeAsync(student, plan.Id, lesson502, 20)).Data!;
            Assert.Equal((20u, 95u, (ushort)20), (other.LessonTotalStudyMinutes, other.PlanTotalStudyMinutes, other.AddedMinutes));

            var read = (await service.GetAsync(client, plan.Id)).Data!;
            Assert.Equal(95u, read.TotalStudyMinutes);
            Assert.Equal(75u, read.Lessons.Single(l => l.Id == lesson501).TotalStudyMinutes);
            Assert.Equal(20u, read.Lessons.Single(l => l.Id == lesson502).TotalStudyMinutes);

            // Validation and access.
            Assert.Contains(ServiceErrorCodes.PlanStudyMinutesInvalid, (await service.AddStudyTimeAsync(client, plan.Id, lesson501, 0)).Messages);
            Assert.Contains(ServiceErrorCodes.PlanStudyMinutesInvalid, (await service.AddStudyTimeAsync(client, plan.Id, lesson501, CustomPlanService.MaxStudyMinutesPerReport + 1)).Messages);
            Assert.Equal(ServiceErrorKind.NotFound, (await service.AddStudyTimeAsync(TestUsers.User(TestUsers.NewId()), plan.Id, lesson501, 10)).ErrorKind);
            Assert.Contains(ServiceErrorCodes.PlanLessonNotFound, (await service.AddStudyTimeAsync(client, plan.Id, ulong.MaxValue, 10)).Messages);
            Assert.True((await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Paused)).Valid);
            Assert.Contains(ServiceErrorCodes.PlanNotActive, (await service.AddStudyTimeAsync(student, plan.Id, lesson501, 10)).Messages);
            Assert.Equal(95u, (await service.GetAsync(client, plan.Id)).Data!.TotalStudyMinutes);
        }

        [Fact]
        public async Task Simultaneous_study_time_reports_are_never_lost()
        {
            var (client, studentId) = NewClientWithStudent();
            ulong planId, lessonId;
            await using (var db = _fixture.CreateContext())
            {
                var service = CreateService(db);
                var plan = (await service.CreateAsync(client, NewRequest(studentId))).Data!;
                Assert.True((await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active)).Valid);
                (planId, lessonId) = (plan.Id, plan.Lessons[0].Id);
            }

            // 20 reports of 5 minutes at the same time, each with its own context (like separate requests).
            var reports = Enumerable.Range(0, 20).Select(async _ =>
            {
                await using var db = _fixture.CreateContext();
                return await CreateService(db).AddStudyTimeAsync(client, planId, lessonId, 5);
            });
            Assert.All(await Task.WhenAll(reports), r => Assert.True(r.Valid, string.Join(",", r.Messages)));

            await using var check = _fixture.CreateContext();
            Assert.Equal(100u, (await check.CustomPlanClients.SingleAsync(p => p.Id == planId)).TotalStudyMinutes);
            Assert.Equal(100u, (await check.CustomPlanClientLessons.SingleAsync(l => l.Id == lessonId)).TotalStudyMinutes);
        }

        [Fact]
        public async Task A_lesson_with_study_time_counts_as_progress_when_removing_its_subject()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);
            var plan = (await service.CreateAsync(client, NewRequest(studentId))).Data!; // both lessons in subject 10
            Assert.True((await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active)).Valid);
            Assert.True((await service.AddStudyTimeAsync(client, plan.Id, plan.Lessons[0].Id, 15)).Valid);

            Assert.Contains(ServiceErrorCodes.PlanSubjectHasProgress, (await service.RemoveSubjectAsync(client, plan.Id, 10, removeLessons: true)).Messages);
        }

        [Fact]
        public async Task Removing_a_subject_with_untouched_lessons_removes_them_and_their_activities()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);
            var request = NewRequest(studentId);
            request.Structure.Lessons[1].SubjectId = 11; // lesson 502 in subject 11, lesson 501 stays in 10
            var plan = (await service.CreateAsync(client, request)).Data!;
            Assert.True((await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active)).Valid);
            var lesson502 = plan.Lessons.Single(l => l.LessonId == 502).Id;

            var removed = await service.RemoveSubjectAsync(client, plan.Id, 11, removeLessons: true);

            Assert.True(removed.Valid, string.Join(",", removed.Messages));
            Assert.Equal(new ulong[] { 10 }, removed.Data!.SubjectIds);
            Assert.Equal(501UL, Assert.Single(removed.Data.Lessons).LessonId);
            await using var check = _fixture.CreateContext();
            Assert.False(await check.CustomPlanClientLessons.AnyAsync(l => l.Id == lesson502));
            Assert.False(await check.Set<CustomPlanClientLessonPractice>().AnyAsync(p => p.CustomPlanClientLessonId == lesson502));
            Assert.False(await check.Set<CustomPlanClientLessonTest>().AnyAsync(t => t.CustomPlanClientLessonId == lesson502));
        }

        [Fact]
        public async Task Create_saves_plan_with_structure_and_defaults_end_date()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();

            var result = await CreateService(db).CreateAsync(client, NewRequest(studentId));

            Assert.True(result.Valid, string.Join(",", result.Messages));
            var plan = result.Data!;
            Assert.Equal(CustomPlanStatus.Draft, plan.Status);
            Assert.Equal(new DateOnly(2026, 12, 7), plan.EndDate); // start + 4 weeks
            Assert.Equal(client.UserId, plan.ClientId);
            Assert.Equal($"User {studentId}", plan.StudentName);
            Assert.Equal(new List<ulong> { 10, 11 }, plan.SubjectIds);
            Assert.Equal(2, plan.Days.Count);
            Assert.Single(plan.Days[0].Times);
            Assert.Equal(2, plan.Lessons.Count);
            Assert.Equal(2, plan.Lessons[0].AssignedDates.Count);
            Assert.Single(plan.Lessons[0].Exams);
            Assert.Equal(new DateOnly(2026, 11, 9), plan.Lessons[0].LearningAssignedDate);
            Assert.Equal(new DateOnly(2026, 11, 13), plan.Lessons[0].LearningDueDate);
            Assert.Equal(PuertoRico, plan.Lessons[0].LearningTimezone);
            var exam = plan.Lessons[0].Exams[0];
            Assert.Equal(new DateTime(2026, 11, 14, 3, 0, 0, DateTimeKind.Utc), exam.DueDateTime);
            Assert.Equal(DateTimeKind.Utc, exam.DueDateTime.Kind);
            Assert.Equal(PuertoRico, exam.Timezone);
            Assert.Equal((ushort)60, exam.DurationMinutes);
            Assert.Equal(PlanActivityStatus.NotStarted, exam.Status);
            Assert.Equal(new[] { PlanTestType.PreTest, PlanTestType.PostTest }, plan.Lessons[0].Tests.Select(t => t.TestType));
            Assert.Equal(5018UL, plan.Lessons[0].Tests[0].TestId);
            Assert.Equal(new DateTime(2026, 11, 9, 16, 0, 0, DateTimeKind.Utc), plan.Lessons[0].Tests[0].DueDateTime);
            var practice = plan.Lessons[0].Practices[0];
            Assert.Equal(new DateTime(2026, 11, 11, 0, 0, 0, DateTimeKind.Utc), practice.DueDateTime); // Tue 10 at 20:00 PR
            Assert.Equal(PlanActivityStatus.NotStarted, practice.Status);
            Assert.Null(practice.Score);
            Assert.Equal(DateTimeKind.Utc, plan.CreatedAt.Kind);
            Assert.NotEqual(default, plan.CreatedAt);
        }

        [Fact]
        public async Task Create_rejects_client_as_student()
        {
            var (client, _) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();

            var result = await CreateService(db).CreateAsync(client, NewRequest(client.UserId));

            Assert.False(result.Valid);
            Assert.Contains(ServiceErrorCodes.PlanClientIsStudent, result.Messages);
        }

        [Fact]
        public async Task Create_rejects_student_that_does_not_belong_to_the_client()
        {
            var (client, _) = NewClientWithStudent();
            var (_, otherClientsStudent) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();

            var result = await CreateService(db).CreateAsync(client, NewRequest(otherClientsStudent));

            Assert.False(result.Valid);
            Assert.Contains(ServiceErrorCodes.PlanStudentNotOfClient, result.Messages);
        }

        [Fact]
        public async Task Create_fails_closed_when_users_api_is_unavailable()
        {
            var (client, studentId) = NewClientWithStudent();
            _users.Available = false;
            await using var db = _fixture.CreateContext();

            var result = await CreateService(db).CreateAsync(client, NewRequest(studentId));

            Assert.Equal(ServiceErrorKind.Unavailable, result.ErrorKind);
        }

        [Fact]
        public async Task Create_rejects_non_client_roles()
        {
            var (client, studentId) = NewClientWithStudent();
            var teacher = new RequestUser { UserId = client.UserId, RoleId = "5", Token = "test" };
            await using var db = _fixture.CreateContext();

            var result = await CreateService(db).CreateAsync(teacher, NewRequest(studentId));

            Assert.Equal(ServiceErrorKind.Forbidden, result.ErrorKind);
        }

        [Fact]
        public async Task Standards_require_a_framework()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();

            var result = await CreateService(db).CreateAsync(client, NewRequest(studentId, frameworkId: null, standardId: 77));

            Assert.False(result.Valid);
            Assert.Contains(ServiceErrorCodes.PlanStandardWithoutFramework, result.Messages);
        }

        [Fact]
        public async Task Standards_are_accepted_with_the_DEPR_framework()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();

            var result = await CreateService(db).CreateAsync(client, NewRequest(studentId, frameworkId: 1, standardId: 77));

            Assert.True(result.Valid, string.Join(",", result.Messages));
            Assert.Equal("DEPR", result.Data!.CurriculumFrameworkCode);
            Assert.All(result.Data.Lessons, l => Assert.Equal(77UL, l.StandardId));
        }

        [Fact]
        public async Task Standards_are_rejected_for_a_framework_they_do_not_belong_to()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var otherFramework = new CurriculumFramework { Code = "TX-" + TestUsers.NewId(), Name = "Texas", CountryCode = "US", Region = "TX" };
            db.CurriculumFrameworks.Add(otherFramework);
            await db.SaveChangesAsync();

            var result = await CreateService(db).CreateAsync(client, NewRequest(studentId, frameworkId: otherFramework.Id, standardId: 77));

            Assert.False(result.Valid);
            Assert.Contains(ServiceErrorCodes.PlanStandardNotInFramework, result.Messages);
            Assert.True(otherFramework.IsActive); // DB default 1 when not set explicitly
        }

        [Fact]
        public async Task Structure_can_only_change_while_draft()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);
            var plan = (await service.CreateAsync(client, NewRequest(studentId))).Data!;

            // Draft: editable. Keep lesson 501 (same row id), drop 502, add 503.
            var update = new UpdateCustomPlanDraftRequestModel
            {
                CustomPlanTypeId = 2,
                LevelId = 3,
                PlanDurationWeeks = 2,
                StartDate = plan.StartDate,
                Structure = new CustomPlanStructureRequestModel
                {
                    SubjectIds = new List<ulong> { 10 },
                    Days = new List<CustomPlanDayRequestModel> { new() { DayOfWeek = 1 } },
                    Lessons = new List<CustomPlanLessonRequestModel>
                    {
                        Lesson(501, 2),
                        Lesson(503, 1),
                    },
                },
            };
            var updated = await service.UpdateDraftAsync(client, plan.Id, update);
            Assert.True(updated.Valid, string.Join(",", updated.Messages));
            Assert.Equal(new DateOnly(2026, 11, 23), updated.Data!.EndDate);
            Assert.Equal(new[] { 503UL, 501UL }, updated.Data.Lessons.Select(l => l.LessonId));
            Assert.Equal(plan.Lessons.Single(l => l.LessonId == 501).Id, updated.Data.Lessons.Single(l => l.LessonId == 501).Id);

            // Active: structure is locked.
            Assert.True((await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active)).Valid);
            var locked = await service.UpdateDraftAsync(client, plan.Id, update);
            Assert.Equal(ServiceErrorKind.Conflict, locked.ErrorKind);
            Assert.Contains(ServiceErrorCodes.PlanNotDraft, locked.Messages);
        }

        [Fact]
        public async Task Status_transitions_follow_the_rules()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);
            var plan = (await service.CreateAsync(client, NewRequest(studentId))).Data!;

            Assert.Equal(ServiceErrorKind.Conflict, (await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Completed)).ErrorKind);
            Assert.True((await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active)).Valid);
            Assert.True((await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Paused)).Valid);
            Assert.True((await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active)).Valid);
            Assert.True((await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Completed)).Valid);
            Assert.Equal(ServiceErrorKind.Conflict, (await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active)).ErrorKind);
        }

        [Fact]
        public async Task Completing_a_lesson_sets_completed_at_and_leaving_completed_clears_it()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);
            var plan = (await service.CreateAsync(client, NewRequest(studentId))).Data!;
            var lesson = plan.Lessons.Single(l => l.LessonId == 502); // three practices, no exam in Athenas
            var lessonId = lesson.Id;

            // Progress is only accepted while the plan is active.
            var whileDraft = await service.UpdateLessonProgressAsync(client, plan.Id, lessonId, new CustomPlanLessonProgressRequestModel { ConceptCompleted = true });
            Assert.Contains(ServiceErrorCodes.PlanNotActive, whileDraft.Messages);
            await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active);

            foreach (var practice in lesson.Practices)
            {
                Assert.True((await Activities(db).RecordPracticeResultAsync(client, plan.Id, practice.Id, new CustomPlanPracticeResultRequestModel { Score = 90 })).Valid);
            }

            var completed = await service.UpdateLessonProgressAsync(client, plan.Id, lessonId,
                new CustomPlanLessonProgressRequestModel { LessonStatus = PlanLessonStatus.Completed, ConceptCompleted = true });
            Assert.True(completed.Valid, string.Join(",", completed.Messages));
            Assert.Equal(_fixture.Time.GetUtcNow().UtcDateTime, completed.Data!.CompletedAt);
            Assert.True(completed.Data.ConceptCompleted);
            Assert.True(completed.Data.PracticesCompleted);

            var reopened = await service.UpdateLessonProgressAsync(client, plan.Id, lessonId,
                new CustomPlanLessonProgressRequestModel { LessonStatus = PlanLessonStatus.InProgress });
            Assert.Null(reopened.Data!.CompletedAt);

            await using var check = _fixture.CreateContext();
            var stored = await check.CustomPlanClientLessons.AsNoTracking().SingleAsync(l => l.Id == lessonId);
            Assert.Equal(PlanLessonStatus.InProgress, stored.LessonStatus);
            Assert.Null(stored.CompletedAt);
            Assert.True(stored.ConceptCompleted);
        }

        [Fact]
        public async Task A_lesson_is_completed_only_when_its_Athenas_practices_and_exams_are_completed()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);
            var request = NewRequest(studentId);
            request.Structure.Lessons[1].Practices.RemoveAt(2); // lesson 502 has 3 practices in Athenas; only 2 scheduled
            var plan = (await service.CreateAsync(client, request)).Data!;
            await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active);
            var lesson501 = plan.Lessons.Single(l => l.LessonId == 501);
            var lesson502 = plan.Lessons.Single(l => l.LessonId == 502);
            var complete = new CustomPlanLessonProgressRequestModel { LessonStatus = PlanLessonStatus.Completed };

            // Nothing done yet: practices and the exam are pending.
            var blocked = await service.UpdateLessonProgressAsync(client, plan.Id, lesson501.Id, complete);
            Assert.Equal(ServiceErrorKind.Conflict, blocked.ErrorKind);
            Assert.Contains(ServiceErrorCodes.PlanLessonPracticesPending, blocked.Messages);
            Assert.Contains(ServiceErrorCodes.PlanLessonExamsPending, blocked.Messages);

            // Practices done, exam still pending.
            foreach (var practice in lesson501.Practices)
            {
                await Activities(db).UpdatePracticeStatusAsync(client, plan.Id, practice.Id, PlanActivityStatus.Completed);
            }

            var examPending = (await service.UpdateLessonProgressAsync(client, plan.Id, lesson501.Id, complete)).Messages;
            Assert.Equal(new[] { ServiceErrorCodes.PlanLessonExamsPending }, examPending);

            await Activities(db).UpdateExamStatusAsync(client, plan.Id, lesson501.Exams[0].Id, PlanActivityStatus.Completed);
            Assert.True((await service.UpdateLessonProgressAsync(client, plan.Id, lesson501.Id, complete)).Valid);

            // Lesson 502: both scheduled practices are done, but Athenas has a third one that is not in the plan.
            foreach (var practice in lesson502.Practices)
            {
                await Activities(db).UpdatePracticeStatusAsync(client, plan.Id, practice.Id, PlanActivityStatus.Completed);
            }

            Assert.Equal(new[] { ServiceErrorCodes.PlanLessonPracticesPending },
                (await service.UpdateLessonProgressAsync(client, plan.Id, lesson502.Id, complete)).Messages);

            // Athenas down: the comparison cannot be made, so the lesson is not completed.
            _athenas.Available = false;
            _athenas.ClearCache();
            Assert.Equal(ServiceErrorKind.Unavailable, (await service.UpdateLessonProgressAsync(client, plan.Id, lesson502.Id, complete)).ErrorKind);
        }

        [Fact]
        public async Task Exam_result_must_be_the_students_attempt_at_that_exam()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);
            var plan = (await service.CreateAsync(client, NewRequest(studentId))).Data!;
            await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active);
            var examDateId = plan.Lessons.Single(l => l.LessonId == 501).Exams.Single().Id;

            await _fixture.ExecuteSqlAsync($"INSERT INTO usqui_user_quiz_results (user_id, quiz_id, is_exam) VALUES ({studentId}, 900, 1), ({studentId}, 901, 1), ({studentId}, 900, 0)");
            var rightResult = Convert.ToInt32(await _fixture.ScalarAsync($"SELECT id FROM usqui_user_quiz_results WHERE user_id = {studentId} AND quiz_id = 900 AND is_exam = 1"));
            var otherExamResult = Convert.ToInt32(await _fixture.ScalarAsync($"SELECT id FROM usqui_user_quiz_results WHERE user_id = {studentId} AND quiz_id = 901"));
            var practiceRow = Convert.ToInt32(await _fixture.ScalarAsync($"SELECT id FROM usqui_user_quiz_results WHERE user_id = {studentId} AND quiz_id = 900 AND is_exam = 0"));

            var activities = Activities(db);
            Assert.Contains(ServiceErrorCodes.PlanExamResultInvalid, (await activities.LinkExamResultAsync(client, plan.Id, examDateId, otherExamResult)).Messages);
            Assert.Contains(ServiceErrorCodes.PlanExamResultInvalid, (await activities.LinkExamResultAsync(client, plan.Id, examDateId, practiceRow)).Messages);
            var linked = await activities.LinkExamResultAsync(client, plan.Id, examDateId, rightResult);
            Assert.True(linked.Valid, string.Join(",", linked.Messages));
            Assert.Equal(rightResult, linked.Data!.GwpUserResultId);
            Assert.Equal(PlanActivityStatus.Completed, linked.Data.Status);
            Assert.Equal(_fixture.Time.GetUtcNow().UtcDateTime, linked.Data.CompletedAt);

            var reopened = (await activities.UpdateExamStatusAsync(client, plan.Id, examDateId, PlanActivityStatus.InProgress)).Data!;
            Assert.Null(reopened.CompletedAt);
        }

        [Fact]
        public async Task Pre_and_post_test_results_come_from_usqui_with_their_own_type()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);
            var plan = (await service.CreateAsync(client, NewRequest(studentId))).Data!;
            await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active);
            var lesson = plan.Lessons.Single(l => l.LessonId == 501);
            var (pre, post) = (lesson.Tests.Single(t => t.TestType == PlanTestType.PreTest), lesson.Tests.Single(t => t.TestType == PlanTestType.PostTest));

            await _fixture.ExecuteSqlAsync($"INSERT INTO usqui_user_quiz_results (user_id, quiz_id, is_exam, total_percent) VALUES ({studentId}, 5018, 2, 40), ({studentId}, 5019, 3, 95), ({studentId}, 5019, 2, 10)");
            int Id(int quiz, int isExam) => Convert.ToInt32(_fixture.ScalarAsync($"SELECT id FROM usqui_user_quiz_results WHERE user_id = {studentId} AND quiz_id = {quiz} AND is_exam = {isExam}").Result);
            var activities = Activities(db);

            Assert.Contains(ServiceErrorCodes.PlanTestResultInvalid, (await activities.LinkTestResultAsync(client, plan.Id, post.Id, Id(5019, 2))).Messages); // wrong type
            Assert.Contains(ServiceErrorCodes.PlanTestResultInvalid, (await activities.LinkTestResultAsync(client, plan.Id, pre.Id, Id(5019, 3))).Messages); // wrong quiz
            var preDone = (await activities.LinkTestResultAsync(client, plan.Id, pre.Id, Id(5018, 2))).Data!;
            Assert.Equal(PlanActivityStatus.Completed, preDone.Status);
            Assert.NotNull(preDone.CompletedAt);
            Assert.Equal(Id(5019, 3), (await activities.LinkTestResultAsync(client, plan.Id, post.Id, Id(5019, 3))).Data!.GwpUserResultId);

            Assert.Equal(PlanActivityStatus.InProgress, (await activities.UpdateTestStatusAsync(client, plan.Id, pre.Id, PlanActivityStatus.InProgress)).Data!.Status);
            Assert.Equal(ServiceErrorKind.NotFound, (await activities.UpdateTestStatusAsync(TestUsers.Client(TestUsers.NewId()), plan.Id, pre.Id, PlanActivityStatus.Completed)).ErrorKind);
        }

        [Fact]
        public async Task Soft_deleted_plan_and_its_children_are_hidden_but_kept()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);
            var plan = (await service.CreateAsync(client, NewRequest(studentId))).Data!;

            Assert.True((await service.DeleteAsync(client, plan.Id)).Valid);

            await using var check = _fixture.CreateContext();
            Assert.False(await check.CustomPlanClients.AnyAsync(p => p.Id == plan.Id));
            Assert.False(await check.CustomPlanClientSubjects.AnyAsync(s => s.CustomPlanClientId == plan.Id));
            Assert.False(await check.CustomPlanClientLessons.AnyAsync(l => l.CustomPlanClientId == plan.Id));
            Assert.False(await check.CustomPlanClientDayTimes.AnyAsync(t => t.Day.CustomPlanClientId == plan.Id));

            var deleted = await check.CustomPlanClients.IgnoreQueryFilters().SingleAsync(p => p.Id == plan.Id);
            Assert.NotNull(deleted.DeletedAt);
            Assert.Equal(2, await check.CustomPlanClientSubjects.IgnoreQueryFilters().CountAsync(s => s.CustomPlanClientId == plan.Id));
            Assert.Equal(ServiceErrorKind.NotFound, (await service.GetAsync(client, plan.Id)).ErrorKind);
        }

        [Fact]
        public async Task Other_clients_cannot_read_a_plan()
        {
            var (client, studentId) = NewClientWithStudent();
            var (otherClient, _) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);
            var plan = (await service.CreateAsync(client, NewRequest(studentId))).Data!;

            Assert.Equal(ServiceErrorKind.NotFound, (await service.GetAsync(otherClient, plan.Id)).ErrorKind);
        }

        public static TheoryData<string, Action<CustomPlanLessonRequestModel>> InvalidLearningWindows => new()
        {
            { ServiceErrorCodes.PlanLearningDatesRequired, l => l.LearningDueDate = null },
            { ServiceErrorCodes.PlanLearningDueBeforeAssigned, l => l.LearningDueDate = new DateOnly(2026, 11, 8) },
            { ServiceErrorCodes.PlanLearningOutsidePlan, l => l.LearningAssignedDate = new DateOnly(2026, 11, 2) },
            { ServiceErrorCodes.PlanLearningOutsidePlan, l => l.LearningDueDate = new DateOnly(2026, 12, 8) }, // plan ends 7 Dec
            { ServiceErrorCodes.PlanLessonDateOutsideLearning, l => l.AssignedDates.Add(new DateOnly(2026, 11, 16)) },
            { ServiceErrorCodes.PlanTimezoneInvalid, l => l.LearningTimezone = "Mars/Olympus_Mons" },
        };

        [Theory]
        [MemberData(nameof(InvalidLearningWindows))]
        public async Task Learning_window_is_validated(string expectedError, Action<CustomPlanLessonRequestModel> breakLesson)
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var request = NewRequest(studentId);
            breakLesson(request.Structure.Lessons[0]);

            var result = await CreateService(db).CreateAsync(client, request);

            Assert.Equal(ServiceErrorKind.Validation, result.ErrorKind);
            Assert.Contains(expectedError, result.Messages);
        }

        public static TheoryData<string, Action<CustomPlanExamRequestModel>> InvalidExams => new()
        {
            { ServiceErrorCodes.PlanExamDueRequired, x => x.DueDateTime = null },
            { ServiceErrorCodes.PlanTimezoneInvalid, x => x.Timezone = "" },
            { ServiceErrorCodes.PlanExamDurationInvalid, x => x.DurationMinutes = 0 },
            { ServiceErrorCodes.PlanExamDurationInvalid, x => x.DurationMinutes = 481 },
            { ServiceErrorCodes.PlanExamBeforeLearning, x => x.AssignedDate = new DateOnly(2026, 11, 8) },
            { ServiceErrorCodes.PlanExamOutsidePlan, x => x.DueDateTime = new DateTime(2026, 12, 9, 12, 0, 0, DateTimeKind.Utc) },
            // 02:00 UTC on the 13th is still the 12th (22:00) in Puerto Rico: before the day the exam opens.
            { ServiceErrorCodes.PlanExamDueBeforeAssigned, x => x.DueDateTime = new DateTime(2026, 11, 13, 2, 0, 0, DateTimeKind.Utc) },
        };

        [Theory]
        [MemberData(nameof(InvalidExams))]
        public async Task Exam_schedule_is_validated(string expectedError, Action<CustomPlanExamRequestModel> breakExam)
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var request = NewRequest(studentId);
            breakExam(request.Structure.Lessons[0].Exams[0]);

            var result = await CreateService(db).CreateAsync(client, request);

            Assert.Equal(ServiceErrorKind.Validation, result.ErrorKind);
            Assert.Contains(expectedError, result.Messages);
        }

        public static TheoryData<string, Action<CustomPlanLessonRequestModel>> InvalidPracticesAndSubjects => new()
        {
            { ServiceErrorCodes.PlanLessonSubjectNotInPlan, l => l.SubjectId = 99 },
            { ServiceErrorCodes.PlanPracticeInvalid, l => l.Practices[0].PracticeId = 0 },
            { ServiceErrorCodes.PlanPracticeDuplicated, l => l.Practices[1].PracticeId = l.Practices[0].PracticeId },
            { ServiceErrorCodes.PlanPracticeSequenceDuplicated, l => l.Practices[1].Sequence = l.Practices[0].Sequence },
            { ServiceErrorCodes.PlanPracticeDueRequired, l => l.Practices[0].DueDateTime = null },
            // 02:00 UTC on Wed 11 is still Tue 10 (22:00) in Puerto Rico: before practice 3 opens (Wed 11).
            { ServiceErrorCodes.PlanPracticeDueBeforeAssigned, l => l.Practices[2].DueDateTime = new DateTime(2026, 11, 11, 2, 0, 0, DateTimeKind.Utc) },
            { ServiceErrorCodes.PlanPracticeOutsidePlan, l => l.Practices[2].DueDateTime = new DateTime(2026, 12, 9, 12, 0, 0, DateTimeKind.Utc) }, // plan ends 7 Dec
            { ServiceErrorCodes.PlanPracticeBeforeLearning, l => l.Practices[0].AssignedDate = new DateOnly(2026, 11, 8) },
            { ServiceErrorCodes.PlanTimezoneInvalid, l => l.Practices[1].Timezone = "Nowhere/Land" },
            { ServiceErrorCodes.PlanTestInvalid, l => l.Tests[0].TestId = 0 },
            { ServiceErrorCodes.PlanTestDuplicated, l => l.Tests[1] = l.Tests[0] },
            { ServiceErrorCodes.PlanTestDueRequired, l => l.Tests[0].DueDateTime = null },
            { ServiceErrorCodes.PlanTestDurationInvalid, l => l.Tests[1].DurationMinutes = 0 },
            { ServiceErrorCodes.PlanTestOutsidePlan, l => l.Tests[0].AssignedDate = new DateOnly(2026, 11, 8) },
        };

        [Theory]
        [MemberData(nameof(InvalidPracticesAndSubjects))]
        public async Task Each_practice_and_the_lesson_subject_are_validated(string expectedError, Action<CustomPlanLessonRequestModel> breakLesson)
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var request = NewRequest(studentId);
            breakLesson(request.Structure.Lessons[0]);

            var result = await CreateService(db).CreateAsync(client, request);

            Assert.Equal(ServiceErrorKind.Validation, result.ErrorKind);
            Assert.Contains(expectedError, result.Messages);
        }

        [Fact]
        public async Task Learning_completion_date_follows_the_three_learning_flags()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);
            var plan = (await service.CreateAsync(client, NewRequest(studentId))).Data!;
            await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active);
            var lessonId = plan.Lessons[0].Id;
            var now = _fixture.Time.GetUtcNow().UtcDateTime;

            // Two of the three learning parts: not finished yet.
            var partial = (await service.UpdateLessonProgressAsync(client, plan.Id, lessonId,
                new CustomPlanLessonProgressRequestModel { ConceptCompleted = true, DefinitionsCompleted = true })).Data!;
            Assert.Null(partial.LearningCompletedAt);

            var learned = (await service.UpdateLessonProgressAsync(client, plan.Id, lessonId,
                new CustomPlanLessonProgressRequestModel { ExamplesCompleted = true })).Data!;
            Assert.Equal(now, learned.LearningCompletedAt);

            var undone = (await service.UpdateLessonProgressAsync(client, plan.Id, lessonId,
                new CustomPlanLessonProgressRequestModel { DefinitionsCompleted = false })).Data!;
            Assert.Null(undone.LearningCompletedAt);

            // The lesson has scheduled practices, so its practices flag follows them and cannot be set by hand.
            Assert.Contains(ServiceErrorCodes.PlanPracticesFlagFollowsPractices, (await service.UpdateLessonProgressAsync(client, plan.Id, lessonId,
                new CustomPlanLessonProgressRequestModel { PracticesCompleted = true })).Messages);
        }

        [Fact]
        public async Task Each_practice_has_its_own_status_and_score()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);
            var plan = (await service.CreateAsync(client, NewRequest(studentId))).Data!;
            var lesson = plan.Lessons.Single(l => l.LessonId == 501);
            Assert.Equal(new ulong[] { 5011, 5012, 5013 }, lesson.Practices.Select(p => p.PracticeId));
            var (first, second, third) = (lesson.Practices[0].Id, lesson.Practices[1].Id, lesson.Practices[2].Id);
            var activities = Activities(db);

            Assert.Contains(ServiceErrorCodes.PlanNotActive, (await activities.UpdatePracticeStatusAsync(client, plan.Id, first, PlanActivityStatus.Completed)).Messages);
            await service.ChangeStatusAsync(client, plan.Id, CustomPlanStatus.Active);

            var started = (await activities.UpdatePracticeStatusAsync(client, plan.Id, first, PlanActivityStatus.InProgress)).Data!;
            Assert.Null(started.CompletedAt);
            Assert.Equal(_fixture.Time.GetUtcNow().UtcDateTime, (await activities.UpdatePracticeStatusAsync(client, plan.Id, first, PlanActivityStatus.Completed)).Data!.CompletedAt);

            // The score is kept on the practice: last and best attempt, attempts counted.
            Assert.Contains(ServiceErrorCodes.PlanPracticeScoreInvalid, (await activities.RecordPracticeResultAsync(client, plan.Id, second, new() { Score = 101 })).Messages);
            Assert.Contains(ServiceErrorCodes.PlanPracticeScoreInvalid, (await activities.RecordPracticeResultAsync(client, plan.Id, second, new() { Score = 50, TotalCorrect = 6, TotalQuestions = 5 })).Messages);
            await activities.RecordPracticeResultAsync(client, plan.Id, second, new() { Score = 85.555m, TotalCorrect = 17, TotalQuestions = 20 });
            var retried = (await activities.RecordPracticeResultAsync(client, plan.Id, second, new() { Score = 60, TotalCorrect = 6, TotalQuestions = 10 })).Data!;
            Assert.Equal(PlanActivityStatus.Completed, retried.Status);
            Assert.Equal(60m, retried.Score);
            Assert.Equal(85.56m, retried.BestScore);
            Assert.Equal((ushort)2, retried.Attempts);
            Assert.Equal((ushort)6, retried.TotalCorrect);
            Assert.NotNull(retried.LastAttemptAt);

            // practices_completed follows the three practices.
            async Task<bool> PracticesCompleted()
            {
                await using var check = _fixture.CreateContext();
                return (await check.CustomPlanClientLessons.AsNoTracking().SingleAsync(l => l.Id == lesson.Id)).PracticesCompleted;
            }

            Assert.False(await PracticesCompleted());
            Assert.True((await activities.UpdatePracticeStatusAsync(client, plan.Id, third, PlanActivityStatus.Completed)).Valid);
            Assert.True(await PracticesCompleted());
            Assert.Null((await activities.UpdatePracticeStatusAsync(client, plan.Id, third, PlanActivityStatus.InProgress)).Data!.CompletedAt);
            Assert.False(await PracticesCompleted());

            var stranger = TestUsers.Client(TestUsers.NewId());
            Assert.Equal(ServiceErrorKind.NotFound, (await activities.UpdatePracticeStatusAsync(stranger, plan.Id, first, PlanActivityStatus.Completed)).ErrorKind);
            Assert.Equal(ServiceErrorKind.NotFound, (await activities.UpdatePracticeStatusAsync(client, plan.Id, ulong.MaxValue, PlanActivityStatus.Completed)).ErrorKind);
        }

        [Fact]
        public async Task Practices_exams_and_tests_must_be_quizzes_of_the_lesson_in_Athenas()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var service = CreateService(db);

            var request = NewRequest(studentId);
            request.Structure.Lessons[0].Practices[0].PracticeId = 5029;          // a practice of lesson 502
            request.Structure.Lessons[0].Exams[0].ExamId = 5012;                  // a practice, not an exam
            request.Structure.Lessons[0].Tests[0].TestId = 5019;                  // the post-test, sent as pre-test
            var result = await service.CreateAsync(client, request);
            Assert.Equal(ServiceErrorKind.Validation, result.ErrorKind);
            Assert.Contains(ServiceErrorCodes.PlanPracticeNotInLesson, result.Messages);
            Assert.Contains(ServiceErrorCodes.PlanExamNotInLesson, result.Messages);
            Assert.Contains(ServiceErrorCodes.PlanTestNotInLesson, result.Messages);

            // No minimum: a lesson can be planned with fewer practices than Athenas has.
            var fewer = NewRequest(studentId);
            fewer.Structure.Lessons[0].Practices.RemoveRange(1, 2);
            Assert.True((await service.CreateAsync(client, fewer)).Valid);

            _athenas.Available = false;
            _athenas.ClearCache();
            Assert.Equal(ServiceErrorKind.Unavailable, (await service.CreateAsync(client, NewRequest(studentId))).ErrorKind);
        }

        [Fact]
        public async Task Duplicated_exams_and_sequences_are_rejected()
        {
            var (client, studentId) = NewClientWithStudent();
            await using var db = _fixture.CreateContext();
            var request = NewRequest(studentId);
            request.Structure.Lessons[0].Exams.Add(Exam());
            request.Structure.Lessons[1].Sequence = request.Structure.Lessons[0].Sequence;

            var result = await CreateService(db).CreateAsync(client, request);

            Assert.Contains(ServiceErrorCodes.PlanExamDuplicated, result.Messages);
            Assert.Contains(ServiceErrorCodes.PlanLessonSequenceDuplicated, result.Messages);
        }
    }
}
