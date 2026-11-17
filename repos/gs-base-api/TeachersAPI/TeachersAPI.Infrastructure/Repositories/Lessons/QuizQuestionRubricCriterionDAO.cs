using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Lessons;

namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons
{
    class QuizQuestionRubricCriterionDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, QuizQuestionRubricCriterionModel> ReadD(string connectionString, QuizQuestionRubricCriterionModel quizQuestionRubricCriterion, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionRubricCriterionModel> response = new Dictionary<string, QuizQuestionRubricCriterionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_question_rubric_id`, `criterion`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubric_criterions ";
                {
                    if (quizQuestionRubricCriterion != null)
                    {
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestionRubricCriterion.Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.QuizQuestionRubricId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quiz_question_rubric_id` = @quizQuestionRubricId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quiz_question_rubric_id` = @quizQuestionRubricId ");
                            }
                            cmd.Parameters.AddWithValue("@quizQuestionRubricId", quizQuestionRubricCriterion.QuizQuestionRubricId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.Criterion))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `criterion` = @criterion ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `criterion` = @criterion ");
                            }
                            cmd.Parameters.AddWithValue("@criterion", quizQuestionRubricCriterion.Criterion);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubricCriterion.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubricCriterion.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubricCriterion.UpdatedAt);
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
                                    quizQuestionRubricCriterion.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionRubricCriterion.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionRubricCriterion.QuizQuestionRubricId = reader.GetValue(1).ToString();
                                else
                                    quizQuestionRubricCriterion.QuizQuestionRubricId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionRubricCriterion.Criterion = reader.GetValue(2).ToString();
                                else
                                    quizQuestionRubricCriterion.Criterion = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionRubricCriterion.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizQuestionRubricCriterion.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionRubricCriterion.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizQuestionRubricCriterion.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionRubricCriterion.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizQuestionRubricCriterion.UpdatedAt = null;
                                response.Add(quizQuestionRubricCriterion.Id, quizQuestionRubricCriterion);
                                quizQuestionRubricCriterion = new QuizQuestionRubricCriterionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizQuestionRubricCriterionModel>();
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
                response = new Dictionary<string, QuizQuestionRubricCriterionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizQuestionRubricCriterionModel> ReadD(string connectionString, QuizQuestionRubricCriterionModel quizQuestionRubricCriterion, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionRubricCriterionModel> response = new Dictionary<string, QuizQuestionRubricCriterionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_question_rubric_id`, `criterion`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubric_criterions ";
                {
                    if (quizQuestionRubricCriterion != null)
                    {
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestionRubricCriterion.Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.QuizQuestionRubricId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quiz_question_rubric_id` = @quizQuestionRubricId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quiz_question_rubric_id` = @quizQuestionRubricId ");
                            }
                            cmd.Parameters.AddWithValue("@quizQuestionRubricId", quizQuestionRubricCriterion.QuizQuestionRubricId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.Criterion))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `criterion` = @criterion ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `criterion` = @criterion ");
                            }
                            cmd.Parameters.AddWithValue("@criterion", quizQuestionRubricCriterion.Criterion);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubricCriterion.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubricCriterion.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubricCriterion.UpdatedAt);
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
                                    quizQuestionRubricCriterion.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionRubricCriterion.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionRubricCriterion.QuizQuestionRubricId = reader.GetValue(1).ToString();
                                else
                                    quizQuestionRubricCriterion.QuizQuestionRubricId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionRubricCriterion.Criterion = reader.GetValue(2).ToString();
                                else
                                    quizQuestionRubricCriterion.Criterion = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionRubricCriterion.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizQuestionRubricCriterion.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionRubricCriterion.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizQuestionRubricCriterion.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionRubricCriterion.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizQuestionRubricCriterion.UpdatedAt = null;
                                response.Add(quizQuestionRubricCriterion.Id, quizQuestionRubricCriterion);
                                quizQuestionRubricCriterion = new QuizQuestionRubricCriterionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizQuestionRubricCriterionModel>();
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
                response = new Dictionary<string, QuizQuestionRubricCriterionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizQuestionRubricCriterionModel> ReadD(string connectionString, List<QuizQuestionRubricCriterionModel> quizQuestionRubricCriterionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionRubricCriterionModel> response = new Dictionary<string, QuizQuestionRubricCriterionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_question_rubric_id`, `criterion`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubric_criterions ";
                if (quizQuestionRubricCriterionWhere != null && quizQuestionRubricCriterionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionRubricCriterionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricCriterionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].QuizQuestionRubricId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_question_rubric_id` = @quizQuestionRubricId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizQuestionRubricId" + i, quizQuestionRubricCriterionWhere[i].QuizQuestionRubricId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].Criterion))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `criterion` = @criterion" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@criterion" + i, quizQuestionRubricCriterionWhere[i].Criterion);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricCriterionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricCriterionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricCriterionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionRubricCriterionWhere.Count - 1)
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
                            QuizQuestionRubricCriterionModel quizQuestionRubricCriterion = new QuizQuestionRubricCriterionModel(); if (!reader.IsDBNull(0))
                                quizQuestionRubricCriterion.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionRubricCriterion.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionRubricCriterion.QuizQuestionRubricId = reader.GetValue(1).ToString();
                            else
                                quizQuestionRubricCriterion.QuizQuestionRubricId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionRubricCriterion.Criterion = reader.GetValue(2).ToString();
                            else
                                quizQuestionRubricCriterion.Criterion = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionRubricCriterion.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizQuestionRubricCriterion.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionRubricCriterion.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizQuestionRubricCriterion.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionRubricCriterion.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizQuestionRubricCriterion.UpdatedAt = null;
                            response.Add(quizQuestionRubricCriterion.Id, quizQuestionRubricCriterion);
                            quizQuestionRubricCriterion = new QuizQuestionRubricCriterionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizQuestionRubricCriterionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizQuestionRubricCriterionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizQuestionRubricCriterionModel> ReadD(string connectionString, List<QuizQuestionRubricCriterionModel> quizQuestionRubricCriterionWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionRubricCriterionModel> response = new Dictionary<string, QuizQuestionRubricCriterionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_question_rubric_id`, `criterion`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubric_criterions ";
                if (quizQuestionRubricCriterionWhere != null && quizQuestionRubricCriterionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionRubricCriterionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricCriterionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].QuizQuestionRubricId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_question_rubric_id` = @quizQuestionRubricId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizQuestionRubricId" + i, quizQuestionRubricCriterionWhere[i].QuizQuestionRubricId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].Criterion))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `criterion` = @criterion" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@criterion" + i, quizQuestionRubricCriterionWhere[i].Criterion);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricCriterionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricCriterionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricCriterionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionRubricCriterionWhere.Count - 1)
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
                            QuizQuestionRubricCriterionModel quizQuestionRubricCriterion = new QuizQuestionRubricCriterionModel(); if (!reader.IsDBNull(0))
                                quizQuestionRubricCriterion.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionRubricCriterion.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionRubricCriterion.QuizQuestionRubricId = reader.GetValue(1).ToString();
                            else
                                quizQuestionRubricCriterion.QuizQuestionRubricId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionRubricCriterion.Criterion = reader.GetValue(2).ToString();
                            else
                                quizQuestionRubricCriterion.Criterion = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionRubricCriterion.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizQuestionRubricCriterion.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionRubricCriterion.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizQuestionRubricCriterion.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionRubricCriterion.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizQuestionRubricCriterion.UpdatedAt = null;
                            response.Add(quizQuestionRubricCriterion.Id, quizQuestionRubricCriterion);
                            quizQuestionRubricCriterion = new QuizQuestionRubricCriterionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizQuestionRubricCriterionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizQuestionRubricCriterionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionRubricCriterionModel> Read(string connectionString, QuizQuestionRubricCriterionModel quizQuestionRubricCriterion, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionModel> response = new List<QuizQuestionRubricCriterionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_question_rubric_id`, `criterion`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubric_criterions ";
                {
                    if (quizQuestionRubricCriterion != null)
                    {
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestionRubricCriterion.Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.QuizQuestionRubricId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quiz_question_rubric_id` = @quizQuestionRubricId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quiz_question_rubric_id` = @quizQuestionRubricId ");
                            }
                            cmd.Parameters.AddWithValue("@quizQuestionRubricId", quizQuestionRubricCriterion.QuizQuestionRubricId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.Criterion))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `criterion` = @criterion ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `criterion` = @criterion ");
                            }
                            cmd.Parameters.AddWithValue("@criterion", quizQuestionRubricCriterion.Criterion);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubricCriterion.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubricCriterion.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubricCriterion.UpdatedAt);
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
                                    quizQuestionRubricCriterion.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionRubricCriterion.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionRubricCriterion.QuizQuestionRubricId = reader.GetValue(1).ToString();
                                else
                                    quizQuestionRubricCriterion.QuizQuestionRubricId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionRubricCriterion.Criterion = reader.GetValue(2).ToString();
                                else
                                    quizQuestionRubricCriterion.Criterion = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionRubricCriterion.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizQuestionRubricCriterion.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionRubricCriterion.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizQuestionRubricCriterion.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionRubricCriterion.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizQuestionRubricCriterion.UpdatedAt = null;
                                response.Add(quizQuestionRubricCriterion);
                                quizQuestionRubricCriterion = new QuizQuestionRubricCriterionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizQuestionRubricCriterionModel>();
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
                response = new List<QuizQuestionRubricCriterionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionRubricCriterionModel> Read(string connectionString, QuizQuestionRubricCriterionModel quizQuestionRubricCriterion, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionModel> response = new List<QuizQuestionRubricCriterionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_question_rubric_id`, `criterion`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubric_criterions ";
                {
                    if (quizQuestionRubricCriterion != null)
                    {
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizQuestionRubricCriterion.Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.QuizQuestionRubricId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `quiz_question_rubric_id` = @quizQuestionRubricId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `quiz_question_rubric_id` = @quizQuestionRubricId ");
                            }
                            cmd.Parameters.AddWithValue("@quizQuestionRubricId", quizQuestionRubricCriterion.QuizQuestionRubricId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.Criterion))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `criterion` = @criterion ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `criterion` = @criterion ");
                            }
                            cmd.Parameters.AddWithValue("@criterion", quizQuestionRubricCriterion.Criterion);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubricCriterion.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubricCriterion.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubricCriterion.UpdatedAt);
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
                                    quizQuestionRubricCriterion.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionRubricCriterion.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionRubricCriterion.QuizQuestionRubricId = reader.GetValue(1).ToString();
                                else
                                    quizQuestionRubricCriterion.QuizQuestionRubricId = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionRubricCriterion.Criterion = reader.GetValue(2).ToString();
                                else
                                    quizQuestionRubricCriterion.Criterion = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionRubricCriterion.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizQuestionRubricCriterion.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionRubricCriterion.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizQuestionRubricCriterion.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionRubricCriterion.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizQuestionRubricCriterion.UpdatedAt = null;
                                response.Add(quizQuestionRubricCriterion);
                                quizQuestionRubricCriterion = new QuizQuestionRubricCriterionModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizQuestionRubricCriterionModel>();
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
                response = new List<QuizQuestionRubricCriterionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionRubricCriterionModel> Read(string connectionString, List<QuizQuestionRubricCriterionModel> quizQuestionRubricCriterionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionModel> response = new List<QuizQuestionRubricCriterionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_question_rubric_id`, `criterion`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubric_criterions ";
                if (quizQuestionRubricCriterionWhere != null && quizQuestionRubricCriterionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionRubricCriterionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricCriterionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].QuizQuestionRubricId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_question_rubric_id` = @quizQuestionRubricId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizQuestionRubricId" + i, quizQuestionRubricCriterionWhere[i].QuizQuestionRubricId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].Criterion))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `criterion` = @criterion" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@criterion" + i, quizQuestionRubricCriterionWhere[i].Criterion);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricCriterionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricCriterionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricCriterionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionRubricCriterionWhere.Count - 1)
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
                            QuizQuestionRubricCriterionModel quizQuestionRubricCriterion = new QuizQuestionRubricCriterionModel(); if (!reader.IsDBNull(0))
                                quizQuestionRubricCriterion.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionRubricCriterion.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionRubricCriterion.QuizQuestionRubricId = reader.GetValue(1).ToString();
                            else
                                quizQuestionRubricCriterion.QuizQuestionRubricId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionRubricCriterion.Criterion = reader.GetValue(2).ToString();
                            else
                                quizQuestionRubricCriterion.Criterion = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionRubricCriterion.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizQuestionRubricCriterion.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionRubricCriterion.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizQuestionRubricCriterion.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionRubricCriterion.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizQuestionRubricCriterion.UpdatedAt = null;
                            response.Add(quizQuestionRubricCriterion);
                            quizQuestionRubricCriterion = new QuizQuestionRubricCriterionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizQuestionRubricCriterionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricCriterionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionRubricCriterionModel> Read(string connectionString, List<QuizQuestionRubricCriterionModel> quizQuestionRubricCriterionWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionModel> response = new List<QuizQuestionRubricCriterionModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_question_rubric_id`, `criterion`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_rubric_criterions ";
                if (quizQuestionRubricCriterionWhere != null && quizQuestionRubricCriterionWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionRubricCriterionWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricCriterionWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].QuizQuestionRubricId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_question_rubric_id` = @quizQuestionRubricId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizQuestionRubricId" + i, quizQuestionRubricCriterionWhere[i].QuizQuestionRubricId);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].Criterion))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `criterion` = @criterion" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@criterion" + i, quizQuestionRubricCriterionWhere[i].Criterion);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricCriterionWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricCriterionWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricCriterionWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizQuestionRubricCriterionWhere.Count - 1)
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
                            QuizQuestionRubricCriterionModel quizQuestionRubricCriterion = new QuizQuestionRubricCriterionModel(); if (!reader.IsDBNull(0))
                                quizQuestionRubricCriterion.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionRubricCriterion.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionRubricCriterion.QuizQuestionRubricId = reader.GetValue(1).ToString();
                            else
                                quizQuestionRubricCriterion.QuizQuestionRubricId = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionRubricCriterion.Criterion = reader.GetValue(2).ToString();
                            else
                                quizQuestionRubricCriterion.Criterion = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionRubricCriterion.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizQuestionRubricCriterion.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionRubricCriterion.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizQuestionRubricCriterion.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionRubricCriterion.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizQuestionRubricCriterion.UpdatedAt = null;
                            response.Add(quizQuestionRubricCriterion);
                            quizQuestionRubricCriterion = new QuizQuestionRubricCriterionModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizQuestionRubricCriterionModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricCriterionModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizQuestionRubricCriterionModel> Create(string connectionString, QuizQuestionRubricCriterionModel quizQuestionRubricCriterion)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionModel> response = new List<QuizQuestionRubricCriterionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO quiz_question_rubric_criterions (";
                string values = "(";
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.Id))
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
                    cmd.Parameters.AddWithValue("@id", quizQuestionRubricCriterion.Id);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.QuizQuestionRubricId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `quiz_question_rubric_id` ");
                        values = string.Concat(values, ", @quizQuestionRubricId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `quiz_question_rubric_id` ");
                        values = string.Concat(values, " @quizQuestionRubricId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizQuestionRubricId", quizQuestionRubricCriterion.QuizQuestionRubricId);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.Criterion))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `criterion` ");
                        values = string.Concat(values, ", @criterion");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `criterion` ");
                        values = string.Concat(values, " @criterion");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@criterion", quizQuestionRubricCriterion.Criterion);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubricCriterion.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubricCriterion.CreatedAt);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubricCriterion.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                quizQuestionRubricCriterion.Id = cmd.LastInsertedId.ToString();
                response.Add(quizQuestionRubricCriterion);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricCriterionModel>();
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
        public List<QuizQuestionRubricCriterionModel> Create(string connectionString, List<QuizQuestionRubricCriterionModel> quizQuestionRubricCriterionList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionModel> response = new List<QuizQuestionRubricCriterionModel>();
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
                        for (int i = 0; i < quizQuestionRubricCriterionList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO quiz_question_rubric_criterions (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricCriterionList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionList[i].QuizQuestionRubricId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `quiz_question_rubric_id` ");
                                        values = string.Concat(values, ", @quizQuestionRubricId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `quiz_question_rubric_id` ");
                                        values = string.Concat(values, "@quizQuestionRubricId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizQuestionRubricId" + i, quizQuestionRubricCriterionList[i].QuizQuestionRubricId);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionList[i].Criterion))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `criterion` ");
                                        values = string.Concat(values, ", @criterion" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `criterion` ");
                                        values = string.Concat(values, "@criterion" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@criterion" + i, quizQuestionRubricCriterionList[i].Criterion);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricCriterionList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricCriterionList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricCriterionList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(quizQuestionRubricCriterionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionRubricCriterionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionRubricCriterionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricCriterionModel>();
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

        public List<QuizQuestionRubricCriterionModel> Update(string connectionString, QuizQuestionRubricCriterionModel quizQuestionRubricCriterion, QuizQuestionRubricCriterionModel quizQuestionRubricCriterionWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionModel> response = new List<QuizQuestionRubricCriterionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE quiz_question_rubric_criterions SET ";
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", quizQuestionRubricCriterion.Id);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.QuizQuestionRubricId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `quiz_question_rubric_id` = @quizQuestionRubricId");
                    else
                        sql = string.Concat(sql, " `quiz_question_rubric_id` = @quizQuestionRubricId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizQuestionRubricId", quizQuestionRubricCriterion.QuizQuestionRubricId);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.Criterion))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `criterion` = @criterion");
                    else
                        sql = string.Concat(sql, " `criterion` = @criterion");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@criterion", quizQuestionRubricCriterion.Criterion);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubricCriterion.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubricCriterion.CreatedAt);
                }
                if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubricCriterion.UpdatedAt);
                }
                if (quizQuestionRubricCriterionWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", quizQuestionRubricCriterionWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere.QuizQuestionRubricId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `quiz_question_rubric_id` = @quizQuestionRubricIdWhere");
                        else
                            sql = string.Concat(sql, " `quiz_question_rubric_id` = @quizQuestionRubricIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizQuestionRubricIdWhere", quizQuestionRubricCriterionWhere.QuizQuestionRubricId);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere.Criterion))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `criterion` = @criterionWhere");
                        else
                            sql = string.Concat(sql, " `criterion` = @criterionWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@criterionWhere", quizQuestionRubricCriterionWhere.Criterion);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", quizQuestionRubricCriterionWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", quizQuestionRubricCriterionWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", quizQuestionRubricCriterionWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(quizQuestionRubricCriterion);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricCriterionModel>();
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

        public List<QuizQuestionRubricCriterionModel> Update(string connectionString, List<QuizQuestionRubricCriterionModel> quizQuestionRubricCriterionList, List<QuizQuestionRubricCriterionModel> quizQuestionRubricCriterionWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionModel> response = new List<QuizQuestionRubricCriterionModel>();
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
                        for (int i = 0; i < quizQuestionRubricCriterionList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE quiz_question_rubric_criterions SET ";
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricCriterionList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionList[i].QuizQuestionRubricId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `quiz_question_rubric_id` = @quizQuestionRubricId" + i);
                                    else
                                        sql = string.Concat(sql, " `quiz_question_rubric_id` = @quizQuestionRubricId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizQuestionRubricId" + i, quizQuestionRubricCriterionList[i].QuizQuestionRubricId);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionList[i].Criterion))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `criterion` = @criterion" + i);
                                    else
                                        sql = string.Concat(sql, " `criterion` = @criterion" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@criterion" + i, quizQuestionRubricCriterionList[i].Criterion);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricCriterionList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricCriterionList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(quizQuestionRubricCriterionList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricCriterionList[i].UpdatedAt);
                                }
                                if (quizQuestionRubricCriterionWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, quizQuestionRubricCriterionWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhereList[i].QuizQuestionRubricId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `quiz_question_rubric_id` = @quizQuestionRubricIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `quiz_question_rubric_id` = @quizQuestionRubricIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizQuestionRubricIdWhere" + i, quizQuestionRubricCriterionWhereList[i].QuizQuestionRubricId);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhereList[i].Criterion))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `criterion` = @criterionWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `criterion` = @criterionWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@criterionWhere" + i, quizQuestionRubricCriterionWhereList[i].Criterion);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, quizQuestionRubricCriterionWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, quizQuestionRubricCriterionWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterionWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, quizQuestionRubricCriterionWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizQuestionRubricCriterionList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionRubricCriterionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionRubricCriterionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricCriterionModel>();
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

        public List<QuizQuestionRubricCriterionModel> Delete(string connectionString, QuizQuestionRubricCriterionModel quizQuestionRubricCriterion)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionModel> response = new List<QuizQuestionRubricCriterionModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM quiz_question_rubric_criterions  ";
                if (quizQuestionRubricCriterion != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", quizQuestionRubricCriterion.Id);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.QuizQuestionRubricId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `quiz_question_rubric_id` = @quizQuestionRubricId");
                        else
                            sql = string.Concat(sql, " `quiz_question_rubric_id` = @quizQuestionRubricId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizQuestionRubricId", quizQuestionRubricCriterion.QuizQuestionRubricId);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.Criterion))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `criterion` = @criterion");
                        else
                            sql = string.Concat(sql, " `criterion` = @criterion");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@criterion", quizQuestionRubricCriterion.Criterion);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionRubricCriterion.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", quizQuestionRubricCriterion.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterion.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", quizQuestionRubricCriterion.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(quizQuestionRubricCriterion);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricCriterionModel>();
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

        public List<QuizQuestionRubricCriterionModel> Delete(string connectionString, List<QuizQuestionRubricCriterionModel> quizQuestionRubricCriterion)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionRubricCriterionModel> response = new List<QuizQuestionRubricCriterionModel>();
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
                        for (int i = 0; i < quizQuestionRubricCriterion.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM quiz_question_rubric_criterions  ";
                                if (quizQuestionRubricCriterion[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterion[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, quizQuestionRubricCriterion[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterion[i].QuizQuestionRubricId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `quiz_question_rubric_id` = @quizQuestionRubricId" + i);
                                        else
                                            sql = string.Concat(sql, " `quiz_question_rubric_id` = @quizQuestionRubricId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizQuestionRubricId" + i, quizQuestionRubricCriterion[i].QuizQuestionRubricId);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterion[i].Criterion))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `criterion` = @criterion" + i);
                                        else
                                            sql = string.Concat(sql, " `criterion` = @criterion" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@criterion" + i, quizQuestionRubricCriterion[i].Criterion);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterion[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionRubricCriterion[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterion[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionRubricCriterion[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(quizQuestionRubricCriterion[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionRubricCriterion[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizQuestionRubricCriterion[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionRubricCriterionModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionRubricCriterionModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionRubricCriterionModel>();
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
