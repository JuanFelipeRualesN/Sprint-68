-- =====================================================================================================================
-- READS to check the sample data (run 20261106_sample_data_custom_plans_community.sql first). Read-only.
--
-- Each query mirrors what the API returns, so the database and the endpoints can be compared:
--   GET gswebapi/custom-plans/overview                                     (all the client's children)
--   GET gswebapi/custom-plans/overview?studentIds=990002                   (one child)
--   GET gswebapi/custom-plans/overview?dateFrom=2026-11-09&dateTo=2026-11-13   (next week's calendar)
--   GET gswebapi/custom-plans/overview?minScore=80                         (lessons with exam average >= 80)
--   GET gswebapi/custom-plans/overview?statuses=cancelled                  (only cancelled plans)
--   GET gswebapi/custom-plans/{plan_a} · GET gswebapi/custom-plans/{plan_a}/evidences
--   GET gswebapi/community/groups/{group}/forums · GET gswebapi/community/forums/{forum_math}/posts
--   GET gswebapi/community/groups/{group}/events · GET gswebapi/community/resources
--
-- Times: stored in UTC. Puerto Rico is UTC-4 with no daylight saving, so CONVERT_TZ(x, '+00:00', '-04:00') shows the
-- local time without needing MySQL's time zone tables. "Today" in the sample is 2026-11-06.
-- =====================================================================================================================

SET @client_id = 990001;
SET @child_a   = 990002;
SET @today     = '2026-11-06';
SET @now_utc   = '2026-11-06 16:00:00';  -- noon in Puerto Rico; use UTC_TIMESTAMP() for the real clock

-- 1) The client's children and their plans (soft-deleted plans are hidden, like the API) -------------------------------
SELECT p.student_id, p.id AS plan_id, t.code AS plan_type, p.status, p.start_date, p.end_date, p.plan_duration_weeks,
       f.code AS framework, f.country_code,
       (SELECT GROUP_CONCAT(s.subject_id ORDER BY s.subject_id) FROM custom_plan_client_subjects s WHERE s.custom_plan_client_id = p.id) AS subject_ids,
       (SELECT COUNT(*) FROM custom_plan_client_lessons l WHERE l.custom_plan_client_id = p.id) AS lessons
  FROM custom_plan_client p
  JOIN custom_plan_types t ON t.id = p.custom_plan_type_id
  LEFT JOIN curriculum_frameworks f ON f.id = p.curriculum_framework_id
 WHERE p.client_id = @client_id AND p.deleted_at IS NULL
 ORDER BY p.student_id, p.start_date DESC;

-- 2) Plan by subject: each lesson with its areas summarized (child A, active plan) -------------------------------------
--    score = average of the linked exam results (usqui total_percent), the same as the overview's lesson score.
SELECT l.subject_id, l.sequence, l.id AS plan_lesson_id, l.lesson_id, l.standard_id, l.lesson_status, l.completed_at,
       l.learning_assigned_date, l.learning_due_date, l.learning_timezone,
       (l.concept_completed AND l.definitions_completed AND l.examples_completed) AS learning_done, l.learning_completed_at,
       (SELECT CONCAT(SUM(pr.status = 'completed'), '/', COUNT(*)) FROM custom_plan_client_lesson_practices pr WHERE pr.custom_plan_client_lesson_id = l.id) AS practices_done,
       (SELECT CONCAT(SUM(e.status = 'completed'), '/', COUNT(*)) FROM custom_plan_client_lesson_exam_dates e WHERE e.custom_plan_client_lesson_id = l.id) AS exams_done,
       (SELECT ROUND(AVG(r.total_percent), 2) FROM custom_plan_client_lesson_exam_dates e
          JOIN usqui_user_quiz_results r ON r.id = e.gwp_user_result_id WHERE e.custom_plan_client_lesson_id = l.id) AS score,
       l.pre_completed, l.practices_completed, l.test_completed, l.post_completed
  FROM custom_plan_client_lessons l
  JOIN custom_plan_client p ON p.id = l.custom_plan_client_id
 WHERE p.client_id = @client_id AND p.student_id = @child_a AND p.status = 'active' AND p.deleted_at IS NULL
 ORDER BY l.subject_id, l.sequence;

