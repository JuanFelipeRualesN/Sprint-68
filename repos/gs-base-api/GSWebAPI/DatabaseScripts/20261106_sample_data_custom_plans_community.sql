-- =====================================================================================================================
-- SAMPLE DATA: Custom Plans and Community (gs_web_api_db)
--
-- For local / dev / QA databases only. Do NOT run it on production.
-- Run it after the migration 20261106194343_AddCustomPlansAndCommunity. It can be run again: it first removes the
-- sample rows it created before, then inserts them again, all in one transaction.
--
-- What it removes before inserting (only sample rows):
--   * custom plans whose client_id = @client_id and student_id is @child_a or @child_b (with all their children)
--   * usqui_user_quiz_results rows with feedback = '[SAMPLE] custom plan'
--   * community groups whose name starts with '[SAMPLE]' (with their members, forums, posts, events and resources)
--   => Use test accounts for @client_id / @child_a / @child_b: every plan of that client for those children is removed.
--
-- The ids of users (ul_user_accounts) and of Athenas (level, subjects, standard, lessons, quizzes) are logical
-- references. The defaults below only need to be consistent with each other to check the reads in the database.
-- To check the API too (names, titles, completion against Athenas), set them to real ids of the environment:
--   * users: a client account and two student profiles it manages
--   * lessons: GET gswebapi/lessons/{lessonId}/practices and .../exams give the quiz ids of each lesson
-- The quiz ids default to lessonId * 10 + n (practices n = 1..3, exam 5, pre-test 8, post-test 9).
--
-- "Today" in the data is Friday 6 Nov 2026. All times are Puerto Rico (UTC-4, no daylight saving), stored in UTC.
-- See 20261106_sample_queries_custom_plans_community.sql for the reads to run afterwards.
-- =====================================================================================================================

-- ---------- Settings ----------
SET @client_id       = 990001;  -- client (parent), owner of the plans and of the sample community group
SET @child_a         = 990002;  -- student with an active plan (and a cancelled one)
SET @child_b         = 990003;  -- student with a draft plan
SET @other_parent    = 990004;  -- another member of the community group
SET @pending_parent  = 990005;  -- asked to join the group, not approved yet

SET @level_id        = 3;       -- Athenas level (grade)
SET @subject_math    = 4;      -- Athenas subject ids
SET @subject_science = 6;
SET @standard_1      = 3930;      -- Athenas standard (code) of lesson 1
SET @lesson_1        = 189;    -- Athenas lesson ids
SET @lesson_2        = 190;
SET @lesson_3        = 203328;
SET @lesson_4        = 205226;
SET @tz              = 'SA Western Standard Time';

SET @old_safe_updates = @@SQL_SAFE_UPDATES;
SET SQL_SAFE_UPDATES = 0;
START TRANSACTION;

-- ---------- Remove the previous sample ----------
-- Evidences first: MySQL cannot cascade a plan delete when an evidence is reached by two paths (plan CASCADE and
-- lesson SET NULL). The API never hard-deletes plans (soft delete), so this only matters for manual cleanups.
DELETE ev FROM custom_plan_evidences ev
  JOIN custom_plan_client p ON p.id = ev.custom_plan_client_id
 WHERE p.client_id = @client_id AND p.student_id IN (@child_a, @child_b);
DELETE FROM custom_plan_client
 WHERE client_id = @client_id AND student_id IN (@child_a, @child_b);           -- other children go with ON DELETE CASCADE
DELETE FROM usqui_user_quiz_results WHERE feedback = '[SAMPLE] custom plan';    -- after the plans (FK RESTRICT)

DELETE p FROM community_forum_posts p
  JOIN community_forums f ON f.id = p.community_forum_id
  JOIN community_groups g ON g.id = f.community_group_id
 WHERE g.community_name LIKE '[SAMPLE]%' AND p.parent_post_id IS NOT NULL;      -- replies first (self FK, no cascade)
DELETE p FROM community_forum_posts p
  JOIN community_forums f ON f.id = p.community_forum_id
  JOIN community_groups g ON g.id = f.community_group_id
 WHERE g.community_name LIKE '[SAMPLE]%';
