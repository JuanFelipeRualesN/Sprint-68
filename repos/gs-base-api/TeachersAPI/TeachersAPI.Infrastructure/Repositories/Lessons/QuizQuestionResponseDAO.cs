using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Lessons;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons
{
    class QuizQuestionResponseDAO
    {
        private MySqlConnection _connection;

        public Dictionary<string, QuizQuestionResponseModel> ReadD(string connectionString, QuizQuestionResponseModel quizQuestionResponse, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionResponseModel> response = new Dictionary<string, QuizQuestionResponseModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `quiz_question_id`, `response`, `correct`, `value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_responses ";
                {
                    if (quizQuestionResponse != null)
                    {
                        if (!string.IsNullOrEmpty(quizQuestionResponse.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestionResponse.Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponse.QuizQuestionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quiz_question_id` = @quizQuestionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quiz_question_id` = @quizQuestionId ");
                            }
                            cmd.Parameters.AddWithValue("@quizQuestionId", quizQuestionResponse.QuizQuestionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponse.Response))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `response` = @response ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `response` = @response ");
                            }
                            cmd.Parameters.AddWithValue("@response", quizQuestionResponse.Response);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponse.Correct))
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
                            cmd.Parameters.AddWithValue("@correct", quizQuestionResponse.Correct);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponse.Value))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `value` = @value ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `value` = @value ");
                            }
                            cmd.Parameters.AddWithValue("@value", quizQuestionResponse.Value);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponse.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionResponse.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponse.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionResponse.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponse.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionResponse.UpdatedAt);
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
                                    quizQuestionResponse.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionResponse.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionResponse.QuizQuestionId = reader.GetValue(1).ToString();
                                else
                                    quizQuestionResponse.QuizQuestionId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionResponse.Response = reader.GetValue(2).ToString();
                                else
                                    quizQuestionResponse.Response = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionResponse.Correct = reader.GetValue(3).ToString();
                                else
                                    quizQuestionResponse.Correct = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionResponse.Value = reader.GetValue(4).ToString();
                                else
                                    quizQuestionResponse.Value = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionResponse.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    quizQuestionResponse.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    quizQuestionResponse.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    quizQuestionResponse.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    quizQuestionResponse.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    quizQuestionResponse.UpdatedAt = null;
                                response.Add(quizQuestionResponse.Id, quizQuestionResponse);
                                quizQuestionResponse = new QuizQuestionResponseModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizQuestionResponseModel>();
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
                response = new Dictionary<string, QuizQuestionResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizQuestionResponseModel> ReadD(string connectionString, List<QuizQuestionResponseModel> quizQuestionResponseWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionResponseModel> response = new Dictionary<string, QuizQuestionResponseModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_question_id`, `response`, `correct`, `value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_responses ";
                if (quizQuestionResponseWhere != null && quizQuestionResponseWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionResponseWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(quizQuestionResponseWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionResponseWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponseWhere[i].QuizQuestionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_question_id` = @quizQuestionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizQuestionId" + i, quizQuestionResponseWhere[i].QuizQuestionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponseWhere[i].Response))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response` = @response" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@response" + i, quizQuestionResponseWhere[i].Response);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponseWhere[i].Correct))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `correct` = @correct" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@correct" + i, quizQuestionResponseWhere[i].Correct);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponseWhere[i].Value))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `value` = @value" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@value" + i, quizQuestionResponseWhere[i].Value);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponseWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionResponseWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponseWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionResponseWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponseWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionResponseWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionResponseWhere.Count - 1)
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
                            QuizQuestionResponseModel quizQuestionResponse = new QuizQuestionResponseModel(); if (!reader.IsDBNull(0))
                                quizQuestionResponse.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionResponse.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionResponse.QuizQuestionId = reader.GetValue(1).ToString();
                            else
                                quizQuestionResponse.QuizQuestionId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionResponse.Response = reader.GetValue(2).ToString();
                            else
                                quizQuestionResponse.Response = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionResponse.Correct = reader.GetValue(3).ToString();
                            else
                                quizQuestionResponse.Correct = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionResponse.Value = reader.GetValue(4).ToString();
                            else
                                quizQuestionResponse.Value = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionResponse.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                quizQuestionResponse.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                quizQuestionResponse.CreatedAt = reader.GetValue(6).ToString();
                            else
                                quizQuestionResponse.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                quizQuestionResponse.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                quizQuestionResponse.UpdatedAt = null;
                            response.Add(quizQuestionResponse.Id, quizQuestionResponse);
                            quizQuestionResponse = new QuizQuestionResponseModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizQuestionResponseModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizQuestionResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        public List<QuizQuestionResponseModel> Read(string connectionString, QuizQuestionResponseModel quizQuestionResponse, bool firstOne, bool orderByLastDate, QuizQuestionResponseModel quizQuestionResponseWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionResponseModel> response = new List<QuizQuestionResponseModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_question_id`, `response`, `correct`, `value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_responses ";
                {
                    if (quizQuestionResponse != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestionResponse.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestionResponse.Id);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponse.QuizQuestionId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_question_id` = @quizQuestionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_question_id` = @quizQuestionId ");
                            }
                            cmd.Parameters.AddWithValue("@quizQuestionId", quizQuestionResponse.QuizQuestionId);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponse.Response))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `response` = @response ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `response` = @response ");
                            }
                            cmd.Parameters.AddWithValue("@response", quizQuestionResponse.Response);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponse.Correct))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `correct` = @correct ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `correct` = @correct ");
                            }
                            cmd.Parameters.AddWithValue("@correct", quizQuestionResponse.Correct);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponse.Value))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `value` = @value ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `value` = @value ");
                            }
                            cmd.Parameters.AddWithValue("@value", quizQuestionResponse.Value);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponse.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionResponse.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponse.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionResponse.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponse.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionResponse.UpdatedAt);
                        }
                    }
                    if (quizQuestionResponseWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizQuestionResponseWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhereLike.QuizQuestionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_question_id` LIKE @quizQuestionIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_question_id` LIKE @quizQuestionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizQuestionIdWhereLike", "%" + quizQuestionResponseWhereLike.QuizQuestionId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhereLike.Response))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `response` LIKE @responseWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `response` LIKE @responseWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@responseWhereLike", "%" + quizQuestionResponseWhereLike.Response + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhereLike.Correct))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `correct` LIKE @correctWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `correct` LIKE @correctWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@correctWhereLike", "%" + quizQuestionResponseWhereLike.Correct + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhereLike.Value))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `value` LIKE @valueWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `value` LIKE @valueWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@valueWhereLike", "%" + quizQuestionResponseWhereLike.Value + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizQuestionResponseWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizQuestionResponseWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizQuestionResponseWhereLike.UpdatedAt + "%");
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
                                quizQuestionResponse = new QuizQuestionResponseModel();
                                if (!reader.IsDBNull(0))
                                    quizQuestionResponse.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionResponse.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionResponse.QuizQuestionId = reader.GetValue(1).ToString();
                                else
                                    quizQuestionResponse.QuizQuestionId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionResponse.Response = reader.GetValue(2).ToString();
                                else
                                    quizQuestionResponse.Response = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionResponse.Correct = reader.GetValue(3).ToString();
                                else
                                    quizQuestionResponse.Correct = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionResponse.Value = reader.GetValue(4).ToString();
                                else
                                    quizQuestionResponse.Value = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionResponse.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    quizQuestionResponse.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    quizQuestionResponse.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    quizQuestionResponse.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    quizQuestionResponse.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    quizQuestionResponse.UpdatedAt = null;
                                response.Add(quizQuestionResponse);
                                quizQuestionResponse = new QuizQuestionResponseModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizQuestionResponseModel>();
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
                response = new List<QuizQuestionResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionResponseModel> Read(string connectionString, QuizQuestionResponseModel quizQuestionResponse, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionResponseModel> response = new List<QuizQuestionResponseModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = _connection;
                string sql = "SELECT `id`, `quiz_question_id`, `response`, `correct`, `value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_responses ";
                {
                    if (quizQuestionResponse != null)
                    {
                        if (!string.IsNullOrEmpty(quizQuestionResponse.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestionResponse.Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponse.QuizQuestionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quiz_question_id` = @quizQuestionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quiz_question_id` = @quizQuestionId ");
                            }
                            cmd.Parameters.AddWithValue("@quizQuestionId", quizQuestionResponse.QuizQuestionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponse.Response))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `response` = @response ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `response` = @response ");
                            }
                            cmd.Parameters.AddWithValue("@response", quizQuestionResponse.Response);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponse.Correct))
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
                            cmd.Parameters.AddWithValue("@correct", quizQuestionResponse.Correct);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponse.Value))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `value` = @value ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `value` = @value ");
                            }
                            cmd.Parameters.AddWithValue("@value", quizQuestionResponse.Value);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponse.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionResponse.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponse.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionResponse.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionResponse.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionResponse.UpdatedAt);
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
                                    quizQuestionResponse.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionResponse.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionResponse.QuizQuestionId = reader.GetValue(1).ToString();
                                else
                                    quizQuestionResponse.QuizQuestionId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionResponse.Response = reader.GetValue(2).ToString();
                                else
                                    quizQuestionResponse.Response = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionResponse.Correct = reader.GetValue(3).ToString();
                                else
                                    quizQuestionResponse.Correct = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionResponse.Value = reader.GetValue(4).ToString();
                                else
                                    quizQuestionResponse.Value = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionResponse.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    quizQuestionResponse.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    quizQuestionResponse.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    quizQuestionResponse.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    quizQuestionResponse.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    quizQuestionResponse.UpdatedAt = null;
                                response.Add(quizQuestionResponse);
                                quizQuestionResponse = new QuizQuestionResponseModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizQuestionResponseModel>();
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
                response = new List<QuizQuestionResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }

        public List<QuizQuestionResponseModel> Read(string connectionString, QuizQuestionResponseModel quizQuestionResponse, bool orderByLastDate, int fromRow, int limit, QuizQuestionResponseModel quizQuestionResponseWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionResponseModel> response = new List<QuizQuestionResponseModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_question_id`, `response`, `correct`, `value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_responses ";
                {
                    if (quizQuestionResponse != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestionResponse.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestionResponse.Id);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponse.QuizQuestionId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_question_id` = @quizQuestionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_question_id` = @quizQuestionId ");
                            }
                            cmd.Parameters.AddWithValue("@quizQuestionId", quizQuestionResponse.QuizQuestionId);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponse.Response))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `response` = @response ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `response` = @response ");
                            }
                            cmd.Parameters.AddWithValue("@response", quizQuestionResponse.Response);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponse.Correct))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `correct` = @correct ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `correct` = @correct ");
                            }
                            cmd.Parameters.AddWithValue("@correct", quizQuestionResponse.Correct);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponse.Value))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `value` = @value ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `value` = @value ");
                            }
                            cmd.Parameters.AddWithValue("@value", quizQuestionResponse.Value);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponse.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionResponse.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponse.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionResponse.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponse.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionResponse.UpdatedAt);
                        }
                    }
                    if (quizQuestionResponseWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizQuestionResponseWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhereLike.QuizQuestionId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_question_id` LIKE @quizQuestionIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_question_id` LIKE @quizQuestionIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizQuestionIdWhereLike", "%" + quizQuestionResponseWhereLike.QuizQuestionId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhereLike.Response))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `response` LIKE @responseWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `response` LIKE @responseWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@responseWhereLike", "%" + quizQuestionResponseWhereLike.Response + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhereLike.Correct))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `correct` LIKE @correctWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `correct` LIKE @correctWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@correctWhereLike", "%" + quizQuestionResponseWhereLike.Correct + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhereLike.Value))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `value` LIKE @valueWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `value` LIKE @valueWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@valueWhereLike", "%" + quizQuestionResponseWhereLike.Value + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizQuestionResponseWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizQuestionResponseWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizQuestionResponseWhereLike.UpdatedAt + "%");
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
                                quizQuestionResponse = new QuizQuestionResponseModel();
                                if (!reader.IsDBNull(0))
                                    quizQuestionResponse.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionResponse.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionResponse.QuizQuestionId = reader.GetValue(1).ToString();
                                else
                                    quizQuestionResponse.QuizQuestionId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionResponse.Response = reader.GetValue(2).ToString();
                                else
                                    quizQuestionResponse.Response = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionResponse.Correct = reader.GetValue(3).ToString();
                                else
                                    quizQuestionResponse.Correct = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionResponse.Value = reader.GetValue(4).ToString();
                                else
                                    quizQuestionResponse.Value = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionResponse.ModifiedBy = reader.GetValue(5).ToString();
                                else
                                    quizQuestionResponse.ModifiedBy = null;
                                if (!reader.IsDBNull(6))
                                    quizQuestionResponse.CreatedAt = reader.GetValue(6).ToString();
                                else
                                    quizQuestionResponse.CreatedAt = null;
                                if (!reader.IsDBNull(7))
                                    quizQuestionResponse.UpdatedAt = reader.GetValue(7).ToString();
                                else
                                    quizQuestionResponse.UpdatedAt = null;
                                response.Add(quizQuestionResponse);
                                quizQuestionResponse = new QuizQuestionResponseModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizQuestionResponseModel>();
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
                response = new List<QuizQuestionResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionResponseModel> Read(string connectionString, List<QuizQuestionResponseModel> quizQuestionResponseWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionResponseModel> response = new List<QuizQuestionResponseModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_question_id`, `response`, `correct`, `value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_responses ";
                if (quizQuestionResponseWhere != null && quizQuestionResponseWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionResponseWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionResponseWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhere[i].QuizQuestionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_question_id` = @quizQuestionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizQuestionId" + i, quizQuestionResponseWhere[i].QuizQuestionId);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhere[i].Response))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response` = @response" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@response" + i, quizQuestionResponseWhere[i].Response);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhere[i].Correct))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `correct` = @correct" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@correct" + i, quizQuestionResponseWhere[i].Correct);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhere[i].Value))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `value` = @value" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@value" + i, quizQuestionResponseWhere[i].Value);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionResponseWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionResponseWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionResponseWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionResponseWhere.Count - 1)
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
                            QuizQuestionResponseModel quizQuestionResponse = new QuizQuestionResponseModel(); if (!reader.IsDBNull(0))
                                quizQuestionResponse.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionResponse.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionResponse.QuizQuestionId = reader.GetValue(1).ToString();
                            else
                                quizQuestionResponse.QuizQuestionId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionResponse.Response = reader.GetValue(2).ToString();
                            else
                                quizQuestionResponse.Response = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionResponse.Correct = reader.GetValue(3).ToString();
                            else
                                quizQuestionResponse.Correct = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionResponse.Value = reader.GetValue(4).ToString();
                            else
                                quizQuestionResponse.Value = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionResponse.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                quizQuestionResponse.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                quizQuestionResponse.CreatedAt = reader.GetValue(6).ToString();
                            else
                                quizQuestionResponse.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                quizQuestionResponse.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                quizQuestionResponse.UpdatedAt = null;
                            response.Add(quizQuestionResponse);
                            quizQuestionResponse = new QuizQuestionResponseModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizQuestionResponseModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionResponseModel> Read(string connectionString, List<QuizQuestionResponseModel> quizQuestionResponseWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionResponseModel> response = new List<QuizQuestionResponseModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_question_id`, `response`, `correct`, `value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_responses ";
                if (quizQuestionResponseWhere != null && quizQuestionResponseWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionResponseWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionResponseWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhere[i].QuizQuestionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_question_id` = @quizQuestionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizQuestionId" + i, quizQuestionResponseWhere[i].QuizQuestionId);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhere[i].Response))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `response` = @response" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@response" + i, quizQuestionResponseWhere[i].Response);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhere[i].Correct))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `correct` = @correct" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@correct" + i, quizQuestionResponseWhere[i].Correct);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhere[i].Value))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `value` = @value" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@value" + i, quizQuestionResponseWhere[i].Value);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionResponseWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionResponseWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizQuestionResponseWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionResponseWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionResponseWhere.Count - 1)
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
                            QuizQuestionResponseModel quizQuestionResponse = new QuizQuestionResponseModel(); if (!reader.IsDBNull(0))
                                quizQuestionResponse.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionResponse.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionResponse.QuizQuestionId = reader.GetValue(1).ToString();
                            else
                                quizQuestionResponse.QuizQuestionId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionResponse.Response = reader.GetValue(2).ToString();
                            else
                                quizQuestionResponse.Response = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionResponse.Correct = reader.GetValue(3).ToString();
                            else
                                quizQuestionResponse.Correct = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionResponse.Value = reader.GetValue(4).ToString();
                            else
                                quizQuestionResponse.Value = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionResponse.ModifiedBy = reader.GetValue(5).ToString();
                            else
                                quizQuestionResponse.ModifiedBy = null;
                            if (!reader.IsDBNull(6))
                                quizQuestionResponse.CreatedAt = reader.GetValue(6).ToString();
                            else
                                quizQuestionResponse.CreatedAt = null;
                            if (!reader.IsDBNull(7))
                                quizQuestionResponse.UpdatedAt = reader.GetValue(7).ToString();
                            else
                                quizQuestionResponse.UpdatedAt = null;
                            response.Add(quizQuestionResponse);
                            quizQuestionResponse = new QuizQuestionResponseModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizQuestionResponseModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionResponseModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionResponseModel> Create(string connectionString, QuizQuestionResponseModel quizQuestionResponse)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionResponseModel> response = new List<QuizQuestionResponseModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO quiz_question_responses (";
                string values = "(";
                if (!String.IsNullOrEmpty(quizQuestionResponse.Id))
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
                    cmd.Parameters.AddWithValue("@id", quizQuestionResponse.Id);
                }
                if (!String.IsNullOrEmpty(quizQuestionResponse.QuizQuestionId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `quiz_question_id` ");
                        values = String.Concat(values, ", @quizQuestionId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `quiz_question_id` ");
                        values = String.Concat(values, " @quizQuestionId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizQuestionId", quizQuestionResponse.QuizQuestionId);
                }
                if (!String.IsNullOrEmpty(quizQuestionResponse.Response))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `response` ");
                        values = String.Concat(values, ", @response");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `response` ");
                        values = String.Concat(values, " @response");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@response", quizQuestionResponse.Response);
                }
                if (!String.IsNullOrEmpty(quizQuestionResponse.Correct))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `correct` ");
                        values = String.Concat(values, ", @correct");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `correct` ");
                        values = String.Concat(values, " @correct");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@correct", quizQuestionResponse.Correct);
                }
                if (!String.IsNullOrEmpty(quizQuestionResponse.Value))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `value` ");
                        values = String.Concat(values, ", @value");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `value` ");
                        values = String.Concat(values, " @value");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@value", quizQuestionResponse.Value);
                }
                if (!String.IsNullOrEmpty(quizQuestionResponse.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionResponse.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(quizQuestionResponse.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", quizQuestionResponse.CreatedAt);
                }
                if (!String.IsNullOrEmpty(quizQuestionResponse.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", quizQuestionResponse.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                quizQuestionResponse.Id = cmd.LastInsertedId.ToString();
                response.Add(quizQuestionResponse);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionResponseModel>();
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
        public List<QuizQuestionResponseModel> Create(string connectionString, List<QuizQuestionResponseModel> quizQuestionResponseList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionResponseModel> response = new List<QuizQuestionResponseModel>();
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
                        for (int i = 0; i < quizQuestionResponseList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO quiz_question_responses (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(quizQuestionResponseList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, quizQuestionResponseList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionResponseList[i].QuizQuestionId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `quiz_question_id` ");
                                        values = String.Concat(values, ", @quizQuestionId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `quiz_question_id` ");
                                        values = String.Concat(values, "@quizQuestionId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizQuestionId" + i, quizQuestionResponseList[i].QuizQuestionId);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionResponseList[i].Response))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `response` ");
                                        values = String.Concat(values, ", @response" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `response` ");
                                        values = String.Concat(values, "@response" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@response" + i, quizQuestionResponseList[i].Response);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionResponseList[i].Correct))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `correct` ");
                                        values = String.Concat(values, ", @correct" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `correct` ");
                                        values = String.Concat(values, "@correct" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@correct" + i, quizQuestionResponseList[i].Correct);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionResponseList[i].Value))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `value` ");
                                        values = String.Concat(values, ", @value" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `value` ");
                                        values = String.Concat(values, "@value" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@value" + i, quizQuestionResponseList[i].Value);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionResponseList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionResponseList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionResponseList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionResponseList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionResponseList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionResponseList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(quizQuestionResponseList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionResponseModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionResponseModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionResponseModel>();
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

        public List<QuizQuestionResponseModel> Update(string connectionString, QuizQuestionResponseModel quizQuestionResponse, QuizQuestionResponseModel quizQuestionResponseWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionResponseModel> response = new List<QuizQuestionResponseModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE quiz_question_responses SET ";
                if (!String.IsNullOrEmpty(quizQuestionResponse.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", quizQuestionResponse.Id);
                }
                if (!String.IsNullOrEmpty(quizQuestionResponse.QuizQuestionId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `quiz_question_id` = @quizQuestionId");
                    else
                        sql = String.Concat(sql, " `quiz_question_id` = @quizQuestionId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizQuestionId", quizQuestionResponse.QuizQuestionId);
                }
                if (!String.IsNullOrEmpty(quizQuestionResponse.Response))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `response` = @response");
                    else
                        sql = String.Concat(sql, " `response` = @response");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@response", quizQuestionResponse.Response);
                }
                if (!String.IsNullOrEmpty(quizQuestionResponse.Correct))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `correct` = @correct");
                    else
                        sql = String.Concat(sql, " `correct` = @correct");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@correct", quizQuestionResponse.Correct);
                }
                if (!String.IsNullOrEmpty(quizQuestionResponse.Value))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `value` = @value");
                    else
                        sql = String.Concat(sql, " `value` = @value");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@value", quizQuestionResponse.Value);
                }
                if (!String.IsNullOrEmpty(quizQuestionResponse.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionResponse.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(quizQuestionResponse.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", quizQuestionResponse.CreatedAt);
                }
                if (!String.IsNullOrEmpty(quizQuestionResponse.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", quizQuestionResponse.UpdatedAt);
                }
                if (quizQuestionResponseWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(quizQuestionResponseWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", quizQuestionResponseWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionResponseWhere.QuizQuestionId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_question_id` = @quizQuestionIdWhere");
                        else
                            sql = String.Concat(sql, " `quiz_question_id` = @quizQuestionIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizQuestionIdWhere", quizQuestionResponseWhere.QuizQuestionId);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionResponseWhere.Response))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `response` = @responseWhere");
                        else
                            sql = String.Concat(sql, " `response` = @responseWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@responseWhere", quizQuestionResponseWhere.Response);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionResponseWhere.Correct))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `correct` = @correctWhere");
                        else
                            sql = String.Concat(sql, " `correct` = @correctWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@correctWhere", quizQuestionResponseWhere.Correct);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionResponseWhere.Value))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `value` = @valueWhere");
                        else
                            sql = String.Concat(sql, " `value` = @valueWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@valueWhere", quizQuestionResponseWhere.Value);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionResponseWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", quizQuestionResponseWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionResponseWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", quizQuestionResponseWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionResponseWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", quizQuestionResponseWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(quizQuestionResponse);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionResponseModel>();
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

        public List<QuizQuestionResponseModel> Update(string connectionString, List<QuizQuestionResponseModel> quizQuestionResponseList, List<QuizQuestionResponseModel> quizQuestionResponseWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionResponseModel> response = new List<QuizQuestionResponseModel>();
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
                        for (int i = 0; i < quizQuestionResponseList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE quiz_question_responses SET ";
                                if (!String.IsNullOrEmpty(quizQuestionResponseList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, quizQuestionResponseList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionResponseList[i].QuizQuestionId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `quiz_question_id` = @quizQuestionId" + i);
                                    else
                                        sql = String.Concat(sql, " `quiz_question_id` = @quizQuestionId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizQuestionId" + i, quizQuestionResponseList[i].QuizQuestionId);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionResponseList[i].Response))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `response` = @response" + i);
                                    else
                                        sql = String.Concat(sql, " `response` = @response" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@response" + i, quizQuestionResponseList[i].Response);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionResponseList[i].Correct))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `correct` = @correct" + i);
                                    else
                                        sql = String.Concat(sql, " `correct` = @correct" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@correct" + i, quizQuestionResponseList[i].Correct);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionResponseList[i].Value))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `value` = @value" + i);
                                    else
                                        sql = String.Concat(sql, " `value` = @value" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@value" + i, quizQuestionResponseList[i].Value);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionResponseList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionResponseList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionResponseList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionResponseList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(quizQuestionResponseList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionResponseList[i].UpdatedAt);
                                }
                                if (quizQuestionResponseWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(quizQuestionResponseWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, quizQuestionResponseWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionResponseWhereList[i].QuizQuestionId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_question_id` = @quizQuestionIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_question_id` = @quizQuestionIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizQuestionIdWhere" + i, quizQuestionResponseWhereList[i].QuizQuestionId);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionResponseWhereList[i].Response))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `response` = @responseWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `response` = @responseWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@responseWhere" + i, quizQuestionResponseWhereList[i].Response);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionResponseWhereList[i].Correct))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `correct` = @correctWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `correct` = @correctWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@correctWhere" + i, quizQuestionResponseWhereList[i].Correct);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionResponseWhereList[i].Value))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `value` = @valueWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `value` = @valueWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@valueWhere" + i, quizQuestionResponseWhereList[i].Value);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionResponseWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, quizQuestionResponseWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionResponseWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, quizQuestionResponseWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionResponseWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, quizQuestionResponseWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizQuestionResponseList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionResponseModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionResponseModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionResponseModel>();
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

        public List<QuizQuestionResponseModel> Delete(string connectionString, QuizQuestionResponseModel quizQuestionResponse)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionResponseModel> response = new List<QuizQuestionResponseModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM quiz_question_responses  ";
                if (quizQuestionResponse != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(quizQuestionResponse.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", quizQuestionResponse.Id);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionResponse.QuizQuestionId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_question_id` = @quizQuestionId");
                        else
                            sql = String.Concat(sql, " `quiz_question_id` = @quizQuestionId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizQuestionId", quizQuestionResponse.QuizQuestionId);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionResponse.Response))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `response` = @response");
                        else
                            sql = String.Concat(sql, " `response` = @response");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@response", quizQuestionResponse.Response);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionResponse.Correct))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `correct` = @correct");
                        else
                            sql = String.Concat(sql, " `correct` = @correct");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@correct", quizQuestionResponse.Correct);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionResponse.Value))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `value` = @value");
                        else
                            sql = String.Concat(sql, " `value` = @value");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@value", quizQuestionResponse.Value);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionResponse.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionResponse.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionResponse.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", quizQuestionResponse.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(quizQuestionResponse.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", quizQuestionResponse.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(quizQuestionResponse);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionResponseModel>();
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

        public List<QuizQuestionResponseModel> Delete(string connectionString, List<QuizQuestionResponseModel> quizQuestionResponse)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionResponseModel> response = new List<QuizQuestionResponseModel>();
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
                        for (int i = 0; i < quizQuestionResponse.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM quiz_question_responses  ";
                                if (quizQuestionResponse[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(quizQuestionResponse[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, quizQuestionResponse[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionResponse[i].QuizQuestionId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_question_id` = @quizQuestionId" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_question_id` = @quizQuestionId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizQuestionId" + i, quizQuestionResponse[i].QuizQuestionId);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionResponse[i].Response))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `response` = @response" + i);
                                        else
                                            sql = String.Concat(sql, " `response` = @response" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@response" + i, quizQuestionResponse[i].Response);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionResponse[i].Correct))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `correct` = @correct" + i);
                                        else
                                            sql = String.Concat(sql, " `correct` = @correct" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@correct" + i, quizQuestionResponse[i].Correct);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionResponse[i].Value))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `value` = @value" + i);
                                        else
                                            sql = String.Concat(sql, " `value` = @value" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@value" + i, quizQuestionResponse[i].Value);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionResponse[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionResponse[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionResponse[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionResponse[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(quizQuestionResponse[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionResponse[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizQuestionResponse[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionResponseModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionResponseModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionResponseModel>();
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