-- 3) Practices of each lesson: own schedule, status and score (kept by the plan, not in usqui) -------------------------
SELECT l.lesson_id, pr.sequence, pr.practice_id, pr.status, pr.assigned_date,
       pr.due_date_time AS due_utc, CONVERT_TZ(pr.due_date_time, '+00:00', '-04:00') AS due_puerto_rico, pr.timezone,
       pr.score, pr.best_score, pr.total_correct, pr.total_questions, pr.attempts, pr.last_attempt_at, pr.completed_at,
       (pr.status <> 'completed' AND pr.due_date_time < @now_utc) AS is_overdue
  FROM custom_plan_client_lesson_practices pr
  JOIN custom_plan_client_lessons l ON l.id = pr.custom_plan_client_lesson_id
  JOIN custom_plan_client p ON p.id = l.custom_plan_client_id
 WHERE p.client_id = @client_id AND p.deleted_at IS NULL
 ORDER BY p.id, l.sequence, pr.sequence;

-- 4) Exams with their result from usqui_user_quiz_results -------------------------------------------------------------
SELECT l.lesson_id, e.exam_id, e.status, e.assigned_date, CONVERT_TZ(e.due_date_time, '+00:00', '-04:00') AS due_puerto_rico,
       e.duration_minutes, e.completed_at, r.id AS usqui_id, r.try_no, r.total_correct, r.total_questions, r.total_percent AS score,
       (e.status <> 'completed' AND e.due_date_time < @now_utc) AS is_overdue
  FROM custom_plan_client_lesson_exam_dates e
  JOIN custom_plan_client_lessons l ON l.id = e.custom_plan_client_lesson_id
  JOIN custom_plan_client p ON p.id = l.custom_plan_client_id
  LEFT JOIN usqui_user_quiz_results r ON r.id = e.gwp_user_result_id
 WHERE p.client_id = @client_id AND p.deleted_at IS NULL
 ORDER BY p.id, l.sequence, e.due_date_time;

-- 5) Pre-tests and post-tests (optional) with their result -----------------------------------------------------------
SELECT l.lesson_id, t.test_type, t.test_id, t.status, CONVERT_TZ(t.due_date_time, '+00:00', '-04:00') AS due_puerto_rico,
       t.duration_minutes, r.total_percent AS score, t.completed_at
  FROM custom_plan_client_lesson_tests t
  JOIN custom_plan_client_lessons l ON l.id = t.custom_plan_client_lesson_id
  JOIN custom_plan_client p ON p.id = l.custom_plan_client_id
  LEFT JOIN usqui_user_quiz_results r ON r.id = t.gwp_user_result_id
 WHERE p.client_id = @client_id AND p.deleted_at IS NULL
 ORDER BY p.id, l.sequence, t.test_type;

-- 6) Calendar: everything assigned or due between two dates, for all the client's children ----------------------------
--    Same overlap rule as the overview's dateFrom/dateTo (an area counts if its window touches the range).
SET @from = '2026-11-09';
SET @to   = '2026-11-13';
SELECT * FROM (
    SELECT p.student_id, l.lesson_id, 'learning' AS area, NULL AS quiz_id, l.learning_assigned_date AS opens, l.learning_due_date AS due_local_date,
           IF(l.learning_completed_at IS NULL, 'pending', 'completed') AS state
      FROM custom_plan_client_lessons l JOIN custom_plan_client p ON p.id = l.custom_plan_client_id
     WHERE p.client_id = @client_id AND p.deleted_at IS NULL AND p.status <> 'cancelled'
       AND l.learning_assigned_date <= @to AND l.learning_due_date >= @from
    UNION ALL
    SELECT p.student_id, l.lesson_id, 'practice', pr.practice_id, pr.assigned_date, DATE(CONVERT_TZ(pr.due_date_time, '+00:00', '-04:00')), pr.status
      FROM custom_plan_client_lesson_practices pr JOIN custom_plan_client_lessons l ON l.id = pr.custom_plan_client_lesson_id
      JOIN custom_plan_client p ON p.id = l.custom_plan_client_id
     WHERE p.client_id = @client_id AND p.deleted_at IS NULL AND p.status <> 'cancelled'
       AND pr.assigned_date <= @to AND DATE(CONVERT_TZ(pr.due_date_time, '+00:00', '-04:00')) >= @from
    UNION ALL
    SELECT p.student_id, l.lesson_id, 'exam', e.exam_id, e.assigned_date, DATE(CONVERT_TZ(e.due_date_time, '+00:00', '-04:00')), e.status
      FROM custom_plan_client_lesson_exam_dates e JOIN custom_plan_client_lessons l ON l.id = e.custom_plan_client_lesson_id
      JOIN custom_plan_client p ON p.id = l.custom_plan_client_id
     WHERE p.client_id = @client_id AND p.deleted_at IS NULL AND p.status <> 'cancelled'
       AND e.assigned_date <= @to AND DATE(CONVERT_TZ(e.due_date_time, '+00:00', '-04:00')) >= @from
    UNION ALL
    SELECT p.student_id, l.lesson_id, t.test_type, t.test_id, t.assigned_date, DATE(CONVERT_TZ(t.due_date_time, '+00:00', '-04:00')), t.status
      FROM custom_plan_client_lesson_tests t JOIN custom_plan_client_lessons l ON l.id = t.custom_plan_client_lesson_id
      JOIN custom_plan_client p ON p.id = l.custom_plan_client_id
     WHERE p.client_id = @client_id AND p.deleted_at IS NULL AND p.status <> 'cancelled'
       AND t.assigned_date <= @to AND DATE(CONVERT_TZ(t.due_date_time, '+00:00', '-04:00')) >= @from
) calendar
ORDER BY student_id, due_local_date, lesson_id, area;

