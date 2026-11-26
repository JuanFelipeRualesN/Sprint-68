CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
CREATE TABLE `community_forum_categories` (
    `id` tinyint unsigned NOT NULL,
    `code` varchar(30) NOT NULL,
    `name` varchar(60) NOT NULL,
    `moderators_only` tinyint(1) NOT NULL DEFAULT FALSE,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `PK_community_forum_categories` PRIMARY KEY (`id`)
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `community_groups` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `community_name` varchar(120) NOT NULL,
    `community_location` varchar(255) NULL,
    `community_description` text NULL,
    `created_by_user_id` bigint unsigned NOT NULL,
    `visibility` enum('public','private') NOT NULL DEFAULT 'public',
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `deleted_at` timestamp NULL,
    CONSTRAINT `PK_community_groups` PRIMARY KEY (`id`)
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `curriculum_frameworks` (
    `id` smallint unsigned NOT NULL AUTO_INCREMENT,
    `code` varchar(30) NOT NULL,
    `name` varchar(120) NOT NULL,
    `country_code` char(2) NOT NULL,
    `region` varchar(80) NULL,
    `is_active` tinyint(1) NOT NULL DEFAULT TRUE,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `PK_curriculum_frameworks` PRIMARY KEY (`id`)
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `custom_plan_types` (
    `id` tinyint unsigned NOT NULL,
    `code` varchar(30) NOT NULL,
    `name` varchar(60) NOT NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `PK_custom_plan_types` PRIMARY KEY (`id`)
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `meeting_formats` (
    `id` tinyint unsigned NOT NULL,
    `code` varchar(30) NOT NULL,
    `name` varchar(60) NOT NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `PK_meeting_formats` PRIMARY KEY (`id`)
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `community_forums` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `community_group_id` bigint unsigned NOT NULL,
    `community_forum_category_id` tinyint unsigned NOT NULL,
    `user_id` bigint unsigned NOT NULL,
    `title` varchar(150) NOT NULL,
    `description` text NULL,
    `is_pinned` tinyint(1) NOT NULL DEFAULT FALSE,
    `is_locked` tinyint(1) NOT NULL DEFAULT FALSE,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `deleted_at` timestamp NULL,
    CONSTRAINT `PK_community_forums` PRIMARY KEY (`id`),
    CONSTRAINT `FK_community_forums_community_forum_categories_community_forum_~` FOREIGN KEY (`community_forum_category_id`) REFERENCES `community_forum_categories` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_community_forums_community_groups_community_group_id` FOREIGN KEY (`community_group_id`) REFERENCES `community_groups` (`id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `community_members` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `community_group_id` bigint unsigned NOT NULL,
    `user_id` bigint unsigned NOT NULL,
    `role` enum('owner','moderator','member') NOT NULL DEFAULT 'member',
    `status` enum('pending','active','banned') NOT NULL DEFAULT 'active',
    `joined_at` datetime NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `deleted_at` timestamp NULL,
    CONSTRAINT `PK_community_members` PRIMARY KEY (`id`),
    CONSTRAINT `FK_community_members_community_groups_community_group_id` FOREIGN KEY (`community_group_id`) REFERENCES `community_groups` (`id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `community_resources` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `community_group_id` bigint unsigned NULL,
    `author_id` bigint unsigned NOT NULL,
    `title` varchar(150) NOT NULL,
    `description` text NULL,
    `resource_type` enum('link','document','video') NOT NULL DEFAULT 'link',
    `link` varchar(500) NOT NULL,
    `published_date` datetime NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `deleted_at` timestamp NULL,
    CONSTRAINT `PK_community_resources` PRIMARY KEY (`id`),
    CONSTRAINT `FK_community_resources_community_groups_community_group_id` FOREIGN KEY (`community_group_id`) REFERENCES `community_groups` (`id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `custom_plan_client` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `custom_plan_type_id` tinyint unsigned NOT NULL,
    `client_id` bigint unsigned NOT NULL,
    `student_id` bigint unsigned NOT NULL,
    `level_id` bigint unsigned NOT NULL,
    `curriculum_framework_id` smallint unsigned NULL,
    `plan_duration_weeks` smallint unsigned NOT NULL,
    `start_date` date NOT NULL,
    `end_date` date NULL,
    `status` enum('draft','active','paused','completed','cancelled') NOT NULL DEFAULT 'draft',
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `deleted_at` timestamp NULL,
    CONSTRAINT `PK_custom_plan_client` PRIMARY KEY (`id`),
    CONSTRAINT `FK_custom_plan_client_curriculum_frameworks_curriculum_framewor~` FOREIGN KEY (`curriculum_framework_id`) REFERENCES `curriculum_frameworks` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_custom_plan_client_custom_plan_types_custom_plan_type_id` FOREIGN KEY (`custom_plan_type_id`) REFERENCES `custom_plan_types` (`id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `community_events` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `community_group_id` bigint unsigned NOT NULL,
    `organizer_user_id` bigint unsigned NOT NULL,
    `event_title` varchar(150) NOT NULL,
    `event_description` text NULL,
    `meeting_format_id` tinyint unsigned NOT NULL,
    `event_location` varchar(255) NULL,
    `event_date_time` datetime NOT NULL,
    `event_end_date_time` datetime NULL,
    `event_timezone` varchar(64) NOT NULL,
    `event_availability` enum('open','closed','cancelled') NOT NULL DEFAULT 'open',
    `event_max_allowed_persons` smallint unsigned NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `deleted_at` timestamp NULL,
    CONSTRAINT `PK_community_events` PRIMARY KEY (`id`),
    CONSTRAINT `CK_community_events_end_after_start` CHECK (`event_end_date_time` IS NULL OR `event_end_date_time` > `event_date_time`),
    CONSTRAINT `FK_community_events_community_groups_community_group_id` FOREIGN KEY (`community_group_id`) REFERENCES `community_groups` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_community_events_meeting_formats_meeting_format_id` FOREIGN KEY (`meeting_format_id`) REFERENCES `meeting_formats` (`id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `community_forum_posts` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `community_forum_id` bigint unsigned NOT NULL,
    `parent_post_id` bigint unsigned NULL,
    `user_id` bigint unsigned NOT NULL,
    `post_text` text NOT NULL,
    `edited_at` datetime NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `deleted_at` timestamp NULL,
    CONSTRAINT `PK_community_forum_posts` PRIMARY KEY (`id`),
    CONSTRAINT `FK_community_forum_posts_community_forum_posts_parent_post_id` FOREIGN KEY (`parent_post_id`) REFERENCES `community_forum_posts` (`id`),
    CONSTRAINT `FK_community_forum_posts_community_forums_community_forum_id` FOREIGN KEY (`community_forum_id`) REFERENCES `community_forums` (`id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `custom_plan_client_days` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `custom_plan_client_id` bigint unsigned NOT NULL,
    `day_of_week` tinyint unsigned NOT NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `PK_custom_plan_client_days` PRIMARY KEY (`id`),
    CONSTRAINT `CK_custom_plan_client_days_day_of_week` CHECK (`day_of_week` BETWEEN 1 AND 7),
    CONSTRAINT `FK_custom_plan_client_days_custom_plan_client_custom_plan_clien~` FOREIGN KEY (`custom_plan_client_id`) REFERENCES `custom_plan_client` (`id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `custom_plan_client_lessons` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `custom_plan_client_id` bigint unsigned NOT NULL,
    `lesson_id` bigint unsigned NOT NULL,
    `standard_id` bigint unsigned NULL,
    `subject_id` bigint unsigned NOT NULL,
    `sequence` smallint unsigned NOT NULL,
    `learning_assigned_date` date NOT NULL,
    `learning_due_date` date NOT NULL,
    `learning_timezone` varchar(64) NOT NULL,
    `learning_completed_at` datetime NULL,
    `lesson_status` enum('not_started','in_progress','completed','skipped') NOT NULL DEFAULT 'not_started',
    `concept_completed` tinyint(1) NOT NULL DEFAULT FALSE,
    `definitions_completed` tinyint(1) NOT NULL DEFAULT FALSE,
    `examples_completed` tinyint(1) NOT NULL DEFAULT FALSE,
    `pre_completed` tinyint(1) NOT NULL DEFAULT FALSE,
    `practices_completed` tinyint(1) NOT NULL DEFAULT FALSE,
    `test_completed` tinyint(1) NOT NULL DEFAULT FALSE,
    `post_completed` tinyint(1) NOT NULL DEFAULT FALSE,
    `completed_at` datetime NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `PK_custom_plan_client_lessons` PRIMARY KEY (`id`),
    CONSTRAINT `CK_custom_plan_client_lessons_learning_dates` CHECK (`learning_due_date` >= `learning_assigned_date`),
    CONSTRAINT `FK_custom_plan_client_lessons_custom_plan_client_custom_plan_cl~` FOREIGN KEY (`custom_plan_client_id`) REFERENCES `custom_plan_client` (`id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `custom_plan_client_subjects` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `custom_plan_client_id` bigint unsigned NOT NULL,
    `subject_id` bigint unsigned NOT NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `PK_custom_plan_client_subjects` PRIMARY KEY (`id`),
    CONSTRAINT `FK_custom_plan_client_subjects_custom_plan_client_custom_plan_c~` FOREIGN KEY (`custom_plan_client_id`) REFERENCES `custom_plan_client` (`id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `community_events_interests` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `community_event_id` bigint unsigned NOT NULL,
    `user_id` bigint unsigned NOT NULL,
    `response` enum('interested','going','not_going') NOT NULL DEFAULT 'going',
    `quantity_of_persons_assisting` tinyint unsigned NOT NULL DEFAULT 1,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `deleted_at` timestamp NULL,
    CONSTRAINT `PK_community_events_interests` PRIMARY KEY (`id`),
    CONSTRAINT `CK_community_events_interests_quantity` CHECK (`quantity_of_persons_assisting` >= 1),
    CONSTRAINT `FK_community_events_interests_community_events_community_event_~` FOREIGN KEY (`community_event_id`) REFERENCES `community_events` (`id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `custom_plan_client_day_times` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `custom_plan_client_day_id` bigint unsigned NOT NULL,
    `start_time` time NOT NULL,
    `duration_minutes` smallint unsigned NOT NULL,
    `timezone` varchar(64) NOT NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `PK_custom_plan_client_day_times` PRIMARY KEY (`id`),
    CONSTRAINT `FK_custom_plan_client_day_times_custom_plan_client_days_custom_~` FOREIGN KEY (`custom_plan_client_day_id`) REFERENCES `custom_plan_client_days` (`id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `custom_plan_client_lesson_dates` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `custom_plan_client_lesson_id` bigint unsigned NOT NULL,
    `assigned_date` date NOT NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `PK_custom_plan_client_lesson_dates` PRIMARY KEY (`id`),
    CONSTRAINT `FK_custom_plan_client_lesson_dates_custom_plan_client_lessons_c~` FOREIGN KEY (`custom_plan_client_lesson_id`) REFERENCES `custom_plan_client_lessons` (`id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `custom_plan_client_lesson_exam_dates` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `custom_plan_client_lesson_id` bigint unsigned NOT NULL,
    `exam_id` bigint unsigned NOT NULL,
    `assigned_date` date NOT NULL,
    `due_date_time` datetime NOT NULL,
    `timezone` varchar(64) NOT NULL,
    `duration_minutes` smallint unsigned NOT NULL,
    `gwp_user_result_id` int NULL,
    `status` enum('not_started','in_progress','completed') NOT NULL DEFAULT 'not_started',
    `completed_at` datetime NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `PK_custom_plan_client_lesson_exam_dates` PRIMARY KEY (`id`),
    CONSTRAINT `CK_custom_plan_client_lesson_exam_dates_duration` CHECK (`duration_minutes` BETWEEN 1 AND 480),
    CONSTRAINT `FK_cpc_lesson_exam_dates_usqui_user_quiz_results` FOREIGN KEY (`gwp_user_result_id`) REFERENCES `usqui_user_quiz_results` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_custom_plan_client_lesson_exam_dates_custom_plan_client_less~` FOREIGN KEY (`custom_plan_client_lesson_id`) REFERENCES `custom_plan_client_lessons` (`id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `custom_plan_client_lesson_practices` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `custom_plan_client_lesson_id` bigint unsigned NOT NULL,
    `practice_id` bigint unsigned NOT NULL,
    `sequence` tinyint unsigned NOT NULL,
    `assigned_date` date NOT NULL,
    `due_date_time` datetime NOT NULL,
    `timezone` varchar(64) NOT NULL,
    `status` enum('not_started','in_progress','completed') NOT NULL DEFAULT 'not_started',
    `score` decimal(5,2) NULL,
    `best_score` decimal(5,2) NULL,
    `total_correct` smallint unsigned NULL,
    `total_questions` smallint unsigned NULL,
    `attempts` smallint unsigned NOT NULL DEFAULT 0,
    `last_attempt_at` datetime NULL,
    `completed_at` datetime NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `PK_custom_plan_client_lesson_practices` PRIMARY KEY (`id`),
    CONSTRAINT `CK_custom_plan_client_lesson_practices_best_score` CHECK (`best_score` IS NULL OR `best_score` BETWEEN 0 AND 100),
    CONSTRAINT `CK_custom_plan_client_lesson_practices_score` CHECK (`score` IS NULL OR `score` BETWEEN 0 AND 100),
    CONSTRAINT `FK_custom_plan_client_lesson_practices_custom_plan_client_lesso~` FOREIGN KEY (`custom_plan_client_lesson_id`) REFERENCES `custom_plan_client_lessons` (`id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `custom_plan_client_lesson_tests` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `custom_plan_client_lesson_id` bigint unsigned NOT NULL,
    `test_type` enum('pre_test','post_test') NOT NULL,
    `test_id` bigint unsigned NOT NULL,
    `assigned_date` date NOT NULL,
    `due_date_time` datetime NOT NULL,
    `timezone` varchar(64) NOT NULL,
    `duration_minutes` smallint unsigned NOT NULL,
    `status` enum('not_started','in_progress','completed') NOT NULL DEFAULT 'not_started',
    `gwp_user_result_id` int NULL,
    `completed_at` datetime NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT `PK_custom_plan_client_lesson_tests` PRIMARY KEY (`id`),
    CONSTRAINT `CK_custom_plan_client_lesson_tests_duration` CHECK (`duration_minutes` BETWEEN 1 AND 480),
    CONSTRAINT `FK_cpc_lesson_tests_usqui_user_quiz_results` FOREIGN KEY (`gwp_user_result_id`) REFERENCES `usqui_user_quiz_results` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_custom_plan_client_lesson_tests_custom_plan_client_lessons_c~` FOREIGN KEY (`custom_plan_client_lesson_id`) REFERENCES `custom_plan_client_lessons` (`id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `custom_plan_evidences` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `custom_plan_client_id` bigint unsigned NOT NULL,
    `custom_plan_client_lesson_id` bigint unsigned NULL,
    `evidence_type` enum('evidence','project') NOT NULL DEFAULT 'evidence',
    `title` varchar(150) NOT NULL,
    `description` text NULL,
    `url` varchar(1000) NOT NULL,
    `blob_name` varchar(500) NULL,
    `file_name` varchar(255) NULL,
    `content_type` varchar(100) NULL,
    `size_bytes` bigint unsigned NULL,
    `uploaded_by_user_id` bigint unsigned NOT NULL,
    `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `deleted_at` timestamp NULL,
    CONSTRAINT `PK_custom_plan_evidences` PRIMARY KEY (`id`),
    CONSTRAINT `FK_custom_plan_evidences_custom_plan_client_custom_plan_client_~` FOREIGN KEY (`custom_plan_client_id`) REFERENCES `custom_plan_client` (`id`) ON DELETE CASCADE,
    CONSTRAINT `FK_custom_plan_evidences_custom_plan_client_lessons_custom_plan~` FOREIGN KEY (`custom_plan_client_lesson_id`) REFERENCES `custom_plan_client_lessons` (`id`) ON DELETE SET NULL
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `community_forum_categories` (`id`, `code`, `created_at`, `name`, `updated_at`)
VALUES (1, 'general', TIMESTAMP '2026-11-05 00:00:00', 'General', TIMESTAMP '2026-11-05 00:00:00'),
(2, 'questions', TIMESTAMP '2026-11-05 00:00:00', 'Questions', TIMESTAMP '2026-11-05 00:00:00');

INSERT INTO `community_forum_categories` (`id`, `code`, `created_at`, `moderators_only`, `name`, `updated_at`)
VALUES (3, 'announcements', TIMESTAMP '2026-11-05 00:00:00', TRUE, 'Announcements', TIMESTAMP '2026-11-05 00:00:00');

INSERT INTO `community_forum_categories` (`id`, `code`, `created_at`, `name`, `updated_at`)
VALUES (4, 'resources', TIMESTAMP '2026-11-05 00:00:00', 'Resources', TIMESTAMP '2026-11-05 00:00:00'),
(5, 'events', TIMESTAMP '2026-11-05 00:00:00', 'Events', TIMESTAMP '2026-11-05 00:00:00');

INSERT INTO `curriculum_frameworks` (`id`, `code`, `country_code`, `created_at`, `is_active`, `name`, `region`, `updated_at`)
VALUES (1, 'DEPR', 'PR', TIMESTAMP '2026-11-05 00:00:00', TRUE, 'Departamento de Educación de Puerto Rico', NULL, TIMESTAMP '2026-11-05 00:00:00');

INSERT INTO `custom_plan_types` (`id`, `code`, `created_at`, `name`, `updated_at`)
VALUES (1, 'homeschool', TIMESTAMP '2026-11-05 00:00:00', 'Homeschool', TIMESTAMP '2026-11-05 00:00:00'),
(2, 'reinforcements', TIMESTAMP '2026-11-05 00:00:00', 'Reinforcements', TIMESTAMP '2026-11-05 00:00:00'),
(3, 'specific_task', TIMESTAMP '2026-11-05 00:00:00', 'Specific task', TIMESTAMP '2026-11-05 00:00:00'),
(4, 'customized', TIMESTAMP '2026-11-05 00:00:00', 'Customized', TIMESTAMP '2026-11-05 00:00:00');

INSERT INTO `meeting_formats` (`id`, `code`, `created_at`, `name`, `updated_at`)
VALUES (1, 'online', TIMESTAMP '2026-11-05 00:00:00', 'Online', TIMESTAMP '2026-11-05 00:00:00'),
(2, 'in_person', TIMESTAMP '2026-11-05 00:00:00', 'In person', TIMESTAMP '2026-11-05 00:00:00');

CREATE INDEX `IX_community_events_community_group_id_event_date_time` ON `community_events` (`community_group_id`, `event_date_time`);

CREATE INDEX `IX_community_events_meeting_format_id` ON `community_events` (`meeting_format_id`);

CREATE INDEX `IX_community_events_organizer_user_id` ON `community_events` (`organizer_user_id`);

CREATE UNIQUE INDEX `IX_community_events_interests_community_event_id_user_id` ON `community_events_interests` (`community_event_id`, `user_id`);

CREATE INDEX `IX_community_events_interests_user_id` ON `community_events_interests` (`user_id`);

CREATE UNIQUE INDEX `IX_community_forum_categories_code` ON `community_forum_categories` (`code`);

CREATE INDEX `IX_community_forum_posts_community_forum_id_created_at` ON `community_forum_posts` (`community_forum_id`, `created_at`);

CREATE INDEX `IX_community_forum_posts_parent_post_id` ON `community_forum_posts` (`parent_post_id`);

CREATE INDEX `IX_community_forum_posts_user_id` ON `community_forum_posts` (`user_id`);

CREATE INDEX `IX_community_forums_community_forum_category_id` ON `community_forums` (`community_forum_category_id`);

CREATE INDEX `IX_community_forums_community_group_id_is_pinned` ON `community_forums` (`community_group_id`, `is_pinned`);

CREATE INDEX `IX_community_forums_user_id` ON `community_forums` (`user_id`);

CREATE INDEX `IX_community_groups_created_by_user_id` ON `community_groups` (`created_by_user_id`);

CREATE UNIQUE INDEX `IX_community_members_community_group_id_user_id` ON `community_members` (`community_group_id`, `user_id`);

CREATE INDEX `IX_community_members_user_id` ON `community_members` (`user_id`);

CREATE INDEX `IX_community_resources_author_id` ON `community_resources` (`author_id`);

CREATE INDEX `IX_community_resources_community_group_id` ON `community_resources` (`community_group_id`);

CREATE UNIQUE INDEX `IX_curriculum_frameworks_code` ON `curriculum_frameworks` (`code`);

CREATE INDEX `IX_custom_plan_client_client_id` ON `custom_plan_client` (`client_id`);

CREATE INDEX `IX_custom_plan_client_curriculum_framework_id` ON `custom_plan_client` (`curriculum_framework_id`);

CREATE INDEX `IX_custom_plan_client_custom_plan_type_id` ON `custom_plan_client` (`custom_plan_type_id`);

CREATE INDEX `IX_custom_plan_client_student_id_status` ON `custom_plan_client` (`student_id`, `status`);

CREATE UNIQUE INDEX `IX_custom_plan_client_day_times_custom_plan_client_day_id_start~` ON `custom_plan_client_day_times` (`custom_plan_client_day_id`, `start_time`);

CREATE UNIQUE INDEX `IX_custom_plan_client_days_custom_plan_client_id_day_of_week` ON `custom_plan_client_days` (`custom_plan_client_id`, `day_of_week`);

CREATE UNIQUE INDEX `IX_custom_plan_client_lesson_dates_custom_plan_client_lesson_id~` ON `custom_plan_client_lesson_dates` (`custom_plan_client_lesson_id`, `assigned_date`);

CREATE UNIQUE INDEX `IX_custom_plan_client_lesson_exam_dates_custom_plan_client_less~` ON `custom_plan_client_lesson_exam_dates` (`custom_plan_client_lesson_id`, `exam_id`, `assigned_date`);

CREATE INDEX `IX_custom_plan_client_lesson_exam_dates_due_date_time` ON `custom_plan_client_lesson_exam_dates` (`due_date_time`);

CREATE INDEX `IX_custom_plan_client_lesson_exam_dates_gwp_user_result_id` ON `custom_plan_client_lesson_exam_dates` (`gwp_user_result_id`);

CREATE UNIQUE INDEX `IX_custom_plan_client_lesson_practices_custom_plan_client_lesso~` ON `custom_plan_client_lesson_practices` (`custom_plan_client_lesson_id`, `practice_id`);

CREATE INDEX `IX_custom_plan_client_lesson_practices_due_date_time` ON `custom_plan_client_lesson_practices` (`due_date_time`);

CREATE UNIQUE INDEX `IX_custom_plan_client_lesson_tests_custom_plan_client_lesson_id~` ON `custom_plan_client_lesson_tests` (`custom_plan_client_lesson_id`, `test_type`, `test_id`);

CREATE INDEX `IX_custom_plan_client_lesson_tests_due_date_time` ON `custom_plan_client_lesson_tests` (`due_date_time`);

CREATE INDEX `IX_custom_plan_client_lesson_tests_gwp_user_result_id` ON `custom_plan_client_lesson_tests` (`gwp_user_result_id`);

CREATE INDEX `IX_custom_plan_client_lessons_custom_plan_client_id_learning_du~` ON `custom_plan_client_lessons` (`custom_plan_client_id`, `learning_due_date`);

CREATE UNIQUE INDEX `IX_custom_plan_client_lessons_custom_plan_client_id_lesson_id` ON `custom_plan_client_lessons` (`custom_plan_client_id`, `lesson_id`);

CREATE INDEX `IX_custom_plan_client_lessons_custom_plan_client_id_sequence` ON `custom_plan_client_lessons` (`custom_plan_client_id`, `sequence`);

CREATE INDEX `IX_custom_plan_client_lessons_custom_plan_client_id_subject_id` ON `custom_plan_client_lessons` (`custom_plan_client_id`, `subject_id`);

CREATE UNIQUE INDEX `IX_custom_plan_client_subjects_custom_plan_client_id_subject_id` ON `custom_plan_client_subjects` (`custom_plan_client_id`, `subject_id`);

CREATE INDEX `IX_custom_plan_evidences_custom_plan_client_id_custom_plan_clie~` ON `custom_plan_evidences` (`custom_plan_client_id`, `custom_plan_client_lesson_id`);

CREATE INDEX `IX_custom_plan_evidences_custom_plan_client_lesson_id` ON `custom_plan_evidences` (`custom_plan_client_lesson_id`);

CREATE INDEX `IX_custom_plan_evidences_uploaded_by_user_id` ON `custom_plan_evidences` (`uploaded_by_user_id`);

CREATE UNIQUE INDEX `IX_custom_plan_types_code` ON `custom_plan_types` (`code`);

CREATE UNIQUE INDEX `IX_meeting_formats_code` ON `meeting_formats` (`code`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20261106194343_AddCustomPlansAndCommunity', '9.0.10');

COMMIT;

