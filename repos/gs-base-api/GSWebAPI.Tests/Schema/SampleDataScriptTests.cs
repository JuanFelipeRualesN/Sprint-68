using GSWebAPI.GSWebAPI.Domain.Models.CustomPlans;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.Community;
using GSWebAPI.GSWebAPI.Services.CustomPlans;
using GSWebAPI.Tests.Infrastructure;
using Xunit;

namespace GSWebAPI.Tests.Schema
{
    /// <summary>
    /// DatabaseScripts/20261106_sample_data_custom_plans_community.sql and its queries file: they run on the real schema,
    /// the seed can be run twice, and the API services read the sample as documented in the scripts.
    /// </summary>
    [Collection(MySqlCollection.Name)]
    public sealed class SampleDataScriptTests
    {
        private const ulong Client = 990001;
        private const ulong ChildA = 990002;
        private const ulong ChildB = 990003;

        private readonly MySqlDatabaseFixture _fixture;

        public SampleDataScriptTests(MySqlDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        private static string Script(string name) => File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Schema", name));

        /// <summary>
        /// Reads a numeric <c>SET @name = value;</c> from the Settings block of the sample script, so the test follows
        /// the Athenas ids the script is configured with (they are meant to be changed to real ids).
        /// </summary>
        private static ulong Setting(string name)
        {
            var match = System.Text.RegularExpressions.Regex.Match(Script("sample_data.sql"), $@"SET\s+@{name}\s*=\s*(\d+)\s*;");
            Assert.True(match.Success, $"@{name} not found in the sample script");
            return ulong.Parse(match.Groups[1].Value);
        }

        private async Task<long> CountAsync(string sql) => Convert.ToInt64(await _fixture.ScalarAsync(sql));

        [Fact]
        public async Task Sample_data_can_be_run_twice_and_the_queries_run()
        {
            await _fixture.ExecuteSqlAsync(Script("sample_data.sql"));
            await _fixture.ExecuteSqlAsync(Script("sample_data.sql"));

            Assert.Equal(3, await CountAsync($"SELECT COUNT(*) FROM custom_plan_client WHERE client_id = {Client}"));
            Assert.Equal(6, await CountAsync($"SELECT COUNT(*) FROM custom_plan_client_lessons l JOIN custom_plan_client p ON p.id = l.custom_plan_client_id WHERE p.client_id = {Client}"));
            Assert.Equal(12, await CountAsync($"SELECT COUNT(*) FROM custom_plan_client_lesson_practices pr JOIN custom_plan_client_lessons l ON l.id = pr.custom_plan_client_lesson_id JOIN custom_plan_client p ON p.id = l.custom_plan_client_id WHERE p.client_id = {Client}"));
            Assert.Equal(3, await CountAsync("SELECT COUNT(*) FROM usqui_user_quiz_results WHERE feedback = '[SAMPLE] custom plan'"));
            Assert.Equal(1, await CountAsync("SELECT COUNT(*) FROM community_groups WHERE community_name LIKE '[SAMPLE]%'"));
            Assert.Equal(5, await CountAsync("SELECT COUNT(*) FROM community_forum_posts p JOIN community_forums f ON f.id = p.community_forum_id JOIN community_groups g ON g.id = f.community_group_id WHERE g.community_name LIKE '[SAMPLE]%'"));

            await _fixture.ExecuteSqlAsync(Script("sample_queries.sql")); // read-only; fails if any query is invalid
        }

        [Fact]
        public async Task The_overview_reads_the_sample_as_documented()
        {
            await _fixture.ExecuteSqlAsync(Script("sample_data.sql"));
            var client = TestUsers.Client(Client);
            await using var db = _fixture.CreateContext();
            var overview = new CustomPlanOverviewService(db, new FakeUserDirectory(), new FakeAthenasCatalog(), new FakeBlobStorage(), _fixture.Time);

            async Task<List<CustomPlanStudentOverviewModel>> Get(CustomPlanOverviewFilterModel filter)
            {
                var result = await overview.GetOverviewAsync(client, filter);
                Assert.True(result.Valid, string.Join(",", result.Messages));
                return result.Data!;
            }

            // Both children; child A has the active and the cancelled plan, child B the draft.
            var all = await Get(new CustomPlanOverviewFilterModel());
            Assert.Equal(new[] { ChildA, ChildB }, all.Select(s => s.StudentId));
            Assert.Equal(new[] { CustomPlanStatus.Active, CustomPlanStatus.Cancelled }, all[0].Plans.Select(p => p.Status));
            Assert.Equal(CustomPlanStatus.Draft, all[1].Plans.Single().Status);

            ulong math = Setting("subject_math"), science = Setting("subject_science");
            ulong lesson1 = Setting("lesson_1"), lesson2 = Setting("lesson_2"), lesson3 = Setting("lesson_3"), lesson4 = Setting("lesson_4");

            var planA = all[0].Plans[0];
            Assert.Equal("DEPR", planA.Framework!.Code);
            Assert.Equal(new[] { math, science }.Order(), planA.Subjects.Select(s => s.Subject.Id));
            var mathLessons = planA.Subjects.Single(s => s.Subject.Id == math).Lessons;
            var scienceLessons = planA.Subjects.Single(s => s.Subject.Id == science).Lessons;
            Assert.Equal(new[] { lesson1, lesson2, lesson4 }, mathLessons.Select(l => l.LessonId));
            Assert.Equal(lesson3, scienceLessons.Single().LessonId);
            Assert.Equal("Volcano model", planA.Evidences.Single().Title); // the deleted evidence is hidden

            // A1: in progress, 2 of 3 practices, exam 85, pre-test done, post-test pending, one file evidence.
            var a1 = mathLessons[0];
            Assert.Equal(PlanLessonStatus.InProgress, a1.LessonStatus);
            Assert.Equal(85, a1.Score);
            Assert.True(a1.Learning.IsCompleted);
            Assert.Equal((3, 2), (a1.Practices.Total, a1.Practices.Completed));
            Assert.Equal(75, a1.Practices.AverageScore);
            Assert.Equal(90, a1.Practices.Items[1].BestScore);
            Assert.Equal(2, a1.Practices.Items[1].Attempts);
            Assert.Equal(85, a1.Exams.Single().Result!.Score);
            Assert.Equal(new[] { PlanActivityStatus.Completed, PlanActivityStatus.NotStarted }, a1.Tests.Select(t => t.Status));
            Assert.Equal(40, a1.Tests[0].Result!.Score);
            Assert.Equal("application/pdf", a1.Evidences.Single().ContentType);
            Assert.Equal(Setting("standard_1"), a1.Standard!.Id);

            // A3: everything overdue. A4: completed with 95.
            var a3 = scienceLessons[0];
            Assert.True(a3.Learning.IsOverdue);
            Assert.Equal(1, a3.Practices.Overdue);
            Assert.True(a3.Exams.Single().IsOverdue);
            var a4 = mathLessons[2];
            Assert.Equal(PlanLessonStatus.Completed, a4.LessonStatus);
            Assert.Equal(95, a4.Score);

            // Filters from the queries file.
            var scored = await Get(new CustomPlanOverviewFilterModel { MinScore = 90 });
            Assert.Equal(lesson4, scored.Single(s => s.StudentId == ChildA).Plans.Single().Subjects.Single().Lessons.Single().LessonId);

            var nextWeek = await Get(new CustomPlanOverviewFilterModel { DateFrom = new DateOnly(2026, 11, 9), DateTo = new DateOnly(2026, 11, 13) });
            Assert.Contains(lesson1, nextWeek[0].Plans.Single().Subjects.SelectMany(s => s.Lessons).Select(l => l.LessonId)); // practice 3 due Mon 9
            Assert.Contains(lesson2, nextWeek[0].Plans.Single().Subjects.SelectMany(s => s.Lessons).Select(l => l.LessonId));
            Assert.DoesNotContain(lesson4, nextWeek[0].Plans.Single().Subjects.SelectMany(s => s.Lessons).Select(l => l.LessonId));

            var cancelled = await Get(new CustomPlanOverviewFilterModel { Statuses = { CustomPlanStatus.Cancelled } });
            Assert.Equal(CustomPlanStatus.Cancelled, cancelled.Single().Plans.Single().Status);
        }

        [Fact]
        public async Task The_overview_only_asks_athenas_about_the_lessons_it_returns()
        {
            await _fixture.ExecuteSqlAsync(Script("sample_data.sql"));
            var client = TestUsers.Client(Client);
            ulong lesson1 = Setting("lesson_1"), lesson2 = Setting("lesson_2"), lesson3 = Setting("lesson_3"), lesson4 = Setting("lesson_4");

            async Task<FakeAthenasCatalog> Run(CustomPlanOverviewFilterModel filter)
            {
                var athenas = new FakeAthenasCatalog();
                await using var db = _fixture.CreateContext();
                var result = await new CustomPlanOverviewService(db, new FakeUserDirectory(), athenas, new FakeBlobStorage(), _fixture.Time)
                    .GetOverviewAsync(client, filter);
                Assert.True(result.Valid, string.Join(",", result.Messages));
                return athenas;
            }

            // Score 90+: only lesson 4 is returned, so only lesson 4 is looked up.
            var scored = await Run(new CustomPlanOverviewFilterModel { MinScore = 90 });
            Assert.Equal(new[] { lesson4 }, scored.QuizLookups.Distinct());
            Assert.Equal(new[] { lesson4 }, scored.LessonLookups.Distinct());

            // Next week (Mon 9 - Fri 13): lesson 4 (done on 2-3 Nov) is not looked up.
            var nextWeek = await Run(new CustomPlanOverviewFilterModel { DateFrom = new DateOnly(2026, 11, 9), DateTo = new DateOnly(2026, 11, 13) });
            Assert.Contains(lesson1, nextWeek.QuizLookups);
            Assert.Contains(lesson2, nextWeek.QuizLookups);
            Assert.DoesNotContain(lesson4, nextWeek.QuizLookups);
            Assert.DoesNotContain(lesson4, nextWeek.LessonLookups);

            // No filter: each lesson once, even when it is in several plans.
            var all = await Run(new CustomPlanOverviewFilterModel());
            Assert.Equal(all.QuizLookups.Count, all.QuizLookups.Distinct().Count());
            Assert.Contains(lesson3, all.QuizLookups);
        }

        [Fact]
        public async Task A_slow_athenas_does_not_hold_the_overview()
        {
            await _fixture.ExecuteSqlAsync(Script("sample_data.sql"));
            var athenas = new FakeAthenasCatalog { StandardsDelay = TimeSpan.FromSeconds(10) }; // an Athenas that hangs
            await using var db = _fixture.CreateContext();
            var service = new CustomPlanOverviewService(db, new FakeUserDirectory(), athenas, new FakeBlobStorage(), _fixture.Time);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var result = await service.GetOverviewAsync(TestUsers.Client(Client), new CustomPlanOverviewFilterModel());
            watch.Stop();

            Assert.True(result.Valid);
            Assert.True(watch.Elapsed < CustomPlanOverviewService.DisplayDataWait + TimeSpan.FromSeconds(2), $"took {watch.Elapsed}");
            var a1 = result.Data![0].Plans[0].Subjects.SelectMany(s => s.Lessons).Single(l => l.LessonId == Setting("lesson_1"));
            Assert.Equal(Setting("standard_1"), a1.Standard!.Id); // the id comes from the plan
            Assert.Null(a1.Standard.Code);                         // the Athenas description was not ready
            Assert.Equal($"Lesson {Setting("lesson_1")}", a1.Title); // the fast reads are still there
            Assert.NotNull(a1.CanBeCompleted);
        }

        [Fact]
        public async Task The_forum_reads_the_sample_thread_with_the_deleted_post()
        {
            await _fixture.ExecuteSqlAsync(Script("sample_data.sql"));
            var forumId = Convert.ToUInt64(await _fixture.ScalarAsync(
                "SELECT f.id FROM community_forums f JOIN community_groups g ON g.id = f.community_group_id WHERE g.community_name LIKE '[SAMPLE]%' AND f.title = 'Math curriculum for 3rd grade'"));
            await using var db = _fixture.CreateContext();
            var forums = new CommunityForumService(db, new FakeUserDirectory(), _fixture.Time, new ProfanityFilter());

            var posts = (await forums.ListPostsAsync(TestUsers.User(Client), forumId, 0, 50)).Data!.Items;

            Assert.Equal(4, posts.Count); // question, reply, deleted post (it has a reply), reply to the deleted post
            var deleted = posts.Single(p => p.IsDeleted);
            Assert.Null(deleted.PostText);
            Assert.Null(deleted.AuthorId);
            Assert.Equal(1, deleted.RepliesCount);
            Assert.Equal(ServiceErrorKind.Forbidden, (await forums.ListPostsAsync(TestUsers.User(990005), forumId, 0, 50)).ErrorKind); // pending member
        }
    }
}