DELETE e FROM community_events e
  JOIN community_groups g ON g.id = e.community_group_id
 WHERE g.community_name LIKE '[SAMPLE]%';                                       -- interests go with the event
DELETE r FROM community_resources r
  JOIN community_groups g ON g.id = r.community_group_id
 WHERE g.community_name LIKE '[SAMPLE]%';
DELETE FROM community_groups WHERE community_name LIKE '[SAMPLE]%';             -- members and forums cascade

-- =====================================================================================================================
-- Results already in usqui_user_quiz_results (exams and pre/post tests; practices are not stored there)
-- =====================================================================================================================
INSERT INTO usqui_user_quiz_results (try_no, user_id, quiz_id, dok, is_exam, lesson_id, subject, level, total_correct, total_questions, total_percent, feedback, created_at)
VALUES (1, @child_a, @lesson_1 * 10 + 8, 1, 2, @lesson_1, 'MAT-EN', '3', 4, 10, 40, '[SAMPLE] custom plan', '2026-11-02 15:10:00');
SET @r_pre_1 = LAST_INSERT_ID();
INSERT INTO usqui_user_quiz_results (try_no, user_id, quiz_id, dok, is_exam, lesson_id, subject, level, total_correct, total_questions, total_percent, feedback, created_at)
VALUES (1, @child_a, @lesson_1 * 10 + 5, 2, 1, @lesson_1, 'MAT-EN', '3', 17, 20, 85, '[SAMPLE] custom plan', '2026-11-05 19:30:00');
SET @r_exam_1 = LAST_INSERT_ID();
INSERT INTO usqui_user_quiz_results (try_no, user_id, quiz_id, dok, is_exam, lesson_id, subject, level, total_correct, total_questions, total_percent, feedback, created_at)
VALUES (1, @child_a, @lesson_4 * 10 + 5, 2, 1, @lesson_4, 'MAT-EN', '3', 19, 20, 95, '[SAMPLE] custom plan', '2026-11-03 20:00:00');
SET @r_exam_4 = LAST_INSERT_ID();

-- =====================================================================================================================
-- PLAN A: child A, homeschool, ACTIVE, DEPR framework, Mon 2 Nov - Mon 30 Nov 2026 (4 weeks), Math + Science
-- =====================================================================================================================
INSERT INTO custom_plan_client (custom_plan_type_id, client_id, student_id, level_id, curriculum_framework_id, plan_duration_weeks, start_date, end_date, status)
SELECT 1, @client_id, @child_a, @level_id, id, 4, '2026-11-02', '2026-11-30', 'active' FROM curriculum_frameworks WHERE code = 'DEPR';
SET @plan_a = LAST_INSERT_ID();

INSERT INTO custom_plan_client_subjects (custom_plan_client_id, subject_id) VALUES (@plan_a, @subject_math), (@plan_a, @subject_science);

-- Study days: Monday and Wednesday 09:00, Friday 10:00 (Puerto Rico)
INSERT INTO custom_plan_client_days (custom_plan_client_id, day_of_week) VALUES (@plan_a, 1);
INSERT INTO custom_plan_client_day_times (custom_plan_client_day_id, start_time, duration_minutes, timezone) VALUES (LAST_INSERT_ID(), '09:00:00', 45, @tz);
INSERT INTO custom_plan_client_days (custom_plan_client_id, day_of_week) VALUES (@plan_a, 3);
INSERT INTO custom_plan_client_day_times (custom_plan_client_day_id, start_time, duration_minutes, timezone) VALUES (LAST_INSERT_ID(), '09:00:00', 45, @tz);
INSERT INTO custom_plan_client_days (custom_plan_client_id, day_of_week) VALUES (@plan_a, 5);
INSERT INTO custom_plan_client_day_times (custom_plan_client_day_id, start_time, duration_minutes, timezone) VALUES (LAST_INSERT_ID(), '10:00:00', 60, @tz);

