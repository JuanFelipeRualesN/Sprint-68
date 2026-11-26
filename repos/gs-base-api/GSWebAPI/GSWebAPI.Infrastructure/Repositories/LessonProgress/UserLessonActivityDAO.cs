using System;
using System.Collections.Generic;
using MySqlConnector;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.LessonProgress
{
    /// <summary>
    /// Data access for the user_lesson_activity table.
    /// Used to create consecutive-day streaks.
    /// </summary>
    class UserLessonActivityDAO
    {
        private MySqlConnection _connection;

        /// <summary>
        /// Records today as an active day for the user.
        /// </summary>
        public void RecordToday(string connectionString, string userId)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand { Connection = _connection };

                cmd.CommandText =
                    "INSERT INTO user_lesson_activity (`user_id`, `activity_date`) " +
                    "VALUES (@userId, CURDATE()) " +
                    "ON DUPLICATE KEY UPDATE `id` = `id`";

                cmd.Parameters.AddWithValue("@userId", userId);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                dbConn.CloseConnection(_connection);
            }
        }

        /// <summary>
        /// Returns the user's active days as DateTime values, ordered most recent first.
        /// </summary>
        public List<DateTime> ReadActivityDates(string connectionString, string userId)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<DateTime> dates = new List<DateTime>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand { Connection = _connection };

                cmd.CommandText =
                    "SELECT `activity_date` FROM user_lesson_activity " +
                    "WHERE `user_id` = @userId ORDER BY `activity_date` DESC";

                cmd.Parameters.AddWithValue("@userId", userId);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dates.Add(reader.GetDateTime(0).Date);
                }
            }
            finally
            {
                dbConn.CloseConnection(_connection);
            }

            return dates;
        }
    }
}
