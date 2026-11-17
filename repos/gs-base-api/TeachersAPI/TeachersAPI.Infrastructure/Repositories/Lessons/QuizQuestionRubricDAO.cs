using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Lessons;

namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons
{
    class QuizQuestionRubricDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, QuizQuestionRubricModel> ReadD(string connectionString, QuizQuestionRubricModel quizQuestionRubric, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionRubricModel> response = new Dictionary<string, QuizQuestionRubricModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `question_id`, `rubric`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubrics ";
                {
                    if (quizQuestionRubric != null)
                    {
                        if (!string.IsNullOrEmpty(quizQuestionRubric.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestionRubric.Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.QuestionId))
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
                            cmd.Parameters.AddWithValue("@questionId", quizQuestionRubric.QuestionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.Rubric))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `rubric` = @rubric ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `rubric` = @rubric ");
                            }
                            cmd.Parameters.AddWithValue("@rubric", quizQuestionRubric.Rubric);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubric.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubric.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubric.UpdatedAt);
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
                                    quizQuestionRubric.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionRubric.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionRubric.QuestionId = reader.GetValue(1).ToString();
                                else
                                    quizQuestionRubric.QuestionId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionRubric.Rubric = reader.GetValue(2).ToString();
                                else
                                    quizQuestionRubric.Rubric = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionRubric.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizQuestionRubric.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionRubric.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizQuestionRubric.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionRubric.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizQuestionRubric.UpdatedAt = null;
                                response.Add(quizQuestionRubric.Id, quizQuestionRubric);
                                quizQuestionRubric = new QuizQuestionRubricModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizQuestionRubricModel>();
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
                response = new Dictionary<string, QuizQuestionRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizQuestionRubricModel> ReadD(string connectionString, QuizQuestionRubricModel quizQuestionRubric, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionRubricModel> response = new Dictionary<string, QuizQuestionRubricModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `question_id`, `rubric`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubrics ";
                {
                    if (quizQuestionRubric != null)
                    {
                        if (!string.IsNullOrEmpty(quizQuestionRubric.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestionRubric.Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.QuestionId))
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
                            cmd.Parameters.AddWithValue("@questionId", quizQuestionRubric.QuestionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.Rubric))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `rubric` = @rubric ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `rubric` = @rubric ");
                            }
                            cmd.Parameters.AddWithValue("@rubric", quizQuestionRubric.Rubric);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubric.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubric.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubric.UpdatedAt);
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
                                    quizQuestionRubric.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionRubric.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionRubric.QuestionId = reader.GetValue(1).ToString();
                                else
                                    quizQuestionRubric.QuestionId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionRubric.Rubric = reader.GetValue(2).ToString();
                                else
                                    quizQuestionRubric.Rubric = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionRubric.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizQuestionRubric.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionRubric.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizQuestionRubric.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionRubric.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizQuestionRubric.UpdatedAt = null;
                                response.Add(quizQuestionRubric.Id, quizQuestionRubric);
                                quizQuestionRubric = new QuizQuestionRubricModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizQuestionRubricModel>();
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
                response = new Dictionary<string, QuizQuestionRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizQuestionRubricModel> ReadD(string connectionString, List<QuizQuestionRubricModel> quizQuestionRubricWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionRubricModel> response = new Dictionary<string, QuizQuestionRubricModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `question_id`, `rubric`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubrics ";
                if (quizQuestionRubricWhere != null && quizQuestionRubricWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionRubricWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].QuestionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `question_id` = @questionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionId" + i, quizQuestionRubricWhere[i].QuestionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].Rubric))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `rubric` = @rubric" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@rubric" + i, quizQuestionRubricWhere[i].Rubric);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionRubricWhere.Count - 1)
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
                            QuizQuestionRubricModel quizQuestionRubric = new QuizQuestionRubricModel(); if (!reader.IsDBNull(0))
                                quizQuestionRubric.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionRubric.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionRubric.QuestionId = reader.GetValue(1).ToString();
                            else
                                quizQuestionRubric.QuestionId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionRubric.Rubric = reader.GetValue(2).ToString();
                            else
                                quizQuestionRubric.Rubric = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionRubric.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizQuestionRubric.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionRubric.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizQuestionRubric.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionRubric.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizQuestionRubric.UpdatedAt = null;
                            response.Add(quizQuestionRubric.Id, quizQuestionRubric);
                            quizQuestionRubric = new QuizQuestionRubricModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizQuestionRubricModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizQuestionRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizQuestionRubricModel> ReadD(string connectionString, List<QuizQuestionRubricModel> quizQuestionRubricWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionRubricModel> response = new Dictionary<string, QuizQuestionRubricModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `question_id`, `rubric`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubrics ";
                if (quizQuestionRubricWhere != null && quizQuestionRubricWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionRubricWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].QuestionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `question_id` = @questionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionId" + i, quizQuestionRubricWhere[i].QuestionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].Rubric))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `rubric` = @rubric" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@rubric" + i, quizQuestionRubricWhere[i].Rubric);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionRubricWhere.Count - 1)
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
                            QuizQuestionRubricModel quizQuestionRubric = new QuizQuestionRubricModel(); if (!reader.IsDBNull(0))
                                quizQuestionRubric.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionRubric.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionRubric.QuestionId = reader.GetValue(1).ToString();
                            else
                                quizQuestionRubric.QuestionId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionRubric.Rubric = reader.GetValue(2).ToString();
                            else
                                quizQuestionRubric.Rubric = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionRubric.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizQuestionRubric.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionRubric.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizQuestionRubric.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionRubric.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizQuestionRubric.UpdatedAt = null;
                            response.Add(quizQuestionRubric.Id, quizQuestionRubric);
                            quizQuestionRubric = new QuizQuestionRubricModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizQuestionRubricModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizQuestionRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionRubricModel> Read(string connectionString, QuizQuestionRubricModel quizQuestionRubric, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricModel> response = new List<QuizQuestionRubricModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `question_id`, `rubric`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubrics ";
                {
                    if (quizQuestionRubric != null)
                    {
                        if (!string.IsNullOrEmpty(quizQuestionRubric.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestionRubric.Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.QuestionId))
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
                            cmd.Parameters.AddWithValue("@questionId", quizQuestionRubric.QuestionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.Rubric))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `rubric` = @rubric ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `rubric` = @rubric ");
                            }
                            cmd.Parameters.AddWithValue("@rubric", quizQuestionRubric.Rubric);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubric.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubric.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubric.UpdatedAt);
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
                                    quizQuestionRubric.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionRubric.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionRubric.QuestionId = reader.GetValue(1).ToString();
                                else
                                    quizQuestionRubric.QuestionId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionRubric.Rubric = reader.GetValue(2).ToString();
                                else
                                    quizQuestionRubric.Rubric = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionRubric.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizQuestionRubric.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionRubric.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizQuestionRubric.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionRubric.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizQuestionRubric.UpdatedAt = null;
                                response.Add(quizQuestionRubric);
                                quizQuestionRubric = new QuizQuestionRubricModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizQuestionRubricModel>();
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
                response = new List<QuizQuestionRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionRubricModel> Read(string connectionString, QuizQuestionRubricModel quizQuestionRubric, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricModel> response = new List<QuizQuestionRubricModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `question_id`, `rubric`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubrics ";
                {
                    if (quizQuestionRubric != null)
                    {
                        if (!string.IsNullOrEmpty(quizQuestionRubric.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestionRubric.Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.QuestionId))
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
                            cmd.Parameters.AddWithValue("@questionId", quizQuestionRubric.QuestionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.Rubric))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `rubric` = @rubric ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `rubric` = @rubric ");
                            }
                            cmd.Parameters.AddWithValue("@rubric", quizQuestionRubric.Rubric);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubric.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubric.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubric.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubric.UpdatedAt);
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
                                    quizQuestionRubric.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionRubric.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionRubric.QuestionId = reader.GetValue(1).ToString();
                                else
                                    quizQuestionRubric.QuestionId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionRubric.Rubric = reader.GetValue(2).ToString();
                                else
                                    quizQuestionRubric.Rubric = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionRubric.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizQuestionRubric.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionRubric.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizQuestionRubric.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionRubric.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizQuestionRubric.UpdatedAt = null;
                                response.Add(quizQuestionRubric);
                                quizQuestionRubric = new QuizQuestionRubricModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizQuestionRubricModel>();
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
                response = new List<QuizQuestionRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionRubricModel> Read(string connectionString, List<QuizQuestionRubricModel> quizQuestionRubricWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricModel> response = new List<QuizQuestionRubricModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `question_id`, `rubric`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubrics ";
                if (quizQuestionRubricWhere != null && quizQuestionRubricWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionRubricWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].QuestionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `question_id` = @questionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionId" + i, quizQuestionRubricWhere[i].QuestionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].Rubric))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `rubric` = @rubric" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@rubric" + i, quizQuestionRubricWhere[i].Rubric);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionRubricWhere.Count - 1)
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
                            QuizQuestionRubricModel quizQuestionRubric = new QuizQuestionRubricModel(); if (!reader.IsDBNull(0))
                                quizQuestionRubric.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionRubric.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionRubric.QuestionId = reader.GetValue(1).ToString();
                            else
                                quizQuestionRubric.QuestionId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionRubric.Rubric = reader.GetValue(2).ToString();
                            else
                                quizQuestionRubric.Rubric = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionRubric.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizQuestionRubric.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionRubric.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizQuestionRubric.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionRubric.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizQuestionRubric.UpdatedAt = null;
                            response.Add(quizQuestionRubric);
                            quizQuestionRubric = new QuizQuestionRubricModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizQuestionRubricModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionRubricModel> Read(string connectionString, List<QuizQuestionRubricModel> quizQuestionRubricWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricModel> response = new List<QuizQuestionRubricModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `question_id`, `rubric`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubrics ";
                if (quizQuestionRubricWhere != null && quizQuestionRubricWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionRubricWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].QuestionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `question_id` = @questionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@questionId" + i, quizQuestionRubricWhere[i].QuestionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].Rubric))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `rubric` = @rubric" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@rubric" + i, quizQuestionRubricWhere[i].Rubric);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionRubricWhere.Count - 1)
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
                            QuizQuestionRubricModel quizQuestionRubric = new QuizQuestionRubricModel(); if (!reader.IsDBNull(0))
                                quizQuestionRubric.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionRubric.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionRubric.QuestionId = reader.GetValue(1).ToString();
                            else
                                quizQuestionRubric.QuestionId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionRubric.Rubric = reader.GetValue(2).ToString();
                            else
                                quizQuestionRubric.Rubric = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionRubric.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizQuestionRubric.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionRubric.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizQuestionRubric.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionRubric.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizQuestionRubric.UpdatedAt = null;
                            response.Add(quizQuestionRubric);
                            quizQuestionRubric = new QuizQuestionRubricModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizQuestionRubricModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionRubricModel> Create(string connectionString, QuizQuestionRubricModel quizQuestionRubric)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricModel> response = new List<QuizQuestionRubricModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO quiz_question_rubrics (";
                string values = "(";
                if (!string.IsNullOrEmpty(quizQuestionRubric.Id))
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
                    cmd.Parameters.AddWithValue("@id", quizQuestionRubric.Id);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubric.QuestionId))
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
                    cmd.Parameters.AddWithValue("@questionId", quizQuestionRubric.QuestionId);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubric.Rubric))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `rubric` ");
                        values = string.Concat(values, ", @rubric");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `rubric` ");
                        values = string.Concat(values, " @rubric");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@rubric", quizQuestionRubric.Rubric);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubric.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubric.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubric.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubric.CreatedAt);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubric.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubric.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                quizQuestionRubric.Id = cmd.LastInsertedId.ToString();
                response.Add(quizQuestionRubric);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricModel>();
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
        public List<QuizQuestionRubricModel> Create(string connectionString, List<QuizQuestionRubricModel> quizQuestionRubricList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricModel> response = new List<QuizQuestionRubricModel>();
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
                        for (int i = 0; i < quizQuestionRubricList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO quiz_question_rubrics (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(quizQuestionRubricList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricList[i].QuestionId))
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
                                    cmd.Parameters.AddWithValue("@questionId" + i, quizQuestionRubricList[i].QuestionId);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricList[i].Rubric))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `rubric` ");
                                        values = string.Concat(values, ", @rubric" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `rubric` ");
                                        values = string.Concat(values, "@rubric" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@rubric" + i, quizQuestionRubricList[i].Rubric);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(quizQuestionRubricList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionRubricModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionRubricModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricModel>();
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

        public List<QuizQuestionRubricModel> Update(string connectionString, QuizQuestionRubricModel quizQuestionRubric, QuizQuestionRubricModel quizQuestionRubricWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricModel> response = new List<QuizQuestionRubricModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE quiz_question_rubrics SET ";
                if (!string.IsNullOrEmpty(quizQuestionRubric.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", quizQuestionRubric.Id);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubric.QuestionId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `question_id` = @questionId");
                    else
                        sql = string.Concat(sql, " `question_id` = @questionId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@questionId", quizQuestionRubric.QuestionId);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubric.Rubric))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `rubric` = @rubric");
                    else
                        sql = string.Concat(sql, " `rubric` = @rubric");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@rubric", quizQuestionRubric.Rubric);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubric.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubric.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubric.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubric.CreatedAt);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubric.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubric.UpdatedAt);
                }
                if (quizQuestionRubricWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(quizQuestionRubricWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", quizQuestionRubricWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricWhere.QuestionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `question_id` = @questionIdWhere");
                        else
                            sql = string.Concat(sql, " `question_id` = @questionIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@questionIdWhere", quizQuestionRubricWhere.QuestionId);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricWhere.Rubric))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `rubric` = @rubricWhere");
                        else
                            sql = string.Concat(sql, " `rubric` = @rubricWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@rubricWhere", quizQuestionRubricWhere.Rubric);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", quizQuestionRubricWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", quizQuestionRubricWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", quizQuestionRubricWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(quizQuestionRubric);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricModel>();
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

        public List<QuizQuestionRubricModel> Update(string connectionString, List<QuizQuestionRubricModel> quizQuestionRubricList, List<QuizQuestionRubricModel> quizQuestionRubricWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricModel> response = new List<QuizQuestionRubricModel>();
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
                        for (int i = 0; i < quizQuestionRubricList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE quiz_question_rubrics SET ";
                                if (!string.IsNullOrEmpty(quizQuestionRubricList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricList[i].QuestionId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `question_id` = @questionId" + i);
                                    else
                                        sql = string.Concat(sql, " `question_id` = @questionId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@questionId" + i, quizQuestionRubricList[i].QuestionId);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricList[i].Rubric))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `rubric` = @rubric" + i);
                                    else
                                        sql = string.Concat(sql, " `rubric` = @rubric" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@rubric" + i, quizQuestionRubricList[i].Rubric);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricList[i].UpdatedAt);
                                }
                                if (quizQuestionRubricWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(quizQuestionRubricWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, quizQuestionRubricWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricWhereList[i].QuestionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `question_id` = @questionIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `question_id` = @questionIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@questionIdWhere" + i, quizQuestionRubricWhereList[i].QuestionId);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricWhereList[i].Rubric))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `rubric` = @rubricWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `rubric` = @rubricWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@rubricWhere" + i, quizQuestionRubricWhereList[i].Rubric);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, quizQuestionRubricWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, quizQuestionRubricWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, quizQuestionRubricWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizQuestionRubricList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionRubricModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionRubricModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricModel>();
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

        public List<QuizQuestionRubricModel> Delete(string connectionString, QuizQuestionRubricModel quizQuestionRubric)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricModel> response = new List<QuizQuestionRubricModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM quiz_question_rubrics  ";
                if (quizQuestionRubric != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(quizQuestionRubric.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", quizQuestionRubric.Id);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubric.QuestionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `question_id` = @questionId");
                        else
                            sql = string.Concat(sql, " `question_id` = @questionId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@questionId", quizQuestionRubric.QuestionId);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubric.Rubric))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `rubric` = @rubric");
                        else
                            sql = string.Concat(sql, " `rubric` = @rubric");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@rubric", quizQuestionRubric.Rubric);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubric.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubric.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubric.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubric.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubric.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubric.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(quizQuestionRubric);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricModel>();
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

        public List<QuizQuestionRubricModel> Delete(string connectionString, List<QuizQuestionRubricModel> quizQuestionRubric)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricModel> response = new List<QuizQuestionRubricModel>();
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
                        for (int i = 0; i < quizQuestionRubric.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM quiz_question_rubrics  ";
                                if (quizQuestionRubric[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(quizQuestionRubric[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubric[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubric[i].QuestionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `question_id` = @questionId" + i);
                                        else
                                            sql = string.Concat(sql, " `question_id` = @questionId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@questionId" + i, quizQuestionRubric[i].QuestionId);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubric[i].Rubric))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `rubric` = @rubric" + i);
                                        else
                                            sql = string.Concat(sql, " `rubric` = @rubric" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@rubric" + i, quizQuestionRubric[i].Rubric);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubric[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubric[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubric[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubric[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubric[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubric[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizQuestionRubric[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionRubricModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionRubricModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricModel>();
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