-- ---------- A1 (Math, standard 77): IN PROGRESS. Learning done, 2 of 3 practices done, exam done, pre-test done, post-test pending
INSERT INTO custom_plan_client_lessons (custom_plan_client_id, lesson_id, standard_id, subject_id, sequence,
    learning_assigned_date, learning_due_date, learning_timezone, learning_completed_at, lesson_status,
    concept_completed, definitions_completed, examples_completed, pre_completed, practices_completed, test_completed, post_completed)
VALUES (@plan_a, @lesson_1, @standard_1, @subject_math, 1, '2026-11-02', '2026-11-04', @tz, '2026-11-04 18:00:00', 'in_progress',
    1, 1, 1, 1, 0, 1, 0);
SET @a1 = LAST_INSERT_ID();
INSERT INTO custom_plan_client_lesson_dates (custom_plan_client_lesson_id, assigned_date) VALUES (@a1, '2026-11-02'), (@a1, '2026-11-04');
INSERT INTO custom_plan_client_lesson_practices (custom_plan_client_lesson_id, practice_id, sequence, assigned_date, due_date_time, timezone,
    status, score, best_score, total_correct, total_questions, attempts, last_attempt_at, completed_at) VALUES
    (@a1, @lesson_1 * 10 + 1, 1, '2026-11-02', '2026-11-04 00:00:00', @tz, 'completed', 80, 80, 8, 10, 1, '2026-11-03 15:00:00', '2026-11-03 15:00:00'),
    (@a1, @lesson_1 * 10 + 2, 2, '2026-11-03', '2026-11-05 00:00:00', @tz, 'completed', 70, 90, 7, 10, 2, '2026-11-04 16:00:00', '2026-11-04 14:00:00'),
    (@a1, @lesson_1 * 10 + 3, 3, '2026-11-04', '2026-11-10 00:00:00', @tz, 'in_progress', NULL, NULL, NULL, NULL, 0, NULL, NULL);
INSERT INTO custom_plan_client_lesson_exam_dates (custom_plan_client_lesson_id, exam_id, assigned_date, due_date_time, timezone, duration_minutes,
    gwp_user_result_id, status, completed_at) VALUES
    (@a1, @lesson_1 * 10 + 5, '2026-11-05', '2026-11-06 03:00:00', @tz, 60, @r_exam_1, 'completed', '2026-11-05 19:30:00');
INSERT INTO custom_plan_client_lesson_tests (custom_plan_client_lesson_id, test_type, test_id, assigned_date, due_date_time, timezone, duration_minutes,
    status, gwp_user_result_id, completed_at) VALUES
    (@a1, 'pre_test',  @lesson_1 * 10 + 8, '2026-11-02', '2026-11-02 16:00:00', @tz, 30, 'completed', @r_pre_1, '2026-11-02 15:10:00'),
    (@a1, 'post_test', @lesson_1 * 10 + 9, '2026-11-06', '2026-11-07 02:00:00', @tz, 30, 'not_started', NULL, NULL);

-- ---------- A2 (Math): NOT STARTED, next week (Mon 9 - Fri 13 Nov)
INSERT INTO custom_plan_client_lessons (custom_plan_client_id, lesson_id, standard_id, subject_id, sequence,
    learning_assigned_date, learning_due_date, learning_timezone)
VALUES (@plan_a, @lesson_2, NULL, @subject_math, 2, '2026-11-09', '2026-11-13', @tz);
SET @a2 = LAST_INSERT_ID();
INSERT INTO custom_plan_client_lesson_dates (custom_plan_client_lesson_id, assigned_date) VALUES (@a2, '2026-11-09'), (@a2, '2026-11-11');
INSERT INTO custom_plan_client_lesson_practices (custom_plan_client_lesson_id, practice_id, sequence, assigned_date, due_date_time, timezone) VALUES
    (@a2, @lesson_2 * 10 + 1, 1, '2026-11-09', '2026-11-11 00:00:00', @tz),
    (@a2, @lesson_2 * 10 + 2, 2, '2026-11-10', '2026-11-12 00:00:00', @tz),
    (@a2, @lesson_2 * 10 + 3, 3, '2026-11-11', '2026-11-13 00:00:00', @tz);
