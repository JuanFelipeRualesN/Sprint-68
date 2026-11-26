-- Existing tables of gs_web_api_db that the new migration references. Copied from production
-- (SHOW CREATE TABLE). The migration must apply on top of them without changing them.
CREATE TABLE `usqui_user_quiz_results` (
  `id` int NOT NULL AUTO_INCREMENT,
  `try_no` int DEFAULT NULL COMMENT 'The try number (how many times the user has taken the same quiz).',
  `user_id` int DEFAULT NULL COMMENT 'The ID of the user who took the quiz. This comes from gs_users_db_prod user''s account table. ',
  `quiz_id` int DEFAULT NULL COMMENT 'The quiz id. This is the quiz id from Athenas API.',
  `dok` int DEFAULT NULL COMMENT 'The level of knowledge. This data comes from Athenas.',
  `is_exam` int DEFAULT NULL COMMENT '1 if exam, 0 if practice.',
  `lesson_id` int DEFAULT NULL COMMENT 'The Id of the lesson. This comes from Athenas API.',
  `subject` char(6) DEFAULT NULL COMMENT 'The subject code. Example: MAT-SP, MAT-EN, etc.',
  `level` char(2) DEFAULT NULL COMMENT 'The level code (grade). For example 1, 2, K, etc. ',
  `total_correct` int DEFAULT NULL COMMENT 'The total of questions that were correct.',
  `total_questions` int DEFAULT NULL COMMENT 'The total questions.',
  `total_percent` float DEFAULT NULL COMMENT 'The total percent.',
  `feedback` text COMMENT 'To handle feedbacks from a teacher. This is NOT used in this project. This is for another project. It was used because the project used to have a shared library.',
  `status` char(1) DEFAULT NULL COMMENT 'The status of the quiz. This is NOT used for this project. It was used because the project used to have a shared library.',
  `modified_by` int DEFAULT '1' COMMENT 'The user who modified the row.',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'The datetime of the creation of the row.',
  `updated_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'The datetime when the row was updated.',
  PRIMARY KEY (`id`),
  KEY `idx_usqui_user_quiz_quiz_id` (`quiz_id` DESC),
  KEY `idx_usqui_user_quiz_results_user_id` (`user_id` DESC) /*!80000 INVISIBLE */,
  KEY `idx_usqui_user_quiz_lesson_id` (`lesson_id` DESC)
) ENGINE=InnoDB AUTO_INCREMENT=9112 DEFAULT CHARSET=latin1 COMMENT='To handle the student-quiz results of Athenas quizzes.';
