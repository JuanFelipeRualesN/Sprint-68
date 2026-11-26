using GSWebAPI.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Xunit;

namespace GSWebAPI.Tests.Schema
{
    /// <summary>MySQL-level behaviour of the migration: seeds, ENUMs, CHECKs, FKs, and the existing table left untouched.</summary>
    [Collection(MySqlCollection.Name)]
    public sealed class MigrationAndConstraintTests
    {
        private readonly MySqlDatabaseFixture _fixture;

        public MigrationAndConstraintTests(MySqlDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Migration_is_applied_and_catalogs_are_seeded()
        {
            await using var db = _fixture.CreateContext();

            Assert.Empty(await db.Database.GetPendingMigrationsAsync());
            Assert.Equal(new[] { "homeschool", "reinforcements", "specific_task", "customized" },
                await db.CustomPlanTypes.OrderBy(t => t.Id).Select(t => t.Code).ToListAsync());
            Assert.Equal(new[] { "online", "in_person" }, await db.MeetingFormats.OrderBy(f => f.Id).Select(f => f.Code).ToListAsync());
            var depr = await db.CurriculumFrameworks.SingleAsync(f => f.Code == "DEPR");
            Assert.Equal("PR", depr.CountryCode);
            Assert.True(depr.IsActive);
        }

        [Fact]
        public async Task Existing_table_is_not_changed_by_the_migration()
        {
            var createTable = await ShowCreateTableAsync("usqui_user_quiz_results");

            Assert.Contains("DEFAULT CHARSET=latin1", createTable);
            Assert.Contains("`id` int NOT NULL AUTO_INCREMENT", createTable);
            Assert.DoesNotContain("deleted_at", createTable);
        }

        [Fact]
        public async Task New_tables_use_unsigned_ids_native_enums_and_utf8mb4()
        {
            var plan = await ShowCreateTableAsync("custom_plan_client");
            Assert.Contains("`id` bigint unsigned NOT NULL AUTO_INCREMENT", plan);
            Assert.Contains("`status` enum('draft','active','paused','completed','cancelled') NOT NULL DEFAULT 'draft'", plan);
            Assert.Contains("`deleted_at` timestamp NULL", plan);
            Assert.Contains("CHARSET=utf8mb4", plan);

            var exams = await ShowCreateTableAsync("custom_plan_client_lesson_exam_dates");
            Assert.Contains("`gwp_user_result_id` int DEFAULT NULL", exams);
            Assert.Contains("REFERENCES `usqui_user_quiz_results` (`id`)", exams);
            Assert.Contains("ON DELETE CASCADE", exams);
            Assert.Contains("`due_date_time` datetime NOT NULL", exams);
            Assert.Contains("`timezone` varchar(64)", exams);
            Assert.Contains("`duration_minutes` smallint unsigned NOT NULL", exams);

            var lessons = await ShowCreateTableAsync("custom_plan_client_lessons");
            Assert.Contains("`learning_assigned_date` date NOT NULL", lessons);
            Assert.Contains("`learning_due_date` date NOT NULL", lessons);
            Assert.Contains("`learning_timezone` varchar(64)", lessons);
            Assert.Contains("`subject_id` bigint unsigned NOT NULL", lessons);
            Assert.DoesNotContain("practices_due_date", lessons); // practices have their own table

            // Practices: scheduled like exams, no duration, score kept here (not in usqui).
            var practices = await ShowCreateTableAsync("custom_plan_client_lesson_practices");
            Assert.Contains("`practice_id` bigint unsigned NOT NULL", practices);
            Assert.Contains("`due_date_time` datetime NOT NULL", practices);
            Assert.Contains("`timezone` varchar(64)", practices);
            Assert.Contains("`status` enum('not_started','in_progress','completed') NOT NULL DEFAULT 'not_started'", practices);
            Assert.Contains("`score` decimal(5,2) DEFAULT NULL", practices);
            Assert.DoesNotContain("duration_minutes", practices);
            Assert.DoesNotContain("usqui_user_quiz_results", practices);
            Assert.Contains("REFERENCES `custom_plan_client_lessons` (`id`) ON DELETE CASCADE", practices);

            var tests = await ShowCreateTableAsync("custom_plan_client_lesson_tests");
            Assert.Contains("`test_type` enum('pre_test','post_test') NOT NULL", tests);
            Assert.Contains("`duration_minutes` smallint unsigned NOT NULL", tests);
            Assert.Contains("REFERENCES `usqui_user_quiz_results` (`id`)", tests);

            Assert.Contains("`status` enum('not_started','in_progress','completed') NOT NULL DEFAULT 'not_started'", exams);

            var evidences = await ShowCreateTableAsync("custom_plan_evidences");
            Assert.Contains("`evidence_type` enum('evidence','project') NOT NULL DEFAULT 'evidence'", evidences);
            Assert.Contains("`url` varchar(1000)", evidences);
            Assert.Contains("REFERENCES `custom_plan_client_lessons` (`id`) ON DELETE SET NULL", evidences);
            Assert.Contains("REFERENCES `custom_plan_client` (`id`) ON DELETE CASCADE", evidences);

            var members = await ShowCreateTableAsync("community_members");
            Assert.Contains("`status` enum('pending','active','banned') NOT NULL DEFAULT 'active'", members);

            // Replies are not cascaded or nulled when their parent goes away (posts are soft-deleted).
            var posts = await ShowCreateTableAsync("community_forum_posts");
            Assert.Contains("FOREIGN KEY (`parent_post_id`) REFERENCES `community_forum_posts` (`id`)", posts);
            Assert.DoesNotContain("FOREIGN KEY (`parent_post_id`) REFERENCES `community_forum_posts` (`id`) ON DELETE", posts);
            Assert.Contains("FOREIGN KEY (`community_forum_id`) REFERENCES `community_forums` (`id`) ON DELETE CASCADE", posts);
        }

        [Theory]
        [InlineData("INSERT INTO custom_plan_client_days (custom_plan_client_id, day_of_week) SELECT id, 8 FROM custom_plan_client LIMIT 1", "CK_custom_plan_client_days_day_of_week")]
        [InlineData("INSERT INTO community_events (community_group_id, organizer_user_id, event_title, meeting_format_id, event_date_time, event_end_date_time, event_timezone) SELECT id, 1, 'x', 1, '2026-12-01 10:00:00', '2026-12-01 09:00:00', 'UTC' FROM community_groups LIMIT 1", "CK_community_events_end_after_start")]
        [InlineData("INSERT INTO custom_plan_client_lessons (custom_plan_client_id, lesson_id, subject_id, sequence, learning_assigned_date, learning_due_date, learning_timezone) SELECT id, 999001, 10, 1, '2026-11-13', '2026-11-09', 'UTC' FROM custom_plan_client LIMIT 1", "CK_custom_plan_client_lessons_learning_dates")]
        [InlineData("INSERT INTO custom_plan_client_lesson_practices (custom_plan_client_lesson_id, practice_id, sequence, assigned_date, due_date_time, timezone, score) SELECT id, 1, 1, '2026-11-13', '2026-11-14 03:00:00', 'UTC', 100.5 FROM custom_plan_client_lessons LIMIT 1", "CK_custom_plan_client_lesson_practices_score")]
        [InlineData("INSERT INTO custom_plan_client_lesson_tests (custom_plan_client_lesson_id, test_type, test_id, assigned_date, due_date_time, timezone, duration_minutes) SELECT id, 'pre_test', 1, '2026-11-09', '2026-11-09 16:00:00', 'UTC', 0 FROM custom_plan_client_lessons LIMIT 1", "CK_custom_plan_client_lesson_tests_duration")]
        [InlineData("INSERT INTO custom_plan_client_lesson_exam_dates (custom_plan_client_lesson_id, exam_id, assigned_date, due_date_time, timezone, duration_minutes) SELECT id, 1, '2026-11-13', '2026-11-14 03:00:00', 'UTC', 0 FROM custom_plan_client_lessons LIMIT 1", "CK_custom_plan_client_lesson_exam_dates_duration")]
        [InlineData("INSERT INTO community_events_interests (community_event_id, user_id, quantity_of_persons_assisting) SELECT id, 424242, 0 FROM community_events LIMIT 1", "CK_community_events_interests_quantity")]
        public async Task Check_constraints_are_enforced_by_MySQL(string sql, string constraintName)
        {
            await EnsureParentRowsAsync();

            var error = await Assert.ThrowsAsync<MySqlException>(() => _fixture.ExecuteSqlAsync(sql));

            Assert.Contains(constraintName, error.Message);
        }

        [Fact]
        public async Task Enum_columns_reject_unknown_values()
        {
            await EnsureParentRowsAsync();

            await Assert.ThrowsAsync<MySqlException>(() => _fixture.ExecuteSqlAsync(
                "UPDATE community_members SET status = 'deleted' WHERE id = (SELECT id FROM (SELECT MIN(id) AS id FROM community_members) AS m)"));
        }

        [Fact]
        public async Task Catalog_rows_in_use_cannot_be_deleted()
        {
            await EnsureParentRowsAsync();

            // custom_plan_types is referenced with ON DELETE RESTRICT.
            await Assert.ThrowsAsync<MySqlException>(() => _fixture.ExecuteSqlAsync(
                "DELETE FROM custom_plan_types WHERE id = (SELECT custom_plan_type_id FROM custom_plan_client LIMIT 1)"));
        }

        private async Task<string> ShowCreateTableAsync(string table)
        {
            await using var connection = new MySqlConnection(_fixture.ConnectionString);
            await connection.OpenAsync();
            await using var command = new MySqlCommand($"SHOW CREATE TABLE `{table}`", connection);
            await using var reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();
            return reader.GetString(1);
        }

        /// <summary>Makes sure there is at least one plan, group, member and event to attach test rows to.</summary>
        private async Task EnsureParentRowsAsync()
        {
            await _fixture.ExecuteSqlAsync(@"
                INSERT INTO custom_plan_client (custom_plan_type_id, client_id, student_id, level_id, plan_duration_weeks, start_date)
                SELECT 1, 1, 2, 1, 1, '2026-11-09' FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM custom_plan_client);
                INSERT INTO custom_plan_client_lessons (custom_plan_client_id, lesson_id, subject_id, sequence, learning_assigned_date, learning_due_date, learning_timezone)
                SELECT MIN(id), 999000, 10, 1, '2026-11-09', '2026-11-13', 'UTC' FROM custom_plan_client HAVING NOT EXISTS (SELECT 1 FROM custom_plan_client_lessons);
                INSERT INTO community_groups (community_name, created_by_user_id)
                SELECT 'seed', 1 FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM community_groups);
                INSERT INTO community_members (community_group_id, user_id)
                SELECT MIN(id), 424242 FROM community_groups HAVING NOT EXISTS (SELECT 1 FROM community_members WHERE user_id = 424242);
                INSERT INTO community_events (community_group_id, organizer_user_id, event_title, meeting_format_id, event_date_time, event_timezone)
                SELECT MIN(id), 1, 'seed', 1, '2026-12-01 10:00:00', 'UTC' FROM community_groups HAVING NOT EXISTS (SELECT 1 FROM community_events);");
        }
    }
}