INSERT INTO custom_plan_client_lesson_exam_dates (custom_plan_client_lesson_id, exam_id, assigned_date, due_date_time, timezone, duration_minutes) VALUES
    (@a2, @lesson_2 * 10 + 5, '2026-11-13', '2026-11-14 03:00:00', @tz, 45);

-- ---------- A3 (Science): OVERDUE. Learning, practice and exam were due this week and are not done
INSERT INTO custom_plan_client_lessons (custom_plan_client_id, lesson_id, standard_id, subject_id, sequence,
    learning_assigned_date, learning_due_date, learning_timezone, lesson_status, concept_completed)
VALUES (@plan_a, @lesson_3, NULL, @subject_science, 3, '2026-11-02', '2026-11-04', @tz, 'in_progress', 1);
SET @a3 = LAST_INSERT_ID();
INSERT INTO custom_plan_client_lesson_practices (custom_plan_client_lesson_id, practice_id, sequence, assigned_date, due_date_time, timezone) VALUES
    (@a3, @lesson_3 * 10 + 1, 1, '2026-11-02', '2026-11-04 00:00:00', @tz);
INSERT INTO custom_plan_client_lesson_exam_dates (custom_plan_client_lesson_id, exam_id, assigned_date, due_date_time, timezone, duration_minutes) VALUES
    (@a3, @lesson_3 * 10 + 5, '2026-11-04', '2026-11-05 03:00:00', @tz, 30);

-- ---------- A4 (Math): COMPLETED. All practices and the exam done (no pre/post test: they are optional)
INSERT INTO custom_plan_client_lessons (custom_plan_client_id, lesson_id, standard_id, subject_id, sequence,
    learning_assigned_date, learning_due_date, learning_timezone, learning_completed_at, lesson_status,
    concept_completed, definitions_completed, examples_completed, practices_completed, test_completed, completed_at)
VALUES (@plan_a, @lesson_4, NULL, @subject_math, 4, '2026-11-02', '2026-11-03', @tz, '2026-11-02 20:00:00', 'completed',
    1, 1, 1, 1, 1, '2026-11-03 20:00:00');
SET @a4 = LAST_INSERT_ID();
INSERT INTO custom_plan_client_lesson_practices (custom_plan_client_lesson_id, practice_id, sequence, assigned_date, due_date_time, timezone,
    status, score, best_score, total_correct, total_questions, attempts, last_attempt_at, completed_at) VALUES
    (@a4, @lesson_4 * 10 + 1, 1, '2026-11-02', '2026-11-04 00:00:00', @tz, 'completed', 95, 95, 19, 20, 1, '2026-11-02 21:00:00', '2026-11-02 21:00:00'),
    (@a4, @lesson_4 * 10 + 2, 2, '2026-11-02', '2026-11-04 00:00:00', @tz, 'completed', 100, 100, 10, 10, 3, '2026-11-03 14:00:00', '2026-11-03 13:00:00');
INSERT INTO custom_plan_client_lesson_exam_dates (custom_plan_client_lesson_id, exam_id, assigned_date, due_date_time, timezone, duration_minutes,
    gwp_user_result_id, status, completed_at) VALUES
    (@a4, @lesson_4 * 10 + 5, '2026-11-03', '2026-11-04 03:00:00', @tz, 60, @r_exam_4, 'completed', '2026-11-03 20:00:00');

-- ---------- Evidences of plan A (one file on lesson A1, one link on the plan, one deleted)
INSERT INTO custom_plan_evidences (custom_plan_client_id, custom_plan_client_lesson_id, evidence_type, title, description, url, blob_name,
    file_name, content_type, size_bytes, uploaded_by_user_id, deleted_at) VALUES
    (@plan_a, @a1, 'evidence', 'Fractions worksheet', 'Worksheet done on Wednesday',
     CONCAT('https://sample.blob.core.windows.net/gsweb-api/custom-plans/', @plan_a, '/evidences/sample-fractions-worksheet.pdf'),
     CONCAT('custom-plans/', @plan_a, '/evidences/sample-fractions-worksheet.pdf'), 'fractions-worksheet.pdf', 'application/pdf', 245760, @client_id, NULL),
    (@plan_a, NULL, 'project', 'Volcano model', 'Science fair project video', 'https://www.youtube.com/watch?v=sample-volcano', NULL,
     NULL, NULL, NULL, @client_id, NULL),
    (@plan_a, @a3, 'evidence', 'Wrong photo (deleted)', NULL, 'https://example.com/wrong-photo.jpg', NULL,
     NULL, NULL, NULL, @client_id, '2026-11-05 12:00:00');

