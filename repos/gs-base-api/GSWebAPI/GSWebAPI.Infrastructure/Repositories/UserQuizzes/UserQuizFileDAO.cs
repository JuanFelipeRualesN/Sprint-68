using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Services.UserQuizzes.CustomClasses;
using GSWebAPI.GSWebAPI.Domain.Models.UserQuiz;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.UserQuizzes
{
    class UserQuizFileDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, UserQuizFileModel> ReadD(string connectionString, UserQuizFileModel userQuizFile, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserQuizFileModel> response = new Dictionary<string, UserQuizFileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `quiz_id`, `file_url`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_quiz_files ";
                {
                    if (userQuizFile != null)
                    {
                        if (!string.IsNullOrEmpty(userQuizFile.Id))
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
                            cmd.Parameters.AddWithValue("@id", userQuizFile.Id);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userQuizFile.UserId);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quiz_id` = @quizId ");
                            }
                            cmd.Parameters.AddWithValue("@quizId", userQuizFile.QuizId);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.FileUrl))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `file_url` = @fileUrl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `file_url` = @fileUrl ");
                            }
                            cmd.Parameters.AddWithValue("@fileUrl", userQuizFile.FileUrl);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userQuizFile.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userQuizFile.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userQuizFile.UpdatedAt);
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
                                    userQuizFile.Id = reader.GetValue(0).ToString();
                                else
                                    userQuizFile.Id = null;
                                if (!reader.IsDBNull(1))
                                    userQuizFile.UserId = reader.GetValue(1).ToString();
                                else
                                    userQuizFile.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userQuizFile.QuizId = reader.GetValue(2).ToString();
                                else
                                    userQuizFile.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    userQuizFile.FileUrl = reader.GetValue(3).ToString();
                                else
                                    userQuizFile.FileUrl = null;
                                if (!reader.IsDBNull(4))
                                    userQuizFile.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    userQuizFile.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    userQuizFile.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    userQuizFile.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    userQuizFile.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    userQuizFile.UpdatedAt = null;
                                response.Add(userQuizFile.Id, userQuizFile);
                                userQuizFile = new UserQuizFileModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserQuizFileModel>();
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
                response = new Dictionary<string, UserQuizFileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserQuizFileModel> ReadD(string connectionString, UserQuizFileModel userQuizFile, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserQuizFileModel> response = new Dictionary<string, UserQuizFileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `quiz_id`, `file_url`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_quiz_files ";
                {
                    if (userQuizFile != null)
                    {
                        if (!string.IsNullOrEmpty(userQuizFile.Id))
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
                            cmd.Parameters.AddWithValue("@id", userQuizFile.Id);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userQuizFile.UserId);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quiz_id` = @quizId ");
                            }
                            cmd.Parameters.AddWithValue("@quizId", userQuizFile.QuizId);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.FileUrl))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `file_url` = @fileUrl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `file_url` = @fileUrl ");
                            }
                            cmd.Parameters.AddWithValue("@fileUrl", userQuizFile.FileUrl);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userQuizFile.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userQuizFile.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userQuizFile.UpdatedAt);
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
                                    userQuizFile.Id = reader.GetValue(0).ToString();
                                else
                                    userQuizFile.Id = null;
                                if (!reader.IsDBNull(1))
                                    userQuizFile.UserId = reader.GetValue(1).ToString();
                                else
                                    userQuizFile.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userQuizFile.QuizId = reader.GetValue(2).ToString();
                                else
                                    userQuizFile.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    userQuizFile.FileUrl = reader.GetValue(3).ToString();
                                else
                                    userQuizFile.FileUrl = null;
                                if (!reader.IsDBNull(4))
                                    userQuizFile.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    userQuizFile.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    userQuizFile.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    userQuizFile.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    userQuizFile.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    userQuizFile.UpdatedAt = null;
                                response.Add(userQuizFile.Id, userQuizFile);
                                userQuizFile = new UserQuizFileModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserQuizFileModel>();
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
                response = new Dictionary<string, UserQuizFileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserQuizFileModel> ReadD(string connectionString, List<UserQuizFileModel> userQuizFileWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserQuizFileModel> response = new Dictionary<string, UserQuizFileModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `quiz_id`, `file_url`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_quiz_files ";
                if (userQuizFileWhere != null && userQuizFileWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userQuizFileWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userQuizFileWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userQuizFileWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, userQuizFileWhere[i].QuizId);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].FileUrl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `file_url` = @fileUrl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@fileUrl" + i, userQuizFileWhere[i].FileUrl);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizFileWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userQuizFileWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizFileWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userQuizFileWhere.Count - 1)
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
                            UserQuizFileModel userQuizFile = new UserQuizFileModel(); if (!reader.IsDBNull(0))
                                userQuizFile.Id = reader.GetValue(0).ToString();
                            else
                                userQuizFile.Id = null;
                            if (!reader.IsDBNull(1))
                                userQuizFile.UserId = reader.GetValue(1).ToString();
                            else
                                userQuizFile.UserId = null;
                            if (!reader.IsDBNull(2))
                                userQuizFile.QuizId = reader.GetValue(2).ToString();
                            else
                                userQuizFile.QuizId = null;
                            if (!reader.IsDBNull(3))
                                userQuizFile.FileUrl = reader.GetValue(3).ToString();
                            else
                                userQuizFile.FileUrl = null;
                            if (!reader.IsDBNull(4))
                                userQuizFile.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                userQuizFile.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                userQuizFile.CreatedAt = reader.GetValue(5).ToString();
                            else
                                userQuizFile.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                userQuizFile.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                userQuizFile.UpdatedAt = null;
                            response.Add(userQuizFile.Id, userQuizFile);
                            userQuizFile = new UserQuizFileModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserQuizFileModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserQuizFileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserQuizFileModel> ReadD(string connectionString, List<UserQuizFileModel> userQuizFileWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserQuizFileModel> response = new Dictionary<string, UserQuizFileModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `quiz_id`, `file_url`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_quiz_files ";
                if (userQuizFileWhere != null && userQuizFileWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userQuizFileWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userQuizFileWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userQuizFileWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, userQuizFileWhere[i].QuizId);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].FileUrl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `file_url` = @fileUrl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@fileUrl" + i, userQuizFileWhere[i].FileUrl);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizFileWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userQuizFileWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizFileWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userQuizFileWhere.Count - 1)
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
                            UserQuizFileModel userQuizFile = new UserQuizFileModel(); if (!reader.IsDBNull(0))
                                userQuizFile.Id = reader.GetValue(0).ToString();
                            else
                                userQuizFile.Id = null;
                            if (!reader.IsDBNull(1))
                                userQuizFile.UserId = reader.GetValue(1).ToString();
                            else
                                userQuizFile.UserId = null;
                            if (!reader.IsDBNull(2))
                                userQuizFile.QuizId = reader.GetValue(2).ToString();
                            else
                                userQuizFile.QuizId = null;
                            if (!reader.IsDBNull(3))
                                userQuizFile.FileUrl = reader.GetValue(3).ToString();
                            else
                                userQuizFile.FileUrl = null;
                            if (!reader.IsDBNull(4))
                                userQuizFile.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                userQuizFile.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                userQuizFile.CreatedAt = reader.GetValue(5).ToString();
                            else
                                userQuizFile.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                userQuizFile.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                userQuizFile.UpdatedAt = null;
                            response.Add(userQuizFile.Id, userQuizFile);
                            userQuizFile = new UserQuizFileModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserQuizFileModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserQuizFileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserQuizFileModel> Read(string connectionString, UserQuizFileModel userQuizFile, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizFileModel> response = new List<UserQuizFileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `quiz_id`, `file_url`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_quiz_files ";
                {
                    if (userQuizFile != null)
                    {
                        if (!string.IsNullOrEmpty(userQuizFile.Id))
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
                            cmd.Parameters.AddWithValue("@id", userQuizFile.Id);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userQuizFile.UserId);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quiz_id` = @quizId ");
                            }
                            cmd.Parameters.AddWithValue("@quizId", userQuizFile.QuizId);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.FileUrl))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `file_url` = @fileUrl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `file_url` = @fileUrl ");
                            }
                            cmd.Parameters.AddWithValue("@fileUrl", userQuizFile.FileUrl);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userQuizFile.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userQuizFile.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userQuizFile.UpdatedAt);
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
                                    userQuizFile.Id = reader.GetValue(0).ToString();
                                else
                                    userQuizFile.Id = null;
                                if (!reader.IsDBNull(1))
                                    userQuizFile.UserId = reader.GetValue(1).ToString();
                                else
                                    userQuizFile.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userQuizFile.QuizId = reader.GetValue(2).ToString();
                                else
                                    userQuizFile.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    userQuizFile.FileUrl = reader.GetValue(3).ToString();
                                else
                                    userQuizFile.FileUrl = null;
                                if (!reader.IsDBNull(4))
                                    userQuizFile.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    userQuizFile.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    userQuizFile.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    userQuizFile.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    userQuizFile.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    userQuizFile.UpdatedAt = null;
                                response.Add(userQuizFile);
                                userQuizFile = new UserQuizFileModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserQuizFileModel>();
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
                response = new List<UserQuizFileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserQuizFileModel> Read(string connectionString, UserQuizFileModel userQuizFile, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizFileModel> response = new List<UserQuizFileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_id`, `quiz_id`, `file_url`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_quiz_files ";
                {
                    if (userQuizFile != null)
                    {
                        if (!string.IsNullOrEmpty(userQuizFile.Id))
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
                            cmd.Parameters.AddWithValue("@id", userQuizFile.Id);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userQuizFile.UserId);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quiz_id` = @quizId ");
                            }
                            cmd.Parameters.AddWithValue("@quizId", userQuizFile.QuizId);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.FileUrl))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `file_url` = @fileUrl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `file_url` = @fileUrl ");
                            }
                            cmd.Parameters.AddWithValue("@fileUrl", userQuizFile.FileUrl);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userQuizFile.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userQuizFile.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userQuizFile.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userQuizFile.UpdatedAt);
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
                                    userQuizFile.Id = reader.GetValue(0).ToString();
                                else
                                    userQuizFile.Id = null;
                                if (!reader.IsDBNull(1))
                                    userQuizFile.UserId = reader.GetValue(1).ToString();
                                else
                                    userQuizFile.UserId = null;
                                if (!reader.IsDBNull(2))
                                    userQuizFile.QuizId = reader.GetValue(2).ToString();
                                else
                                    userQuizFile.QuizId = null;
                                if (!reader.IsDBNull(3))
                                    userQuizFile.FileUrl = reader.GetValue(3).ToString();
                                else
                                    userQuizFile.FileUrl = null;
                                if (!reader.IsDBNull(4))
                                    userQuizFile.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    userQuizFile.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    userQuizFile.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    userQuizFile.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    userQuizFile.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    userQuizFile.UpdatedAt = null;
                                response.Add(userQuizFile);
                                userQuizFile = new UserQuizFileModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserQuizFileModel>();
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
                response = new List<UserQuizFileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserQuizFileModel> Read(string connectionString, List<UserQuizFileModel> userQuizFileWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizFileModel> response = new List<UserQuizFileModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `quiz_id`, `file_url`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_quiz_files ";
                if (userQuizFileWhere != null && userQuizFileWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userQuizFileWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userQuizFileWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userQuizFileWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, userQuizFileWhere[i].QuizId);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].FileUrl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `file_url` = @fileUrl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@fileUrl" + i, userQuizFileWhere[i].FileUrl);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizFileWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userQuizFileWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizFileWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userQuizFileWhere.Count - 1)
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
                            UserQuizFileModel userQuizFile = new UserQuizFileModel(); if (!reader.IsDBNull(0))
                                userQuizFile.Id = reader.GetValue(0).ToString();
                            else
                                userQuizFile.Id = null;
                            if (!reader.IsDBNull(1))
                                userQuizFile.UserId = reader.GetValue(1).ToString();
                            else
                                userQuizFile.UserId = null;
                            if (!reader.IsDBNull(2))
                                userQuizFile.QuizId = reader.GetValue(2).ToString();
                            else
                                userQuizFile.QuizId = null;
                            if (!reader.IsDBNull(3))
                                userQuizFile.FileUrl = reader.GetValue(3).ToString();
                            else
                                userQuizFile.FileUrl = null;
                            if (!reader.IsDBNull(4))
                                userQuizFile.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                userQuizFile.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                userQuizFile.CreatedAt = reader.GetValue(5).ToString();
                            else
                                userQuizFile.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                userQuizFile.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                userQuizFile.UpdatedAt = null;
                            response.Add(userQuizFile);
                            userQuizFile = new UserQuizFileModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserQuizFileModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizFileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserQuizFileModel> Read(string connectionString, List<UserQuizFileModel> userQuizFileWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizFileModel> response = new List<UserQuizFileModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `quiz_id`, `file_url`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_quiz_files ";
                if (userQuizFileWhere != null && userQuizFileWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userQuizFileWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userQuizFileWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userQuizFileWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, userQuizFileWhere[i].QuizId);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].FileUrl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `file_url` = @fileUrl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@fileUrl" + i, userQuizFileWhere[i].FileUrl);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizFileWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userQuizFileWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userQuizFileWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizFileWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userQuizFileWhere.Count - 1)
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
                            UserQuizFileModel userQuizFile = new UserQuizFileModel(); if (!reader.IsDBNull(0))
                                userQuizFile.Id = reader.GetValue(0).ToString();
                            else
                                userQuizFile.Id = null;
                            if (!reader.IsDBNull(1))
                                userQuizFile.UserId = reader.GetValue(1).ToString();
                            else
                                userQuizFile.UserId = null;
                            if (!reader.IsDBNull(2))
                                userQuizFile.QuizId = reader.GetValue(2).ToString();
                            else
                                userQuizFile.QuizId = null;
                            if (!reader.IsDBNull(3))
                                userQuizFile.FileUrl = reader.GetValue(3).ToString();
                            else
                                userQuizFile.FileUrl = null;
                            if (!reader.IsDBNull(4))
                                userQuizFile.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                userQuizFile.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                userQuizFile.CreatedAt = reader.GetValue(5).ToString();
                            else
                                userQuizFile.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                userQuizFile.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                userQuizFile.UpdatedAt = null;
                            response.Add(userQuizFile);
                            userQuizFile = new UserQuizFileModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserQuizFileModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizFileModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserQuizFileModel> Create(string connectionString, UserQuizFileModel userQuizFile)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizFileModel> response = new List<UserQuizFileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO usqui_user_quiz_files (";
                string values = "(";
                if (!string.IsNullOrEmpty(userQuizFile.Id))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `id` ");
                        values = string.Concat(values, ", @id");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `id` ");
                        values = string.Concat(values, " @id");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userQuizFile.Id);
                }
                if (!string.IsNullOrEmpty(userQuizFile.UserId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `user_id` ");
                        values = string.Concat(values, ", @userId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `user_id` ");
                        values = string.Concat(values, " @userId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userQuizFile.UserId);
                }
                if (!string.IsNullOrEmpty(userQuizFile.QuizId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `quiz_id` ");
                        values = string.Concat(values, ", @quizId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `quiz_id` ");
                        values = string.Concat(values, " @quizId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", userQuizFile.QuizId);
                }
                if (!string.IsNullOrEmpty(userQuizFile.FileUrl))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `file_url` ");
                        values = string.Concat(values, ", @fileUrl");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `file_url` ");
                        values = string.Concat(values, " @fileUrl");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@fileUrl", userQuizFile.FileUrl);
                }
                if (!string.IsNullOrEmpty(userQuizFile.ModifiedBy))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `modified_by` ");
                        values = string.Concat(values, ", @modifiedBy");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `modified_by` ");
                        values = string.Concat(values, " @modifiedBy");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userQuizFile.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userQuizFile.CreatedAt))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `created_at` ");
                        values = string.Concat(values, ", @createdAt");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `created_at` ");
                        values = string.Concat(values, " @createdAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userQuizFile.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userQuizFile.UpdatedAt))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `updated_at` ");
                        values = string.Concat(values, ", @updatedAt");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `updated_at` ");
                        values = string.Concat(values, " @updatedAt");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userQuizFile.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userQuizFile.Id = cmd.LastInsertedId.ToString();
                response.Add(userQuizFile);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizFileModel>();
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
        public List<UserQuizFileModel> Create(string connectionString, List<UserQuizFileModel> userQuizFileList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizFileModel> response = new List<UserQuizFileModel>();
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
                        for (int i = 0; i < userQuizFileList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO usqui_user_quiz_files (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(userQuizFileList[i].Id))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `id` ");
                                        values = string.Concat(values, ", @id" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `id` ");
                                        values = string.Concat(values, "@id" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userQuizFileList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userQuizFileList[i].UserId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `user_id` ");
                                        values = string.Concat(values, ", @userId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `user_id` ");
                                        values = string.Concat(values, "@userId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userQuizFileList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userQuizFileList[i].QuizId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `quiz_id` ");
                                        values = string.Concat(values, ", @quizId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `quiz_id` ");
                                        values = string.Concat(values, "@quizId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, userQuizFileList[i].QuizId);
                                }
                                if (!string.IsNullOrEmpty(userQuizFileList[i].FileUrl))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `file_url` ");
                                        values = string.Concat(values, ", @fileUrl" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `file_url` ");
                                        values = string.Concat(values, "@fileUrl" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@fileUrl" + i, userQuizFileList[i].FileUrl);
                                }
                                if (!string.IsNullOrEmpty(userQuizFileList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `modified_by` ");
                                        values = string.Concat(values, ", @modifiedBy" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `modified_by` ");
                                        values = string.Concat(values, "@modifiedBy" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizFileList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userQuizFileList[i].CreatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `created_at` ");
                                        values = string.Concat(values, ", @createdAt" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `created_at` ");
                                        values = string.Concat(values, "@createdAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userQuizFileList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userQuizFileList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `updated_at` ");
                                        values = string.Concat(values, ", @updatedAt" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `updated_at` ");
                                        values = string.Concat(values, "@updatedAt" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizFileList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userQuizFileList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserQuizFileModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserQuizFileModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizFileModel>();
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

        public List<UserQuizFileModel> Update(string connectionString, UserQuizFileModel userQuizFile, UserQuizFileModel userQuizFileWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizFileModel> response = new List<UserQuizFileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE usqui_user_quiz_files SET ";
                if (!string.IsNullOrEmpty(userQuizFile.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userQuizFile.Id);
                }
                if (!string.IsNullOrEmpty(userQuizFile.UserId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = string.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userQuizFile.UserId);
                }
                if (!string.IsNullOrEmpty(userQuizFile.QuizId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `quiz_id` = @quizId");
                    else
                        sql = string.Concat(sql, " `quiz_id` = @quizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", userQuizFile.QuizId);
                }
                if (!string.IsNullOrEmpty(userQuizFile.FileUrl))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `file_url` = @fileUrl");
                    else
                        sql = string.Concat(sql, " `file_url` = @fileUrl");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@fileUrl", userQuizFile.FileUrl);
                }
                if (!string.IsNullOrEmpty(userQuizFile.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userQuizFile.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userQuizFile.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userQuizFile.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userQuizFile.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userQuizFile.UpdatedAt);
                }
                if (userQuizFileWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(userQuizFileWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userQuizFileWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(userQuizFileWhere.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = string.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userQuizFileWhere.UserId);
                    }
                    if (!string.IsNullOrEmpty(userQuizFileWhere.QuizId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `quiz_id` = @quizIdWhere");
                        else
                            sql = string.Concat(sql, " `quiz_id` = @quizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizIdWhere", userQuizFileWhere.QuizId);
                    }
                    if (!string.IsNullOrEmpty(userQuizFileWhere.FileUrl))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `file_url` = @fileUrlWhere");
                        else
                            sql = string.Concat(sql, " `file_url` = @fileUrlWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@fileUrlWhere", userQuizFileWhere.FileUrl);
                    }
                    if (!string.IsNullOrEmpty(userQuizFileWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userQuizFileWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userQuizFileWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userQuizFileWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userQuizFileWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userQuizFileWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userQuizFile);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizFileModel>();
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

        public List<UserQuizFileModel> Update(string connectionString, List<UserQuizFileModel> userQuizFileList, List<UserQuizFileModel> userQuizFileWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizFileModel> response = new List<UserQuizFileModel>();
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
                        for (int i = 0; i < userQuizFileList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE usqui_user_quiz_files SET ";
                                if (!string.IsNullOrEmpty(userQuizFileList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userQuizFileList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userQuizFileList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = string.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userQuizFileList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userQuizFileList[i].QuizId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `quiz_id` = @quizId" + i);
                                    else
                                        sql = string.Concat(sql, " `quiz_id` = @quizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, userQuizFileList[i].QuizId);
                                }
                                if (!string.IsNullOrEmpty(userQuizFileList[i].FileUrl))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `file_url` = @fileUrl" + i);
                                    else
                                        sql = string.Concat(sql, " `file_url` = @fileUrl" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@fileUrl" + i, userQuizFileList[i].FileUrl);
                                }
                                if (!string.IsNullOrEmpty(userQuizFileList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizFileList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userQuizFileList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userQuizFileList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userQuizFileList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizFileList[i].UpdatedAt);
                                }
                                if (userQuizFileWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(userQuizFileWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userQuizFileWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizFileWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userQuizFileWhereList[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizFileWhereList[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `quiz_id` = @quizIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `quiz_id` = @quizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizIdWhere" + i, userQuizFileWhereList[i].QuizId);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizFileWhereList[i].FileUrl))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `file_url` = @fileUrlWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `file_url` = @fileUrlWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@fileUrlWhere" + i, userQuizFileWhereList[i].FileUrl);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizFileWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userQuizFileWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizFileWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userQuizFileWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizFileWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userQuizFileWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userQuizFileList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserQuizFileModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserQuizFileModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizFileModel>();
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

        public List<UserQuizFileModel> Delete(string connectionString, UserQuizFileModel userQuizFile)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizFileModel> response = new List<UserQuizFileModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM usqui_user_quiz_files  ";
                if (userQuizFile != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(userQuizFile.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userQuizFile.Id);
                    }
                    if (!string.IsNullOrEmpty(userQuizFile.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = string.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userQuizFile.UserId);
                    }
                    if (!string.IsNullOrEmpty(userQuizFile.QuizId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `quiz_id` = @quizId");
                        else
                            sql = string.Concat(sql, " `quiz_id` = @quizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizId", userQuizFile.QuizId);
                    }
                    if (!string.IsNullOrEmpty(userQuizFile.FileUrl))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `file_url` = @fileUrl");
                        else
                            sql = string.Concat(sql, " `file_url` = @fileUrl");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@fileUrl", userQuizFile.FileUrl);
                    }
                    if (!string.IsNullOrEmpty(userQuizFile.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userQuizFile.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userQuizFile.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userQuizFile.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userQuizFile.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userQuizFile.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userQuizFile);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizFileModel>();
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

        public List<UserQuizFileModel> Delete(string connectionString, List<UserQuizFileModel> userQuizFile)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserQuizFileModel> response = new List<UserQuizFileModel>();
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
                        for (int i = 0; i < userQuizFile.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM usqui_user_quiz_files  ";
                                if (userQuizFile[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(userQuizFile[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userQuizFile[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizFile[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userQuizFile[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizFile[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `quiz_id` = @quizId" + i);
                                        else
                                            sql = string.Concat(sql, " `quiz_id` = @quizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizId" + i, userQuizFile[i].QuizId);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizFile[i].FileUrl))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `file_url` = @fileUrl" + i);
                                        else
                                            sql = string.Concat(sql, " `file_url` = @fileUrl" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@fileUrl" + i, userQuizFile[i].FileUrl);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizFile[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userQuizFile[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizFile[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userQuizFile[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userQuizFile[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userQuizFile[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userQuizFile[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserQuizFileModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserQuizFileModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserQuizFileModel>();
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
