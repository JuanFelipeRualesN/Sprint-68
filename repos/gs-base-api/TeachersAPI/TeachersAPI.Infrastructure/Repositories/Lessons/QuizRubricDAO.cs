using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Lessons;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons
{
    class QuizRubricDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, QuizRubricModel> ReadD(string connectionString, QuizRubricModel quizRubric, bool firstOne, bool orderByLastDate, QuizRubricModel quizRubricWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizRubricModel> response = new Dictionary<string, QuizRubricModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_id`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rubrics ";
                {
                    if (quizRubric != null)
                    {
                        if (!String.IsNullOrEmpty(quizRubric.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizRubric.Id);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_id` = @quizId ");
                            }
                            cmd.Parameters.AddWithValue("@quizId", quizRubric.QuizId);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", quizRubric.Desc);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizRubric.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizRubric.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizRubric.UpdatedAt);
                        }
                    }
                    if (quizRubricWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizRubricWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.QuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + quizRubricWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.Desc))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `desc` LIKE @descWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `desc` LIKE @descWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@descWhereLike", "%" + quizRubricWhereLike.Desc + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizRubricWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizRubricWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizRubricWhereLike.UpdatedAt + "%");
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
                                quizRubric = new QuizRubricModel();
                                if (!reader.IsDBNull(0))
                                    quizRubric.Id = reader.GetValue(0).ToString();
                                else
                                    quizRubric.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizRubric.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizRubric.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizRubric.Desc = reader.GetValue(2).ToString();
                                else
                                    quizRubric.Desc = null;
                                if (!reader.IsDBNull(3))
                                    quizRubric.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizRubric.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizRubric.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizRubric.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizRubric.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizRubric.UpdatedAt = null;
                                response.Add(quizRubric.Id, quizRubric);
                                quizRubric = new QuizRubricModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizRubricModel>();
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
                response = new Dictionary<string, QuizRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizRubricModel> ReadD(string connectionString, QuizRubricModel quizRubric, bool orderByLastDate, int fromRow, int limit, QuizRubricModel quizRubricWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizRubricModel> response = new Dictionary<string, QuizRubricModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_id`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rubrics ";
                {
                    if (quizRubric != null)
                    {
                        if (!String.IsNullOrEmpty(quizRubric.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizRubric.Id);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_id` = @quizId ");
                            }
                            cmd.Parameters.AddWithValue("@quizId", quizRubric.QuizId);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", quizRubric.Desc);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizRubric.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizRubric.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizRubric.UpdatedAt);
                        }
                    }
                    if (quizRubricWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizRubricWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.QuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + quizRubricWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.Desc))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `desc` LIKE @descWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `desc` LIKE @descWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@descWhereLike", "%" + quizRubricWhereLike.Desc + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizRubricWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizRubricWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizRubricWhereLike.UpdatedAt + "%");
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
                                quizRubric = new QuizRubricModel();
                                if (!reader.IsDBNull(0))
                                    quizRubric.Id = reader.GetValue(0).ToString();
                                else
                                    quizRubric.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizRubric.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizRubric.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizRubric.Desc = reader.GetValue(2).ToString();
                                else
                                    quizRubric.Desc = null;
                                if (!reader.IsDBNull(3))
                                    quizRubric.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizRubric.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizRubric.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizRubric.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizRubric.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizRubric.UpdatedAt = null;
                                response.Add(quizRubric.Id, quizRubric);
                                quizRubric = new QuizRubricModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizRubricModel>();
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
                response = new Dictionary<string, QuizRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizRubricModel> ReadD(string connectionString, List<QuizRubricModel> quizRubricWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizRubricModel> response = new Dictionary<string, QuizRubricModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_id`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rubrics ";
                if (quizRubricWhere != null && quizRubricWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizRubricWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizRubricWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, quizRubricWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, quizRubricWhere[i].Desc);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizRubricWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizRubricWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizRubricWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizRubricWhere.Count - 1)
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
                            QuizRubricModel quizRubric = new QuizRubricModel(); if (!reader.IsDBNull(0))
                                quizRubric.Id = reader.GetValue(0).ToString();
                            else
                                quizRubric.Id = null;
                            if (!reader.IsDBNull(1))
                                quizRubric.QuizId = reader.GetValue(1).ToString();
                            else
                                quizRubric.QuizId = null;
                            if (!reader.IsDBNull(2))
                                quizRubric.Desc = reader.GetValue(2).ToString();
                            else
                                quizRubric.Desc = null;
                            if (!reader.IsDBNull(3))
                                quizRubric.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizRubric.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizRubric.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizRubric.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizRubric.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizRubric.UpdatedAt = null;
                            response.Add(quizRubric.Id, quizRubric);
                            quizRubric = new QuizRubricModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizRubricModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizRubricModel> ReadD(string connectionString, List<QuizRubricModel> quizRubricWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizRubricModel> response = new Dictionary<string, QuizRubricModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_id`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rubrics ";
                if (quizRubricWhere != null && quizRubricWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizRubricWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizRubricWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, quizRubricWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, quizRubricWhere[i].Desc);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizRubricWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizRubricWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizRubricWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizRubricWhere.Count - 1)
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
                            QuizRubricModel quizRubric = new QuizRubricModel(); if (!reader.IsDBNull(0))
                                quizRubric.Id = reader.GetValue(0).ToString();
                            else
                                quizRubric.Id = null;
                            if (!reader.IsDBNull(1))
                                quizRubric.QuizId = reader.GetValue(1).ToString();
                            else
                                quizRubric.QuizId = null;
                            if (!reader.IsDBNull(2))
                                quizRubric.Desc = reader.GetValue(2).ToString();
                            else
                                quizRubric.Desc = null;
                            if (!reader.IsDBNull(3))
                                quizRubric.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizRubric.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizRubric.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizRubric.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizRubric.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizRubric.UpdatedAt = null;
                            response.Add(quizRubric.Id, quizRubric);
                            quizRubric = new QuizRubricModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizRubricModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizRubricModel> Read(string connectionString, QuizRubricModel quizRubric, bool firstOne, bool orderByLastDate, QuizRubricModel quizRubricWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizRubricModel> response = new List<QuizRubricModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_id`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rubrics ";
                {
                    if (quizRubric != null)
                    {
                        if (!String.IsNullOrEmpty(quizRubric.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizRubric.Id);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_id` = @quizId ");
                            }
                            cmd.Parameters.AddWithValue("@quizId", quizRubric.QuizId);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", quizRubric.Desc);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizRubric.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizRubric.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizRubric.UpdatedAt);
                        }
                    }
                    if (quizRubricWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizRubricWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.QuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + quizRubricWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.Desc))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `desc` LIKE @descWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `desc` LIKE @descWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@descWhereLike", "%" + quizRubricWhereLike.Desc + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizRubricWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizRubricWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizRubricWhereLike.UpdatedAt + "%");
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
                                quizRubric = new QuizRubricModel();
                                if (!reader.IsDBNull(0))
                                    quizRubric.Id = reader.GetValue(0).ToString();
                                else
                                    quizRubric.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizRubric.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizRubric.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizRubric.Desc = reader.GetValue(2).ToString();
                                else
                                    quizRubric.Desc = null;
                                if (!reader.IsDBNull(3))
                                    quizRubric.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizRubric.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizRubric.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizRubric.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizRubric.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizRubric.UpdatedAt = null;
                                response.Add(quizRubric);
                                quizRubric = new QuizRubricModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizRubricModel>();
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
                response = new List<QuizRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizRubricModel> Read(string connectionString, QuizRubricModel quizRubric, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizRubricModel> response = new List<QuizRubricModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_id`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rubrics ";
                {
                    if (quizRubric != null)
                    {
                        if (!string.IsNullOrEmpty(quizRubric.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizRubric.Id);
                        }
                        if (!string.IsNullOrEmpty(quizRubric.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", quizRubric.QuizId);
                        }
                        if (!string.IsNullOrEmpty(quizRubric.Desc))
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
                            cmd.Parameters.AddWithValue("@desc", quizRubric.Desc);
                        }
                        if (!string.IsNullOrEmpty(quizRubric.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizRubric.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizRubric.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizRubric.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizRubric.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizRubric.UpdatedAt);
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
                                    quizRubric.Id = reader.GetValue(0).ToString();
                                else
                                    quizRubric.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizRubric.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizRubric.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizRubric.Desc = reader.GetValue(2).ToString();
                                else
                                    quizRubric.Desc = null;
                                if (!reader.IsDBNull(3))
                                    quizRubric.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizRubric.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizRubric.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizRubric.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizRubric.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizRubric.UpdatedAt = null;
                                response.Add(quizRubric);
                                quizRubric = new QuizRubricModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizRubricModel>();
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
                response = new List<QuizRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizRubricModel> Read(string connectionString, QuizRubricModel quizRubric, bool orderByLastDate, int fromRow, int limit, QuizRubricModel quizRubricWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizRubricModel> response = new List<QuizRubricModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_id`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rubrics ";
                {
                    if (quizRubric != null)
                    {
                        if (!String.IsNullOrEmpty(quizRubric.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizRubric.Id);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.QuizId))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `quiz_id` = @quizId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `quiz_id` = @quizId ");
                            }
                            cmd.Parameters.AddWithValue("@quizId", quizRubric.QuizId);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.Desc))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `desc` = @desc ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `desc` = @desc ");
                            }
                            cmd.Parameters.AddWithValue("@desc", quizRubric.Desc);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizRubric.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizRubric.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizRubric.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizRubric.UpdatedAt);
                        }
                    }
                    if (quizRubricWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizRubricWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.QuizId))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `quiz_id` LIKE @quizIdWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + quizRubricWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.Desc))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `desc` LIKE @descWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `desc` LIKE @descWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@descWhereLike", "%" + quizRubricWhereLike.Desc + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizRubricWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizRubricWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizRubricWhereLike.UpdatedAt + "%");
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
                                quizRubric = new QuizRubricModel();
                                if (!reader.IsDBNull(0))
                                    quizRubric.Id = reader.GetValue(0).ToString();
                                else
                                    quizRubric.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizRubric.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizRubric.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizRubric.Desc = reader.GetValue(2).ToString();
                                else
                                    quizRubric.Desc = null;
                                if (!reader.IsDBNull(3))
                                    quizRubric.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizRubric.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizRubric.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizRubric.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizRubric.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizRubric.UpdatedAt = null;
                                response.Add(quizRubric);
                                quizRubric = new QuizRubricModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizRubricModel>();
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
                response = new List<QuizRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizRubricModel> Read(string connectionString, QuizRubricModel quizRubric, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizRubricModel> response = new List<QuizRubricModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_id`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rubrics ";
                {
                    if (quizRubric != null)
                    {
                        if (!string.IsNullOrEmpty(quizRubric.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizRubric.Id);
                        }
                        if (!string.IsNullOrEmpty(quizRubric.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", quizRubric.QuizId);
                        }
                        if (!string.IsNullOrEmpty(quizRubric.Desc))
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
                            cmd.Parameters.AddWithValue("@desc", quizRubric.Desc);
                        }
                        if (!string.IsNullOrEmpty(quizRubric.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizRubric.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizRubric.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizRubric.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizRubric.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizRubric.UpdatedAt);
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
                                    quizRubric.Id = reader.GetValue(0).ToString();
                                else
                                    quizRubric.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizRubric.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizRubric.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizRubric.Desc = reader.GetValue(2).ToString();
                                else
                                    quizRubric.Desc = null;
                                if (!reader.IsDBNull(3))
                                    quizRubric.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizRubric.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizRubric.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizRubric.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizRubric.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizRubric.UpdatedAt = null;
                                response.Add(quizRubric);
                                quizRubric = new QuizRubricModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizRubricModel>();
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
                response = new List<QuizRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizRubricModel> Read(string connectionString, List<QuizRubricModel> quizRubricWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizRubricModel> response = new List<QuizRubricModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_id`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rubrics ";
                if (quizRubricWhere != null && quizRubricWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizRubricWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizRubricWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, quizRubricWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, quizRubricWhere[i].Desc);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizRubricWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizRubricWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizRubricWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizRubricWhere.Count - 1)
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
                            QuizRubricModel quizRubric = new QuizRubricModel(); if (!reader.IsDBNull(0))
                                quizRubric.Id = reader.GetValue(0).ToString();
                            else
                                quizRubric.Id = null;
                            if (!reader.IsDBNull(1))
                                quizRubric.QuizId = reader.GetValue(1).ToString();
                            else
                                quizRubric.QuizId = null;
                            if (!reader.IsDBNull(2))
                                quizRubric.Desc = reader.GetValue(2).ToString();
                            else
                                quizRubric.Desc = null;
                            if (!reader.IsDBNull(3))
                                quizRubric.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizRubric.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizRubric.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizRubric.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizRubric.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizRubric.UpdatedAt = null;
                            response.Add(quizRubric);
                            quizRubric = new QuizRubricModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizRubricModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizRubricModel> Read(string connectionString, List<QuizRubricModel> quizRubricWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizRubricModel> response = new List<QuizRubricModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_id`, `desc`, `modified_by`, `created_at`, `updated_at`  FROM quiz_rubrics ";
                if (quizRubricWhere != null && quizRubricWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizRubricWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizRubricWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, quizRubricWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].Desc))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `desc` = @desc" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@desc" + i, quizRubricWhere[i].Desc);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizRubricWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizRubricWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizRubricWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizRubricWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizRubricWhere.Count - 1)
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
                            QuizRubricModel quizRubric = new QuizRubricModel(); if (!reader.IsDBNull(0))
                                quizRubric.Id = reader.GetValue(0).ToString();
                            else
                                quizRubric.Id = null;
                            if (!reader.IsDBNull(1))
                                quizRubric.QuizId = reader.GetValue(1).ToString();
                            else
                                quizRubric.QuizId = null;
                            if (!reader.IsDBNull(2))
                                quizRubric.Desc = reader.GetValue(2).ToString();
                            else
                                quizRubric.Desc = null;
                            if (!reader.IsDBNull(3))
                                quizRubric.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizRubric.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizRubric.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizRubric.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizRubric.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizRubric.UpdatedAt = null;
                            response.Add(quizRubric);
                            quizRubric = new QuizRubricModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizRubricModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizRubricModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizRubricModel> Create(string connectionString, QuizRubricModel quizRubric)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizRubricModel> response = new List<QuizRubricModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO quiz_rubrics (";
                string values = "(";
                if (!String.IsNullOrEmpty(quizRubric.Id))
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
                    cmd.Parameters.AddWithValue("@id", quizRubric.Id);
                }
                if (!String.IsNullOrEmpty(quizRubric.QuizId))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `quiz_id` ");
                        values = String.Concat(values, ", @quizId");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `quiz_id` ");
                        values = String.Concat(values, " @quizId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", quizRubric.QuizId);
                }
                if (!String.IsNullOrEmpty(quizRubric.Desc))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `desc` ");
                        values = String.Concat(values, ", @desc");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `desc` ");
                        values = String.Concat(values, " @desc");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", quizRubric.Desc);
                }
                if (!String.IsNullOrEmpty(quizRubric.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", quizRubric.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(quizRubric.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", quizRubric.CreatedAt);
                }
                if (!String.IsNullOrEmpty(quizRubric.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", quizRubric.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                quizRubric.Id = cmd.LastInsertedId.ToString();
                response.Add(quizRubric);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizRubricModel>();
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
        public List<QuizRubricModel> Create(string connectionString, List<QuizRubricModel> quizRubricList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizRubricModel> response = new List<QuizRubricModel>();
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
                        for (int i = 0; i < quizRubricList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO quiz_rubrics (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(quizRubricList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, quizRubricList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(quizRubricList[i].QuizId))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `quiz_id` ");
                                        values = String.Concat(values, ", @quizId" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `quiz_id` ");
                                        values = String.Concat(values, "@quizId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, quizRubricList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(quizRubricList[i].Desc))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `desc` ");
                                        values = String.Concat(values, ", @desc" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `desc` ");
                                        values = String.Concat(values, "@desc" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, quizRubricList[i].Desc);
                                }
                                if (!String.IsNullOrEmpty(quizRubricList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizRubricList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(quizRubricList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizRubricList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(quizRubricList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizRubricList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(quizRubricList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizRubricModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizRubricModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizRubricModel>();
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

        public List<QuizRubricModel> Update(string connectionString, QuizRubricModel quizRubric, QuizRubricModel quizRubricWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizRubricModel> response = new List<QuizRubricModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE quiz_rubrics SET ";
                if (!String.IsNullOrEmpty(quizRubric.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", quizRubric.Id);
                }
                if (!String.IsNullOrEmpty(quizRubric.QuizId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `quiz_id` = @quizId");
                    else
                        sql = String.Concat(sql, " `quiz_id` = @quizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", quizRubric.QuizId);
                }
                if (!String.IsNullOrEmpty(quizRubric.Desc))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `desc` = @desc");
                    else
                        sql = String.Concat(sql, " `desc` = @desc");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@desc", quizRubric.Desc);
                }
                if (!String.IsNullOrEmpty(quizRubric.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", quizRubric.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(quizRubric.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", quizRubric.CreatedAt);
                }
                if (!String.IsNullOrEmpty(quizRubric.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", quizRubric.UpdatedAt);
                }
                if (quizRubricWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(quizRubricWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", quizRubricWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(quizRubricWhere.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizIdWhere", quizRubricWhere.QuizId);
                    }
                    if (!String.IsNullOrEmpty(quizRubricWhere.Desc))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `desc` = @descWhere");
                        else
                            sql = String.Concat(sql, " `desc` = @descWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@descWhere", quizRubricWhere.Desc);
                    }
                    if (!String.IsNullOrEmpty(quizRubricWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", quizRubricWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(quizRubricWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", quizRubricWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(quizRubricWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", quizRubricWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(quizRubric);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizRubricModel>();
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

        public List<QuizRubricModel> Update(string connectionString, List<QuizRubricModel> quizRubricList, List<QuizRubricModel> quizRubricWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizRubricModel> response = new List<QuizRubricModel>();
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
                        for (int i = 0; i < quizRubricList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE quiz_rubrics SET ";
                                if (!String.IsNullOrEmpty(quizRubricList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, quizRubricList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(quizRubricList[i].QuizId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `quiz_id` = @quizId" + i);
                                    else
                                        sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, quizRubricList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(quizRubricList[i].Desc))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `desc` = @desc" + i);
                                    else
                                        sql = String.Concat(sql, " `desc` = @desc" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@desc" + i, quizRubricList[i].Desc);
                                }
                                if (!String.IsNullOrEmpty(quizRubricList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizRubricList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(quizRubricList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizRubricList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(quizRubricList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizRubricList[i].UpdatedAt);
                                }
                                if (quizRubricWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(quizRubricWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, quizRubricWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(quizRubricWhereList[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizIdWhere" + i, quizRubricWhereList[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(quizRubricWhereList[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `desc` = @descWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `desc` = @descWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@descWhere" + i, quizRubricWhereList[i].Desc);
                                    }
                                    if (!String.IsNullOrEmpty(quizRubricWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, quizRubricWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(quizRubricWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, quizRubricWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(quizRubricWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, quizRubricWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizRubricList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizRubricModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizRubricModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizRubricModel>();
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

        public List<QuizRubricModel> Delete(string connectionString, QuizRubricModel quizRubric)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizRubricModel> response = new List<QuizRubricModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM quiz_rubrics  ";
                if (quizRubric != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(quizRubric.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", quizRubric.Id);
                    }
                    if (!String.IsNullOrEmpty(quizRubric.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizId");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizId", quizRubric.QuizId);
                    }
                    if (!String.IsNullOrEmpty(quizRubric.Desc))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `desc` = @desc");
                        else
                            sql = String.Concat(sql, " `desc` = @desc");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@desc", quizRubric.Desc);
                    }
                    if (!String.IsNullOrEmpty(quizRubric.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", quizRubric.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(quizRubric.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", quizRubric.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(quizRubric.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", quizRubric.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(quizRubric);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizRubricModel>();
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

        public List<QuizRubricModel> Delete(string connectionString, List<QuizRubricModel> quizRubric)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizRubricModel> response = new List<QuizRubricModel>();
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
                        for (int i = 0; i < quizRubric.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM quiz_rubrics  ";
                                if (quizRubric[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(quizRubric[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, quizRubric[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(quizRubric[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizId" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizId" + i, quizRubric[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(quizRubric[i].Desc))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `desc` = @desc" + i);
                                        else
                                            sql = String.Concat(sql, " `desc` = @desc" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@desc" + i, quizRubric[i].Desc);
                                    }
                                    if (!String.IsNullOrEmpty(quizRubric[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, quizRubric[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(quizRubric[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, quizRubric[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(quizRubric[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, quizRubric[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizRubric[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizRubricModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizRubricModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizRubricModel>();
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