-- =====================================================================================================================
-- PLAN B: child B, reinforcements, DRAFT, no framework, Mon 9 - Mon 23 Nov 2026 (2 weeks)
-- =====================================================================================================================
INSERT INTO custom_plan_client (custom_plan_type_id, client_id, student_id, level_id, curriculum_framework_id, plan_duration_weeks, start_date, end_date, status)
VALUES (2, @client_id, @child_b, @level_id, NULL, 2, '2026-11-09', '2026-11-23', 'draft');
SET @plan_b = LAST_INSERT_ID();
INSERT INTO custom_plan_client_subjects (custom_plan_client_id, subject_id) VALUES (@plan_b, @subject_math);
INSERT INTO custom_plan_client_days (custom_plan_client_id, day_of_week) VALUES (@plan_b, 2);
INSERT INTO custom_plan_client_day_times (custom_plan_client_day_id, start_time, duration_minutes, timezone) VALUES (LAST_INSERT_ID(), '15:30:00', 30, @tz);
INSERT INTO custom_plan_client_lessons (custom_plan_client_id, lesson_id, standard_id, subject_id, sequence,
    learning_assigned_date, learning_due_date, learning_timezone)
VALUES (@plan_b, @lesson_2, NULL, @subject_math, 1, '2026-11-09', '2026-11-12', @tz);
SET @b1 = LAST_INSERT_ID();
INSERT INTO custom_plan_client_lesson_practices (custom_plan_client_lesson_id, practice_id, sequence, assigned_date, due_date_time, timezone) VALUES
    (@b1, @lesson_2 * 10 + 1, 1, '2026-11-09', '2026-11-11 00:00:00', @tz),
    (@b1, @lesson_2 * 10 + 2, 2, '2026-11-10', '2026-11-12 00:00:00', @tz),
    (@b1, @lesson_2 * 10 + 3, 3, '2026-11-11', '2026-11-13 00:00:00', @tz);

-- =====================================================================================================================
-- PLAN C: child A, specific task, CANCELLED (October). Shows up only when the overview asks for cancelled plans
-- =====================================================================================================================
INSERT INTO custom_plan_client (custom_plan_type_id, client_id, student_id, level_id, curriculum_framework_id, plan_duration_weeks, start_date, end_date, status)
VALUES (3, @client_id, @child_a, @level_id, NULL, 1, '2026-10-05', '2026-10-12', 'cancelled');
SET @plan_c = LAST_INSERT_ID();
INSERT INTO custom_plan_client_subjects (custom_plan_client_id, subject_id) VALUES (@plan_c, @subject_science);
INSERT INTO custom_plan_client_lessons (custom_plan_client_id, lesson_id, standard_id, subject_id, sequence,
    learning_assigned_date, learning_due_date, learning_timezone)
VALUES (@plan_c, @lesson_3, NULL, @subject_science, 1, '2026-10-05', '2026-10-09', @tz);

-- =====================================================================================================================
-- COMMUNITY: one public group with members, forums, posts (with a reply and a deleted post), an event and resources
-- =====================================================================================================================
INSERT INTO community_groups (community_name, community_location, community_description, created_by_user_id, visibility)
VALUES ('[SAMPLE] Homeschool families - San Juan', 'San Juan, PR', 'Families that homeschool in the metro area.', @client_id, 'public');
SET @group = LAST_INSERT_ID();
INSERT INTO community_members (community_group_id, user_id, role, status, joined_at) VALUES
    (@group, @client_id,      'owner',  'active',  '2026-10-01 12:00:00'),
    (@group, @other_parent,   'member', 'active',  '2026-10-03 12:00:00'),
    (@group, @pending_parent, 'member', 'pending', NULL);

