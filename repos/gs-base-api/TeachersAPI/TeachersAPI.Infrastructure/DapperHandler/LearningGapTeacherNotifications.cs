using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySqlConnector;
using TeachersAPI.Domain.Models.LearningGap;

namespace TeachersAPI.TeachersAPI.Infrastructure.DapperHandler
{
    /// <summary>
    /// Handle notifications for learning gap with dapper
    /// </summary>
    public class LearningGapTeacherNotifications
    {
        private readonly string _connectionString;

        public LearningGapTeacherNotifications(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Get the unread notifications from the teacher for a specific student.
        /// </summary>
        /// <param name="teacherId">The teacher's id</param>
        /// <param name="studentId">The student's id</param>
        /// <returns>Returns the teacher group student model list</returns>
        public List<LearningGapNotificationModel> GetUnreadNotificationsByTeacherId(string teacherId, string studentId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            string query = @"
                SELECT id, notification_text, academic_year_id, lesson_id, subject_code, teacher_id, student_id,
                    lesson_title,limit_date,assigned_lesson_id, diagnostic_test_id, is_read, notification_type, modified_by, created_at, updated_at
                    FROM gs_teachers_web_db_prod.learning_gap_notifications
                    WHERE student_id = @StudentId 
                        AND teacher_id = @TeacherId
                        AND is_read = 0
                        AND is_active = 1
                ORDER BY created_at DESC;
            ";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var notifications = connection.Query<LearningGapNotificationModel>(
                    query,
                    new { StudentId = studentId, TeacherId = teacherId }
                );

                return notifications.ToList();
            }
        }

        /// <summary>
        /// Set the notification as read (1)
        /// </summary>
        /// <param name="notificationId">The teacher's id</param>
        public void MarkNotificationAsRead(string notificationId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            string query = @"
                UPDATE gs_teachers_web_db_prod.learning_gap_notifications
                SET is_read = 1, updated_at = NOW()
                WHERE id = @NotificationId;
            ";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                connection.Execute(query, new { NotificationId = notificationId });
                }
            }

        /// <summary>
        /// Get the notification by assignedLessonId or diagnosticTestId
        /// </summary>
        /// <param name="assignedLessonId">Id referenced in assigned lessons table</param>
        /// <param name="diagnosticTestId">Id referenced in diagnostic tests table</param>
        /// <returns>The notification that matches the assignedLessonId or diagnosticTestId</returns>
        public LearningGapNotificationModel GetNotificationByLessonOrDiagnosticId(string assignedLessonId = null, string diagnosticTestId = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            string query = @"
                SELECT id, notification_text, academic_year_id, lesson_id, subject_code, 
                    teacher_id, student_id, is_read, notification_type, created_at, updated_at 
                FROM gs_teachers_web_db_prod.learning_gap_notifications
                WHERE 
                    (@AssignedLessonId IS NOT NULL AND assigned_lesson_id = @AssignedLessonId)
                    OR (@DiagnosticTestId IS NOT NULL AND diagnostic_test_id = @DiagnosticTestId)
                LIMIT 1;
            ";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                
                var notification = connection.QueryFirstOrDefault<LearningGapNotificationModel>(
                    query,
                    new { AssignedLessonId = assignedLessonId, DiagnosticTestId = diagnosticTestId }
                );

                return notification;
            }
        }

        /// <summary>
        /// Delete any notification linked with the lesson to be deleted
        /// </summary>
        /// <param name="assignedLessonId">Id referenced in assigned lessons table</param>
        public void DeleteNotificationByAssignedLessonId(string assignedLessonId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            string query = @"
                DELETE FROM gs_teachers_web_db_prod.learning_gap_notifications
                WHERE assigned_lesson_id = @AssignedLessonId;
            ";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                connection.Execute(query, new { AssignedLessonId = assignedLessonId });

            }
        }

        /// <summary>
        /// Check if the teacher has unread notifications
        /// </summary>
        /// <param name="teacherId">The teacher's id</param>
        /// <returns>true or false</returns>
        public bool HasUnreadNotificationsByTeacherId(string teacherId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            string query = @"
                SELECT COUNT(*)
                FROM gs_teachers_web_db_prod.learning_gap_notifications
                WHERE teacher_id = @TeacherId AND is_read = 0 AND is_active = 1;
            ";
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                
                int count = connection.ExecuteScalar<int>(query, new { TeacherId = teacherId });
                return count > 0;
            }
        }

        /// <summary>
        /// Check if the student has active notifications
        /// </summary>
        /// <param name="teacherId">The teacher's id</param>
        /// <returns>A list with the student id and a flag to show if the student has notifications</returns>
        public List<LearningGapStudentHasNotificationsModel> StudentHasNotifications(string teacherId)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            string query = @"
                SELECT student_id AS StudentId, COUNT(CASE WHEN is_read = 0 THEN 1 END) > 0 AS HasNotifications
                FROM gs_teachers_web_db_prod.learning_gap_notifications
                WHERE teacher_id = @TeacherId
                AND is_active = 1
                GROUP BY student_id;
            ";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var result = connection.Query<LearningGapStudentHasNotificationsModel>(query, new { TeacherId = teacherId });
                return result.ToList();
            }
        }

    }
}