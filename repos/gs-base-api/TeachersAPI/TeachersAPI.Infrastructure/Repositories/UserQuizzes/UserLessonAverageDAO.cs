using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.UserQuizzes;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.UserQuizzes
{
    class UserLessonAverageDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, UserLessonAverageModel> ReadD(string connectionString, UserLessonAverageModel userLessonAverage, bool firstOne, bool orderByLastDate, UserLessonAverageModel userLessonAverageWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserLessonAverageModel> response = new Dictionary<string, UserLessonAverageModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `lesson_id`, `average`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM user_lesson_averages ";
                {
                    if (userLessonAverage != null)
                    {
                        if (!String.IsNullOrEmpty(userLessonAverage.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", userLessonAverage.Id);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userLessonAverage.UserId);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", userLessonAverage.LessonId);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.Average))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `average` = @average ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `average` = @average ");
                            }
                            cmd.Parameters.AddWithValue("@average", userLessonAverage.Average);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.Completed))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `completed` = @completed ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `completed` = @completed ");
                            }
                            cmd.Parameters.AddWithValue("@completed", userLessonAverage.Completed);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", userLessonAverage.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", userLessonAverage.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", userLessonAverage.UpdatedAt);
                        }
                    }
                    if (userLessonAverageWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userLessonAverageWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userLessonAverageWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.LessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + userLessonAverageWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.Average))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `average` LIKE @averageWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `average` LIKE @averageWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@averageWhereLike", "%" + userLessonAverageWhereLike.Average + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.Completed))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `completed` LIKE @completedWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `completed` LIKE @completedWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@completedWhereLike", "%" + userLessonAverageWhereLike.Completed + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userLessonAverageWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userLessonAverageWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userLessonAverageWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = String.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = String.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                userLessonAverage = new UserLessonAverageModel();
                                if (!reader.IsDBNull(0))
                                    userLessonAverage.Id = reader.GetValue(0).ToString();
                                else
                                    userLessonAverage.Id = null;
                                if (!reader.IsDBNull(1))
                                    userLessonAverage.UserId = reader.GetValue(1).ToString();
                                else
                                    userLessonAverage.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userLessonAverage.LessonId = reader.GetValue(2).ToString();
                                else
                                    userLessonAverage.LessonId = null;
                                if (!reader.IsDBNull(3))
                                    userLessonAverage.Average = reader.GetValue(3).ToString();
                                else
                                    userLessonAverage.Average = null;
                                if (!reader.IsDBNull(4))
                                    userLessonAverage.Completed = reader.GetValue(4).ToString();
                                else
                                    userLessonAverage.Completed = null;
                                if (!reader.IsDBNull(5))
                                    userLessonAverage.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    userLessonAverage.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    userLessonAverage.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    userLessonAverage.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    userLessonAverage.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    userLessonAverage.UpdatedAt = null;
                                response.Add(userLessonAverage.Id, userLessonAverage);
                                userLessonAverage = new UserLessonAverageModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserLessonAverageModel>();
                        reader.Close();
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserLessonAverageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserLessonAverageModel> ReadD(string connectionString, UserLessonAverageModel userLessonAverage, bool orderByLastDate, int fromRow, int limit, UserLessonAverageModel userLessonAverageWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserLessonAverageModel> response = new Dictionary<string, UserLessonAverageModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `lesson_id`, `average`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM user_lesson_averages ";
                {
                    if (userLessonAverage != null)
                    {
                        if (!String.IsNullOrEmpty(userLessonAverage.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", userLessonAverage.Id);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userLessonAverage.UserId);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", userLessonAverage.LessonId);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.Average))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `average` = @average ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `average` = @average ");
                            }
                            cmd.Parameters.AddWithValue("@average", userLessonAverage.Average);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.Completed))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `completed` = @completed ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `completed` = @completed ");
                            }
                            cmd.Parameters.AddWithValue("@completed", userLessonAverage.Completed);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", userLessonAverage.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", userLessonAverage.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", userLessonAverage.UpdatedAt);
                        }
                    }
                    if (userLessonAverageWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userLessonAverageWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userLessonAverageWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.LessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + userLessonAverageWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.Average))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `average` LIKE @averageWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `average` LIKE @averageWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@averageWhereLike", "%" + userLessonAverageWhereLike.Average + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.Completed))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `completed` LIKE @completedWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `completed` LIKE @completedWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@completedWhereLike", "%" + userLessonAverageWhereLike.Completed + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userLessonAverageWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userLessonAverageWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userLessonAverageWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = String.Concat(sql, " ORDER BY updated_at DESC");
                    sql = String.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                userLessonAverage = new UserLessonAverageModel();
                                if (!reader.IsDBNull(0))
                                    userLessonAverage.Id = reader.GetValue(0).ToString();
                                else
                                    userLessonAverage.Id = null;
                                if (!reader.IsDBNull(1))
                                    userLessonAverage.UserId = reader.GetValue(1).ToString();
                                else
                                    userLessonAverage.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userLessonAverage.LessonId = reader.GetValue(2).ToString();
                                else
                                    userLessonAverage.LessonId = null;
                                if (!reader.IsDBNull(3))
                                    userLessonAverage.Average = reader.GetValue(3).ToString();
                                else
                                    userLessonAverage.Average = null;
                                if (!reader.IsDBNull(4))
                                    userLessonAverage.Completed = reader.GetValue(4).ToString();
                                else
                                    userLessonAverage.Completed = null;
                                if (!reader.IsDBNull(5))
                                    userLessonAverage.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    userLessonAverage.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    userLessonAverage.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    userLessonAverage.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    userLessonAverage.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    userLessonAverage.UpdatedAt = null;
                                response.Add(userLessonAverage.Id, userLessonAverage);
                                userLessonAverage = new UserLessonAverageModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserLessonAverageModel>();
                        reader.Close();
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserLessonAverageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserLessonAverageModel> ReadD(string connectionString, UserLessonAverageModel userLessonAverage, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserLessonAverageModel> response = new Dictionary<string, UserLessonAverageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `lesson_id`, `average`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM user_lesson_averages ";
                {
                    if (userLessonAverage != null)
                    {
                        if (!string.IsNullOrEmpty(userLessonAverage.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", userLessonAverage.Id);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userLessonAverage.UserId);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", userLessonAverage.LessonId);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.Average))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `average` = @average ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `average` = @average ");
                            }
                            cmd.Parameters.AddWithValue("@average", userLessonAverage.Average);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.Completed))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `completed` = @completed ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `completed` = @completed ");
                            }
                            cmd.Parameters.AddWithValue("@completed", userLessonAverage.Completed);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", userLessonAverage.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", userLessonAverage.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", userLessonAverage.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        if (firstOne)
                            sql = string.Concat(sql, " LIMIT 1 ");
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    userLessonAverage.Id = reader.GetValue(0).ToString();
                                else
                                    userLessonAverage.Id = null;
                                if (!reader.IsDBNull(1))
                                    userLessonAverage.UserId = reader.GetValue(1).ToString();
                                else
                                    userLessonAverage.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userLessonAverage.LessonId = reader.GetValue(2).ToString();
                                else
                                    userLessonAverage.LessonId = null;
                                if (!reader.IsDBNull(3))
                                    userLessonAverage.Average = reader.GetValue(3).ToString();
                                else
                                    userLessonAverage.Average = null;
                                if (!reader.IsDBNull(4))
                                    userLessonAverage.Completed = reader.GetValue(4).ToString();
                                else
                                    userLessonAverage.Completed = null;
                                if (!reader.IsDBNull(5))
                                    userLessonAverage.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    userLessonAverage.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    userLessonAverage.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    userLessonAverage.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    userLessonAverage.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    userLessonAverage.UpdatedAt = null;
                                response.Add(userLessonAverage.Id, userLessonAverage);
                                userLessonAverage = new UserLessonAverageModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserLessonAverageModel>();
                        reader.Close();
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserLessonAverageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserLessonAverageModel> ReadD(string connectionString, UserLessonAverageModel userLessonAverage, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserLessonAverageModel> response = new Dictionary<string, UserLessonAverageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `lesson_id`, `average`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM user_lesson_averages ";
                {
                    if (userLessonAverage != null)
                    {
                        if (!string.IsNullOrEmpty(userLessonAverage.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", userLessonAverage.Id);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userLessonAverage.UserId);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", userLessonAverage.LessonId);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.Average))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `average` = @average ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `average` = @average ");
                            }
                            cmd.Parameters.AddWithValue("@average", userLessonAverage.Average);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.Completed))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `completed` = @completed ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `completed` = @completed ");
                            }
                            cmd.Parameters.AddWithValue("@completed", userLessonAverage.Completed);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", userLessonAverage.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", userLessonAverage.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", userLessonAverage.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                        cmd.Parameters.AddWithValue("@fromRow", fromRow);
                        cmd.Parameters.AddWithValue("@limit", limit);
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    userLessonAverage.Id = reader.GetValue(0).ToString();
                                else
                                    userLessonAverage.Id = null;
                                if (!reader.IsDBNull(1))
                                    userLessonAverage.UserId = reader.GetValue(1).ToString();
                                else
                                    userLessonAverage.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userLessonAverage.LessonId = reader.GetValue(2).ToString();
                                else
                                    userLessonAverage.LessonId = null;
                                if (!reader.IsDBNull(3))
                                    userLessonAverage.Average = reader.GetValue(3).ToString();
                                else
                                    userLessonAverage.Average = null;
                                if (!reader.IsDBNull(4))
                                    userLessonAverage.Completed = reader.GetValue(4).ToString();
                                else
                                    userLessonAverage.Completed = null;
                                if (!reader.IsDBNull(5))
                                    userLessonAverage.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    userLessonAverage.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    userLessonAverage.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    userLessonAverage.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    userLessonAverage.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    userLessonAverage.UpdatedAt = null;
                                response.Add(userLessonAverage.Id, userLessonAverage);
                                userLessonAverage = new UserLessonAverageModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserLessonAverageModel>();
                        reader.Close();
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserLessonAverageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserLessonAverageModel> ReadD(string connectionString, List<UserLessonAverageModel> userLessonAverageWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserLessonAverageModel> response = new Dictionary<string, UserLessonAverageModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `lesson_id`, `average`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM user_lesson_averages ";
                if (userLessonAverageWhere != null && userLessonAverageWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userLessonAverageWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userLessonAverageWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userLessonAverageWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, userLessonAverageWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].Average))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `average` = @average" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@average" + i, userLessonAverageWhere[i].Average);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].Completed))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `completed` = @completed" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@completed" + i, userLessonAverageWhere[i].Completed);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userLessonAverageWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userLessonAverageWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userLessonAverageWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userLessonAverageWhere.Count - 1)
                            sqlIn += " OR ";
                    }
                }
                cmd.CommandText = sql + " " + sqlIn;
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            UserLessonAverageModel userLessonAverage = new UserLessonAverageModel(); if (!reader.IsDBNull(0))
                                userLessonAverage.Id = reader.GetValue(0).ToString();
                            else
                                userLessonAverage.Id = null;
                            if (!reader.IsDBNull(1))
                                userLessonAverage.UserId = reader.GetValue(1).ToString();
                            else
                                userLessonAverage.UserId = null;
                            if (!reader.IsDBNull(2))
                                userLessonAverage.LessonId = reader.GetValue(2).ToString();
                            else
                                userLessonAverage.LessonId = null;
                            if (!reader.IsDBNull(3))
                                userLessonAverage.Average = reader.GetValue(3).ToString();
                            else
                                userLessonAverage.Average = null;
                            if (!reader.IsDBNull(4))
                                userLessonAverage.Completed = reader.GetValue(4).ToString();
                            else
                                userLessonAverage.Completed = null;
                            if (!reader.IsDBNull(5))
                                userLessonAverage.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                userLessonAverage.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                userLessonAverage.CreatedAt = reader.GetValue(6).ToString();
                            else
                                userLessonAverage.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                userLessonAverage.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                userLessonAverage.UpdatedAt = null;
                            response.Add(userLessonAverage.Id, userLessonAverage);
                            userLessonAverage = new UserLessonAverageModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserLessonAverageModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserLessonAverageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserLessonAverageModel> ReadD(string connectionString, List<UserLessonAverageModel> userLessonAverageWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserLessonAverageModel> response = new Dictionary<string, UserLessonAverageModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `lesson_id`, `average`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM user_lesson_averages ";
                if (userLessonAverageWhere != null && userLessonAverageWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userLessonAverageWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userLessonAverageWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userLessonAverageWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, userLessonAverageWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].Average))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `average` = @average" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@average" + i, userLessonAverageWhere[i].Average);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].Completed))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `completed` = @completed" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@completed" + i, userLessonAverageWhere[i].Completed);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userLessonAverageWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userLessonAverageWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userLessonAverageWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userLessonAverageWhere.Count - 1)
                            sqlIn += " OR ";
                    }
                }
                cmd.CommandText = sql + " " + sqlIn + " LIMIT @fromRow, @limit ";
                cmd.Parameters.AddWithValue("@fromRow", fromRow);
                cmd.Parameters.AddWithValue("@limit", limit);
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            UserLessonAverageModel userLessonAverage = new UserLessonAverageModel(); if (!reader.IsDBNull(0))
                                userLessonAverage.Id = reader.GetValue(0).ToString();
                            else
                                userLessonAverage.Id = null;
                            if (!reader.IsDBNull(1))
                                userLessonAverage.UserId = reader.GetValue(1).ToString();
                            else
                                userLessonAverage.UserId = null;
                            if (!reader.IsDBNull(2))
                                userLessonAverage.LessonId = reader.GetValue(2).ToString();
                            else
                                userLessonAverage.LessonId = null;
                            if (!reader.IsDBNull(3))
                                userLessonAverage.Average = reader.GetValue(3).ToString();
                            else
                                userLessonAverage.Average = null;
                            if (!reader.IsDBNull(4))
                                userLessonAverage.Completed = reader.GetValue(4).ToString();
                            else
                                userLessonAverage.Completed = null;
                            if (!reader.IsDBNull(5))
                                userLessonAverage.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                userLessonAverage.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                userLessonAverage.CreatedAt = reader.GetValue(6).ToString();
                            else
                                userLessonAverage.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                userLessonAverage.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                userLessonAverage.UpdatedAt = null;
                            response.Add(userLessonAverage.Id, userLessonAverage);
                            userLessonAverage = new UserLessonAverageModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserLessonAverageModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserLessonAverageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserLessonAverageModel> Read(string connectionString, UserLessonAverageModel userLessonAverage, bool firstOne, bool orderByLastDate, UserLessonAverageModel userLessonAverageWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLessonAverageModel> response = new List<UserLessonAverageModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `lesson_id`, `average`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM user_lesson_averages ";
                {
                    if (userLessonAverage != null)
                    {
                        if (!String.IsNullOrEmpty(userLessonAverage.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", userLessonAverage.Id);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userLessonAverage.UserId);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", userLessonAverage.LessonId);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.Average))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `average` = @average ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `average` = @average ");
                            }
                            cmd.Parameters.AddWithValue("@average", userLessonAverage.Average);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.Completed))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `completed` = @completed ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `completed` = @completed ");
                            }
                            cmd.Parameters.AddWithValue("@completed", userLessonAverage.Completed);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", userLessonAverage.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", userLessonAverage.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", userLessonAverage.UpdatedAt);
                        }
                    }
                    if (userLessonAverageWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userLessonAverageWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userLessonAverageWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.LessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + userLessonAverageWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.Average))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `average` LIKE @averageWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `average` LIKE @averageWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@averageWhereLike", "%" + userLessonAverageWhereLike.Average + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.Completed))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `completed` LIKE @completedWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `completed` LIKE @completedWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@completedWhereLike", "%" + userLessonAverageWhereLike.Completed + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userLessonAverageWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userLessonAverageWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userLessonAverageWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = String.Concat(sql, " ORDER BY updated_at DESC");
                    if (firstOne)
                        sql = String.Concat(sql, " LIMIT 1 ");
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                userLessonAverage = new UserLessonAverageModel();
                                if (!reader.IsDBNull(0))
                                    userLessonAverage.Id = reader.GetValue(0).ToString();
                                else
                                    userLessonAverage.Id = null;
                                if (!reader.IsDBNull(1))
                                    userLessonAverage.UserId = reader.GetValue(1).ToString();
                                else
                                    userLessonAverage.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userLessonAverage.LessonId = reader.GetValue(2).ToString();
                                else
                                    userLessonAverage.LessonId = null;
                                if (!reader.IsDBNull(3))
                                    userLessonAverage.Average = reader.GetValue(3).ToString();
                                else
                                    userLessonAverage.Average = null;
                                if (!reader.IsDBNull(4))
                                    userLessonAverage.Completed = reader.GetValue(4).ToString();
                                else
                                    userLessonAverage.Completed = null;
                                if (!reader.IsDBNull(5))
                                    userLessonAverage.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    userLessonAverage.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    userLessonAverage.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    userLessonAverage.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    userLessonAverage.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    userLessonAverage.UpdatedAt = null;
                                response.Add(userLessonAverage);
                                userLessonAverage = new UserLessonAverageModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserLessonAverageModel>();
                        reader.Close();
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserLessonAverageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserLessonAverageModel> Read(string connectionString, UserLessonAverageModel userLessonAverage, bool orderByLastDate, int fromRow, int limit, UserLessonAverageModel userLessonAverageWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLessonAverageModel> response = new List<UserLessonAverageModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `lesson_id`, `average`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM user_lesson_averages ";
                {
                    if (userLessonAverage != null)
                    {
                        if (!String.IsNullOrEmpty(userLessonAverage.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", userLessonAverage.Id);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userLessonAverage.UserId);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", userLessonAverage.LessonId);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.Average))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `average` = @average ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `average` = @average ");
                            }
                            cmd.Parameters.AddWithValue("@average", userLessonAverage.Average);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.Completed))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `completed` = @completed ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `completed` = @completed ");
                            }
                            cmd.Parameters.AddWithValue("@completed", userLessonAverage.Completed);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", userLessonAverage.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", userLessonAverage.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverage.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", userLessonAverage.UpdatedAt);
                        }
                    }
                    if (userLessonAverageWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.Id))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `id` LIKE @idWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `id` LIKE @idWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + userLessonAverageWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.UserId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `user_id` LIKE @userIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `user_id` LIKE @userIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@userIdWhereLike", "%" + userLessonAverageWhereLike.UserId + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.LessonId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `lesson_id` LIKE @lessonIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@lessonIdWhereLike", "%" + userLessonAverageWhereLike.LessonId + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.Average))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `average` LIKE @averageWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `average` LIKE @averageWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@averageWhereLike", "%" + userLessonAverageWhereLike.Average + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.Completed))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `completed` LIKE @completedWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `completed` LIKE @completedWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@completedWhereLike", "%" + userLessonAverageWhereLike.Completed + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.ModifiedBy))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `modified_by` LIKE @modifiedByWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + userLessonAverageWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.CreatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `created_at` LIKE @createdAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `created_at` LIKE @createdAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + userLessonAverageWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhereLike.UpdatedAt))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `updated_at` LIKE @updatedAtWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + userLessonAverageWhereLike.UpdatedAt + "%");
                        }
                    }
                    if (orderByLastDate)
                        sql = String.Concat(sql, " ORDER BY updated_at DESC");
                    sql = String.Concat(sql, " LIMIT @fromRow, @limit");
                    cmd.Parameters.AddWithValue("@fromRow", fromRow);
                    cmd.Parameters.AddWithValue("@limit", limit);
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                userLessonAverage = new UserLessonAverageModel();
                                if (!reader.IsDBNull(0))
                                    userLessonAverage.Id = reader.GetValue(0).ToString();
                                else
                                    userLessonAverage.Id = null;
                                if (!reader.IsDBNull(1))
                                    userLessonAverage.UserId = reader.GetValue(1).ToString();
                                else
                                    userLessonAverage.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userLessonAverage.LessonId = reader.GetValue(2).ToString();
                                else
                                    userLessonAverage.LessonId = null;
                                if (!reader.IsDBNull(3))
                                    userLessonAverage.Average = reader.GetValue(3).ToString();
                                else
                                    userLessonAverage.Average = null;
                                if (!reader.IsDBNull(4))
                                    userLessonAverage.Completed = reader.GetValue(4).ToString();
                                else
                                    userLessonAverage.Completed = null;
                                if (!reader.IsDBNull(5))
                                    userLessonAverage.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    userLessonAverage.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    userLessonAverage.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    userLessonAverage.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    userLessonAverage.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    userLessonAverage.UpdatedAt = null;
                                response.Add(userLessonAverage);
                                userLessonAverage = new UserLessonAverageModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserLessonAverageModel>();
                        reader.Close();
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserLessonAverageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserLessonAverageModel> Read(string connectionString, UserLessonAverageModel userLessonAverage, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLessonAverageModel> response = new List<UserLessonAverageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `lesson_id`, `average`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM user_lesson_averages ";
                {
                    if (userLessonAverage != null)
                    {
                        if (!string.IsNullOrEmpty(userLessonAverage.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", userLessonAverage.Id);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userLessonAverage.UserId);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", userLessonAverage.LessonId);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.Average))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `average` = @average ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `average` = @average ");
                            }
                            cmd.Parameters.AddWithValue("@average", userLessonAverage.Average);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.Completed))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `completed` = @completed ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `completed` = @completed ");
                            }
                            cmd.Parameters.AddWithValue("@completed", userLessonAverage.Completed);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", userLessonAverage.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", userLessonAverage.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", userLessonAverage.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        if (firstOne)
                            sql = string.Concat(sql, " LIMIT 1 ");
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    userLessonAverage.Id = reader.GetValue(0).ToString();
                                else
                                    userLessonAverage.Id = null;
                                if (!reader.IsDBNull(1))
                                    userLessonAverage.UserId = reader.GetValue(1).ToString();
                                else
                                    userLessonAverage.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userLessonAverage.LessonId = reader.GetValue(2).ToString();
                                else
                                    userLessonAverage.LessonId = null;
                                if (!reader.IsDBNull(3))
                                    userLessonAverage.Average = reader.GetValue(3).ToString();
                                else
                                    userLessonAverage.Average = null;
                                if (!reader.IsDBNull(4))
                                    userLessonAverage.Completed = reader.GetValue(4).ToString();
                                else
                                    userLessonAverage.Completed = null;
                                if (!reader.IsDBNull(5))
                                    userLessonAverage.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    userLessonAverage.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    userLessonAverage.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    userLessonAverage.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    userLessonAverage.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    userLessonAverage.UpdatedAt = null;
                                response.Add(userLessonAverage);
                                userLessonAverage = new UserLessonAverageModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserLessonAverageModel>();
                        reader.Close();
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserLessonAverageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserLessonAverageModel> Read(string connectionString, UserLessonAverageModel userLessonAverage, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLessonAverageModel> response = new List<UserLessonAverageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `lesson_id`, `average`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM user_lesson_averages ";
                {
                    if (userLessonAverage != null)
                    {
                        if (!string.IsNullOrEmpty(userLessonAverage.Id))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `id` = @id ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `id` = @id ");
                            }
                            cmd.Parameters.AddWithValue("@id", userLessonAverage.Id);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.UserId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `user_id` = @userId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `user_id` = @userId ");
                            }
                            cmd.Parameters.AddWithValue("@userId", userLessonAverage.UserId);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.LessonId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `lesson_id` = @lessonId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `lesson_id` = @lessonId ");
                            }
                            cmd.Parameters.AddWithValue("@lessonId", userLessonAverage.LessonId);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.Average))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `average` = @average ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `average` = @average ");
                            }
                            cmd.Parameters.AddWithValue("@average", userLessonAverage.Average);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.Completed))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `completed` = @completed ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `completed` = @completed ");
                            }
                            cmd.Parameters.AddWithValue("@completed", userLessonAverage.Completed);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.ModifiedBy))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `modified_by` = @modifiedBy ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `modified_by` = @modifiedBy ");
                            }
                            cmd.Parameters.AddWithValue("@modifiedBy", userLessonAverage.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.CreatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `created_at` = @createdAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `created_at` = @createdAt ");
                            }
                            cmd.Parameters.AddWithValue("@createdAt", userLessonAverage.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userLessonAverage.UpdatedAt))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `updated_at` = @updatedAt ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `updated_at` = @updatedAt ");
                            }
                            cmd.Parameters.AddWithValue("@updatedAt", userLessonAverage.UpdatedAt);
                        }
                        if (orderByLastDate)
                            sql = string.Concat(sql, " ORDER BY updated_at DESC");
                        sql = string.Concat(sql, " LIMIT @fromRow, @limit");
                        cmd.Parameters.AddWithValue("@fromRow", fromRow);
                        cmd.Parameters.AddWithValue("@limit", limit);
                    }
                    cmd.CommandText = sql;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    userLessonAverage.Id = reader.GetValue(0).ToString();
                                else
                                    userLessonAverage.Id = null;
                                if (!reader.IsDBNull(1))
                                    userLessonAverage.UserId = reader.GetValue(1).ToString();
                                else
                                    userLessonAverage.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userLessonAverage.LessonId = reader.GetValue(2).ToString();
                                else
                                    userLessonAverage.LessonId = null;
                                if (!reader.IsDBNull(3))
                                    userLessonAverage.Average = reader.GetValue(3).ToString();
                                else
                                    userLessonAverage.Average = null;
                                if (!reader.IsDBNull(4))
                                    userLessonAverage.Completed = reader.GetValue(4).ToString();
                                else
                                    userLessonAverage.Completed = null;
                                if (!reader.IsDBNull(5))
                                    userLessonAverage.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    userLessonAverage.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    userLessonAverage.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    userLessonAverage.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    userLessonAverage.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    userLessonAverage.UpdatedAt = null;
                                response.Add(userLessonAverage);
                                userLessonAverage = new UserLessonAverageModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserLessonAverageModel>();
                        reader.Close();
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserLessonAverageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserLessonAverageModel> Read(string connectionString, List<UserLessonAverageModel> userLessonAverageWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLessonAverageModel> response = new List<UserLessonAverageModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `lesson_id`, `average`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM user_lesson_averages ";
                if (userLessonAverageWhere != null && userLessonAverageWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userLessonAverageWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userLessonAverageWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userLessonAverageWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, userLessonAverageWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].Average))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `average` = @average" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@average" + i, userLessonAverageWhere[i].Average);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].Completed))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `completed` = @completed" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@completed" + i, userLessonAverageWhere[i].Completed);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userLessonAverageWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userLessonAverageWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userLessonAverageWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userLessonAverageWhere.Count - 1)
                            sqlIn += " OR ";
                    }
                }
                cmd.CommandText = sql + " " + sqlIn;
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            UserLessonAverageModel userLessonAverage = new UserLessonAverageModel(); if (!reader.IsDBNull(0))
                                userLessonAverage.Id = reader.GetValue(0).ToString();
                            else
                                userLessonAverage.Id = null;
                            if (!reader.IsDBNull(1))
                                userLessonAverage.UserId = reader.GetValue(1).ToString();
                            else
                                userLessonAverage.UserId = null;
                            if (!reader.IsDBNull(2))
                                userLessonAverage.LessonId = reader.GetValue(2).ToString();
                            else
                                userLessonAverage.LessonId = null;
                            if (!reader.IsDBNull(3))
                                userLessonAverage.Average = reader.GetValue(3).ToString();
                            else
                                userLessonAverage.Average = null;
                            if (!reader.IsDBNull(4))
                                userLessonAverage.Completed = reader.GetValue(4).ToString();
                            else
                                userLessonAverage.Completed = null;
                            if (!reader.IsDBNull(5))
                                userLessonAverage.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                userLessonAverage.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                userLessonAverage.CreatedAt = reader.GetValue(6).ToString();
                            else
                                userLessonAverage.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                userLessonAverage.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                userLessonAverage.UpdatedAt = null;
                            response.Add(userLessonAverage);
                            userLessonAverage = new UserLessonAverageModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserLessonAverageModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserLessonAverageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserLessonAverageModel> Read(string connectionString, List<UserLessonAverageModel> userLessonAverageWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLessonAverageModel> response = new List<UserLessonAverageModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `lesson_id`, `average`, `completed`, `modified_by`, `created_at`, `updated_at`  FROM user_lesson_averages ";
                if (userLessonAverageWhere != null && userLessonAverageWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userLessonAverageWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userLessonAverageWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userLessonAverageWhere[i].UserId);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].LessonId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `lesson_id` = @lessonId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@lessonId" + i, userLessonAverageWhere[i].LessonId);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].Average))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `average` = @average" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@average" + i, userLessonAverageWhere[i].Average);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].Completed))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `completed` = @completed" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@completed" + i, userLessonAverageWhere[i].Completed);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userLessonAverageWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userLessonAverageWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(userLessonAverageWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userLessonAverageWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userLessonAverageWhere.Count - 1)
                            sqlIn += " OR ";
                    }
                }
                cmd.CommandText = sql + " " + sqlIn + " LIMIT @fromRow, @limit ";
                cmd.Parameters.AddWithValue("@fromRow", fromRow);
                cmd.Parameters.AddWithValue("@limit", limit);
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            UserLessonAverageModel userLessonAverage = new UserLessonAverageModel(); if (!reader.IsDBNull(0))
                                userLessonAverage.Id = reader.GetValue(0).ToString();
                            else
                                userLessonAverage.Id = null;
                            if (!reader.IsDBNull(1))
                                userLessonAverage.UserId = reader.GetValue(1).ToString();
                            else
                                userLessonAverage.UserId = null;
                            if (!reader.IsDBNull(2))
                                userLessonAverage.LessonId = reader.GetValue(2).ToString();
                            else
                                userLessonAverage.LessonId = null;
                            if (!reader.IsDBNull(3))
                                userLessonAverage.Average = reader.GetValue(3).ToString();
                            else
                                userLessonAverage.Average = null;
                            if (!reader.IsDBNull(4))
                                userLessonAverage.Completed = reader.GetValue(4).ToString();
                            else
                                userLessonAverage.Completed = null;
                            if (!reader.IsDBNull(5))
                                userLessonAverage.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                userLessonAverage.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                userLessonAverage.CreatedAt = reader.GetValue(6).ToString();
                            else
                                userLessonAverage.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                userLessonAverage.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                userLessonAverage.UpdatedAt = null;
                            response.Add(userLessonAverage);
                            userLessonAverage = new UserLessonAverageModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserLessonAverageModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserLessonAverageModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserLessonAverageModel> Create(string connectionString, UserLessonAverageModel userLessonAverage)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLessonAverageModel> response = new List<UserLessonAverageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO user_lesson_averages (";
                string values = "(";
                if (!String.IsNullOrEmpty(userLessonAverage.Id))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `id` ");
                        values = String.Concat(values, ", @id");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `id` ");
                        values = String.Concat(values, " @id");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userLessonAverage.Id);
                }
                if (!String.IsNullOrEmpty(userLessonAverage.UserId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `user_id` ");
                        values = String.Concat(values, ", @userId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `user_id` ");
                        values = String.Concat(values, " @userId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userLessonAverage.UserId);
                }
                if (!String.IsNullOrEmpty(userLessonAverage.LessonId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `lesson_id` ");
                        values = String.Concat(values, ", @lessonId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `lesson_id` ");
                        values = String.Concat(values, " @lessonId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@lessonId", userLessonAverage.LessonId);
                }
                if (!String.IsNullOrEmpty(userLessonAverage.Average))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `average` ");
                        values = String.Concat(values, ", @average");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `average` ");
                        values = String.Concat(values, " @average");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@average", userLessonAverage.Average);
                }
                if (!String.IsNullOrEmpty(userLessonAverage.Completed))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `completed` ");
                        values = String.Concat(values, ", @completed");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `completed` ");
                        values = String.Concat(values, " @completed");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@completed", userLessonAverage.Completed);
                }
                if (!String.IsNullOrEmpty(userLessonAverage.ModifiedBy))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `modified_by` ");
                        values = String.Concat(values, ", @modifiedBy");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `modified_by` ");
                        values = String.Concat(values, " @modifiedBy");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userLessonAverage.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userLessonAverage.CreatedAt))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `created_at` ");
                        values = String.Concat(values, ", @createdAt");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `created_at` ");
                        values = String.Concat(values, " @createdAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userLessonAverage.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userLessonAverage.UpdatedAt))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `updated_at` ");
                        values = String.Concat(values, ", @updatedAt");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `updated_at` ");
                        values = String.Concat(values, " @updatedAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userLessonAverage.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userLessonAverage.Id = cmd.LastInsertedId.ToString();
                response.Add(userLessonAverage);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserLessonAverageModel>();
            }
            finally
            {
                if (_connection != null)
                {
                    dbConn.CloseConnection(_connection);
                }
            }
            return response;
        }
        public List<UserLessonAverageModel> Create(string connectionString, List<UserLessonAverageModel> userLessonAverageList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLessonAverageModel> response = new List<UserLessonAverageModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userLessonAverageList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO user_lesson_averages (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(userLessonAverageList[i].Id))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `id` ");
                                        values = String.Concat(values, ", @id" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `id` ");
                                        values = String.Concat(values, "@id" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userLessonAverageList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userLessonAverageList[i].UserId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `user_id` ");
                                        values = String.Concat(values, ", @userId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `user_id` ");
                                        values = String.Concat(values, "@userId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userLessonAverageList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userLessonAverageList[i].LessonId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `lesson_id` ");
                                        values = String.Concat(values, ", @lessonId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `lesson_id` ");
                                        values = String.Concat(values, "@lessonId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@lessonId" + i, userLessonAverageList[i].LessonId);
                                }
                                if (!String.IsNullOrEmpty(userLessonAverageList[i].Average))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `average` ");
                                        values = String.Concat(values, ", @average" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `average` ");
                                        values = String.Concat(values, "@average" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@average" + i, userLessonAverageList[i].Average);
                                }
                                if (!String.IsNullOrEmpty(userLessonAverageList[i].Completed))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `completed` ");
                                        values = String.Concat(values, ", @completed" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `completed` ");
                                        values = String.Concat(values, "@completed" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@completed" + i, userLessonAverageList[i].Completed);
                                }
                                if (!String.IsNullOrEmpty(userLessonAverageList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `modified_by` ");
                                        values = String.Concat(values, ", @modifiedBy" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `modified_by` ");
                                        values = String.Concat(values, "@modifiedBy" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userLessonAverageList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userLessonAverageList[i].CreatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `created_at` ");
                                        values = String.Concat(values, ", @createdAt" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `created_at` ");
                                        values = String.Concat(values, "@createdAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userLessonAverageList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userLessonAverageList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `updated_at` ");
                                        values = String.Concat(values, ", @updatedAt" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `updated_at` ");
                                        values = String.Concat(values, "@updatedAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userLessonAverageList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userLessonAverageList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserLessonAverageModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserLessonAverageModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserLessonAverageModel>();
            }
            finally
            {
                if (_connection != null)
                {
                    dbConn.CloseConnection(_connection);
                }
            }
            return response;
        }

        public List<UserLessonAverageModel> Update(string connectionString, UserLessonAverageModel userLessonAverage, UserLessonAverageModel userLessonAverageWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLessonAverageModel> response = new List<UserLessonAverageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE user_lesson_averages SET ";
                if (!String.IsNullOrEmpty(userLessonAverage.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userLessonAverage.Id);
                }
                if (!String.IsNullOrEmpty(userLessonAverage.UserId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = String.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userLessonAverage.UserId);
                }
                if (!String.IsNullOrEmpty(userLessonAverage.LessonId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `lesson_id` = @lessonId");
                    else
                        sql = String.Concat(sql, " `lesson_id` = @lessonId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@lessonId", userLessonAverage.LessonId);
                }
                if (!String.IsNullOrEmpty(userLessonAverage.Average))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `average` = @average");
                    else
                        sql = String.Concat(sql, " `average` = @average");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@average", userLessonAverage.Average);
                }
                if (!String.IsNullOrEmpty(userLessonAverage.Completed))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `completed` = @completed");
                    else
                        sql = String.Concat(sql, " `completed` = @completed");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@completed", userLessonAverage.Completed);
                }
                if (!String.IsNullOrEmpty(userLessonAverage.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userLessonAverage.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(userLessonAverage.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userLessonAverage.CreatedAt);
                }
                if (!String.IsNullOrEmpty(userLessonAverage.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userLessonAverage.UpdatedAt);
                }
                if (userLessonAverageWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(userLessonAverageWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userLessonAverageWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(userLessonAverageWhere.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = String.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userLessonAverageWhere.UserId);
                    }
                    if (!String.IsNullOrEmpty(userLessonAverageWhere.LessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lesson_id` = @lessonIdWhere");
                        else
                            sql = String.Concat(sql, " `lesson_id` = @lessonIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lessonIdWhere", userLessonAverageWhere.LessonId);
                    }
                    if (!String.IsNullOrEmpty(userLessonAverageWhere.Average))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `average` = @averageWhere");
                        else
                            sql = String.Concat(sql, " `average` = @averageWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@averageWhere", userLessonAverageWhere.Average);
                    }
                    if (!String.IsNullOrEmpty(userLessonAverageWhere.Completed))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `completed` = @completedWhere");
                        else
                            sql = String.Concat(sql, " `completed` = @completedWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@completedWhere", userLessonAverageWhere.Completed);
                    }
                    if (!String.IsNullOrEmpty(userLessonAverageWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userLessonAverageWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userLessonAverageWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userLessonAverageWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userLessonAverageWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userLessonAverageWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userLessonAverage);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserLessonAverageModel>();
            }
            finally
            {
                if (_connection != null)
                {
                    dbConn.CloseConnection(_connection);
                }
            }
            return response;
        }

        public List<UserLessonAverageModel> Update(string connectionString, List<UserLessonAverageModel> userLessonAverageList, List<UserLessonAverageModel> userLessonAverageWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLessonAverageModel> response = new List<UserLessonAverageModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userLessonAverageList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE user_lesson_averages SET ";
                                if (!String.IsNullOrEmpty(userLessonAverageList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userLessonAverageList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(userLessonAverageList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = String.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userLessonAverageList[i].UserId);
                                }
                                if (!String.IsNullOrEmpty(userLessonAverageList[i].LessonId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `lesson_id` = @lessonId" + i);
                                    else
                                        sql = String.Concat(sql, " `lesson_id` = @lessonId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@lessonId" + i, userLessonAverageList[i].LessonId);
                                }
                                if (!String.IsNullOrEmpty(userLessonAverageList[i].Average))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `average` = @average" + i);
                                    else
                                        sql = String.Concat(sql, " `average` = @average" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@average" + i, userLessonAverageList[i].Average);
                                }
                                if (!String.IsNullOrEmpty(userLessonAverageList[i].Completed))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `completed` = @completed" + i);
                                    else
                                        sql = String.Concat(sql, " `completed` = @completed" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@completed" + i, userLessonAverageList[i].Completed);
                                }
                                if (!String.IsNullOrEmpty(userLessonAverageList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userLessonAverageList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(userLessonAverageList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userLessonAverageList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(userLessonAverageList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userLessonAverageList[i].UpdatedAt);
                                }
                                if (userLessonAverageWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(userLessonAverageWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userLessonAverageWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userLessonAverageWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userLessonAverageWhereList[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userLessonAverageWhereList[i].LessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lesson_id` = @lessonIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `lesson_id` = @lessonIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lessonIdWhere" + i, userLessonAverageWhereList[i].LessonId);
                                    }
                                    if (!String.IsNullOrEmpty(userLessonAverageWhereList[i].Average))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `average` = @averageWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `average` = @averageWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@averageWhere" + i, userLessonAverageWhereList[i].Average);
                                    }
                                    if (!String.IsNullOrEmpty(userLessonAverageWhereList[i].Completed))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `completed` = @completedWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `completed` = @completedWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@completedWhere" + i, userLessonAverageWhereList[i].Completed);
                                    }
                                    if (!String.IsNullOrEmpty(userLessonAverageWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userLessonAverageWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userLessonAverageWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userLessonAverageWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userLessonAverageWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userLessonAverageWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userLessonAverageList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserLessonAverageModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserLessonAverageModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserLessonAverageModel>();
            }
            finally
            {
                {
                    if (_connection != null)
                    {
                        dbConn.CloseConnection(_connection);
                    }
                }
            }
            return response;
        }

        public List<UserLessonAverageModel> Delete(string connectionString, UserLessonAverageModel userLessonAverage)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLessonAverageModel> response = new List<UserLessonAverageModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM user_lesson_averages  ";
                if (userLessonAverage != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(userLessonAverage.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userLessonAverage.Id);
                    }
                    if (!String.IsNullOrEmpty(userLessonAverage.UserId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = String.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userLessonAverage.UserId);
                    }
                    if (!String.IsNullOrEmpty(userLessonAverage.LessonId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `lesson_id` = @lessonId");
                        else
                            sql = String.Concat(sql, " `lesson_id` = @lessonId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@lessonId", userLessonAverage.LessonId);
                    }
                    if (!String.IsNullOrEmpty(userLessonAverage.Average))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `average` = @average");
                        else
                            sql = String.Concat(sql, " `average` = @average");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@average", userLessonAverage.Average);
                    }
                    if (!String.IsNullOrEmpty(userLessonAverage.Completed))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `completed` = @completed");
                        else
                            sql = String.Concat(sql, " `completed` = @completed");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@completed", userLessonAverage.Completed);
                    }
                    if (!String.IsNullOrEmpty(userLessonAverage.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userLessonAverage.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(userLessonAverage.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userLessonAverage.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(userLessonAverage.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userLessonAverage.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userLessonAverage);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserLessonAverageModel>();
            }
            finally
            {
                if (_connection != null)
                {
                    dbConn.CloseConnection(_connection);
                }
            }
            return response;
        }

        public List<UserLessonAverageModel> Delete(string connectionString, List<UserLessonAverageModel> userLessonAverage)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserLessonAverageModel> response = new List<UserLessonAverageModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userLessonAverage.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM user_lesson_averages  ";
                                if (userLessonAverage[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(userLessonAverage[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userLessonAverage[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(userLessonAverage[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = String.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userLessonAverage[i].UserId);
                                    }
                                    if (!String.IsNullOrEmpty(userLessonAverage[i].LessonId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `lesson_id` = @lessonId" + i);
                                        else
                                            sql = String.Concat(sql, " `lesson_id` = @lessonId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@lessonId" + i, userLessonAverage[i].LessonId);
                                    }
                                    if (!String.IsNullOrEmpty(userLessonAverage[i].Average))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `average` = @average" + i);
                                        else
                                            sql = String.Concat(sql, " `average` = @average" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@average" + i, userLessonAverage[i].Average);
                                    }
                                    if (!String.IsNullOrEmpty(userLessonAverage[i].Completed))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `completed` = @completed" + i);
                                        else
                                            sql = String.Concat(sql, " `completed` = @completed" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@completed" + i, userLessonAverage[i].Completed);
                                    }
                                    if (!String.IsNullOrEmpty(userLessonAverage[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userLessonAverage[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(userLessonAverage[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userLessonAverage[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(userLessonAverage[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userLessonAverage[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userLessonAverage[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserLessonAverageModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserLessonAverageModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserLessonAverageModel>();
            }
            finally
            {
                {
                    if (_connection != null)
                    {
                        dbConn.CloseConnection(_connection);
                    }
                }
            }
            return response;
        }
    }
}
