using System;
using System.Collections.Generic;
using MySqlConnector;
using GSWebAPI.GSWebAPI.Services.GameQuizCompetitions.CustomClasses;
using GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition;

namespace GSWebAPI.GSWebAPI.Infrastructure.Repositories.GameQuizCompetitions
{
    class UserResponseDAO
    {
        private MySqlConnection _connection;
        public List<UserResponseModel> Read(string connectionString, UserResponseModel userResponse, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<UserResponseModel> response = new List<UserResponseModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `user_id`, `quiz_id`, `subject_code`, `level_code`, `question_id`, `response_id`, `response_time`, `correct`, `modified_by`, `created_at`, `updated_at`  FROM gqz_user_responses ";
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
                        if (!string.IsNullOrEmpty(userResponse.UserId))
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
                            cmd.Parameters.AddWithValue("@userId", userResponse.UserId);
                        }
                        if (!string.IsNullOrEmpty(userResponse.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", userResponse.QuizId);
                        }
                        if (!string.IsNullOrEmpty(userResponse.SubjectCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `subject_code` = @subjectCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `subject_code` = @subjectCode ");
                            }
                            cmd.Parameters.AddWithValue("@subjectCode", userResponse.SubjectCode);
                        }
                        if (!string.IsNullOrEmpty(userResponse.LevelCode))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `level_code` = @levelCode ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `level_code` = @levelCode ");
                            }
                            cmd.Parameters.AddWithValue("@levelCode", userResponse.LevelCode);
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
                    MySqlDataReader Reader = cmd.ExecuteReader();
                    try
                    {
                        if (Reader.HasRows)
                            while (Reader.Read())
                            {
                                if (!Reader.IsDBNull(0))
                                    userResponse.Id = Reader.GetValue(0).ToString();
                                else
                                    userResponse.Id = null;
                                if (!Reader.IsDBNull(1))
                                    userResponse.UserId = Reader.GetValue(1).ToString();
                                else
                                    userResponse.UserId = null;
                                if (!Reader.IsDBNull(2))
                                    userResponse.QuizId = Reader.GetValue(2).ToString();
                                else
                                    userResponse.QuizId = null;
                                if (!Reader.IsDBNull(3))
                                    userResponse.SubjectCode = Reader.GetValue(3).ToString();
                                else
                                    userResponse.SubjectCode = null;
                                if (!Reader.IsDBNull(4))
                                    userResponse.LevelCode = Reader.GetValue(4).ToString();
                                else
                                    userResponse.LevelCode = null;
                                if (!Reader.IsDBNull(5))
                                    userResponse.QuestionId = Reader.GetValue(5).ToString();
                                else
                                    userResponse.QuestionId = null;
                                if (!Reader.IsDBNull(6))
                                    userResponse.ResponseId = Reader.GetValue(6).ToString();
                                else
                                    userResponse.ResponseId = null;
                                if (!Reader.IsDBNull(7))
                                    userResponse.ResponseTime = Reader.GetValue(7).ToString();
                                else
                                    userResponse.ResponseTime = null;
                                if (!Reader.IsDBNull(8))
                                    userResponse.Correct = Reader.GetValue(8).ToString();
                                else
                                    userResponse.Correct = null;
                                if (!Reader.IsDBNull(9))
                                    userResponse.ModifiedBy = Reader.GetValue(9).ToString();
                                else
                                    userResponse.ModifiedBy = null;
                                if (!Reader.IsDBNull(10))
                                    userResponse.CreatedAt = Reader.GetValue(10).ToString();
                                else
                                    userResponse.CreatedAt = null;
                                if (!Reader.IsDBNull(11))
                                    userResponse.UpdatedAt = Reader.GetValue(11).ToString();
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
                        Reader.Close();
                    }
                    finally
                    {
                        if (Reader != null)
                            Reader.Close();
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
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `user_id`, `quiz_id`, `subject_code`, `level_code`, `question_id`, `response_id`, `response_time`, `correct`, `modified_by`, `created_at`, `updated_at`  FROM gqz_user_responses ";
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
                        if (!string.IsNullOrEmpty(userResponseWhere[i].UserId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `user_id` = @userId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@userId" + i, userResponseWhere[i].UserId);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, userResponseWhere[i].QuizId);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].SubjectCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `subject_code` = @subjectCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@subjectCode" + i, userResponseWhere[i].SubjectCode);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].LevelCode))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `level_code` = @levelCode" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@levelCode" + i, userResponseWhere[i].LevelCode);
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
                        if (!string.IsNullOrEmpty(userResponseWhere[i].ResponseTime))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response_time` = @responseTime" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@responseTime" + i, userResponseWhere[i].ResponseTime);
                        }
                        if (!string.IsNullOrEmpty(userResponseWhere[i].Correct))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `correct` = @correct" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@correct" + i, userResponseWhere[i].Correct);
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
                MySqlDataReader Reader = cmd.ExecuteReader();
                try
                {
                    if (Reader.HasRows)
                        while (Reader.Read())
                        {
                            UserResponseModel userResponse = new UserResponseModel(); if (!Reader.IsDBNull(0))
                                userResponse.Id = Reader.GetValue(0).ToString();
                            else
                                userResponse.Id = null;
                            if (!Reader.IsDBNull(1))
                                userResponse.UserId = Reader.GetValue(1).ToString();
                            else
                                userResponse.UserId = null;
                            if (!Reader.IsDBNull(2))
                                userResponse.QuizId = Reader.GetValue(2).ToString();
                            else
                                userResponse.QuizId = null;
                            if (!Reader.IsDBNull(3))
                                userResponse.SubjectCode = Reader.GetValue(3).ToString();
                            else
                                userResponse.SubjectCode = null;
                            if (!Reader.IsDBNull(4))
                                userResponse.LevelCode = Reader.GetValue(4).ToString();
                            else
                                userResponse.LevelCode = null;
                            if (!Reader.IsDBNull(5))
                                userResponse.QuestionId = Reader.GetValue(5).ToString();
                            else
                                userResponse.QuestionId = null;
                            if (!Reader.IsDBNull(6))
                                userResponse.ResponseId = Reader.GetValue(6).ToString();
                            else
                                userResponse.ResponseId = null;
                            if (!Reader.IsDBNull(7))
                                userResponse.ResponseTime = Reader.GetValue(7).ToString();
                            else
                                userResponse.ResponseTime = null;
                            if (!Reader.IsDBNull(8))
                                userResponse.Correct = Reader.GetValue(8).ToString();
                            else
                                userResponse.Correct = null;
                            if (!Reader.IsDBNull(9))
                                userResponse.ModifiedBy = Reader.GetValue(9).ToString();
                            else
                                userResponse.ModifiedBy = null;
                            if (!Reader.IsDBNull(10))
                                userResponse.CreatedAt = Reader.GetValue(10).ToString();
                            else
                                userResponse.CreatedAt = null;
                            if (!Reader.IsDBNull(11))
                                userResponse.UpdatedAt = Reader.GetValue(11).ToString();
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
                    Reader.Close();
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
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "INSERT INTO gqz_user_responses (";
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
                if (!string.IsNullOrEmpty(userResponse.UserId))
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
                    cmd.Parameters.AddWithValue("@userId", userResponse.UserId);
                }
                if (!string.IsNullOrEmpty(userResponse.QuizId))
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
                    cmd.Parameters.AddWithValue("@quizId", userResponse.QuizId);
                }
                if (!string.IsNullOrEmpty(userResponse.SubjectCode))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `subject_code` ");
                        values = string.Concat(values, ", @subjectCode");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `subject_code` ");
                        values = string.Concat(values, " @subjectCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subjectCode", userResponse.SubjectCode);
                }
                if (!string.IsNullOrEmpty(userResponse.LevelCode))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `level_code` ");
                        values = string.Concat(values, ", @levelCode");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `level_code` ");
                        values = string.Concat(values, " @levelCode");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@levelCode", userResponse.LevelCode);
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
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userResponseList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO gqz_user_responses (";
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
                                if (!string.IsNullOrEmpty(userResponseList[i].UserId))
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
                                    cmd.Parameters.AddWithValue("@userId" + i, userResponseList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].QuizId))
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
                                    cmd.Parameters.AddWithValue("@quizId" + i, userResponseList[i].QuizId);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].SubjectCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `subject_code` ");
                                        values = string.Concat(values, ", @subjectCode" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `subject_code` ");
                                        values = string.Concat(values, "@subjectCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, userResponseList[i].SubjectCode);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].LevelCode))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `level_code` ");
                                        values = string.Concat(values, ", @levelCode" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `level_code` ");
                                        values = string.Concat(values, "@levelCode" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@levelCode" + i, userResponseList[i].LevelCode);
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
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "UPDATE gqz_user_responses SET ";
                if (!string.IsNullOrEmpty(userResponse.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", userResponse.Id);
                }
                if (!string.IsNullOrEmpty(userResponse.UserId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `user_id` = @userId");
                    else
                        sql = string.Concat(sql, " `user_id` = @userId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@userId", userResponse.UserId);
                }
                if (!string.IsNullOrEmpty(userResponse.QuizId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `quiz_id` = @quizId");
                    else
                        sql = string.Concat(sql, " `quiz_id` = @quizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", userResponse.QuizId);
                }
                if (!string.IsNullOrEmpty(userResponse.SubjectCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `subject_code` = @subjectCode");
                    else
                        sql = string.Concat(sql, " `subject_code` = @subjectCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@subjectCode", userResponse.SubjectCode);
                }
                if (!string.IsNullOrEmpty(userResponse.LevelCode))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `level_code` = @levelCode");
                    else
                        sql = string.Concat(sql, " `level_code` = @levelCode");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@levelCode", userResponse.LevelCode);
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
                if (!string.IsNullOrEmpty(userResponse.ResponseTime))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `response_time` = @responseTime");
                    else
                        sql = string.Concat(sql, " `response_time` = @responseTime");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@responseTime", userResponse.ResponseTime);
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
                    if (!string.IsNullOrEmpty(userResponseWhere.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere");
                        else
                            sql = string.Concat(sql, " `user_id` = @userIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userIdWhere", userResponseWhere.UserId);
                    }
                    if (!string.IsNullOrEmpty(userResponseWhere.QuizId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `quiz_id` = @quizIdWhere");
                        else
                            sql = string.Concat(sql, " `quiz_id` = @quizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizIdWhere", userResponseWhere.QuizId);
                    }
                    if (!string.IsNullOrEmpty(userResponseWhere.SubjectCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subject_code` = @subjectCodeWhere");
                        else
                            sql = string.Concat(sql, " `subject_code` = @subjectCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCodeWhere", userResponseWhere.SubjectCode);
                    }
                    if (!string.IsNullOrEmpty(userResponseWhere.LevelCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `level_code` = @levelCodeWhere");
                        else
                            sql = string.Concat(sql, " `level_code` = @levelCodeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelCodeWhere", userResponseWhere.LevelCode);
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
                    if (!string.IsNullOrEmpty(userResponseWhere.ResponseTime))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `response_time` = @responseTimeWhere");
                        else
                            sql = string.Concat(sql, " `response_time` = @responseTimeWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@responseTimeWhere", userResponseWhere.ResponseTime);
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
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userResponseList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE gqz_user_responses SET ";
                                if (!string.IsNullOrEmpty(userResponseList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, userResponseList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].UserId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `user_id` = @userId" + i);
                                    else
                                        sql = string.Concat(sql, " `user_id` = @userId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@userId" + i, userResponseList[i].UserId);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].QuizId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `quiz_id` = @quizId" + i);
                                    else
                                        sql = string.Concat(sql, " `quiz_id` = @quizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, userResponseList[i].QuizId);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].SubjectCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `subject_code` = @subjectCode" + i);
                                    else
                                        sql = string.Concat(sql, " `subject_code` = @subjectCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@subjectCode" + i, userResponseList[i].SubjectCode);
                                }
                                if (!string.IsNullOrEmpty(userResponseList[i].LevelCode))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `level_code` = @levelCode" + i);
                                    else
                                        sql = string.Concat(sql, " `level_code` = @levelCode" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@levelCode" + i, userResponseList[i].LevelCode);
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
                                if (!string.IsNullOrEmpty(userResponseList[i].ResponseTime))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `response_time` = @responseTime" + i);
                                    else
                                        sql = string.Concat(sql, " `response_time` = @responseTime" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@responseTime" + i, userResponseList[i].ResponseTime);
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
                                    if (!string.IsNullOrEmpty(userResponseWhereList[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userIdWhere" + i, userResponseWhereList[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userResponseWhereList[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `quiz_id` = @quizIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `quiz_id` = @quizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizIdWhere" + i, userResponseWhereList[i].QuizId);
                                    }
                                    if (!string.IsNullOrEmpty(userResponseWhereList[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subject_code` = @subjectCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `subject_code` = @subjectCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCodeWhere" + i, userResponseWhereList[i].SubjectCode);
                                    }
                                    if (!string.IsNullOrEmpty(userResponseWhereList[i].LevelCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `level_code` = @levelCodeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `level_code` = @levelCodeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelCodeWhere" + i, userResponseWhereList[i].LevelCode);
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
                                    if (!string.IsNullOrEmpty(userResponseWhereList[i].ResponseTime))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `response_time` = @responseTimeWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `response_time` = @responseTimeWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@responseTimeWhere" + i, userResponseWhereList[i].ResponseTime);
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
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "DELETE FROM gqz_user_responses  ";
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
                    if (!string.IsNullOrEmpty(userResponse.UserId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `user_id` = @userId");
                        else
                            sql = string.Concat(sql, " `user_id` = @userId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@userId", userResponse.UserId);
                    }
                    if (!string.IsNullOrEmpty(userResponse.QuizId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `quiz_id` = @quizId");
                        else
                            sql = string.Concat(sql, " `quiz_id` = @quizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizId", userResponse.QuizId);
                    }
                    if (!string.IsNullOrEmpty(userResponse.SubjectCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `subject_code` = @subjectCode");
                        else
                            sql = string.Concat(sql, " `subject_code` = @subjectCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@subjectCode", userResponse.SubjectCode);
                    }
                    if (!string.IsNullOrEmpty(userResponse.LevelCode))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `level_code` = @levelCode");
                        else
                            sql = string.Concat(sql, " `level_code` = @levelCode");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@levelCode", userResponse.LevelCode);
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
                    if (!string.IsNullOrEmpty(userResponse.ResponseTime))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `response_time` = @responseTime");
                        else
                            sql = string.Concat(sql, " `response_time` = @responseTime");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@responseTime", userResponse.ResponseTime);
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
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                using (MySqlTransaction trans = cmd.Connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < userResponse.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM gqz_user_responses  ";
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
                                    if (!string.IsNullOrEmpty(userResponse[i].UserId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `user_id` = @userId" + i);
                                        else
                                            sql = string.Concat(sql, " `user_id` = @userId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@userId" + i, userResponse[i].UserId);
                                    }
                                    if (!string.IsNullOrEmpty(userResponse[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `quiz_id` = @quizId" + i);
                                        else
                                            sql = string.Concat(sql, " `quiz_id` = @quizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizId" + i, userResponse[i].QuizId);
                                    }
                                    if (!string.IsNullOrEmpty(userResponse[i].SubjectCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `subject_code` = @subjectCode" + i);
                                        else
                                            sql = string.Concat(sql, " `subject_code` = @subjectCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@subjectCode" + i, userResponse[i].SubjectCode);
                                    }
                                    if (!string.IsNullOrEmpty(userResponse[i].LevelCode))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `level_code` = @levelCode" + i);
                                        else
                                            sql = string.Concat(sql, " `level_code` = @levelCode" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@levelCode" + i, userResponse[i].LevelCode);
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
                                    if (!string.IsNullOrEmpty(userResponse[i].ResponseTime))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `response_time` = @responseTime" + i);
                                        else
                                            sql = string.Concat(sql, " `response_time` = @responseTime" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@responseTime" + i, userResponse[i].ResponseTime);
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
