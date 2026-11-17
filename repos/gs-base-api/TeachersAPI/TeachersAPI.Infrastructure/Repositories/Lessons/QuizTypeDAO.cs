using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Lessons;

namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons
{
    class QuizTypeDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, QuizTypeModel> ReadD(string connectionString, QuizTypeModel quizType, bool firstOne, bool orderByLastDate, QuizTypeModel quizTypeWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizTypeModel> response = new Dictionary<string, QuizTypeModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_types ";
                {
                    if (quizType != null)
                    {
                        if (!String.IsNullOrEmpty(quizType.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizType.Id);
                        }
                        if (!String.IsNullOrEmpty(quizType.Name))
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
                            cmd.Parameters.AddWithValue("@name", quizType.Name);
                        }
                        if (!String.IsNullOrEmpty(quizType.AutoCorrect))
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
                            cmd.Parameters.AddWithValue("@autoCorrect", quizType.AutoCorrect);
                        }
                        if (!String.IsNullOrEmpty(quizType.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizType.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizType.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizType.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizType.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizType.UpdatedAt);
                        }
                    }
                    if (quizTypeWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizTypeWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.Name))
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
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + quizTypeWhereLike.Name + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.AutoCorrect))
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
                            cmd.Parameters.AddWithValue("@autoCorrectWhereLike", "%" + quizTypeWhereLike.AutoCorrect + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizTypeWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizTypeWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizTypeWhereLike.UpdatedAt + "%");
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
                                quizType = new QuizTypeModel();
                                if (!reader.IsDBNull(0))
                                    quizType.Id = reader.GetValue(0).ToString();
                                else
                                    quizType.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizType.Name = reader.GetValue(1).ToString();
                                else
                                    quizType.Name = null;
                                if (!reader.IsDBNull(2))
                                    quizType.AutoCorrect = reader.GetValue(2).ToString();
                                else
                                    quizType.AutoCorrect = null;
                                if (!reader.IsDBNull(3))
                                    quizType.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizType.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizType.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizType.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizType.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizType.UpdatedAt = null;
                                response.Add(quizType.Id, quizType);
                                quizType = new QuizTypeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizTypeModel>();
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
                response = new Dictionary<string, QuizTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizTypeModel> ReadD(string connectionString, QuizTypeModel quizType, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizTypeModel> response = new Dictionary<string, QuizTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_types ";
                {
                    if (quizType != null)
                    {
                        if (!string.IsNullOrEmpty(quizType.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizType.Id);
                        }
                        if (!string.IsNullOrEmpty(quizType.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", quizType.Name);
                        }
                        if (!string.IsNullOrEmpty(quizType.AutoCorrect))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `auto_correct` = @autoCorrect ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `auto_correct` = @autoCorrect ");
                            }
                            cmd.Parameters.AddWithValue("@autoCorrect", quizType.AutoCorrect);
                        }
                        if (!string.IsNullOrEmpty(quizType.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizType.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizType.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizType.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizType.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizType.UpdatedAt);
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
                                    quizType.Id = reader.GetValue(0).ToString();
                                else
                                    quizType.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizType.Name = reader.GetValue(1).ToString();
                                else
                                    quizType.Name = null;
                                if (!reader.IsDBNull(2))
                                    quizType.AutoCorrect = reader.GetValue(2).ToString();
                                else
                                    quizType.AutoCorrect = null;
                                if (!reader.IsDBNull(3))
                                    quizType.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizType.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizType.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizType.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizType.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizType.UpdatedAt = null;
                                response.Add(quizType.Id, quizType);
                                quizType = new QuizTypeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizTypeModel>();
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
                response = new Dictionary<string, QuizTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizTypeModel> ReadD(string connectionString, QuizTypeModel quizType, bool orderByLastDate, int fromRow, int limit, QuizTypeModel quizTypeWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizTypeModel> response = new Dictionary<string, QuizTypeModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_types ";
                {
                    if (quizType != null)
                    {
                        if (!String.IsNullOrEmpty(quizType.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizType.Id);
                        }
                        if (!String.IsNullOrEmpty(quizType.Name))
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
                            cmd.Parameters.AddWithValue("@name", quizType.Name);
                        }
                        if (!String.IsNullOrEmpty(quizType.AutoCorrect))
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
                            cmd.Parameters.AddWithValue("@autoCorrect", quizType.AutoCorrect);
                        }
                        if (!String.IsNullOrEmpty(quizType.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizType.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizType.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizType.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizType.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizType.UpdatedAt);
                        }
                    }
                    if (quizTypeWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizTypeWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.Name))
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
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + quizTypeWhereLike.Name + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.AutoCorrect))
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
                            cmd.Parameters.AddWithValue("@autoCorrectWhereLike", "%" + quizTypeWhereLike.AutoCorrect + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizTypeWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizTypeWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizTypeWhereLike.UpdatedAt + "%");
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
                                quizType = new QuizTypeModel();
                                if (!reader.IsDBNull(0))
                                    quizType.Id = reader.GetValue(0).ToString();
                                else
                                    quizType.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizType.Name = reader.GetValue(1).ToString();
                                else
                                    quizType.Name = null;
                                if (!reader.IsDBNull(2))
                                    quizType.AutoCorrect = reader.GetValue(2).ToString();
                                else
                                    quizType.AutoCorrect = null;
                                if (!reader.IsDBNull(3))
                                    quizType.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizType.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizType.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizType.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizType.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizType.UpdatedAt = null;
                                response.Add(quizType.Id, quizType);
                                quizType = new QuizTypeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizTypeModel>();
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
                response = new Dictionary<string, QuizTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizTypeModel> ReadD(string connectionString, QuizTypeModel quizType, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizTypeModel> response = new Dictionary<string, QuizTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_types ";
                {
                    if (quizType != null)
                    {
                        if (!string.IsNullOrEmpty(quizType.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizType.Id);
                        }
                        if (!string.IsNullOrEmpty(quizType.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", quizType.Name);
                        }
                        if (!string.IsNullOrEmpty(quizType.AutoCorrect))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `auto_correct` = @autoCorrect ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `auto_correct` = @autoCorrect ");
                            }
                            cmd.Parameters.AddWithValue("@autoCorrect", quizType.AutoCorrect);
                        }
                        if (!string.IsNullOrEmpty(quizType.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizType.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizType.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizType.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizType.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizType.UpdatedAt);
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
                                    quizType.Id = reader.GetValue(0).ToString();
                                else
                                    quizType.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizType.Name = reader.GetValue(1).ToString();
                                else
                                    quizType.Name = null;
                                if (!reader.IsDBNull(2))
                                    quizType.AutoCorrect = reader.GetValue(2).ToString();
                                else
                                    quizType.AutoCorrect = null;
                                if (!reader.IsDBNull(3))
                                    quizType.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizType.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizType.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizType.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizType.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizType.UpdatedAt = null;
                                response.Add(quizType.Id, quizType);
                                quizType = new QuizTypeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizTypeModel>();
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
                response = new Dictionary<string, QuizTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizTypeModel> ReadD(string connectionString, List<QuizTypeModel> quizTypeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizTypeModel> response = new Dictionary<string, QuizTypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_types ";
                if (quizTypeWhere != null && quizTypeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizTypeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizTypeWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, quizTypeWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].AutoCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `auto_correct` = @autoCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@autoCorrect" + i, quizTypeWhere[i].AutoCorrect);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizTypeWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizTypeWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizTypeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizTypeWhere.Count - 1)
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
                            QuizTypeModel quizType = new QuizTypeModel(); if (!reader.IsDBNull(0))
                                quizType.Id = reader.GetValue(0).ToString();
                            else
                                quizType.Id = null;
                            if (!reader.IsDBNull(1))
                                quizType.Name = reader.GetValue(1).ToString();
                            else
                                quizType.Name = null;
                            if (!reader.IsDBNull(2))
                                quizType.AutoCorrect = reader.GetValue(2).ToString();
                            else
                                quizType.AutoCorrect = null;
                            if (!reader.IsDBNull(3))
                                quizType.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizType.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizType.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizType.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizType.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizType.UpdatedAt = null;
                            response.Add(quizType.Id, quizType);
                            quizType = new QuizTypeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizTypeModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizTypeModel> ReadD(string connectionString, List<QuizTypeModel> quizTypeWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizTypeModel> response = new Dictionary<string, QuizTypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_types ";
                if (quizTypeWhere != null && quizTypeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizTypeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizTypeWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, quizTypeWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].AutoCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `auto_correct` = @autoCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@autoCorrect" + i, quizTypeWhere[i].AutoCorrect);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizTypeWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizTypeWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizTypeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizTypeWhere.Count - 1)
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
                            QuizTypeModel quizType = new QuizTypeModel(); if (!reader.IsDBNull(0))
                                quizType.Id = reader.GetValue(0).ToString();
                            else
                                quizType.Id = null;
                            if (!reader.IsDBNull(1))
                                quizType.Name = reader.GetValue(1).ToString();
                            else
                                quizType.Name = null;
                            if (!reader.IsDBNull(2))
                                quizType.AutoCorrect = reader.GetValue(2).ToString();
                            else
                                quizType.AutoCorrect = null;
                            if (!reader.IsDBNull(3))
                                quizType.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizType.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizType.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizType.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizType.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizType.UpdatedAt = null;
                            response.Add(quizType.Id, quizType);
                            quizType = new QuizTypeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizTypeModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizTypeModel> Read(string connectionString, QuizTypeModel quizType, bool firstOne, bool orderByLastDate, QuizTypeModel quizTypeWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizTypeModel> response = new List<QuizTypeModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_types ";
                {
                    if (quizType != null)
                    {
                        if (!String.IsNullOrEmpty(quizType.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizType.Id);
                        }
                        if (!String.IsNullOrEmpty(quizType.Name))
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
                            cmd.Parameters.AddWithValue("@name", quizType.Name);
                        }
                        if (!String.IsNullOrEmpty(quizType.AutoCorrect))
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
                            cmd.Parameters.AddWithValue("@autoCorrect", quizType.AutoCorrect);
                        }
                        if (!String.IsNullOrEmpty(quizType.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizType.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizType.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizType.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizType.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizType.UpdatedAt);
                        }
                    }
                    if (quizTypeWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizTypeWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.Name))
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
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + quizTypeWhereLike.Name + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.AutoCorrect))
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
                            cmd.Parameters.AddWithValue("@autoCorrectWhereLike", "%" + quizTypeWhereLike.AutoCorrect + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizTypeWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizTypeWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizTypeWhereLike.UpdatedAt + "%");
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
                                quizType = new QuizTypeModel();
                                if (!reader.IsDBNull(0))
                                    quizType.Id = reader.GetValue(0).ToString();
                                else
                                    quizType.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizType.Name = reader.GetValue(1).ToString();
                                else
                                    quizType.Name = null;
                                if (!reader.IsDBNull(2))
                                    quizType.AutoCorrect = reader.GetValue(2).ToString();
                                else
                                    quizType.AutoCorrect = null;
                                if (!reader.IsDBNull(3))
                                    quizType.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizType.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizType.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizType.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizType.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizType.UpdatedAt = null;
                                response.Add(quizType);
                                quizType = new QuizTypeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizTypeModel>();
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
                response = new List<QuizTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizTypeModel> Read(string connectionString, QuizTypeModel quizType, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizTypeModel> response = new List<QuizTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_types ";
                {
                    if (quizType != null)
                    {
                        if (!string.IsNullOrEmpty(quizType.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizType.Id);
                        }
                        if (!string.IsNullOrEmpty(quizType.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", quizType.Name);
                        }
                        if (!string.IsNullOrEmpty(quizType.AutoCorrect))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `auto_correct` = @autoCorrect ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `auto_correct` = @autoCorrect ");
                            }
                            cmd.Parameters.AddWithValue("@autoCorrect", quizType.AutoCorrect);
                        }
                        if (!string.IsNullOrEmpty(quizType.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizType.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizType.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizType.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizType.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizType.UpdatedAt);
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
                                    quizType.Id = reader.GetValue(0).ToString();
                                else
                                    quizType.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizType.Name = reader.GetValue(1).ToString();
                                else
                                    quizType.Name = null;
                                if (!reader.IsDBNull(2))
                                    quizType.AutoCorrect = reader.GetValue(2).ToString();
                                else
                                    quizType.AutoCorrect = null;
                                if (!reader.IsDBNull(3))
                                    quizType.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizType.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizType.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizType.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizType.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizType.UpdatedAt = null;
                                response.Add(quizType);
                                quizType = new QuizTypeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizTypeModel>();
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
                response = new List<QuizTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizTypeModel> Read(string connectionString, QuizTypeModel quizType, bool orderByLastDate, int fromRow, int limit, QuizTypeModel quizTypeWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizTypeModel> response = new List<QuizTypeModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_types ";
                {
                    if (quizType != null)
                    {
                        if (!String.IsNullOrEmpty(quizType.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizType.Id);
                        }
                        if (!String.IsNullOrEmpty(quizType.Name))
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
                            cmd.Parameters.AddWithValue("@name", quizType.Name);
                        }
                        if (!String.IsNullOrEmpty(quizType.AutoCorrect))
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
                            cmd.Parameters.AddWithValue("@autoCorrect", quizType.AutoCorrect);
                        }
                        if (!String.IsNullOrEmpty(quizType.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizType.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizType.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizType.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizType.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizType.UpdatedAt);
                        }
                    }
                    if (quizTypeWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizTypeWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.Name))
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
                            cmd.Parameters.AddWithValue("@nameWhereLike", "%" + quizTypeWhereLike.Name + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.AutoCorrect))
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
                            cmd.Parameters.AddWithValue("@autoCorrectWhereLike", "%" + quizTypeWhereLike.AutoCorrect + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizTypeWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizTypeWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizTypeWhereLike.UpdatedAt + "%");
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
                                quizType = new QuizTypeModel();
                                if (!reader.IsDBNull(0))
                                    quizType.Id = reader.GetValue(0).ToString();
                                else
                                    quizType.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizType.Name = reader.GetValue(1).ToString();
                                else
                                    quizType.Name = null;
                                if (!reader.IsDBNull(2))
                                    quizType.AutoCorrect = reader.GetValue(2).ToString();
                                else
                                    quizType.AutoCorrect = null;
                                if (!reader.IsDBNull(3))
                                    quizType.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizType.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizType.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizType.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizType.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizType.UpdatedAt = null;
                                response.Add(quizType);
                                quizType = new QuizTypeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizTypeModel>();
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
                response = new List<QuizTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizTypeModel> Read(string connectionString, QuizTypeModel quizType, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizTypeModel> response = new List<QuizTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_types ";
                {
                    if (quizType != null)
                    {
                        if (!string.IsNullOrEmpty(quizType.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizType.Id);
                        }
                        if (!string.IsNullOrEmpty(quizType.Name))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `name` = @name ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `name` = @name ");
                            }
                            cmd.Parameters.AddWithValue("@name", quizType.Name);
                        }
                        if (!string.IsNullOrEmpty(quizType.AutoCorrect))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `auto_correct` = @autoCorrect ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `auto_correct` = @autoCorrect ");
                            }
                            cmd.Parameters.AddWithValue("@autoCorrect", quizType.AutoCorrect);
                        }
                        if (!string.IsNullOrEmpty(quizType.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizType.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(quizType.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizType.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(quizType.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizType.UpdatedAt);
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
                                    quizType.Id = reader.GetValue(0).ToString();
                                else
                                    quizType.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizType.Name = reader.GetValue(1).ToString();
                                else
                                    quizType.Name = null;
                                if (!reader.IsDBNull(2))
                                    quizType.AutoCorrect = reader.GetValue(2).ToString();
                                else
                                    quizType.AutoCorrect = null;
                                if (!reader.IsDBNull(3))
                                    quizType.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizType.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizType.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizType.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizType.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizType.UpdatedAt = null;
                                response.Add(quizType);
                                quizType = new QuizTypeModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizTypeModel>();
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
                response = new List<QuizTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizTypeModel> Read(string connectionString, List<QuizTypeModel> quizTypeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizTypeModel> response = new List<QuizTypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_types ";
                if (quizTypeWhere != null && quizTypeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizTypeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizTypeWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, quizTypeWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].AutoCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `auto_correct` = @autoCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@autoCorrect" + i, quizTypeWhere[i].AutoCorrect);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizTypeWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizTypeWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizTypeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizTypeWhere.Count - 1)
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
                            QuizTypeModel quizType = new QuizTypeModel(); if (!reader.IsDBNull(0))
                                quizType.Id = reader.GetValue(0).ToString();
                            else
                                quizType.Id = null;
                            if (!reader.IsDBNull(1))
                                quizType.Name = reader.GetValue(1).ToString();
                            else
                                quizType.Name = null;
                            if (!reader.IsDBNull(2))
                                quizType.AutoCorrect = reader.GetValue(2).ToString();
                            else
                                quizType.AutoCorrect = null;
                            if (!reader.IsDBNull(3))
                                quizType.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizType.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizType.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizType.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizType.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizType.UpdatedAt = null;
                            response.Add(quizType);
                            quizType = new QuizTypeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizTypeModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizTypeModel> Read(string connectionString, List<QuizTypeModel> quizTypeWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizTypeModel> response = new List<QuizTypeModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `name`, `auto_correct`, `modified_by`, `created_at`, `updated_at`  FROM quiz_types ";
                if (quizTypeWhere != null && quizTypeWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizTypeWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizTypeWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].Name))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `name` = @name" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@name" + i, quizTypeWhere[i].Name);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].AutoCorrect))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `auto_correct` = @autoCorrect" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@autoCorrect" + i, quizTypeWhere[i].AutoCorrect);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizTypeWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizTypeWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizTypeWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizTypeWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizTypeWhere.Count - 1)
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
                            QuizTypeModel quizType = new QuizTypeModel(); if (!reader.IsDBNull(0))
                                quizType.Id = reader.GetValue(0).ToString();
                            else
                                quizType.Id = null;
                            if (!reader.IsDBNull(1))
                                quizType.Name = reader.GetValue(1).ToString();
                            else
                                quizType.Name = null;
                            if (!reader.IsDBNull(2))
                                quizType.AutoCorrect = reader.GetValue(2).ToString();
                            else
                                quizType.AutoCorrect = null;
                            if (!reader.IsDBNull(3))
                                quizType.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizType.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizType.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizType.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizType.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizType.UpdatedAt = null;
                            response.Add(quizType);
                            quizType = new QuizTypeModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizTypeModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizTypeModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizTypeModel> Create(string connectionString, QuizTypeModel quizType)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizTypeModel> response = new List<QuizTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO quiz_types (";
                string values = "(";
                if (!String.IsNullOrEmpty(quizType.Id))
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
                    cmd.Parameters.AddWithValue("@id", quizType.Id);
                }
                if (!String.IsNullOrEmpty(quizType.Name))
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
                    cmd.Parameters.AddWithValue("@name", quizType.Name);
                }
                if (!String.IsNullOrEmpty(quizType.AutoCorrect))
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
                    cmd.Parameters.AddWithValue("@autoCorrect", quizType.AutoCorrect);
                }
                if (!String.IsNullOrEmpty(quizType.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", quizType.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(quizType.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", quizType.CreatedAt);
                }
                if (!String.IsNullOrEmpty(quizType.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", quizType.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                quizType.Id = cmd.LastInsertedId.ToString();
                response.Add(quizType);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizTypeModel>();
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
        public List<QuizTypeModel> Create(string connectionString, List<QuizTypeModel> quizTypeList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizTypeModel> response = new List<QuizTypeModel>();
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
                        for (int i = 0; i < quizTypeList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO quiz_types (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(quizTypeList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, quizTypeList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(quizTypeList[i].Name))
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
                                    cmd.Parameters.AddWithValue("@name" + i, quizTypeList[i].Name);
                                }
                                if (!String.IsNullOrEmpty(quizTypeList[i].AutoCorrect))
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
                                    cmd.Parameters.AddWithValue("@autoCorrect" + i, quizTypeList[i].AutoCorrect);
                                }
                                if (!String.IsNullOrEmpty(quizTypeList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizTypeList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(quizTypeList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizTypeList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(quizTypeList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizTypeList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(quizTypeList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizTypeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizTypeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizTypeModel>();
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

        public List<QuizTypeModel> Update(string connectionString, QuizTypeModel quizType, QuizTypeModel quizTypeWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizTypeModel> response = new List<QuizTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE quiz_types SET ";
                if (!String.IsNullOrEmpty(quizType.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", quizType.Id);
                }
                if (!String.IsNullOrEmpty(quizType.Name))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `name` = @name");
                    else
                        sql = String.Concat(sql, " `name` = @name");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@name", quizType.Name);
                }
                if (!String.IsNullOrEmpty(quizType.AutoCorrect))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `auto_correct` = @autoCorrect");
                    else
                        sql = String.Concat(sql, " `auto_correct` = @autoCorrect");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@autoCorrect", quizType.AutoCorrect);
                }
                if (!String.IsNullOrEmpty(quizType.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", quizType.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(quizType.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", quizType.CreatedAt);
                }
                if (!String.IsNullOrEmpty(quizType.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", quizType.UpdatedAt);
                }
                if (quizTypeWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(quizTypeWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", quizTypeWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(quizTypeWhere.Name))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `name` = @nameWhere");
                        else
                            sql = String.Concat(sql, " `name` = @nameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@nameWhere", quizTypeWhere.Name);
                    }
                    if (!String.IsNullOrEmpty(quizTypeWhere.AutoCorrect))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `auto_correct` = @autoCorrectWhere");
                        else
                            sql = String.Concat(sql, " `auto_correct` = @autoCorrectWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@autoCorrectWhere", quizTypeWhere.AutoCorrect);
                    }
                    if (!String.IsNullOrEmpty(quizTypeWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", quizTypeWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(quizTypeWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", quizTypeWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(quizTypeWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", quizTypeWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(quizType);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizTypeModel>();
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

        public List<QuizTypeModel> Update(string connectionString, List<QuizTypeModel> quizTypeList, List<QuizTypeModel> quizTypeWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizTypeModel> response = new List<QuizTypeModel>();
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
                        for (int i = 0; i < quizTypeList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE quiz_types SET ";
                                if (!String.IsNullOrEmpty(quizTypeList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, quizTypeList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(quizTypeList[i].Name))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `name` = @name" + i);
                                    else
                                        sql = String.Concat(sql, " `name` = @name" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@name" + i, quizTypeList[i].Name);
                                }
                                if (!String.IsNullOrEmpty(quizTypeList[i].AutoCorrect))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `auto_correct` = @autoCorrect" + i);
                                    else
                                        sql = String.Concat(sql, " `auto_correct` = @autoCorrect" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@autoCorrect" + i, quizTypeList[i].AutoCorrect);
                                }
                                if (!String.IsNullOrEmpty(quizTypeList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizTypeList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(quizTypeList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizTypeList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(quizTypeList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizTypeList[i].UpdatedAt);
                                }
                                if (quizTypeWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(quizTypeWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, quizTypeWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(quizTypeWhereList[i].Name))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `name` = @nameWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `name` = @nameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@nameWhere" + i, quizTypeWhereList[i].Name);
                                    }
                                    if (!String.IsNullOrEmpty(quizTypeWhereList[i].AutoCorrect))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `auto_correct` = @autoCorrectWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `auto_correct` = @autoCorrectWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@autoCorrectWhere" + i, quizTypeWhereList[i].AutoCorrect);
                                    }
                                    if (!String.IsNullOrEmpty(quizTypeWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, quizTypeWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(quizTypeWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, quizTypeWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(quizTypeWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, quizTypeWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizTypeList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizTypeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizTypeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizTypeModel>();
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

        public List<QuizTypeModel> Delete(string connectionString, QuizTypeModel quizType)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizTypeModel> response = new List<QuizTypeModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM quiz_types  ";
                if (quizType != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(quizType.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", quizType.Id);
                    }
                    if (!String.IsNullOrEmpty(quizType.Name))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `name` = @name");
                        else
                            sql = String.Concat(sql, " `name` = @name");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@name", quizType.Name);
                    }
                    if (!String.IsNullOrEmpty(quizType.AutoCorrect))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `auto_correct` = @autoCorrect");
                        else
                            sql = String.Concat(sql, " `auto_correct` = @autoCorrect");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@autoCorrect", quizType.AutoCorrect);
                    }
                    if (!String.IsNullOrEmpty(quizType.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", quizType.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(quizType.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", quizType.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(quizType.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", quizType.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(quizType);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizTypeModel>();
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

        public List<QuizTypeModel> Delete(string connectionString, List<QuizTypeModel> quizType)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizTypeModel> response = new List<QuizTypeModel>();
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
                        for (int i = 0; i < quizType.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM quiz_types  ";
                                if (quizType[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(quizType[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, quizType[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(quizType[i].Name))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `name` = @name" + i);
                                        else
                                            sql = String.Concat(sql, " `name` = @name" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@name" + i, quizType[i].Name);
                                    }
                                    if (!String.IsNullOrEmpty(quizType[i].AutoCorrect))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `auto_correct` = @autoCorrect" + i);
                                        else
                                            sql = String.Concat(sql, " `auto_correct` = @autoCorrect" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@autoCorrect" + i, quizType[i].AutoCorrect);
                                    }
                                    if (!String.IsNullOrEmpty(quizType[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, quizType[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(quizType[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, quizType[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(quizType[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, quizType[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizType[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizTypeModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizTypeModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizTypeModel>();
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
