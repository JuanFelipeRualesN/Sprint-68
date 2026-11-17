using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Lessons;

namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons
{
    class QuizQuestionTypesDAO
    {
        private MySqlConnection _connection;

        public Dictionary<string, QuizQuestionTypesModel> ReadD(string connectionString,
            QuizQuestionTypesModel quizQuestionTypes, bool firstOne, bool orderByLastDate,
            QuizQuestionTypesModel quizQuestionTypesWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionTypesModel> response = new Dictionary<string, QuizQuestionTypesModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql =
                    "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_types ";
                {
                    if (quizQuestionTypes != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestionTypes.Id))
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

                            cmd.Parameters.AddWithValue("@id", quizQuestionTypes.Id);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `name` = @name ");
                            }

                            cmd.Parameters.AddWithValue("@name", quizQuestionTypes.Name);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.AutoCorrect))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `auto_correct` = @autoCorrect ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `auto_correct` = @autoCorrect ");
                            }

                            cmd.Parameters.AddWithValue("@autoCorrect", quizQuestionTypes.AutoCorrect);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.ModifiedBy))
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

                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionTypes.ModifiedBy);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.CreatedAt))
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

                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionTypes.CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.UpdatedAt))
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

                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionTypes.UpdatedAt);
                        }
                    }

                    if (quizQuestionTypesWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizQuestionTypesWhereLike.Id + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + quizQuestionTypesWhereLike.Name + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.AutoCorrect))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `auto_correct` LIKE @autoCorrectWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `auto_correct` LIKE @autoCorrectWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@autoCorrectWhereLike",
                                "%" + quizQuestionTypesWhereLike.AutoCorrect + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike",
                                "%" + quizQuestionTypesWhereLike.ModifiedBy + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike",
                                "%" + quizQuestionTypesWhereLike.CreatedAt + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike",
                                "%" + quizQuestionTypesWhereLike.UpdatedAt + "%");
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
                                quizQuestionTypes = new QuizQuestionTypesModel();
                                if (!reader.IsDBNull(0))
                                    quizQuestionTypes.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionTypes.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionTypes.Name = reader.GetValue(1).ToString();
                                else
                                    quizQuestionTypes.Name = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionTypes.AutoCorrect = reader.GetValue(2).ToString();
                                else
                                    quizQuestionTypes.AutoCorrect = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionTypes.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizQuestionTypes.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionTypes.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizQuestionTypes.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionTypes.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizQuestionTypes.UpdatedAt = null;
                                response.Add(quizQuestionTypes.Id, quizQuestionTypes);
                                quizQuestionTypes = new QuizQuestionTypesModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizQuestionTypesModel>();
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
                string MessageString =
                    " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizQuestionTypesModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public Dictionary<string, QuizQuestionTypesModel> ReadD(string connectionString,
            QuizQuestionTypesModel quizQuestionTypes, bool orderByLastDate, int fromRow, int limit,
            QuizQuestionTypesModel quizQuestionTypesWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionTypesModel> response = new Dictionary<string, QuizQuestionTypesModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql =
                    "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_types ";
                {
                    if (quizQuestionTypes != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestionTypes.Id))
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

                            cmd.Parameters.AddWithValue("@id", quizQuestionTypes.Id);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `name` = @name ");
                            }

                            cmd.Parameters.AddWithValue("@name", quizQuestionTypes.Name);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.AutoCorrect))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `auto_correct` = @autoCorrect ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `auto_correct` = @autoCorrect ");
                            }

                            cmd.Parameters.AddWithValue("@autoCorrect", quizQuestionTypes.AutoCorrect);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.ModifiedBy))
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

                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionTypes.ModifiedBy);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.CreatedAt))
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

                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionTypes.CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.UpdatedAt))
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

                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionTypes.UpdatedAt);
                        }
                    }

                    if (quizQuestionTypesWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizQuestionTypesWhereLike.Id + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + quizQuestionTypesWhereLike.Name + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.AutoCorrect))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `auto_correct` LIKE @autoCorrectWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `auto_correct` LIKE @autoCorrectWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@autoCorrectWhereLike",
                                "%" + quizQuestionTypesWhereLike.AutoCorrect + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike",
                                "%" + quizQuestionTypesWhereLike.ModifiedBy + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike",
                                "%" + quizQuestionTypesWhereLike.CreatedAt + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike",
                                "%" + quizQuestionTypesWhereLike.UpdatedAt + "%");
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
                                quizQuestionTypes = new QuizQuestionTypesModel();
                                if (!reader.IsDBNull(0))
                                    quizQuestionTypes.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionTypes.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionTypes.Name = reader.GetValue(1).ToString();
                                else
                                    quizQuestionTypes.Name = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionTypes.AutoCorrect = reader.GetValue(2).ToString();
                                else
                                    quizQuestionTypes.AutoCorrect = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionTypes.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizQuestionTypes.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionTypes.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizQuestionTypes.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionTypes.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizQuestionTypes.UpdatedAt = null;
                                response.Add(quizQuestionTypes.Id, quizQuestionTypes);
                                quizQuestionTypes = new QuizQuestionTypesModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizQuestionTypesModel>();
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
                string MessageString =
                    " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizQuestionTypesModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public Dictionary<string, QuizQuestionTypesModel> ReadD(string connectionString,
            List<QuizQuestionTypesModel> quizQuestionTypesWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionTypesModel> response = new Dictionary<string, QuizQuestionTypesModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql =
                    "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_types ";
                if (quizQuestionTypesWhere != null && quizQuestionTypesWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionTypesWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionTypesWhere[i].Id);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, quizQuestionTypesWhere[i].Name);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].AutoCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `auto_correct` = @autoCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@autoCorrect" + i, quizQuestionTypesWhere[i].AutoCorrect);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionTypesWhere[i].ModifiedBy);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionTypesWhere[i].CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionTypesWhere[i].UpdatedAt);
                        }

                        sqlIn += ")";
                        if (i != quizQuestionTypesWhere.Count - 1)
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
                            QuizQuestionTypesModel quizQuestionTypes = new QuizQuestionTypesModel();
                            if (!reader.IsDBNull(0))
                                quizQuestionTypes.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionTypes.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionTypes.Name = reader.GetValue(1).ToString();
                            else
                                quizQuestionTypes.Name = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionTypes.AutoCorrect = reader.GetValue(2).ToString();
                            else
                                quizQuestionTypes.AutoCorrect = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionTypes.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizQuestionTypes.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionTypes.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizQuestionTypes.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionTypes.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizQuestionTypes.UpdatedAt = null;
                            response.Add(quizQuestionTypes.Id, quizQuestionTypes);
                            quizQuestionTypes = new QuizQuestionTypesModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizQuestionTypesModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString =
                    " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizQuestionTypesModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public Dictionary<string, QuizQuestionTypesModel> ReadD(string connectionString,
            List<QuizQuestionTypesModel> quizQuestionTypesWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizQuestionTypesModel> response = new Dictionary<string, QuizQuestionTypesModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql =
                    "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_types ";
                if (quizQuestionTypesWhere != null && quizQuestionTypesWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionTypesWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionTypesWhere[i].Id);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, quizQuestionTypesWhere[i].Name);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].AutoCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `auto_correct` = @autoCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@autoCorrect" + i, quizQuestionTypesWhere[i].AutoCorrect);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionTypesWhere[i].ModifiedBy);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionTypesWhere[i].CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionTypesWhere[i].UpdatedAt);
                        }

                        sqlIn += ")";
                        if (i != quizQuestionTypesWhere.Count - 1)
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
                            QuizQuestionTypesModel quizQuestionTypes = new QuizQuestionTypesModel();
                            if (!reader.IsDBNull(0))
                                quizQuestionTypes.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionTypes.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionTypes.Name = reader.GetValue(1).ToString();
                            else
                                quizQuestionTypes.Name = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionTypes.AutoCorrect = reader.GetValue(2).ToString();
                            else
                                quizQuestionTypes.AutoCorrect = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionTypes.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizQuestionTypes.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionTypes.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizQuestionTypes.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionTypes.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizQuestionTypes.UpdatedAt = null;
                            response.Add(quizQuestionTypes.Id, quizQuestionTypes);
                            quizQuestionTypes = new QuizQuestionTypesModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizQuestionTypesModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString =
                    " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizQuestionTypesModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public List<QuizQuestionTypesModel> Read(string connectionString, QuizQuestionTypesModel quizQuestionTypes,
            bool firstOne, bool orderByLastDate, QuizQuestionTypesModel quizQuestionTypesWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionTypesModel> response = new List<QuizQuestionTypesModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql =
                    "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_types ";
                {
                    if (quizQuestionTypes != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestionTypes.Id))
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

                            cmd.Parameters.AddWithValue("@id", quizQuestionTypes.Id);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `name` = @name ");
                            }

                            cmd.Parameters.AddWithValue("@name", quizQuestionTypes.Name);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.AutoCorrect))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `auto_correct` = @autoCorrect ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `auto_correct` = @autoCorrect ");
                            }

                            cmd.Parameters.AddWithValue("@autoCorrect", quizQuestionTypes.AutoCorrect);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.ModifiedBy))
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

                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionTypes.ModifiedBy);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.CreatedAt))
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

                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionTypes.CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.UpdatedAt))
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

                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionTypes.UpdatedAt);
                        }
                    }

                    if (quizQuestionTypesWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizQuestionTypesWhereLike.Id + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + quizQuestionTypesWhereLike.Name + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.AutoCorrect))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `auto_correct` LIKE @autoCorrectWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `auto_correct` LIKE @autoCorrectWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@autoCorrectWhereLike",
                                "%" + quizQuestionTypesWhereLike.AutoCorrect + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike",
                                "%" + quizQuestionTypesWhereLike.ModifiedBy + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike",
                                "%" + quizQuestionTypesWhereLike.CreatedAt + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike",
                                "%" + quizQuestionTypesWhereLike.UpdatedAt + "%");
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
                                quizQuestionTypes = new QuizQuestionTypesModel();
                                if (!reader.IsDBNull(0))
                                    quizQuestionTypes.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionTypes.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionTypes.Name = reader.GetValue(1).ToString();
                                else
                                    quizQuestionTypes.Name = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionTypes.AutoCorrect = reader.GetValue(2).ToString();
                                else
                                    quizQuestionTypes.AutoCorrect = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionTypes.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizQuestionTypes.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionTypes.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizQuestionTypes.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionTypes.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizQuestionTypes.UpdatedAt = null;
                                response.Add(quizQuestionTypes);
                                quizQuestionTypes = new QuizQuestionTypesModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizQuestionTypesModel>();
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
                string MessageString =
                    " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionTypesModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public List<QuizQuestionTypesModel> Read(string connectionString, QuizQuestionTypesModel quizQuestionTypes,
            bool orderByLastDate, int fromRow, int limit, QuizQuestionTypesModel quizQuestionTypesWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionTypesModel> response = new List<QuizQuestionTypesModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql =
                    "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_types ";
                {
                    if (quizQuestionTypes != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestionTypes.Id))
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

                            cmd.Parameters.AddWithValue("@id", quizQuestionTypes.Id);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `name` = @name ");
                            }

                            cmd.Parameters.AddWithValue("@name", quizQuestionTypes.Name);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.AutoCorrect))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `auto_correct` = @autoCorrect ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `auto_correct` = @autoCorrect ");
                            }

                            cmd.Parameters.AddWithValue("@autoCorrect", quizQuestionTypes.AutoCorrect);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.ModifiedBy))
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

                            cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionTypes.ModifiedBy);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.CreatedAt))
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

                            cmd.Parameters.AddWithValue("@createdAt", quizQuestionTypes.CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypes.UpdatedAt))
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

                            cmd.Parameters.AddWithValue("@updatedAt", quizQuestionTypes.UpdatedAt);
                        }
                    }

                    if (quizQuestionTypesWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizQuestionTypesWhereLike.Id + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.Name))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `name` LIKE @nameWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `name` LIKE @nameWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + quizQuestionTypesWhereLike.Name + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.AutoCorrect))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `auto_correct` LIKE @autoCorrectWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `auto_correct` LIKE @autoCorrectWhereLike ");
                            }

                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@autoCorrectWhereLike",
                                "%" + quizQuestionTypesWhereLike.AutoCorrect + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike",
                                "%" + quizQuestionTypesWhereLike.ModifiedBy + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike",
                                "%" + quizQuestionTypesWhereLike.CreatedAt + "%");
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike",
                                "%" + quizQuestionTypesWhereLike.UpdatedAt + "%");
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
                                quizQuestionTypes = new QuizQuestionTypesModel();
                                if (!reader.IsDBNull(0))
                                    quizQuestionTypes.Id = reader.GetValue(0).ToString();
                                else
                                    quizQuestionTypes.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizQuestionTypes.Name = reader.GetValue(1).ToString();
                                else
                                    quizQuestionTypes.Name = null;
                                if (!reader.IsDBNull(2))
                                    quizQuestionTypes.AutoCorrect = reader.GetValue(2).ToString();
                                else
                                    quizQuestionTypes.AutoCorrect = null;
                                if (!reader.IsDBNull(3))
                                    quizQuestionTypes.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizQuestionTypes.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizQuestionTypes.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizQuestionTypes.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizQuestionTypes.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizQuestionTypes.UpdatedAt = null;
                                response.Add(quizQuestionTypes);
                                quizQuestionTypes = new QuizQuestionTypesModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizQuestionTypesModel>();
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
                string MessageString =
                    " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionTypesModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public List<QuizQuestionTypesModel> Read(string connectionString,
            List<QuizQuestionTypesModel> quizQuestionTypesWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionTypesModel> response = new List<QuizQuestionTypesModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql =
                    "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_types ";
                if (quizQuestionTypesWhere != null && quizQuestionTypesWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionTypesWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionTypesWhere[i].Id);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, quizQuestionTypesWhere[i].Name);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].AutoCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `auto_correct` = @autoCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@autoCorrect" + i, quizQuestionTypesWhere[i].AutoCorrect);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionTypesWhere[i].ModifiedBy);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionTypesWhere[i].CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionTypesWhere[i].UpdatedAt);
                        }

                        sqlIn += ")";
                        if (i != quizQuestionTypesWhere.Count - 1)
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
                            QuizQuestionTypesModel quizQuestionTypes = new QuizQuestionTypesModel();
                            if (!reader.IsDBNull(0))
                                quizQuestionTypes.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionTypes.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionTypes.Name = reader.GetValue(1).ToString();
                            else
                                quizQuestionTypes.Name = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionTypes.AutoCorrect = reader.GetValue(2).ToString();
                            else
                                quizQuestionTypes.AutoCorrect = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionTypes.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizQuestionTypes.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionTypes.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizQuestionTypes.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionTypes.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizQuestionTypes.UpdatedAt = null;
                            response.Add(quizQuestionTypes);
                            quizQuestionTypes = new QuizQuestionTypesModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizQuestionTypesModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString =
                    " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionTypesModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public List<QuizQuestionTypesModel> Read(string connectionString,
            List<QuizQuestionTypesModel> quizQuestionTypesWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionTypesModel> response = new List<QuizQuestionTypesModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql =
                    "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_question_types ";
                if (quizQuestionTypesWhere != null && quizQuestionTypesWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizQuestionTypesWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizQuestionTypesWhere[i].Id);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, quizQuestionTypesWhere[i].Name);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].AutoCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `auto_correct` = @autoCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@autoCorrect" + i, quizQuestionTypesWhere[i].AutoCorrect);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionTypesWhere[i].ModifiedBy);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionTypesWhere[i].CreatedAt);
                        }

                        if (!String.IsNullOrEmpty(quizQuestionTypesWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionTypesWhere[i].UpdatedAt);
                        }

                        sqlIn += ")";
                        if (i != quizQuestionTypesWhere.Count - 1)
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
                            QuizQuestionTypesModel quizQuestionTypes = new QuizQuestionTypesModel();
                            if (!reader.IsDBNull(0))
                                quizQuestionTypes.Id = reader.GetValue(0).ToString();
                            else
                                quizQuestionTypes.Id = null;
                            if (!reader.IsDBNull(1))
                                quizQuestionTypes.Name = reader.GetValue(1).ToString();
                            else
                                quizQuestionTypes.Name = null;
                            if (!reader.IsDBNull(2))
                                quizQuestionTypes.AutoCorrect = reader.GetValue(2).ToString();
                            else
                                quizQuestionTypes.AutoCorrect = null;
                            if (!reader.IsDBNull(3))
                                quizQuestionTypes.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizQuestionTypes.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizQuestionTypes.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizQuestionTypes.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizQuestionTypes.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizQuestionTypes.UpdatedAt = null;
                            response.Add(quizQuestionTypes);
                            quizQuestionTypes = new QuizQuestionTypesModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizQuestionTypesModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString =
                    " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionTypesModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }

            return response;
        }

        public List<QuizQuestionTypesModel> Create(string connectionString, QuizQuestionTypesModel quizQuestionTypes)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionTypesModel> response = new List<QuizQuestionTypesModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO quiz_question_types (";
                string values = "(";
                if (!String.IsNullOrEmpty(quizQuestionTypes.Id))
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
                    cmd.Parameters.AddWithValue("@id", quizQuestionTypes.Id);
                }

                if (!String.IsNullOrEmpty(quizQuestionTypes.Name))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `name` ");
                        values = String.Concat(values, ", @name");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `name` ");
                        values = String.Concat(values, " @name");
                    }

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", quizQuestionTypes.Name);
                }

                if (!String.IsNullOrEmpty(quizQuestionTypes.AutoCorrect))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `auto_correct` ");
                        values = String.Concat(values, ", @autoCorrect");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `auto_correct` ");
                        values = String.Concat(values, " @autoCorrect");
                    }

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@autoCorrect", quizQuestionTypes.AutoCorrect);
                }

                if (!String.IsNullOrEmpty(quizQuestionTypes.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionTypes.ModifiedBy);
                }

                if (!String.IsNullOrEmpty(quizQuestionTypes.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", quizQuestionTypes.CreatedAt);
                }

                if (!String.IsNullOrEmpty(quizQuestionTypes.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", quizQuestionTypes.UpdatedAt);
                }

                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                quizQuestionTypes.Id = cmd.LastInsertedId.ToString();
                response.Add(quizQuestionTypes);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " +
                                       e.Message;
                response = new List<QuizQuestionTypesModel>();
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

        public List<QuizQuestionTypesModel> Create(string connectionString,
            List<QuizQuestionTypesModel> quizQuestionTypesList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionTypesModel> response = new List<QuizQuestionTypesModel>();
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
                        for (int i = 0; i < quizQuestionTypesList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO quiz_question_types (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(quizQuestionTypesList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, quizQuestionTypesList[i].Id);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionTypesList[i].Name))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `name` ");
                                        values = String.Concat(values, ", @name" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `name` ");
                                        values = String.Concat(values, "@name" + i);
                                    }

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, quizQuestionTypesList[i].Name);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionTypesList[i].AutoCorrect))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `auto_correct` ");
                                        values = String.Concat(values, ", @autoCorrect" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `auto_correct` ");
                                        values = String.Concat(values, "@autoCorrect" + i);
                                    }

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@autoCorrect" + i,
                                        quizQuestionTypesList[i].AutoCorrect);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionTypesList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionTypesList[i].ModifiedBy);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionTypesList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionTypesList[i].CreatedAt);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionTypesList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionTypesList[i].UpdatedAt);
                                }

                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(quizQuestionTypesList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " +
                                                       e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionTypesModel>();
                            }
                        }

                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " +
                                               e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionTypesModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString =
                    "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizQuestionTypesModel>();
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

        public List<QuizQuestionTypesModel> Update(string connectionString, QuizQuestionTypesModel quizQuestionTypes,
            QuizQuestionTypesModel quizQuestionTypesWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionTypesModel> response = new List<QuizQuestionTypesModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE quiz_question_types SET ";
                if (!String.IsNullOrEmpty(quizQuestionTypes.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", quizQuestionTypes.Id);
                }

                if (!String.IsNullOrEmpty(quizQuestionTypes.Name))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `name` = @name");
                    else
                        sql = String.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", quizQuestionTypes.Name);
                }

                if (!String.IsNullOrEmpty(quizQuestionTypes.AutoCorrect))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `auto_correct` = @autoCorrect");
                    else
                        sql = String.Concat(sql, " `auto_correct` = @autoCorrect");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@autoCorrect", quizQuestionTypes.AutoCorrect);
                }

                if (!String.IsNullOrEmpty(quizQuestionTypes.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionTypes.ModifiedBy);
                }

                if (!String.IsNullOrEmpty(quizQuestionTypes.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", quizQuestionTypes.CreatedAt);
                }

                if (!String.IsNullOrEmpty(quizQuestionTypes.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", quizQuestionTypes.UpdatedAt);
                }

                if (quizQuestionTypesWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(quizQuestionTypesWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", quizQuestionTypesWhere.Id);
                    }

                    if (!String.IsNullOrEmpty(quizQuestionTypesWhere.Name))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = String.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", quizQuestionTypesWhere.Name);
                    }

                    if (!String.IsNullOrEmpty(quizQuestionTypesWhere.AutoCorrect))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `auto_correct` = @autoCorrectWhere");
                        else
                            sql = String.Concat(sql, " `auto_correct` = @autoCorrectWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@autoCorrectWhere", quizQuestionTypesWhere.AutoCorrect);
                    }

                    if (!String.IsNullOrEmpty(quizQuestionTypesWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", quizQuestionTypesWhere.ModifiedBy);
                    }

                    if (!String.IsNullOrEmpty(quizQuestionTypesWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", quizQuestionTypesWhere.CreatedAt);
                    }

                    if (!String.IsNullOrEmpty(quizQuestionTypesWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", quizQuestionTypesWhere.UpdatedAt);
                    }
                }

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(quizQuestionTypes);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " +
                                       e.Message;
                response = new List<QuizQuestionTypesModel>();
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

        public List<QuizQuestionTypesModel> Update(string connectionString,
            List<QuizQuestionTypesModel> quizQuestionTypesList, List<QuizQuestionTypesModel> quizQuestionTypesWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionTypesModel> response = new List<QuizQuestionTypesModel>();
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
                        for (int i = 0; i < quizQuestionTypesList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE quiz_question_types SET ";
                                if (!String.IsNullOrEmpty(quizQuestionTypesList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, quizQuestionTypesList[i].Id);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionTypesList[i].Name))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = String.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, quizQuestionTypesList[i].Name);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionTypesList[i].AutoCorrect))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `auto_correct` = @autoCorrect" + i);
                                    else
                                        sql = String.Concat(sql, " `auto_correct` = @autoCorrect" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@autoCorrect" + i,
                                        quizQuestionTypesList[i].AutoCorrect);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionTypesList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionTypesList[i].ModifiedBy);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionTypesList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionTypesList[i].CreatedAt);
                                }

                                if (!String.IsNullOrEmpty(quizQuestionTypesList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionTypesList[i].UpdatedAt);
                                }

                                if (quizQuestionTypesWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(quizQuestionTypesWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, quizQuestionTypesWhereList[i].Id);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestionTypesWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i,
                                            quizQuestionTypesWhereList[i].Name);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestionTypesWhereList[i].AutoCorrect))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `auto_correct` = @autoCorrectWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `auto_correct` = @autoCorrectWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@autoCorrectWhere" + i,
                                            quizQuestionTypesWhereList[i].AutoCorrect);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestionTypesWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i,
                                            quizQuestionTypesWhereList[i].ModifiedBy);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestionTypesWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i,
                                            quizQuestionTypesWhereList[i].CreatedAt);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestionTypesWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i,
                                            quizQuestionTypesWhereList[i].UpdatedAt);
                                    }
                                }

                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizQuestionTypesList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" +
                                                       e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionTypesModel>();
                            }
                        }

                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode +
                                               " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionTypesModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " +
                                       e.Message;
                response = new List<QuizQuestionTypesModel>();
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

        public List<QuizQuestionTypesModel> Delete(string connectionString, QuizQuestionTypesModel quizQuestionTypes)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionTypesModel> response = new List<QuizQuestionTypesModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM quiz_question_types  ";
                if (quizQuestionTypes != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(quizQuestionTypes.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", quizQuestionTypes.Id);
                    }

                    if (!String.IsNullOrEmpty(quizQuestionTypes.Name))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `name` = @name");
                        else
                            sql = String.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", quizQuestionTypes.Name);
                    }

                    if (!String.IsNullOrEmpty(quizQuestionTypes.AutoCorrect))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `auto_correct` = @autoCorrect");
                        else
                            sql = String.Concat(sql, " `auto_correct` = @autoCorrect");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@autoCorrect", quizQuestionTypes.AutoCorrect);
                    }

                    if (!String.IsNullOrEmpty(quizQuestionTypes.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", quizQuestionTypes.ModifiedBy);
                    }

                    if (!String.IsNullOrEmpty(quizQuestionTypes.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", quizQuestionTypes.CreatedAt);
                    }

                    if (!String.IsNullOrEmpty(quizQuestionTypes.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", quizQuestionTypes.UpdatedAt);
                    }
                }

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(quizQuestionTypes);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " +
                                       e.Message;
                response = new List<QuizQuestionTypesModel>();
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

        public List<QuizQuestionTypesModel> Delete(string connectionString,
            List<QuizQuestionTypesModel> quizQuestionTypes)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizQuestionTypesModel> response = new List<QuizQuestionTypesModel>();
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
                        for (int i = 0; i < quizQuestionTypes.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM quiz_question_types  ";
                                if (quizQuestionTypes[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(quizQuestionTypes[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, quizQuestionTypes[i].Id);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestionTypes[i].Name))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = String.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, quizQuestionTypes[i].Name);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestionTypes[i].AutoCorrect))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `auto_correct` = @autoCorrect" + i);
                                        else
                                            sql = String.Concat(sql, " `auto_correct` = @autoCorrect" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@autoCorrect" + i,
                                            quizQuestionTypes[i].AutoCorrect);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestionTypes[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, quizQuestionTypes[i].ModifiedBy);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestionTypes[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, quizQuestionTypes[i].CreatedAt);
                                    }

                                    if (!String.IsNullOrEmpty(quizQuestionTypes[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, quizQuestionTypes[i].UpdatedAt);
                                    }
                                }

                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizQuestionTypes[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" +
                                                       e.ErrorCode + " - " + e.Message;
                                response = new List<QuizQuestionTypesModel>();
                            }
                        }

                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode +
                                               " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizQuestionTypesModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " +
                                       e.Message;
                response = new List<QuizQuestionTypesModel>();
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