-- 7) Overdue as of "now": not completed and past the deadline ---------------------------------------------------------
SELECT p.student_id, l.lesson_id, 'learning' AS area, NULL AS quiz_id, l.learning_due_date AS due
  FROM custom_plan_client_lessons l JOIN custom_plan_client p ON p.id = l.custom_plan_client_id
 WHERE p.client_id = @client_id AND p.status = 'active' AND p.deleted_at IS NULL
   AND l.learning_completed_at IS NULL AND l.learning_due_date < @today
UNION ALL
SELECT p.student_id, l.lesson_id, 'practice', pr.practice_id, pr.due_date_time
  FROM custom_plan_client_lesson_practices pr JOIN custom_plan_client_lessons l ON l.id = pr.custom_plan_client_lesson_id
  JOIN custom_plan_client p ON p.id = l.custom_plan_client_id
 WHERE p.client_id = @client_id AND p.status = 'active' AND p.deleted_at IS NULL
   AND pr.status <> 'completed' AND pr.due_date_time < @now_utc
UNION ALL
SELECT p.student_id, l.lesson_id, 'exam', e.exam_id, e.due_date_time
  FROM custom_plan_client_lesson_exam_dates e JOIN custom_plan_client_lessons l ON l.id = e.custom_plan_client_lesson_id
  JOIN custom_plan_client p ON p.id = l.custom_plan_client_id
 WHERE p.client_id = @client_id AND p.status = 'active' AND p.deleted_at IS NULL
   AND e.status <> 'completed' AND e.due_date_time < @now_utc
ORDER BY student_id, lesson_id, area;

-- 8) Lessons by score (the overview's minScore / maxScore) ------------------------------------------------------------
SET @min_score = 80;
SELECT p.student_id, l.lesson_id, ROUND(AVG(r.total_percent), 2) AS score
  FROM custom_plan_client_lessons l
  JOIN custom_plan_client p ON p.id = l.custom_plan_client_id
  JOIN custom_plan_client_lesson_exam_dates e ON e.custom_plan_client_lesson_id = l.id
  JOIN usqui_user_quiz_results r ON r.id = e.gwp_user_result_id
 WHERE p.client_id = @client_id AND p.deleted_at IS NULL
 GROUP BY p.student_id, l.lesson_id
HAVING score >= @min_score
 ORDER BY score DESC;

-- 9) What blocks each lesson from being completed (scheduled practices/exams not completed) ---------------------------
--    The API also compares with the lesson's quizzes in Athenas (missingPracticeIds / missingExamIds in the overview).
SELECT l.lesson_id, l.lesson_status,
       (SELECT COUNT(*) FROM custom_plan_client_lesson_practices pr WHERE pr.custom_plan_client_lesson_id = l.id AND pr.status <> 'completed') AS practices_pending,
       (SELECT COUNT(*) FROM custom_plan_client_lesson_exam_dates e WHERE e.custom_plan_client_lesson_id = l.id AND e.status <> 'completed') AS exams_pending
  FROM custom_plan_client_lessons l JOIN custom_plan_client p ON p.id = l.custom_plan_client_id
 WHERE p.client_id = @client_id AND p.status = 'active' AND p.deleted_at IS NULL
 ORDER BY l.sequence;

