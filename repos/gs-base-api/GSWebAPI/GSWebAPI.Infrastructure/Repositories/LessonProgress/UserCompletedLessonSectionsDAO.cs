using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Domain.Models.LessonProgress;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.LessonProgress
{
    /// <summary>
    /// Data access for the user_completed_lesson_sections table
    /// A row marks that a user completed a given lesson section.
    /// </summary>
    class UserCompletedLessonSectionsDAO
    {
        private MySqlConnection _connection;

        /// <summary>
        /// Marks a lesson section as completed for a user.
        /// </summary>
        public void Create(string connectionString, UserCompletedLessonSectionModel section)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand { Connection = _connection };

                cmd.CommandText =
                    "INSERT INTO user_completed_lesson_sections (`user_id`, `lesson_id`, `section`) " +
                    "VALUES (@userId, @lessonId, @section) " +
                    "ON DUPLICATE KEY UPDATE `updated_at` = NOW()";

                cmd.Parameters.AddWithValue("@userId", section.UserId);
                cmd.Parameters.AddWithValue("@lessonId", section.LessonId);
                cmd.Parameters.AddWithValue("@section", section.Section);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                dbConn.CloseConnection(_connection);
            }
        }

        /// <summary>
        /// Returns the completed sections for a user in a given lesson.
        /// </summary>
        public List<UserCompletedLessonSectionModel> ReadByUserAndLesson(
            string connectionString, string userId, string lessonId)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserCompletedLessonSectionModel> response = new List<UserCompletedLessonSectionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand { Connection = _connection };

                cmd.CommandText =
                    "SELECT `id`, `user_id`, `lesson_id`, `section`, `created_at`, `updated_at` " +
                    "FROM user_completed_lesson_sections WHERE `user_id` = @userId AND `lesson_id` = @lessonId";

                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@lessonId", lessonId);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    response.Add(new UserCompletedLessonSectionModel
                    {
                        Id = Convert.ToString(reader["id"]) ?? string.Empty,
                        UserId = Convert.ToString(reader["user_id"]) ?? string.Empty,
                        LessonId = Convert.ToString(reader["lesson_id"]) ?? string.Empty,
                        Section = Convert.ToString(reader["section"]) ?? string.Empty,
                        CreatedAt = Convert.ToString(reader["created_at"]) ?? string.Empty,
                        UpdatedAt = Convert.ToString(reader["updated_at"]) ?? string.Empty,
                    });
                }
            }
            finally
            {
                dbConn.CloseConnection(_connection);
            }

            return response;
        }
    }
}
