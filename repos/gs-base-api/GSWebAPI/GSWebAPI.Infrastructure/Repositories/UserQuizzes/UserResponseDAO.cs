using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Services.UserQuizzes.CustomClasses;
using GSWebAPI.GSWebAPI.Domain.Models.UserQuiz;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.UserQuizzes
{
    class UserResponseDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, UserResponseModel> ReadD(string connectionString, UserResponseModel userResponse, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserResponseModel> response = new Dictionary<string, UserResponseModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_quiz_result_id`, `question_id`, `response_id`, `correct`, `response_extra`, `response_time`, `assigned_value`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_responses ";
                {
                    if (userResponse != null)
                    {
                        if (!string.IsNullOrEmpty(userResponse.Id))
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
                            cmd.Parameters.AddWithValue("@id", userResponse.Id);
                        }
                        if (!string.IsNullOrEmpty(userResponse.UserQuizResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `user_quiz_result_id` = @userQuizResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `user_quiz_result_id` = @userQuizResultId ");
                            }
                            cmd.Parameters.AddWithValue("@userQuizResultId", userResponse.UserQuizResultId);
                        }
                        if (!string.IsNullOrEmpty(userResponse.QuestionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `question_id` = @questionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `question_id` = @questionId ");
                            }
                            cmd.Parameters.AddWithValue("@questionId", userResponse.QuestionId);
                        }
                        if (!string.IsNullOrEmpty(userResponse.ResponseId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `response_id` = @responseId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `response_id` = @responseId ");
                            }
                            cmd.Parameters.AddWithValue("@responseId", userResponse.ResponseId);
                        }
                        if (!string.IsNullOrEmpty(userResponse.Correct))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `correct` = @correct ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `correct` = @correct ");
                            }
                            cmd.Parameters.AddWithValue("@correct", userResponse.Correct);
                        }
                        if (!string.IsNullOrEmpty(userResponse.ResponseExtra))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `response_extra` = @responseExtra ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `response_extra` = @responseExtra ");
                            }
                            cmd.Parameters.AddWithValue("@responseExtra", userResponse.ResponseExtra);
                        }
                        if (!string.IsNullOrEmpty(userResponse.ResponseTime))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `response_time` = @responseTime ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `response_time` = @responseTime ");
                            }
                            cmd.Parameters.AddWithValue("@responseTime", userResponse.ResponseTime);
                        }
                        if (!string.IsNullOrEmpty(userResponse.AssignedValue))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `assigned_value` = @assignedValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `assigned_value` = @assignedValue ");
                            }
                            cmd.Parameters.AddWithValue("@assignedValue", userResponse.AssignedValue);
                        }
                        if (!string.IsNullOrEmpty(userResponse.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userResponse.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userResponse.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userResponse.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userResponse.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userResponse.UpdatedAt);
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
                                    userResponse.Id = reader.GetValue(0).ToString();
                                else
                                    userResponse.Id = null;
                                if (!reader.IsDBNull(1))
                                    userResponse.UserQuizResultId = reader.GetValue(1).ToString();
                                else
                                    userResponse.UserQuizResultId = null;
                                if (!reader.IsDBNull(2))
                                    userResponse.QuestionId = reader.GetValue(2).ToString();
                                else
                                    userResponse.QuestionId = null;
                                if (!reader.IsDBNull(3))
                                    userResponse.ResponseId = reader.GetValue(3).ToString();
                                else
                                    userResponse.ResponseId = null;
                                if (!reader.IsDBNull(4))
                                    userResponse.Correct = reader.GetValue(4).ToString();
                                else
                                    userResponse.Correct = null;
                                if (!reader.IsDBNull(5))
                                    userResponse.ResponseExtra = reader.GetValue(5).ToString();
                                else
                                    userResponse.ResponseExtra = null;
                                if (!reader.IsDBNull(6))
                                    userResponse.ResponseTime = reader.GetValue(6).ToString();
                                else
                                    userResponse.ResponseTime = null;
                                if (!reader.IsDBNull(7))
                                    userResponse.AssignedValue = reader.GetValue(7).ToString();
                                else
                                    userResponse.AssignedValue = null;
                                if (!reader.IsDBNull(8))
                                    userResponse.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    userResponse.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    userResponse.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    userResponse.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    userResponse.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    userResponse.UpdatedAt = null;
                                response.Add(userResponse.Id, userResponse);
                                userResponse = new UserResponseModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserResponseModel>();
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
                response = new Dictionary<string, UserResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserResponseModel> ReadD(string connectionString, UserResponseModel userResponse, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserResponseModel> response = new Dictionary<string, UserResponseModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_quiz_result_id`, `question_id`, `response_id`, `correct`, `response_extra`, `response_time`, `assigned_value`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_responses ";
                {
                    if (userResponse != null)
                    {
                        if (!string.IsNullOrEmpty(userResponse.Id))
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
                            cmd.Parameters.AddWithValue("@id", userResponse.Id);
                        }
                        if (!string.IsNullOrEmpty(userResponse.UserQuizResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `user_quiz_result_id` = @userQuizResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `user_quiz_result_id` = @userQuizResultId ");
                            }
                            cmd.Parameters.AddWithValue("@userQuizResultId", userResponse.UserQuizResultId);
                        }
                        if (!string.IsNullOrEmpty(userResponse.QuestionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `question_id` = @questionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `question_id` = @questionId ");
                            }
                            cmd.Parameters.AddWithValue("@questionId", userResponse.QuestionId);
                        }
                        if (!string.IsNullOrEmpty(userResponse.ResponseId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `response_id` = @responseId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `response_id` = @responseId ");
                            }
                            cmd.Parameters.AddWithValue("@responseId", userResponse.ResponseId);
                        }
                        if (!string.IsNullOrEmpty(userResponse.Correct))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `correct` = @correct ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `correct` = @correct ");
                            }
                            cmd.Parameters.AddWithValue("@correct", userResponse.Correct);
                        }
                        if (!string.IsNullOrEmpty(userResponse.ResponseExtra))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `response_extra` = @responseExtra ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `response_extra` = @responseExtra ");
                            }
                            cmd.Parameters.AddWithValue("@responseExtra", userResponse.ResponseExtra);
                        }
                        if (!string.IsNullOrEmpty(userResponse.ResponseTime))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `response_time` = @responseTime ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `response_time` = @responseTime ");
                            }
                            cmd.Parameters.AddWithValue("@responseTime", userResponse.ResponseTime);
                        }
                        if (!string.IsNullOrEmpty(userResponse.AssignedValue))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `assigned_value` = @assignedValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `assigned_value` = @assignedValue ");
                            }
                            cmd.Parameters.AddWithValue("@assignedValue", userResponse.AssignedValue);
                        }
                        if (!string.IsNullOrEmpty(userResponse.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userResponse.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userResponse.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userResponse.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userResponse.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userResponse.UpdatedAt);
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
                                    userResponse.Id = reader.GetValue(0).ToString();
                                else
                                    userResponse.Id = null;
                                if (!reader.IsDBNull(1))
                                    userResponse.UserQuizResultId = reader.GetValue(1).ToString();
                                else
                                    userResponse.UserQuizResultId = null;
                                if (!reader.IsDBNull(2))
                                    userResponse.QuestionId = reader.GetValue(2).ToString();
                                else
                                    userResponse.QuestionId = null;
                                if (!reader.IsDBNull(3))
                                    userResponse.ResponseId = reader.GetValue(3).ToString();
                                else
                                    userResponse.ResponseId = null;
                                if (!reader.IsDBNull(4))
                                    userResponse.Correct = reader.GetValue(4).ToString();
                                else
                                    userResponse.Correct = null;
                                if (!reader.IsDBNull(5))
                                    userResponse.ResponseExtra = reader.GetValue(5).ToString();
                                else
                                    userResponse.ResponseExtra = null;
                                if (!reader.IsDBNull(6))
                                    userResponse.ResponseTime = reader.GetValue(6).ToString();
                                else
                                    userResponse.ResponseTime = null;
                                if (!reader.IsDBNull(7))
                                    userResponse.AssignedValue = reader.GetValue(7).ToString();
                                else
                                    userResponse.AssignedValue = null;
                                if (!reader.IsDBNull(8))
                                    userResponse.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    userResponse.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    userResponse.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    userResponse.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    userResponse.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    userResponse.UpdatedAt = null;
                                response.Add(userResponse.Id, userResponse);
                                userResponse = new UserResponseModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, UserResponseModel>();
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
                response = new Dictionary<string, UserResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserResponseModel> ReadD(string connectionString, List<UserResponseModel> userResponseWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserResponseModel> response = new Dictionary<string, UserResponseModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_quiz_result_id`, `question_id`, `response_id`, `correct`, `response_extra`, `response_time`, `assigned_value`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_responses ";
                if (userResponseWhere != null && userResponseWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userResponseWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userResponseWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userResponseWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].UserQuizResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_quiz_result_id` = @userQuizResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userQuizResultId" + i, userResponseWhere[i].UserQuizResultId);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].QuestionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `question_id` = @questionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionId" + i, userResponseWhere[i].QuestionId);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ResponseId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_id` = @responseId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseId" + i, userResponseWhere[i].ResponseId);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].Correct))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `correct` = @correct" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@correct" + i, userResponseWhere[i].Correct);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ResponseExtra))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_extra` = @responseExtra" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseExtra" + i, userResponseWhere[i].ResponseExtra);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ResponseTime))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_time` = @responseTime" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseTime" + i, userResponseWhere[i].ResponseTime);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].AssignedValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `assigned_value` = @assignedValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@assignedValue" + i, userResponseWhere[i].AssignedValue);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userResponseWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userResponseWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userResponseWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userResponseWhere.Count - 1)
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
                            UserResponseModel userResponse = new UserResponseModel(); if (!reader.IsDBNull(0))
                                userResponse.Id = reader.GetValue(0).ToString();
                            else
                                userResponse.Id = null;
                            if (!reader.IsDBNull(1))
                                userResponse.UserQuizResultId = reader.GetValue(1).ToString();
                            else
                                userResponse.UserQuizResultId = null;
                            if (!reader.IsDBNull(2))
                                userResponse.QuestionId = reader.GetValue(2).ToString();
                            else
                                userResponse.QuestionId = null;
                            if (!reader.IsDBNull(3))
                                userResponse.ResponseId = reader.GetValue(3).ToString();
                            else
                                userResponse.ResponseId = null;
                            if (!reader.IsDBNull(4))
                                userResponse.Correct = reader.GetValue(4).ToString();
                            else
                                userResponse.Correct = null;
                            if (!reader.IsDBNull(5))
                                userResponse.ResponseExtra = reader.GetValue(5).ToString();
                            else
                                userResponse.ResponseExtra = null;
                            if (!reader.IsDBNull(6))
                                userResponse.ResponseTime = reader.GetValue(6).ToString();
                            else
                                userResponse.ResponseTime = null;
                            if (!reader.IsDBNull(7))
                                userResponse.AssignedValue = reader.GetValue(7).ToString();
                            else
                                userResponse.AssignedValue = null;
                            if (!reader.IsDBNull(8))
                                userResponse.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                userResponse.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                userResponse.CreatedAt = reader.GetValue(9).ToString();
                            else
                                userResponse.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                userResponse.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                userResponse.UpdatedAt = null;
                            response.Add(userResponse.Id, userResponse);
                            userResponse = new UserResponseModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserResponseModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, UserResponseModel> ReadD(string connectionString, List<UserResponseModel> userResponseWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, UserResponseModel> response = new Dictionary<string, UserResponseModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_quiz_result_id`, `question_id`, `response_id`, `correct`, `response_extra`, `response_time`, `assigned_value`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_responses ";
                if (userResponseWhere != null && userResponseWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userResponseWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userResponseWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userResponseWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].UserQuizResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_quiz_result_id` = @userQuizResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userQuizResultId" + i, userResponseWhere[i].UserQuizResultId);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].QuestionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `question_id` = @questionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionId" + i, userResponseWhere[i].QuestionId);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ResponseId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_id` = @responseId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseId" + i, userResponseWhere[i].ResponseId);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].Correct))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `correct` = @correct" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@correct" + i, userResponseWhere[i].Correct);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ResponseExtra))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_extra` = @responseExtra" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseExtra" + i, userResponseWhere[i].ResponseExtra);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ResponseTime))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_time` = @responseTime" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseTime" + i, userResponseWhere[i].ResponseTime);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].AssignedValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `assigned_value` = @assignedValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@assignedValue" + i, userResponseWhere[i].AssignedValue);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userResponseWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userResponseWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userResponseWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userResponseWhere.Count - 1)
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
                            UserResponseModel userResponse = new UserResponseModel(); if (!reader.IsDBNull(0))
                                userResponse.Id = reader.GetValue(0).ToString();
                            else
                                userResponse.Id = null;
                            if (!reader.IsDBNull(1))
                                userResponse.UserQuizResultId = reader.GetValue(1).ToString();
                            else
                                userResponse.UserQuizResultId = null;
                            if (!reader.IsDBNull(2))
                                userResponse.QuestionId = reader.GetValue(2).ToString();
                            else
                                userResponse.QuestionId = null;
                            if (!reader.IsDBNull(3))
                                userResponse.ResponseId = reader.GetValue(3).ToString();
                            else
                                userResponse.ResponseId = null;
                            if (!reader.IsDBNull(4))
                                userResponse.Correct = reader.GetValue(4).ToString();
                            else
                                userResponse.Correct = null;
                            if (!reader.IsDBNull(5))
                                userResponse.ResponseExtra = reader.GetValue(5).ToString();
                            else
                                userResponse.ResponseExtra = null;
                            if (!reader.IsDBNull(6))
                                userResponse.ResponseTime = reader.GetValue(6).ToString();
                            else
                                userResponse.ResponseTime = null;
                            if (!reader.IsDBNull(7))
                                userResponse.AssignedValue = reader.GetValue(7).ToString();
                            else
                                userResponse.AssignedValue = null;
                            if (!reader.IsDBNull(8))
                                userResponse.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                userResponse.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                userResponse.CreatedAt = reader.GetValue(9).ToString();
                            else
                                userResponse.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                userResponse.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                userResponse.UpdatedAt = null;
                            response.Add(userResponse.Id, userResponse);
                            userResponse = new UserResponseModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, UserResponseModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, UserResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserResponseModel> Read(string connectionString, UserResponseModel userResponse, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserResponseModel> response = new List<UserResponseModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_quiz_result_id`, `question_id`, `response_id`, `correct`, `response_extra`, `response_time`, `assigned_value`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_responses ";
                {
                    if (userResponse != null)
                    {
                        if (!string.IsNullOrEmpty(userResponse.Id))
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
                            cmd.Parameters.AddWithValue("@id", userResponse.Id);
                        }
                        if (!string.IsNullOrEmpty(userResponse.UserQuizResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `user_quiz_result_id` = @userQuizResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `user_quiz_result_id` = @userQuizResultId ");
                            }
                            cmd.Parameters.AddWithValue("@userQuizResultId", userResponse.UserQuizResultId);
                        }
                        if (!string.IsNullOrEmpty(userResponse.QuestionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `question_id` = @questionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `question_id` = @questionId ");
                            }
                            cmd.Parameters.AddWithValue("@questionId", userResponse.QuestionId);
                        }
                        if (!string.IsNullOrEmpty(userResponse.ResponseId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `response_id` = @responseId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `response_id` = @responseId ");
                            }
                            cmd.Parameters.AddWithValue("@responseId", userResponse.ResponseId);
                        }
                        if (!string.IsNullOrEmpty(userResponse.Correct))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `correct` = @correct ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `correct` = @correct ");
                            }
                            cmd.Parameters.AddWithValue("@correct", userResponse.Correct);
                        }
                        if (!string.IsNullOrEmpty(userResponse.ResponseExtra))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `response_extra` = @responseExtra ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `response_extra` = @responseExtra ");
                            }
                            cmd.Parameters.AddWithValue("@responseExtra", userResponse.ResponseExtra);
                        }
                        if (!string.IsNullOrEmpty(userResponse.ResponseTime))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `response_time` = @responseTime ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `response_time` = @responseTime ");
                            }
                            cmd.Parameters.AddWithValue("@responseTime", userResponse.ResponseTime);
                        }
                        if (!string.IsNullOrEmpty(userResponse.AssignedValue))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `assigned_value` = @assignedValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `assigned_value` = @assignedValue ");
                            }
                            cmd.Parameters.AddWithValue("@assignedValue", userResponse.AssignedValue);
                        }
                        if (!string.IsNullOrEmpty(userResponse.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userResponse.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userResponse.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userResponse.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userResponse.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userResponse.UpdatedAt);
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
                                    userResponse.Id = reader.GetValue(0).ToString();
                                else
                                    userResponse.Id = null;
                                if (!reader.IsDBNull(1))
                                    userResponse.UserQuizResultId = reader.GetValue(1).ToString();
                                else
                                    userResponse.UserQuizResultId = null;
                                if (!reader.IsDBNull(2))
                                    userResponse.QuestionId = reader.GetValue(2).ToString();
                                else
                                    userResponse.QuestionId = null;
                                if (!reader.IsDBNull(3))
                                    userResponse.ResponseId = reader.GetValue(3).ToString();
                                else
                                    userResponse.ResponseId = null;
                                if (!reader.IsDBNull(4))
                                    userResponse.Correct = reader.GetValue(4).ToString();
                                else
                                    userResponse.Correct = null;
                                if (!reader.IsDBNull(5))
                                    userResponse.ResponseExtra = reader.GetValue(5).ToString();
                                else
                                    userResponse.ResponseExtra = null;
                                if (!reader.IsDBNull(6))
                                    userResponse.ResponseTime = reader.GetValue(6).ToString();
                                else
                                    userResponse.ResponseTime = null;
                                if (!reader.IsDBNull(7))
                                    userResponse.AssignedValue = reader.GetValue(7).ToString();
                                else
                                    userResponse.AssignedValue = null;
                                if (!reader.IsDBNull(8))
                                    userResponse.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    userResponse.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    userResponse.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    userResponse.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    userResponse.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    userResponse.UpdatedAt = null;
                                response.Add(userResponse);
                                userResponse = new UserResponseModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserResponseModel>();
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
                response = new List<UserResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserResponseModel> Read(string connectionString, UserResponseModel userResponse, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserResponseModel> response = new List<UserResponseModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `user_quiz_result_id`, `question_id`, `response_id`, `correct`, `response_extra`, `response_time`, `assigned_value`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_responses ";
                {
                    if (userResponse != null)
                    {
                        if (!string.IsNullOrEmpty(userResponse.Id))
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
                            cmd.Parameters.AddWithValue("@id", userResponse.Id);
                        }
                        if (!string.IsNullOrEmpty(userResponse.UserQuizResultId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `user_quiz_result_id` = @userQuizResultId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `user_quiz_result_id` = @userQuizResultId ");
                            }
                            cmd.Parameters.AddWithValue("@userQuizResultId", userResponse.UserQuizResultId);
                        }
                        if (!string.IsNullOrEmpty(userResponse.QuestionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `question_id` = @questionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `question_id` = @questionId ");
                            }
                            cmd.Parameters.AddWithValue("@questionId", userResponse.QuestionId);
                        }
                        if (!string.IsNullOrEmpty(userResponse.ResponseId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `response_id` = @responseId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `response_id` = @responseId ");
                            }
                            cmd.Parameters.AddWithValue("@responseId", userResponse.ResponseId);
                        }
                        if (!string.IsNullOrEmpty(userResponse.Correct))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `correct` = @correct ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `correct` = @correct ");
                            }
                            cmd.Parameters.AddWithValue("@correct", userResponse.Correct);
                        }
                        if (!string.IsNullOrEmpty(userResponse.ResponseExtra))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `response_extra` = @responseExtra ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `response_extra` = @responseExtra ");
                            }
                            cmd.Parameters.AddWithValue("@responseExtra", userResponse.ResponseExtra);
                        }
                        if (!string.IsNullOrEmpty(userResponse.ResponseTime))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `response_time` = @responseTime ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `response_time` = @responseTime ");
                            }
                            cmd.Parameters.AddWithValue("@responseTime", userResponse.ResponseTime);
                        }
                        if (!string.IsNullOrEmpty(userResponse.AssignedValue))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `assigned_value` = @assignedValue ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `assigned_value` = @assignedValue ");
                            }
                            cmd.Parameters.AddWithValue("@assignedValue", userResponse.AssignedValue);
                        }
                        if (!string.IsNullOrEmpty(userResponse.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", userResponse.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userResponse.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", userResponse.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userResponse.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", userResponse.UpdatedAt);
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
                                    userResponse.Id = reader.GetValue(0).ToString();
                                else
                                    userResponse.Id = null;
                                if (!reader.IsDBNull(1))
                                    userResponse.UserQuizResultId = reader.GetValue(1).ToString();
                                else
                                    userResponse.UserQuizResultId = null;
                                if (!reader.IsDBNull(2))
                                    userResponse.QuestionId = reader.GetValue(2).ToString();
                                else
                                    userResponse.QuestionId = null;
                                if (!reader.IsDBNull(3))
                                    userResponse.ResponseId = reader.GetValue(3).ToString();
                                else
                                    userResponse.ResponseId = null;
                                if (!reader.IsDBNull(4))
                                    userResponse.Correct = reader.GetValue(4).ToString();
                                else
                                    userResponse.Correct = null;
                                if (!reader.IsDBNull(5))
                                    userResponse.ResponseExtra = reader.GetValue(5).ToString();
                                else
                                    userResponse.ResponseExtra = null;
                                if (!reader.IsDBNull(6))
                                    userResponse.ResponseTime = reader.GetValue(6).ToString();
                                else
                                    userResponse.ResponseTime = null;
                                if (!reader.IsDBNull(7))
                                    userResponse.AssignedValue = reader.GetValue(7).ToString();
                                else
                                    userResponse.AssignedValue = null;
                                if (!reader.IsDBNull(8))
                                    userResponse.ModifiedBy = reader.GetValue(8).ToString();
                                else
                                    userResponse.ModifiedBy = null;
                                if (!reader.IsDBNull(9))
                                    userResponse.CreatedAt = reader.GetValue(9).ToString();
                                else
                                    userResponse.CreatedAt = null;
                                if (!reader.IsDBNull(10))
                                    userResponse.UpdatedAt = reader.GetValue(10).ToString();
                                else
                                    userResponse.UpdatedAt = null;
                                response.Add(userResponse);
                                userResponse = new UserResponseModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<UserResponseModel>();
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
                response = new List<UserResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserResponseModel> Read(string connectionString, List<UserResponseModel> userResponseWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserResponseModel> response = new List<UserResponseModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_quiz_result_id`, `question_id`, `response_id`, `correct`, `response_extra`, `response_time`, `assigned_value`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_responses ";
                if (userResponseWhere != null && userResponseWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userResponseWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userResponseWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userResponseWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].UserQuizResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_quiz_result_id` = @userQuizResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userQuizResultId" + i, userResponseWhere[i].UserQuizResultId);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].QuestionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `question_id` = @questionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionId" + i, userResponseWhere[i].QuestionId);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ResponseId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_id` = @responseId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseId" + i, userResponseWhere[i].ResponseId);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].Correct))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `correct` = @correct" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@correct" + i, userResponseWhere[i].Correct);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ResponseExtra))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_extra` = @responseExtra" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseExtra" + i, userResponseWhere[i].ResponseExtra);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ResponseTime))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_time` = @responseTime" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseTime" + i, userResponseWhere[i].ResponseTime);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].AssignedValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `assigned_value` = @assignedValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@assignedValue" + i, userResponseWhere[i].AssignedValue);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userResponseWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userResponseWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userResponseWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userResponseWhere.Count - 1)
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
                            UserResponseModel userResponse = new UserResponseModel(); if (!reader.IsDBNull(0))
                                userResponse.Id = reader.GetValue(0).ToString();
                            else
                                userResponse.Id = null;
                            if (!reader.IsDBNull(1))
                                userResponse.UserQuizResultId = reader.GetValue(1).ToString();
                            else
                                userResponse.UserQuizResultId = null;
                            if (!reader.IsDBNull(2))
                                userResponse.QuestionId = reader.GetValue(2).ToString();
                            else
                                userResponse.QuestionId = null;
                            if (!reader.IsDBNull(3))
                                userResponse.ResponseId = reader.GetValue(3).ToString();
                            else
                                userResponse.ResponseId = null;
                            if (!reader.IsDBNull(4))
                                userResponse.Correct = reader.GetValue(4).ToString();
                            else
                                userResponse.Correct = null;
                            if (!reader.IsDBNull(5))
                                userResponse.ResponseExtra = reader.GetValue(5).ToString();
                            else
                                userResponse.ResponseExtra = null;
                            if (!reader.IsDBNull(6))
                                userResponse.ResponseTime = reader.GetValue(6).ToString();
                            else
                                userResponse.ResponseTime = null;
                            if (!reader.IsDBNull(7))
                                userResponse.AssignedValue = reader.GetValue(7).ToString();
                            else
                                userResponse.AssignedValue = null;
                            if (!reader.IsDBNull(8))
                                userResponse.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                userResponse.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                userResponse.CreatedAt = reader.GetValue(9).ToString();
                            else
                                userResponse.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                userResponse.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                userResponse.UpdatedAt = null;
                            response.Add(userResponse);
                            userResponse = new UserResponseModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserResponseModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserResponseModel> Read(string connectionString, List<UserResponseModel> userResponseWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserResponseModel> response = new List<UserResponseModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `user_quiz_result_id`, `question_id`, `response_id`, `correct`, `response_extra`, `response_time`, `assigned_value`, `modified_by`, `created_at`, `updated_at`  FROM usqui_user_responses ";
                if (userResponseWhere != null && userResponseWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < userResponseWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(userResponseWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, userResponseWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].UserQuizResultId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_quiz_result_id` = @userQuizResultId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userQuizResultId" + i, userResponseWhere[i].UserQuizResultId);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].QuestionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `question_id` = @questionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionId" + i, userResponseWhere[i].QuestionId);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ResponseId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_id` = @responseId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseId" + i, userResponseWhere[i].ResponseId);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].Correct))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `correct` = @correct" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@correct" + i, userResponseWhere[i].Correct);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ResponseExtra))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_extra` = @responseExtra" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseExtra" + i, userResponseWhere[i].ResponseExtra);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ResponseTime))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_time` = @responseTime" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseTime" + i, userResponseWhere[i].ResponseTime);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].AssignedValue))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `assigned_value` = @assignedValue" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@assignedValue" + i, userResponseWhere[i].AssignedValue);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, userResponseWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, userResponseWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, userResponseWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != userResponseWhere.Count - 1)
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
                            UserResponseModel userResponse = new UserResponseModel(); if (!reader.IsDBNull(0))
                                userResponse.Id = reader.GetValue(0).ToString();
                            else
                                userResponse.Id = null;
                            if (!reader.IsDBNull(1))
                                userResponse.UserQuizResultId = reader.GetValue(1).ToString();
                            else
                                userResponse.UserQuizResultId = null;
                            if (!reader.IsDBNull(2))
                                userResponse.QuestionId = reader.GetValue(2).ToString();
                            else
                                userResponse.QuestionId = null;
                            if (!reader.IsDBNull(3))
                                userResponse.ResponseId = reader.GetValue(3).ToString();
                            else
                                userResponse.ResponseId = null;
                            if (!reader.IsDBNull(4))
                                userResponse.Correct = reader.GetValue(4).ToString();
                            else
                                userResponse.Correct = null;
                            if (!reader.IsDBNull(5))
                                userResponse.ResponseExtra = reader.GetValue(5).ToString();
                            else
                                userResponse.ResponseExtra = null;
                            if (!reader.IsDBNull(6))
                                userResponse.ResponseTime = reader.GetValue(6).ToString();
                            else
                                userResponse.ResponseTime = null;
                            if (!reader.IsDBNull(7))
                                userResponse.AssignedValue = reader.GetValue(7).ToString();
                            else
                                userResponse.AssignedValue = null;
                            if (!reader.IsDBNull(8))
                                userResponse.ModifiedBy = reader.GetValue(8).ToString();
                            else
                                userResponse.ModifiedBy = null;
                            if (!reader.IsDBNull(9))
                                userResponse.CreatedAt = reader.GetValue(9).ToString();
                            else
                                userResponse.CreatedAt = null;
                            if (!reader.IsDBNull(10))
                                userResponse.UpdatedAt = reader.GetValue(10).ToString();
                            else
                                userResponse.UpdatedAt = null;
                            response.Add(userResponse);
                            userResponse = new UserResponseModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<UserResponseModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<UserResponseModel> Create(string connectionString, UserResponseModel userResponse)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserResponseModel> response = new List<UserResponseModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO usqui_user_responses (";
                string values = "(";
                if (!string.IsNullOrEmpty(userResponse.Id))
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
                    cmd.Parameters.AddWithValue("@id", userResponse.Id);
                }
                if (!string.IsNullOrEmpty(userResponse.UserQuizResultId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `user_quiz_result_id` ");
                        values = string.Concat(values, ", @userQuizResultId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `user_quiz_result_id` ");
                        values = string.Concat(values, " @userQuizResultId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userQuizResultId", userResponse.UserQuizResultId);
                }
                if (!string.IsNullOrEmpty(userResponse.QuestionId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `question_id` ");
                        values = string.Concat(values, ", @questionId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `question_id` ");
                        values = string.Concat(values, " @questionId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@questionId", userResponse.QuestionId);
                }
                if (!string.IsNullOrEmpty(userResponse.ResponseId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `response_id` ");
                        values = string.Concat(values, ", @responseId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `response_id` ");
                        values = string.Concat(values, " @responseId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@responseId", userResponse.ResponseId);
                }
                if (!string.IsNullOrEmpty(userResponse.Correct))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `correct` ");
                        values = string.Concat(values, ", @correct");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `correct` ");
                        values = string.Concat(values, " @correct");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@correct", userResponse.Correct);
                }
                if (!string.IsNullOrEmpty(userResponse.ResponseExtra))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `response_extra` ");
                        values = string.Concat(values, ", @responseExtra");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `response_extra` ");
                        values = string.Concat(values, " @responseExtra");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@responseExtra", userResponse.ResponseExtra);
                }
                if (!string.IsNullOrEmpty(userResponse.ResponseTime))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `response_time` ");
                        values = string.Concat(values, ", @responseTime");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `response_time` ");
                        values = string.Concat(values, " @responseTime");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@responseTime", userResponse.ResponseTime);
                }
                if (!string.IsNullOrEmpty(userResponse.AssignedValue))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `assigned_value` ");
                        values = string.Concat(values, ", @assignedValue");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `assigned_value` ");
                        values = string.Concat(values, " @assignedValue");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@assignedValue", userResponse.AssignedValue);
                }
                if (!string.IsNullOrEmpty(userResponse.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", userResponse.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userResponse.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", userResponse.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userResponse.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", userResponse.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                userResponse.Id = cmd.LastInsertedId.ToString();
                response.Add(userResponse);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserResponseModel>();
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
        public List<UserResponseModel> Create(string connectionString, List<UserResponseModel> userResponseList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserResponseModel> response = new List<UserResponseModel>();
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
                        for (int i = 0; i < userResponseList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO usqui_user_responses (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(userResponseList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, userResponseList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].UserQuizResultId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `user_quiz_result_id` ");
                                        values = string.Concat(values, ", @userQuizResultId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `user_quiz_result_id` ");
                                        values = string.Concat(values, "@userQuizResultId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userQuizResultId" + i, userResponseList[i].UserQuizResultId);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].QuestionId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `question_id` ");
                                        values = string.Concat(values, ", @questionId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `question_id` ");
                                        values = string.Concat(values, "@questionId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@questionId" + i, userResponseList[i].QuestionId);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].ResponseId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `response_id` ");
                                        values = string.Concat(values, ", @responseId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `response_id` ");
                                        values = string.Concat(values, "@responseId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@responseId" + i, userResponseList[i].ResponseId);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].Correct))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `correct` ");
                                        values = string.Concat(values, ", @correct" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `correct` ");
                                        values = string.Concat(values, "@correct" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@correct" + i, userResponseList[i].Correct);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].ResponseExtra))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `response_extra` ");
                                        values = string.Concat(values, ", @responseExtra" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `response_extra` ");
                                        values = string.Concat(values, "@responseExtra" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@responseExtra" + i, userResponseList[i].ResponseExtra);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].ResponseTime))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `response_time` ");
                                        values = string.Concat(values, ", @responseTime" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `response_time` ");
                                        values = string.Concat(values, "@responseTime" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@responseTime" + i, userResponseList[i].ResponseTime);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].AssignedValue))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `assigned_value` ");
                                        values = string.Concat(values, ", @assignedValue" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `assigned_value` ");
                                        values = string.Concat(values, "@assignedValue" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@assignedValue" + i, userResponseList[i].AssignedValue);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userResponseList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userResponseList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userResponseList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(userResponseList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<UserResponseModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserResponseModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserResponseModel>();
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

        public List<UserResponseModel> Update(string connectionString, UserResponseModel userResponse, UserResponseModel userResponseWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserResponseModel> response = new List<UserResponseModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE usqui_user_responses SET ";
                if (!string.IsNullOrEmpty(userResponse.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userResponse.Id);
                }
                if (!string.IsNullOrEmpty(userResponse.UserQuizResultId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `user_quiz_result_id` = @userQuizResultId");
                    else
                        sql = string.Concat(sql, " `user_quiz_result_id` = @userQuizResultId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userQuizResultId", userResponse.UserQuizResultId);
                }
                if (!string.IsNullOrEmpty(userResponse.QuestionId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `question_id` = @questionId");
                    else
                        sql = string.Concat(sql, " `question_id` = @questionId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@questionId", userResponse.QuestionId);
                }
                if (!string.IsNullOrEmpty(userResponse.ResponseId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `response_id` = @responseId");
                    else
                        sql = string.Concat(sql, " `response_id` = @responseId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@responseId", userResponse.ResponseId);
                }
                if (!string.IsNullOrEmpty(userResponse.Correct))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `correct` = @correct");
                    else
                        sql = string.Concat(sql, " `correct` = @correct");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@correct", userResponse.Correct);
                }
                if (!string.IsNullOrEmpty(userResponse.ResponseExtra))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `response_extra` = @responseExtra");
                    else
                        sql = string.Concat(sql, " `response_extra` = @responseExtra");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@responseExtra", userResponse.ResponseExtra);
                }
                if (!string.IsNullOrEmpty(userResponse.ResponseTime))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `response_time` = @responseTime");
                    else
                        sql = string.Concat(sql, " `response_time` = @responseTime");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@responseTime", userResponse.ResponseTime);
                }
                if (!string.IsNullOrEmpty(userResponse.AssignedValue))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `assigned_value` = @assignedValue");
                    else
                        sql = string.Concat(sql, " `assigned_value` = @assignedValue");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@assignedValue", userResponse.AssignedValue);
                }
                if (!string.IsNullOrEmpty(userResponse.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", userResponse.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(userResponse.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", userResponse.CreatedAt);
                }
                if (!string.IsNullOrEmpty(userResponse.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", userResponse.UpdatedAt);
                }
                if (userResponseWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(userResponseWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", userResponseWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(userResponseWhere.UserQuizResultId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_quiz_result_id` = @userQuizResultIdWhere");
                        else
                            sql = string.Concat(sql, " `user_quiz_result_id` = @userQuizResultIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userQuizResultIdWhere", userResponseWhere.UserQuizResultId);
                    }
                    if (!string.IsNullOrEmpty(userResponseWhere.QuestionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `question_id` = @questionIdWhere");
                        else
                            sql = string.Concat(sql, " `question_id` = @questionIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@questionIdWhere", userResponseWhere.QuestionId);
                    }
                    if (!string.IsNullOrEmpty(userResponseWhere.ResponseId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `response_id` = @responseIdWhere");
                        else
                            sql = string.Concat(sql, " `response_id` = @responseIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@responseIdWhere", userResponseWhere.ResponseId);
                    }
                    if (!string.IsNullOrEmpty(userResponseWhere.Correct))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `correct` = @correctWhere");
                        else
                            sql = string.Concat(sql, " `correct` = @correctWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@correctWhere", userResponseWhere.Correct);
                    }
                    if (!string.IsNullOrEmpty(userResponseWhere.ResponseExtra))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `response_extra` = @responseExtraWhere");
                        else
                            sql = string.Concat(sql, " `response_extra` = @responseExtraWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@responseExtraWhere", userResponseWhere.ResponseExtra);
                    }
                    if (!string.IsNullOrEmpty(userResponseWhere.ResponseTime))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `response_time` = @responseTimeWhere");
                        else
                            sql = string.Concat(sql, " `response_time` = @responseTimeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@responseTimeWhere", userResponseWhere.ResponseTime);
                    }
                    if (!string.IsNullOrEmpty(userResponseWhere.AssignedValue))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `assigned_value` = @assignedValueWhere");
                        else
                            sql = string.Concat(sql, " `assigned_value` = @assignedValueWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@assignedValueWhere", userResponseWhere.AssignedValue);
                    }
                    if (!string.IsNullOrEmpty(userResponseWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", userResponseWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userResponseWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", userResponseWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userResponseWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", userResponseWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(userResponse);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserResponseModel>();
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

        public List<UserResponseModel> Update(string connectionString, List<UserResponseModel> userResponseList, List<UserResponseModel> userResponseWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserResponseModel> response = new List<UserResponseModel>();
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
                        for (int i = 0; i < userResponseList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE usqui_user_responses SET ";
                                if (!string.IsNullOrEmpty(userResponseList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userResponseList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].UserQuizResultId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `user_quiz_result_id` = @userQuizResultId" + i);
                                    else
                                        sql = string.Concat(sql, " `user_quiz_result_id` = @userQuizResultId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userQuizResultId" + i, userResponseList[i].UserQuizResultId);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].QuestionId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `question_id` = @questionId" + i);
                                    else
                                        sql = string.Concat(sql, " `question_id` = @questionId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@questionId" + i, userResponseList[i].QuestionId);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].ResponseId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `response_id` = @responseId" + i);
                                    else
                                        sql = string.Concat(sql, " `response_id` = @responseId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@responseId" + i, userResponseList[i].ResponseId);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].Correct))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `correct` = @correct" + i);
                                    else
                                        sql = string.Concat(sql, " `correct` = @correct" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@correct" + i, userResponseList[i].Correct);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].ResponseExtra))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `response_extra` = @responseExtra" + i);
                                    else
                                        sql = string.Concat(sql, " `response_extra` = @responseExtra" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@responseExtra" + i, userResponseList[i].ResponseExtra);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].ResponseTime))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `response_time` = @responseTime" + i);
                                    else
                                        sql = string.Concat(sql, " `response_time` = @responseTime" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@responseTime" + i, userResponseList[i].ResponseTime);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].AssignedValue))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `assigned_value` = @assignedValue" + i);
                                    else
                                        sql = string.Concat(sql, " `assigned_value` = @assignedValue" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@assignedValue" + i, userResponseList[i].AssignedValue);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, userResponseList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, userResponseList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, userResponseList[i].UpdatedAt);
                                }
                                if (userResponseWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(userResponseWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, userResponseWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userResponseWhereList[i].UserQuizResultId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_quiz_result_id` = @userQuizResultIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `user_quiz_result_id` = @userQuizResultIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userQuizResultIdWhere" + i, userResponseWhereList[i].UserQuizResultId);
                                    }
                                    if (!string.IsNullOrEmpty(userResponseWhereList[i].QuestionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `question_id` = @questionIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `question_id` = @questionIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@questionIdWhere" + i, userResponseWhereList[i].QuestionId);
                                    }
                                    if (!string.IsNullOrEmpty(userResponseWhereList[i].ResponseId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `response_id` = @responseIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `response_id` = @responseIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@responseIdWhere" + i, userResponseWhereList[i].ResponseId);
                                    }
                                    if (!string.IsNullOrEmpty(userResponseWhereList[i].Correct))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `correct` = @correctWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `correct` = @correctWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@correctWhere" + i, userResponseWhereList[i].Correct);
                                    }
                                    if (!string.IsNullOrEmpty(userResponseWhereList[i].ResponseExtra))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `response_extra` = @responseExtraWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `response_extra` = @responseExtraWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@responseExtraWhere" + i, userResponseWhereList[i].ResponseExtra);
                                    }
                                    if (!string.IsNullOrEmpty(userResponseWhereList[i].ResponseTime))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `response_time` = @responseTimeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `response_time` = @responseTimeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@responseTimeWhere" + i, userResponseWhereList[i].ResponseTime);
                                    }
                                    if (!string.IsNullOrEmpty(userResponseWhereList[i].AssignedValue))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `assigned_value` = @assignedValueWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `assigned_value` = @assignedValueWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@assignedValueWhere" + i, userResponseWhereList[i].AssignedValue);
                                    }
                                    if (!string.IsNullOrEmpty(userResponseWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, userResponseWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userResponseWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, userResponseWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userResponseWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, userResponseWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userResponseList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserResponseModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserResponseModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserResponseModel>();
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

        public List<UserResponseModel> Delete(string connectionString, UserResponseModel userResponse)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserResponseModel> response = new List<UserResponseModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM usqui_user_responses  ";
                if (userResponse != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(userResponse.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", userResponse.Id);
                    }
                    if (!string.IsNullOrEmpty(userResponse.UserQuizResultId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_quiz_result_id` = @userQuizResultId");
                        else
                            sql = string.Concat(sql, " `user_quiz_result_id` = @userQuizResultId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userQuizResultId", userResponse.UserQuizResultId);
                    }
                    if (!string.IsNullOrEmpty(userResponse.QuestionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `question_id` = @questionId");
                        else
                            sql = string.Concat(sql, " `question_id` = @questionId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@questionId", userResponse.QuestionId);
                    }
                    if (!string.IsNullOrEmpty(userResponse.ResponseId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `response_id` = @responseId");
                        else
                            sql = string.Concat(sql, " `response_id` = @responseId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@responseId", userResponse.ResponseId);
                    }
                    if (!string.IsNullOrEmpty(userResponse.Correct))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `correct` = @correct");
                        else
                            sql = string.Concat(sql, " `correct` = @correct");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@correct", userResponse.Correct);
                    }
                    if (!string.IsNullOrEmpty(userResponse.ResponseExtra))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `response_extra` = @responseExtra");
                        else
                            sql = string.Concat(sql, " `response_extra` = @responseExtra");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@responseExtra", userResponse.ResponseExtra);
                    }
                    if (!string.IsNullOrEmpty(userResponse.ResponseTime))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `response_time` = @responseTime");
                        else
                            sql = string.Concat(sql, " `response_time` = @responseTime");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@responseTime", userResponse.ResponseTime);
                    }
                    if (!string.IsNullOrEmpty(userResponse.AssignedValue))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `assigned_value` = @assignedValue");
                        else
                            sql = string.Concat(sql, " `assigned_value` = @assignedValue");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@assignedValue", userResponse.AssignedValue);
                    }
                    if (!string.IsNullOrEmpty(userResponse.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", userResponse.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(userResponse.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", userResponse.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(userResponse.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", userResponse.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(userResponse);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<UserResponseModel>();
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

        public List<UserResponseModel> Delete(string connectionString, List<UserResponseModel> userResponse)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserResponseModel> response = new List<UserResponseModel>();
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
                        for (int i = 0; i < userResponse.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM usqui_user_responses  ";
                                if (userResponse[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(userResponse[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, userResponse[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(userResponse[i].UserQuizResultId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_quiz_result_id` = @userQuizResultId" + i);
                                        else
                                            sql = string.Concat(sql, " `user_quiz_result_id` = @userQuizResultId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userQuizResultId" + i, userResponse[i].UserQuizResultId);
                                    }
                                    if (!string.IsNullOrEmpty(userResponse[i].QuestionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `question_id` = @questionId" + i);
                                        else
                                            sql = string.Concat(sql, " `question_id` = @questionId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@questionId" + i, userResponse[i].QuestionId);
                                    }
                                    if (!string.IsNullOrEmpty(userResponse[i].ResponseId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `response_id` = @responseId" + i);
                                        else
                                            sql = string.Concat(sql, " `response_id` = @responseId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@responseId" + i, userResponse[i].ResponseId);
                                    }
                                    if (!string.IsNullOrEmpty(userResponse[i].Correct))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `correct` = @correct" + i);
                                        else
                                            sql = string.Concat(sql, " `correct` = @correct" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@correct" + i, userResponse[i].Correct);
                                    }
                                    if (!string.IsNullOrEmpty(userResponse[i].ResponseExtra))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `response_extra` = @responseExtra" + i);
                                        else
                                            sql = string.Concat(sql, " `response_extra` = @responseExtra" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@responseExtra" + i, userResponse[i].ResponseExtra);
                                    }
                                    if (!string.IsNullOrEmpty(userResponse[i].ResponseTime))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `response_time` = @responseTime" + i);
                                        else
                                            sql = string.Concat(sql, " `response_time` = @responseTime" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@responseTime" + i, userResponse[i].ResponseTime);
                                    }
                                    if (!string.IsNullOrEmpty(userResponse[i].AssignedValue))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `assigned_value` = @assignedValue" + i);
                                        else
                                            sql = string.Concat(sql, " `assigned_value` = @assignedValue" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@assignedValue" + i, userResponse[i].AssignedValue);
                                    }
                                    if (!string.IsNullOrEmpty(userResponse[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, userResponse[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(userResponse[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, userResponse[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(userResponse[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, userResponse[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(userResponse[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<UserResponseModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<UserResponseModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<UserResponseModel>();
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