-- 10) Evidences: visible ones (the API hides deleted_at) and the soft-deleted one ------------------------------------
SELECT ev.custom_plan_client_id AS plan_id, l.lesson_id, ev.evidence_type, ev.title, ev.url, ev.blob_name, ev.file_name,
       ev.content_type, ev.size_bytes, ev.created_at, ev.deleted_at,
       IF(ev.deleted_at IS NULL, 'visible', 'hidden (soft-deleted)') AS in_api
  FROM custom_plan_evidences ev
  JOIN custom_plan_client p ON p.id = ev.custom_plan_client_id
  LEFT JOIN custom_plan_client_lessons l ON l.id = ev.custom_plan_client_lesson_id
 WHERE p.client_id = @client_id
 ORDER BY ev.deleted_at IS NOT NULL, ev.created_at DESC;

-- =====================================================================================================================
-- COMMUNITY
-- =====================================================================================================================
SELECT id INTO @group FROM community_groups WHERE community_name LIKE '[SAMPLE]%' AND deleted_at IS NULL ORDER BY id DESC LIMIT 1;

-- 11) Members (pending ones need an owner/moderator to approve) -------------------------------------------------------
SELECT m.user_id, m.role, m.status, m.joined_at
  FROM community_members m WHERE m.community_group_id = @group AND m.deleted_at IS NULL
 ORDER BY FIELD(m.role, 'owner', 'moderator', 'member'), m.joined_at;

-- 12) Forums: pinned first, then by last activity; counters computed like the API -------------------------------------
SELECT f.id, c.code AS category, f.title, f.user_id AS author_id, f.is_pinned, f.is_locked,
       (SELECT COUNT(*) FROM community_forum_posts p WHERE p.community_forum_id = f.id AND p.deleted_at IS NULL) AS posts_count,
       (SELECT MAX(p.created_at) FROM community_forum_posts p WHERE p.community_forum_id = f.id AND p.deleted_at IS NULL) AS last_post_at
  FROM community_forums f JOIN community_forum_categories c ON c.id = f.community_forum_category_id
 WHERE f.community_group_id = @group AND f.deleted_at IS NULL
 ORDER BY f.is_pinned DESC, COALESCE(last_post_at, f.created_at) DESC;

-- 13) Posts of the math forum as a thread. A deleted post with replies is listed as "deleted" (no author, no text) ----
SELECT p.id, p.parent_post_id,
       IF(p.deleted_at IS NULL, p.user_id, NULL) AS author_id,
       IF(p.deleted_at IS NULL, p.post_text, NULL) AS post_text,
       p.deleted_at IS NOT NULL AS is_deleted, p.created_at, p.edited_at,
       (SELECT COUNT(*) FROM community_forum_posts r WHERE r.parent_post_id = p.id AND r.deleted_at IS NULL) AS replies
  FROM community_forum_posts p
  JOIN community_forums f ON f.id = p.community_forum_id
 WHERE f.community_group_id = @group AND f.title = 'Math curriculum for 3rd grade'
   AND (p.deleted_at IS NULL OR EXISTS (SELECT 1 FROM community_forum_posts r WHERE r.parent_post_id = p.id AND r.deleted_at IS NULL))
 ORDER BY COALESCE(p.parent_post_id, p.id), p.parent_post_id IS NOT NULL, p.created_at;

-- 14) Events with capacity: "going" seats against the maximum ---------------------------------------------------------
SELECT e.id, e.event_title, mf.code AS meeting_format, e.event_location,
       CONVERT_TZ(e.event_date_time, '+00:00', '-04:00') AS starts_puerto_rico, e.event_availability, e.event_max_allowed_persons,
       COALESCE(SUM(CASE WHEN i.response = 'going' AND i.deleted_at IS NULL THEN i.quantity_of_persons_assisting END), 0) AS going_seats,
       SUM(i.response = 'interested' AND i.deleted_at IS NULL) AS interested
  FROM community_events e
  JOIN meeting_formats mf ON mf.id = e.meeting_format_id
  LEFT JOIN community_events_interests i ON i.community_event_id = e.id
 WHERE e.community_group_id = @group AND e.deleted_at IS NULL
 GROUP BY e.id;

-- 15) Resources: published for members; drafts only for the author and owners/moderators ------------------------------
SELECT r.title, r.resource_type, r.link, r.author_id, r.published_date,
       IF(r.published_date IS NULL, 'draft', 'published') AS state
  FROM community_resources r
 WHERE r.community_group_id = @group AND r.deleted_at IS NULL
 ORDER BY r.published_date IS NULL, r.published_date DESC;
