using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Lessons;

namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons
{
    class QuizQuestionRubricCriterionValueDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, QuizQuestionRubricCriterionValueModel> ReadD(string connectionString, QuizQuestionRubricCriterionValueModel quizQuestionRubricCriterionValue, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionRubricCriterionValueModel> response = new Dictionary<string, QuizQuestionRubricCriterionValueModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_question_rubric_criterion_id`, `desc`, `value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubric_criterion_values ";
                {
                    if (quizQuestionRubricCriterionValue != null)
                    {
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestionRubricCriterionValue.Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId ");
                            }
                            cmd.Parameters.AddWithValue("@quizQuestionRubricCriterionId", quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", quizQuestionRubricCriterionValue.Desc);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Value))
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
                            cmd.Parameters.AddWithValue("@value", quizQuestionRubricCriterionValue.Value);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubricCriterionValue.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubricCriterionValue.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubricCriterionValue.UpdatedAt);
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
                                    quizQuestionRubricCriterionValue.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionRubricCriterionValue.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId = reader.GetValue(1).ToString();
                                else
                                    quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionRubricCriterionValue.Desc = reader.GetValue(2).ToString();
                                else
                                    quizQuestionRubricCriterionValue.Desc = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionRubricCriterionValue.Value = reader.GetValue(3).ToString();
                                else
                                    quizQuestionRubricCriterionValue.Value = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionRubricCriterionValue.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    quizQuestionRubricCriterionValue.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionRubricCriterionValue.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    quizQuestionRubricCriterionValue.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    quizQuestionRubricCriterionValue.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    quizQuestionRubricCriterionValue.UpdatedAt = null;
                                response.Add(quizQuestionRubricCriterionValue.Id, quizQuestionRubricCriterionValue);
                                quizQuestionRubricCriterionValue = new QuizQuestionRubricCriterionValueModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizQuestionRubricCriterionValueModel>();
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
                response = new Dictionary<string, QuizQuestionRubricCriterionValueModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizQuestionRubricCriterionValueModel> ReadD(string connectionString, QuizQuestionRubricCriterionValueModel quizQuestionRubricCriterionValue, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionRubricCriterionValueModel> response = new Dictionary<string, QuizQuestionRubricCriterionValueModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_question_rubric_criterion_id`, `desc`, `value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubric_criterion_values ";
                {
                    if (quizQuestionRubricCriterionValue != null)
                    {
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestionRubricCriterionValue.Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId ");
                            }
                            cmd.Parameters.AddWithValue("@quizQuestionRubricCriterionId", quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", quizQuestionRubricCriterionValue.Desc);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Value))
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
                            cmd.Parameters.AddWithValue("@value", quizQuestionRubricCriterionValue.Value);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubricCriterionValue.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubricCriterionValue.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubricCriterionValue.UpdatedAt);
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
                                    quizQuestionRubricCriterionValue.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionRubricCriterionValue.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId = reader.GetValue(1).ToString();
                                else
                                    quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionRubricCriterionValue.Desc = reader.GetValue(2).ToString();
                                else
                                    quizQuestionRubricCriterionValue.Desc = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionRubricCriterionValue.Value = reader.GetValue(3).ToString();
                                else
                                    quizQuestionRubricCriterionValue.Value = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionRubricCriterionValue.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    quizQuestionRubricCriterionValue.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionRubricCriterionValue.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    quizQuestionRubricCriterionValue.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    quizQuestionRubricCriterionValue.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    quizQuestionRubricCriterionValue.UpdatedAt = null;
                                response.Add(quizQuestionRubricCriterionValue.Id, quizQuestionRubricCriterionValue);
                                quizQuestionRubricCriterionValue = new QuizQuestionRubricCriterionValueModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizQuestionRubricCriterionValueModel>();
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
                response = new Dictionary<string, QuizQuestionRubricCriterionValueModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizQuestionRubricCriterionValueModel> ReadD(string connectionString, List<QuizQuestionRubricCriterionValueModel> quizQuestionRubricCriterionValueWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionRubricCriterionValueModel> response = new Dictionary<string, QuizQuestionRubricCriterionValueModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_question_rubric_criterion_id`, `desc`, `value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubric_criterion_values ";
                if (quizQuestionRubricCriterionValueWhere != null && quizQuestionRubricCriterionValueWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionRubricCriterionValueWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricCriterionValueWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].QuizQuestionRubricCriterionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizQuestionRubricCriterionId" + i, quizQuestionRubricCriterionValueWhere[i].QuizQuestionRubricCriterionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, quizQuestionRubricCriterionValueWhere[i].Desc);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].Value))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `value` = @value" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@value" + i, quizQuestionRubricCriterionValueWhere[i].Value);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricCriterionValueWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricCriterionValueWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricCriterionValueWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionRubricCriterionValueWhere.Count - 1)
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
                            QuizQuestionRubricCriterionValueModel quizQuestionRubricCriterionValue = new QuizQuestionRubricCriterionValueModel(); if (!reader.IsDBNull(0))
                                quizQuestionRubricCriterionValue.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionRubricCriterionValue.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId = reader.GetValue(1).ToString();
                            else
                                quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionRubricCriterionValue.Desc = reader.GetValue(2).ToString();
                            else
                                quizQuestionRubricCriterionValue.Desc = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionRubricCriterionValue.Value = reader.GetValue(3).ToString();
                            else
                                quizQuestionRubricCriterionValue.Value = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionRubricCriterionValue.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                quizQuestionRubricCriterionValue.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionRubricCriterionValue.CreatedAt = reader.GetValue(5).ToString();
                            else
                                quizQuestionRubricCriterionValue.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                quizQuestionRubricCriterionValue.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                quizQuestionRubricCriterionValue.UpdatedAt = null;
                            response.Add(quizQuestionRubricCriterionValue.Id, quizQuestionRubricCriterionValue);
                            quizQuestionRubricCriterionValue = new QuizQuestionRubricCriterionValueModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizQuestionRubricCriterionValueModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizQuestionRubricCriterionValueModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizQuestionRubricCriterionValueModel> ReadD(string connectionString, List<QuizQuestionRubricCriterionValueModel> quizQuestionRubricCriterionValueWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionRubricCriterionValueModel> response = new Dictionary<string, QuizQuestionRubricCriterionValueModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_question_rubric_criterion_id`, `desc`, `value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubric_criterion_values ";
                if (quizQuestionRubricCriterionValueWhere != null && quizQuestionRubricCriterionValueWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionRubricCriterionValueWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricCriterionValueWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].QuizQuestionRubricCriterionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizQuestionRubricCriterionId" + i, quizQuestionRubricCriterionValueWhere[i].QuizQuestionRubricCriterionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, quizQuestionRubricCriterionValueWhere[i].Desc);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].Value))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `value` = @value" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@value" + i, quizQuestionRubricCriterionValueWhere[i].Value);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricCriterionValueWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricCriterionValueWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricCriterionValueWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionRubricCriterionValueWhere.Count - 1)
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
                            QuizQuestionRubricCriterionValueModel quizQuestionRubricCriterionValue = new QuizQuestionRubricCriterionValueModel(); if (!reader.IsDBNull(0))
                                quizQuestionRubricCriterionValue.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionRubricCriterionValue.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId = reader.GetValue(1).ToString();
                            else
                                quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionRubricCriterionValue.Desc = reader.GetValue(2).ToString();
                            else
                                quizQuestionRubricCriterionValue.Desc = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionRubricCriterionValue.Value = reader.GetValue(3).ToString();
                            else
                                quizQuestionRubricCriterionValue.Value = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionRubricCriterionValue.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                quizQuestionRubricCriterionValue.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionRubricCriterionValue.CreatedAt = reader.GetValue(5).ToString();
                            else
                                quizQuestionRubricCriterionValue.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                quizQuestionRubricCriterionValue.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                quizQuestionRubricCriterionValue.UpdatedAt = null;
                            response.Add(quizQuestionRubricCriterionValue.Id, quizQuestionRubricCriterionValue);
                            quizQuestionRubricCriterionValue = new QuizQuestionRubricCriterionValueModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizQuestionRubricCriterionValueModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizQuestionRubricCriterionValueModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionRubricCriterionValueModel> Read(string connectionString, QuizQuestionRubricCriterionValueModel quizQuestionRubricCriterionValue, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionValueModel> response = new List<QuizQuestionRubricCriterionValueModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_question_rubric_criterion_id`, `desc`, `value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubric_criterion_values ";
                {
                    if (quizQuestionRubricCriterionValue != null)
                    {
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestionRubricCriterionValue.Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId ");
                            }
                            cmd.Parameters.AddWithValue("@quizQuestionRubricCriterionId", quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", quizQuestionRubricCriterionValue.Desc);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Value))
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
                            cmd.Parameters.AddWithValue("@value", quizQuestionRubricCriterionValue.Value);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubricCriterionValue.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubricCriterionValue.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubricCriterionValue.UpdatedAt);
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
                                    quizQuestionRubricCriterionValue.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionRubricCriterionValue.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId = reader.GetValue(1).ToString();
                                else
                                    quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionRubricCriterionValue.Desc = reader.GetValue(2).ToString();
                                else
                                    quizQuestionRubricCriterionValue.Desc = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionRubricCriterionValue.Value = reader.GetValue(3).ToString();
                                else
                                    quizQuestionRubricCriterionValue.Value = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionRubricCriterionValue.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    quizQuestionRubricCriterionValue.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionRubricCriterionValue.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    quizQuestionRubricCriterionValue.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    quizQuestionRubricCriterionValue.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    quizQuestionRubricCriterionValue.UpdatedAt = null;
                                response.Add(quizQuestionRubricCriterionValue);
                                quizQuestionRubricCriterionValue = new QuizQuestionRubricCriterionValueModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizQuestionRubricCriterionValueModel>();
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
                response = new List<QuizQuestionRubricCriterionValueModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionRubricCriterionValueModel> Read(string connectionString, QuizQuestionRubricCriterionValueModel quizQuestionRubricCriterionValue, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionValueModel> response = new List<QuizQuestionRubricCriterionValueModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_question_rubric_criterion_id`, `desc`, `value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubric_criterion_values ";
                {
                    if (quizQuestionRubricCriterionValue != null)
                    {
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestionRubricCriterionValue.Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId ");
                            }
                            cmd.Parameters.AddWithValue("@quizQuestionRubricCriterionId", quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", quizQuestionRubricCriterionValue.Desc);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Value))
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
                            cmd.Parameters.AddWithValue("@value", quizQuestionRubricCriterionValue.Value);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubricCriterionValue.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubricCriterionValue.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubricCriterionValue.UpdatedAt);
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
                                    quizQuestionRubricCriterionValue.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionRubricCriterionValue.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId = reader.GetValue(1).ToString();
                                else
                                    quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionRubricCriterionValue.Desc = reader.GetValue(2).ToString();
                                else
                                    quizQuestionRubricCriterionValue.Desc = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionRubricCriterionValue.Value = reader.GetValue(3).ToString();
                                else
                                    quizQuestionRubricCriterionValue.Value = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionRubricCriterionValue.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    quizQuestionRubricCriterionValue.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionRubricCriterionValue.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    quizQuestionRubricCriterionValue.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    quizQuestionRubricCriterionValue.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    quizQuestionRubricCriterionValue.UpdatedAt = null;
                                response.Add(quizQuestionRubricCriterionValue);
                                quizQuestionRubricCriterionValue = new QuizQuestionRubricCriterionValueModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizQuestionRubricCriterionValueModel>();
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
                response = new List<QuizQuestionRubricCriterionValueModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionRubricCriterionValueModel> Read(string connectionString, List<QuizQuestionRubricCriterionValueModel> quizQuestionRubricCriterionValueWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionValueModel> response = new List<QuizQuestionRubricCriterionValueModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_question_rubric_criterion_id`, `desc`, `value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubric_criterion_values ";
                if (quizQuestionRubricCriterionValueWhere != null && quizQuestionRubricCriterionValueWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionRubricCriterionValueWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricCriterionValueWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].QuizQuestionRubricCriterionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizQuestionRubricCriterionId" + i, quizQuestionRubricCriterionValueWhere[i].QuizQuestionRubricCriterionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, quizQuestionRubricCriterionValueWhere[i].Desc);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].Value))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `value` = @value" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@value" + i, quizQuestionRubricCriterionValueWhere[i].Value);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricCriterionValueWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricCriterionValueWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricCriterionValueWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionRubricCriterionValueWhere.Count - 1)
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
                            QuizQuestionRubricCriterionValueModel quizQuestionRubricCriterionValue = new QuizQuestionRubricCriterionValueModel(); if (!reader.IsDBNull(0))
                                quizQuestionRubricCriterionValue.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionRubricCriterionValue.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId = reader.GetValue(1).ToString();
                            else
                                quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionRubricCriterionValue.Desc = reader.GetValue(2).ToString();
                            else
                                quizQuestionRubricCriterionValue.Desc = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionRubricCriterionValue.Value = reader.GetValue(3).ToString();
                            else
                                quizQuestionRubricCriterionValue.Value = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionRubricCriterionValue.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                quizQuestionRubricCriterionValue.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionRubricCriterionValue.CreatedAt = reader.GetValue(5).ToString();
                            else
                                quizQuestionRubricCriterionValue.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                quizQuestionRubricCriterionValue.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                quizQuestionRubricCriterionValue.UpdatedAt = null;
                            response.Add(quizQuestionRubricCriterionValue);
                            quizQuestionRubricCriterionValue = new QuizQuestionRubricCriterionValueModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizQuestionRubricCriterionValueModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricCriterionValueModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionRubricCriterionValueModel> Read(string connectionString, List<QuizQuestionRubricCriterionValueModel> quizQuestionRubricCriterionValueWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionValueModel> response = new List<QuizQuestionRubricCriterionValueModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_question_rubric_criterion_id`, `desc`, `value`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubric_criterion_values ";
                if (quizQuestionRubricCriterionValueWhere != null && quizQuestionRubricCriterionValueWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionRubricCriterionValueWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricCriterionValueWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].QuizQuestionRubricCriterionId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizQuestionRubricCriterionId" + i, quizQuestionRubricCriterionValueWhere[i].QuizQuestionRubricCriterionId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, quizQuestionRubricCriterionValueWhere[i].Desc);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].Value))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `value` = @value" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@value" + i, quizQuestionRubricCriterionValueWhere[i].Value);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricCriterionValueWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricCriterionValueWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricCriterionValueWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionRubricCriterionValueWhere.Count - 1)
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
                            QuizQuestionRubricCriterionValueModel quizQuestionRubricCriterionValue = new QuizQuestionRubricCriterionValueModel(); if (!reader.IsDBNull(0))
                                quizQuestionRubricCriterionValue.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionRubricCriterionValue.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId = reader.GetValue(1).ToString();
                            else
                                quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionRubricCriterionValue.Desc = reader.GetValue(2).ToString();
                            else
                                quizQuestionRubricCriterionValue.Desc = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionRubricCriterionValue.Value = reader.GetValue(3).ToString();
                            else
                                quizQuestionRubricCriterionValue.Value = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionRubricCriterionValue.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                quizQuestionRubricCriterionValue.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionRubricCriterionValue.CreatedAt = reader.GetValue(5).ToString();
                            else
                                quizQuestionRubricCriterionValue.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                quizQuestionRubricCriterionValue.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                quizQuestionRubricCriterionValue.UpdatedAt = null;
                            response.Add(quizQuestionRubricCriterionValue);
                            quizQuestionRubricCriterionValue = new QuizQuestionRubricCriterionValueModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizQuestionRubricCriterionValueModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricCriterionValueModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionRubricCriterionValueModel> Create(string connectionString, QuizQuestionRubricCriterionValueModel quizQuestionRubricCriterionValue)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionValueModel> response = new List<QuizQuestionRubricCriterionValueModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO quiz_question_rubric_criterion_values (";
                string values = "(";
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Id))
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
                    cmd.Parameters.AddWithValue("@id", quizQuestionRubricCriterionValue.Id);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `quiz_question_rubric_criterion_id` ");
                        values = string.Concat(values, ", @quizQuestionRubricCriterionId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `quiz_question_rubric_criterion_id` ");
                        values = string.Concat(values, " @quizQuestionRubricCriterionId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizQuestionRubricCriterionId", quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Desc))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `desc` ");
                        values = string.Concat(values, ", @desc");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `desc` ");
                        values = string.Concat(values, " @desc");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", quizQuestionRubricCriterionValue.Desc);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Value))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `value` ");
                        values = string.Concat(values, ", @value");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `value` ");
                        values = string.Concat(values, " @value");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@value", quizQuestionRubricCriterionValue.Value);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubricCriterionValue.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubricCriterionValue.CreatedAt);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubricCriterionValue.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                quizQuestionRubricCriterionValue.Id = cmd.LastInsertedId.ToString();
                response.Add(quizQuestionRubricCriterionValue);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricCriterionValueModel>();
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
        public List<QuizQuestionRubricCriterionValueModel> Create(string connectionString, List<QuizQuestionRubricCriterionValueModel> quizQuestionRubricCriterionValueList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionValueModel> response = new List<QuizQuestionRubricCriterionValueModel>();
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
                        for (int i = 0; i < quizQuestionRubricCriterionValueList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO quiz_question_rubric_criterion_values (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricCriterionValueList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueList[i].QuizQuestionRubricCriterionId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `quiz_question_rubric_criterion_id` ");
                                        values = string.Concat(values, ", @quizQuestionRubricCriterionId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `quiz_question_rubric_criterion_id` ");
                                        values = string.Concat(values, "@quizQuestionRubricCriterionId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizQuestionRubricCriterionId" + i, quizQuestionRubricCriterionValueList[i].QuizQuestionRubricCriterionId);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueList[i].Desc))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `desc` ");
                                        values = string.Concat(values, ", @desc" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `desc` ");
                                        values = string.Concat(values, "@desc" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, quizQuestionRubricCriterionValueList[i].Desc);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueList[i].Value))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `value` ");
                                        values = string.Concat(values, ", @value" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `value` ");
                                        values = string.Concat(values, "@value" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@value" + i, quizQuestionRubricCriterionValueList[i].Value);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricCriterionValueList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricCriterionValueList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricCriterionValueList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(quizQuestionRubricCriterionValueList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionRubricCriterionValueModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionRubricCriterionValueModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricCriterionValueModel>();
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

        public List<QuizQuestionRubricCriterionValueModel> Update(string connectionString, QuizQuestionRubricCriterionValueModel quizQuestionRubricCriterionValue, QuizQuestionRubricCriterionValueModel quizQuestionRubricCriterionValueWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionValueModel> response = new List<QuizQuestionRubricCriterionValueModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE quiz_question_rubric_criterion_values SET ";
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", quizQuestionRubricCriterionValue.Id);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId");
                    else
                        sql = string.Concat(sql, " `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizQuestionRubricCriterionId", quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Desc))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `desc` = @desc");
                    else
                        sql = string.Concat(sql, " `desc` = @desc");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", quizQuestionRubricCriterionValue.Desc);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Value))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `value` = @value");
                    else
                        sql = string.Concat(sql, " `value` = @value");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@value", quizQuestionRubricCriterionValue.Value);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubricCriterionValue.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubricCriterionValue.CreatedAt);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubricCriterionValue.UpdatedAt);
                }
                if (quizQuestionRubricCriterionValueWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", quizQuestionRubricCriterionValueWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere.QuizQuestionRubricCriterionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionIdWhere");
                        else
                            sql = string.Concat(sql, " `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizQuestionRubricCriterionIdWhere", quizQuestionRubricCriterionValueWhere.QuizQuestionRubricCriterionId);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere.Desc))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `desc` = @descWhere");
                        else
                            sql = string.Concat(sql, " `desc` = @descWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descWhere", quizQuestionRubricCriterionValueWhere.Desc);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere.Value))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `value` = @valueWhere");
                        else
                            sql = string.Concat(sql, " `value` = @valueWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@valueWhere", quizQuestionRubricCriterionValueWhere.Value);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", quizQuestionRubricCriterionValueWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", quizQuestionRubricCriterionValueWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", quizQuestionRubricCriterionValueWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(quizQuestionRubricCriterionValue);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricCriterionValueModel>();
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

        public List<QuizQuestionRubricCriterionValueModel> Update(string connectionString, List<QuizQuestionRubricCriterionValueModel> quizQuestionRubricCriterionValueList, List<QuizQuestionRubricCriterionValueModel> quizQuestionRubricCriterionValueWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionValueModel> response = new List<QuizQuestionRubricCriterionValueModel>();
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
                        for (int i = 0; i < quizQuestionRubricCriterionValueList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE quiz_question_rubric_criterion_values SET ";
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricCriterionValueList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueList[i].QuizQuestionRubricCriterionId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId" + i);
                                    else
                                        sql = string.Concat(sql, " `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizQuestionRubricCriterionId" + i, quizQuestionRubricCriterionValueList[i].QuizQuestionRubricCriterionId);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueList[i].Desc))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `desc` = @desc" + i);
                                    else
                                        sql = string.Concat(sql, " `desc` = @desc" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, quizQuestionRubricCriterionValueList[i].Desc);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueList[i].Value))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `value` = @value" + i);
                                    else
                                        sql = string.Concat(sql, " `value` = @value" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@value" + i, quizQuestionRubricCriterionValueList[i].Value);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricCriterionValueList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricCriterionValueList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricCriterionValueList[i].UpdatedAt);
                                }
                                if (quizQuestionRubricCriterionValueWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, quizQuestionRubricCriterionValueWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhereList[i].QuizQuestionRubricCriterionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizQuestionRubricCriterionIdWhere" + i, quizQuestionRubricCriterionValueWhereList[i].QuizQuestionRubricCriterionId);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhereList[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `desc` = @descWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `desc` = @descWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descWhere" + i, quizQuestionRubricCriterionValueWhereList[i].Desc);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhereList[i].Value))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `value` = @valueWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `value` = @valueWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@valueWhere" + i, quizQuestionRubricCriterionValueWhereList[i].Value);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, quizQuestionRubricCriterionValueWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, quizQuestionRubricCriterionValueWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValueWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, quizQuestionRubricCriterionValueWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizQuestionRubricCriterionValueList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionRubricCriterionValueModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionRubricCriterionValueModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricCriterionValueModel>();
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

        public List<QuizQuestionRubricCriterionValueModel> Delete(string connectionString, QuizQuestionRubricCriterionValueModel quizQuestionRubricCriterionValue)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionValueModel> response = new List<QuizQuestionRubricCriterionValueModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM quiz_question_rubric_criterion_values  ";
                if (quizQuestionRubricCriterionValue != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", quizQuestionRubricCriterionValue.Id);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId");
                        else
                            sql = string.Concat(sql, " `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizQuestionRubricCriterionId", quizQuestionRubricCriterionValue.QuizQuestionRubricCriterionId);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Desc))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `desc` = @desc");
                        else
                            sql = string.Concat(sql, " `desc` = @desc");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@desc", quizQuestionRubricCriterionValue.Desc);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.Value))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `value` = @value");
                        else
                            sql = string.Concat(sql, " `value` = @value");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@value", quizQuestionRubricCriterionValue.Value);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubricCriterionValue.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubricCriterionValue.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubricCriterionValue.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(quizQuestionRubricCriterionValue);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricCriterionValueModel>();
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

        public List<QuizQuestionRubricCriterionValueModel> Delete(string connectionString, List<QuizQuestionRubricCriterionValueModel> quizQuestionRubricCriterionValue)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionValueModel> response = new List<QuizQuestionRubricCriterionValueModel>();
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
                        for (int i = 0; i < quizQuestionRubricCriterionValue.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM quiz_question_rubric_criterion_values  ";
                                if (quizQuestionRubricCriterionValue[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricCriterionValue[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue[i].QuizQuestionRubricCriterionId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId" + i);
                                        else
                                            sql = string.Concat(sql, " `quiz_question_rubric_criterion_id` = @quizQuestionRubricCriterionId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizQuestionRubricCriterionId" + i, quizQuestionRubricCriterionValue[i].QuizQuestionRubricCriterionId);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `desc` = @desc" + i);
                                        else
                                            sql = string.Concat(sql, " `desc` = @desc" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@desc" + i, quizQuestionRubricCriterionValue[i].Desc);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue[i].Value))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `value` = @value" + i);
                                        else
                                            sql = string.Concat(sql, " `value` = @value" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@value" + i, quizQuestionRubricCriterionValue[i].Value);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricCriterionValue[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricCriterionValue[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionValue[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricCriterionValue[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizQuestionRubricCriterionValue[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionRubricCriterionValueModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionRubricCriterionValueModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricCriterionValueModel>();
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