-- Forums: a pinned, locked welcome forum (general) and a math question
INSERT INTO community_forums (community_group_id, community_forum_category_id, user_id, title, description, is_pinned, is_locked, created_at)
VALUES (@group, 1, @client_id, 'Welcome! Read before posting', 'Group rules and calendar.', 1, 1, '2026-10-01 12:30:00');
SET @forum_rules = LAST_INSERT_ID();
INSERT INTO community_forum_posts (community_forum_id, parent_post_id, user_id, post_text, created_at)
VALUES (@forum_rules, NULL, @client_id, 'Be kind, share resources, and check the events tab.', '2026-10-01 12:31:00');

INSERT INTO community_forums (community_group_id, community_forum_category_id, user_id, title, description, created_at)
VALUES (@group, 2, @other_parent, 'Math curriculum for 3rd grade', 'Which books are you using?', '2026-11-02 13:00:00');
SET @forum_math = LAST_INSERT_ID();
INSERT INTO community_forum_posts (community_forum_id, parent_post_id, user_id, post_text, created_at)
VALUES (@forum_math, NULL, @other_parent, 'We are starting fractions. Any book you recommend?', '2026-11-02 13:05:00');
SET @post_question = LAST_INSERT_ID();
INSERT INTO community_forum_posts (community_forum_id, parent_post_id, user_id, post_text, created_at, edited_at)
VALUES (@forum_math, @post_question, @client_id, 'We use the plan worksheets plus a fractions game on Fridays.', '2026-11-02 14:00:00', '2026-11-02 14:10:00');
-- A deleted post that still has a reply: the API lists it as "deleted" with no author or text
INSERT INTO community_forum_posts (community_forum_id, parent_post_id, user_id, post_text, created_at, deleted_at)
VALUES (@forum_math, NULL, @client_id, 'Posted in the wrong forum.', '2026-11-03 10:00:00', '2026-11-03 10:05:00');
SET @post_deleted = LAST_INSERT_ID();
INSERT INTO community_forum_posts (community_forum_id, parent_post_id, user_id, post_text, created_at)
VALUES (@forum_math, @post_deleted, @other_parent, 'No worries, I saw it anyway!', '2026-11-03 10:02:00');

-- Event: field trip on Fri 20 Nov 2026, 10:00-13:00 Puerto Rico, max 20 people
INSERT INTO community_events (community_group_id, organizer_user_id, event_title, event_description, meeting_format_id, event_location,
    event_date_time, event_end_date_time, event_timezone, event_availability, event_max_allowed_persons)
VALUES (@group, @client_id, 'Field trip: El Yunque', 'Guided walk for kids 7-10.', 2, 'El Yunque National Forest',
    '2026-11-20 14:00:00', '2026-11-20 17:00:00', @tz, 'open', 20);
SET @event = LAST_INSERT_ID();
INSERT INTO community_events_interests (community_event_id, user_id, response, quantity_of_persons_assisting) VALUES
    (@event, @client_id, 'going', 3),
    (@event, @other_parent, 'interested', 1);

-- Resources: one published, one draft (visible to the author and owners/moderators only)
INSERT INTO community_resources (community_group_id, author_id, title, description, resource_type, link, published_date) VALUES
    (@group, @other_parent, 'Fractions printable pack', 'Free printables for 3rd grade.', 'document', 'https://example.com/fractions-pack.pdf', '2026-11-02 15:00:00'),
    (@group, @client_id, 'Science fair checklist (draft)', NULL, 'link', 'https://example.com/science-fair', NULL);

COMMIT;
SET SQL_SAFE_UPDATES = @old_safe_updates;

-- Ids created (useful for the API calls in the queries file)
SELECT @plan_a AS plan_a_active, @plan_b AS plan_b_draft, @plan_c AS plan_c_cancelled,
       @a1 AS lesson_a1, @a2 AS lesson_a2, @a3 AS lesson_a3, @a4 AS lesson_a4,
       @group AS community_group, @forum_math AS forum_math, @event AS community_event;
