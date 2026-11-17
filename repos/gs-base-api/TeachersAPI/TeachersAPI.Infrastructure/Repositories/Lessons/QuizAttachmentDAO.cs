using System;
using System.Collections.Generic;
using MySqlConnector;
using TeachersAPI.Domain.Models.Lessons;
namespace TeachersAPI.TeachersAPI.Infrastructure.Repositories.Lessons
{
    class QuizAttachmentDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, QuizAttachmentModel> ReadD(string connectionString, QuizAttachmentModel quizAttachment, bool firstOne, bool orderByLastDate, QuizAttachmentModel quizAttachmentWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizAttachmentModel> response = new Dictionary<string, QuizAttachmentModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_id`, `attachment_url`, `modified_by`, `created_at`, `updated_at`  FROM quiz_attachments ";
                {
                    if (quizAttachment != null)
                    {
                        if (!String.IsNullOrEmpty(quizAttachment.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizAttachment.Id);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", quizAttachment.QuizId);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.AttachmentUrl))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `attachment_url` = @attachmentUrl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `attachment_url` = @attachmentUrl ");
                            }
                            cmd.Parameters.AddWithValue("@attachmentUrl", quizAttachment.AttachmentUrl);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizAttachment.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizAttachment.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizAttachment.UpdatedAt);
                        }
                    }
                    if (quizAttachmentWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizAttachmentWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + quizAttachmentWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.AttachmentUrl))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `attachment_url` LIKE @attachmentUrlWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `attachment_url` LIKE @attachmentUrlWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@attachmentUrlWhereLike", "%" + quizAttachmentWhereLike.AttachmentUrl + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizAttachmentWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizAttachmentWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizAttachmentWhereLike.UpdatedAt + "%");
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
                                quizAttachment = new QuizAttachmentModel();
                                if (!reader.IsDBNull(0))
                                    quizAttachment.Id = reader.GetValue(0).ToString();
                                else
                                    quizAttachment.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizAttachment.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizAttachment.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizAttachment.AttachmentUrl = reader.GetValue(2).ToString();
                                else
                                    quizAttachment.AttachmentUrl = null;
                                if (!reader.IsDBNull(3))
                                    quizAttachment.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizAttachment.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizAttachment.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizAttachment.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizAttachment.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizAttachment.UpdatedAt = null;
                                response.Add(quizAttachment.Id, quizAttachment);
                                quizAttachment = new QuizAttachmentModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizAttachmentModel>();
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
                response = new Dictionary<string, QuizAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizAttachmentModel> ReadD(string connectionString, QuizAttachmentModel quizAttachment, bool orderByLastDate, int fromRow, int limit, QuizAttachmentModel quizAttachmentWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizAttachmentModel> response = new Dictionary<string, QuizAttachmentModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_id`, `attachment_url`, `modified_by`, `created_at`, `updated_at`  FROM quiz_attachments ";
                {
                    if (quizAttachment != null)
                    {
                        if (!String.IsNullOrEmpty(quizAttachment.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizAttachment.Id);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", quizAttachment.QuizId);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.AttachmentUrl))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `attachment_url` = @attachmentUrl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `attachment_url` = @attachmentUrl ");
                            }
                            cmd.Parameters.AddWithValue("@attachmentUrl", quizAttachment.AttachmentUrl);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizAttachment.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizAttachment.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizAttachment.UpdatedAt);
                        }
                    }
                    if (quizAttachmentWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizAttachmentWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + quizAttachmentWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.AttachmentUrl))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `attachment_url` LIKE @attachmentUrlWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `attachment_url` LIKE @attachmentUrlWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@attachmentUrlWhereLike", "%" + quizAttachmentWhereLike.AttachmentUrl + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizAttachmentWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizAttachmentWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizAttachmentWhereLike.UpdatedAt + "%");
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
                                quizAttachment = new QuizAttachmentModel();
                                if (!reader.IsDBNull(0))
                                    quizAttachment.Id = reader.GetValue(0).ToString();
                                else
                                    quizAttachment.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizAttachment.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizAttachment.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizAttachment.AttachmentUrl = reader.GetValue(2).ToString();
                                else
                                    quizAttachment.AttachmentUrl = null;
                                if (!reader.IsDBNull(3))
                                    quizAttachment.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizAttachment.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizAttachment.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizAttachment.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizAttachment.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizAttachment.UpdatedAt = null;
                                response.Add(quizAttachment.Id, quizAttachment);
                                quizAttachment = new QuizAttachmentModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, QuizAttachmentModel>();
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
                response = new Dictionary<string, QuizAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizAttachmentModel> ReadD(string connectionString, List<QuizAttachmentModel> quizAttachmentWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizAttachmentModel> response = new Dictionary<string, QuizAttachmentModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_id`, `attachment_url`, `modified_by`, `created_at`, `updated_at`  FROM quiz_attachments ";
                if (quizAttachmentWhere != null && quizAttachmentWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizAttachmentWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizAttachmentWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, quizAttachmentWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].AttachmentUrl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `attachment_url` = @attachmentUrl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@attachmentUrl" + i, quizAttachmentWhere[i].AttachmentUrl);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizAttachmentWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizAttachmentWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizAttachmentWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizAttachmentWhere.Count - 1)
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
                            QuizAttachmentModel quizAttachment = new QuizAttachmentModel(); if (!reader.IsDBNull(0))
                                quizAttachment.Id = reader.GetValue(0).ToString();
                            else
                                quizAttachment.Id = null;
                            if (!reader.IsDBNull(1))
                                quizAttachment.QuizId = reader.GetValue(1).ToString();
                            else
                                quizAttachment.QuizId = null;
                            if (!reader.IsDBNull(2))
                                quizAttachment.AttachmentUrl = reader.GetValue(2).ToString();
                            else
                                quizAttachment.AttachmentUrl = null;
                            if (!reader.IsDBNull(3))
                                quizAttachment.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizAttachment.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizAttachment.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizAttachment.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizAttachment.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizAttachment.UpdatedAt = null;
                            response.Add(quizAttachment.Id, quizAttachment);
                            quizAttachment = new QuizAttachmentModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizAttachmentModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, QuizAttachmentModel> ReadD(string connectionString, List<QuizAttachmentModel> quizAttachmentWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, QuizAttachmentModel> response = new Dictionary<string, QuizAttachmentModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_id`, `attachment_url`, `modified_by`, `created_at`, `updated_at`  FROM quiz_attachments ";
                if (quizAttachmentWhere != null && quizAttachmentWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizAttachmentWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizAttachmentWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, quizAttachmentWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].AttachmentUrl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `attachment_url` = @attachmentUrl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@attachmentUrl" + i, quizAttachmentWhere[i].AttachmentUrl);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizAttachmentWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizAttachmentWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizAttachmentWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizAttachmentWhere.Count - 1)
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
                            QuizAttachmentModel quizAttachment = new QuizAttachmentModel(); if (!reader.IsDBNull(0))
                                quizAttachment.Id = reader.GetValue(0).ToString();
                            else
                                quizAttachment.Id = null;
                            if (!reader.IsDBNull(1))
                                quizAttachment.QuizId = reader.GetValue(1).ToString();
                            else
                                quizAttachment.QuizId = null;
                            if (!reader.IsDBNull(2))
                                quizAttachment.AttachmentUrl = reader.GetValue(2).ToString();
                            else
                                quizAttachment.AttachmentUrl = null;
                            if (!reader.IsDBNull(3))
                                quizAttachment.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizAttachment.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizAttachment.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizAttachment.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizAttachment.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizAttachment.UpdatedAt = null;
                            response.Add(quizAttachment.Id, quizAttachment);
                            quizAttachment = new QuizAttachmentModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, QuizAttachmentModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, QuizAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizAttachmentModel> Read(string connectionString, QuizAttachmentModel quizAttachment, bool firstOne, bool orderByLastDate, QuizAttachmentModel quizAttachmentWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizAttachmentModel> response = new List<QuizAttachmentModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_id`, `attachment_url`, `modified_by`, `created_at`, `updated_at`  FROM quiz_attachments ";
                {
                    if (quizAttachment != null)
                    {
                        if (!String.IsNullOrEmpty(quizAttachment.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizAttachment.Id);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", quizAttachment.QuizId);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.AttachmentUrl))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `attachment_url` = @attachmentUrl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `attachment_url` = @attachmentUrl ");
                            }
                            cmd.Parameters.AddWithValue("@attachmentUrl", quizAttachment.AttachmentUrl);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizAttachment.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizAttachment.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizAttachment.UpdatedAt);
                        }
                    }
                    if (quizAttachmentWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizAttachmentWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + quizAttachmentWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.AttachmentUrl))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `attachment_url` LIKE @attachmentUrlWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `attachment_url` LIKE @attachmentUrlWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@attachmentUrlWhereLike", "%" + quizAttachmentWhereLike.AttachmentUrl + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizAttachmentWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizAttachmentWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizAttachmentWhereLike.UpdatedAt + "%");
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
                                quizAttachment = new QuizAttachmentModel();
                                if (!reader.IsDBNull(0))
                                    quizAttachment.Id = reader.GetValue(0).ToString();
                                else
                                    quizAttachment.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizAttachment.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizAttachment.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizAttachment.AttachmentUrl = reader.GetValue(2).ToString();
                                else
                                    quizAttachment.AttachmentUrl = null;
                                if (!reader.IsDBNull(3))
                                    quizAttachment.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizAttachment.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizAttachment.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizAttachment.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizAttachment.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizAttachment.UpdatedAt = null;
                                response.Add(quizAttachment);
                                quizAttachment = new QuizAttachmentModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizAttachmentModel>();
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
                response = new List<QuizAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizAttachmentModel> Read(string connectionString, QuizAttachmentModel quizAttachment, bool orderByLastDate, int fromRow, int limit, QuizAttachmentModel quizAttachmentWhereLike = null)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizAttachmentModel> response = new List<QuizAttachmentModel>();
            bool addAnd = false;
            bool addAndWhereLike = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `quiz_id`, `attachment_url`, `modified_by`, `created_at`, `updated_at`  FROM quiz_attachments ";
                {
                    if (quizAttachment != null)
                    {
                        if (!String.IsNullOrEmpty(quizAttachment.Id))
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
                            cmd.Parameters.AddWithValue("@id", quizAttachment.Id);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizId", quizAttachment.QuizId);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.AttachmentUrl))
                        {
                            if (addAnd != true)
                            {
                                sql = String.Concat(sql, " where `attachment_url` = @attachmentUrl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = String.Concat(sql, " and `attachment_url` = @attachmentUrl ");
                            }
                            cmd.Parameters.AddWithValue("@attachmentUrl", quizAttachment.AttachmentUrl);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", quizAttachment.ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", quizAttachment.CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizAttachment.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", quizAttachment.UpdatedAt);
                        }
                    }
                    if (quizAttachmentWhereLike != null)
                    {
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.Id))
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
                            cmd.Parameters.AddWithValue("@idWhereLike", "%" + quizAttachmentWhereLike.Id + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.QuizId))
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
                            cmd.Parameters.AddWithValue("@quizIdWhereLike", "%" + quizAttachmentWhereLike.QuizId + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.AttachmentUrl))
                        {
                            if (addAnd || addAndWhereLike)
                            {
                                sql = String.Concat(sql, " and `attachment_url` LIKE @attachmentUrlWhereLike ");
                            }
                            else
                            {
                                sql = String.Concat(sql, " where `attachment_url` LIKE @attachmentUrlWhereLike ");
                            }
                            addAndWhereLike = true;
                            cmd.Parameters.AddWithValue("@attachmentUrlWhereLike", "%" + quizAttachmentWhereLike.AttachmentUrl + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedByWhereLike", "%" + quizAttachmentWhereLike.ModifiedBy + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAtWhereLike", "%" + quizAttachmentWhereLike.CreatedAt + "%");
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhereLike.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAtWhereLike", "%" + quizAttachmentWhereLike.UpdatedAt + "%");
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
                                quizAttachment = new QuizAttachmentModel();
                                if (!reader.IsDBNull(0))
                                    quizAttachment.Id = reader.GetValue(0).ToString();
                                else
                                    quizAttachment.Id = null;
                                if (!reader.IsDBNull(1))
                                    quizAttachment.QuizId = reader.GetValue(1).ToString();
                                else
                                    quizAttachment.QuizId = null;
                                if (!reader.IsDBNull(2))
                                    quizAttachment.AttachmentUrl = reader.GetValue(2).ToString();
                                else
                                    quizAttachment.AttachmentUrl = null;
                                if (!reader.IsDBNull(3))
                                    quizAttachment.ModifiedBy = reader.GetValue(3).ToString();
                                else
                                    quizAttachment.ModifiedBy = null;
                                if (!reader.IsDBNull(4))
                                    quizAttachment.CreatedAt = reader.GetValue(4).ToString();
                                else
                                    quizAttachment.CreatedAt = null;
                                if (!reader.IsDBNull(5))
                                    quizAttachment.UpdatedAt = reader.GetValue(5).ToString();
                                else
                                    quizAttachment.UpdatedAt = null;
                                response.Add(quizAttachment);
                                quizAttachment = new QuizAttachmentModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<QuizAttachmentModel>();
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
                response = new List<QuizAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizAttachmentModel> Read(string connectionString, List<QuizAttachmentModel> quizAttachmentWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizAttachmentModel> response = new List<QuizAttachmentModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_id`, `attachment_url`, `modified_by`, `created_at`, `updated_at`  FROM quiz_attachments ";
                if (quizAttachmentWhere != null && quizAttachmentWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizAttachmentWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizAttachmentWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, quizAttachmentWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].AttachmentUrl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `attachment_url` = @attachmentUrl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@attachmentUrl" + i, quizAttachmentWhere[i].AttachmentUrl);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizAttachmentWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizAttachmentWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizAttachmentWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizAttachmentWhere.Count - 1)
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
                            QuizAttachmentModel quizAttachment = new QuizAttachmentModel(); if (!reader.IsDBNull(0))
                                quizAttachment.Id = reader.GetValue(0).ToString();
                            else
                                quizAttachment.Id = null;
                            if (!reader.IsDBNull(1))
                                quizAttachment.QuizId = reader.GetValue(1).ToString();
                            else
                                quizAttachment.QuizId = null;
                            if (!reader.IsDBNull(2))
                                quizAttachment.AttachmentUrl = reader.GetValue(2).ToString();
                            else
                                quizAttachment.AttachmentUrl = null;
                            if (!reader.IsDBNull(3))
                                quizAttachment.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizAttachment.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizAttachment.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizAttachment.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizAttachment.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizAttachment.UpdatedAt = null;
                            response.Add(quizAttachment);
                            quizAttachment = new QuizAttachmentModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizAttachmentModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizAttachmentModel> Read(string connectionString, List<QuizAttachmentModel> quizAttachmentWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizAttachmentModel> response = new List<QuizAttachmentModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `quiz_id`, `attachment_url`, `modified_by`, `created_at`, `updated_at`  FROM quiz_attachments ";
                if (quizAttachmentWhere != null && quizAttachmentWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < quizAttachmentWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, quizAttachmentWhere[i].Id);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].QuizId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `quiz_id` = @quizId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@quizId" + i, quizAttachmentWhere[i].QuizId);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].AttachmentUrl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `attachment_url` = @attachmentUrl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@attachmentUrl" + i, quizAttachmentWhere[i].AttachmentUrl);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, quizAttachmentWhere[i].ModifiedBy);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, quizAttachmentWhere[i].CreatedAt);
                        }
                        if (!String.IsNullOrEmpty(quizAttachmentWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, quizAttachmentWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != quizAttachmentWhere.Count - 1)
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
                            QuizAttachmentModel quizAttachment = new QuizAttachmentModel(); if (!reader.IsDBNull(0))
                                quizAttachment.Id = reader.GetValue(0).ToString();
                            else
                                quizAttachment.Id = null;
                            if (!reader.IsDBNull(1))
                                quizAttachment.QuizId = reader.GetValue(1).ToString();
                            else
                                quizAttachment.QuizId = null;
                            if (!reader.IsDBNull(2))
                                quizAttachment.AttachmentUrl = reader.GetValue(2).ToString();
                            else
                                quizAttachment.AttachmentUrl = null;
                            if (!reader.IsDBNull(3))
                                quizAttachment.ModifiedBy = reader.GetValue(3).ToString();
                            else
                                quizAttachment.ModifiedBy = null;
                            if (!reader.IsDBNull(4))
                                quizAttachment.CreatedAt = reader.GetValue(4).ToString();
                            else
                                quizAttachment.CreatedAt = null;
                            if (!reader.IsDBNull(5))
                                quizAttachment.UpdatedAt = reader.GetValue(5).ToString();
                            else
                                quizAttachment.UpdatedAt = null;
                            response.Add(quizAttachment);
                            quizAttachment = new QuizAttachmentModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<QuizAttachmentModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<QuizAttachmentModel> Create(string connectionString, QuizAttachmentModel quizAttachment)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizAttachmentModel> response = new List<QuizAttachmentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO quiz_attachments (";
                string values = "(";
                if (!String.IsNullOrEmpty(quizAttachment.Id))
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
                    cmd.Parameters.AddWithValue("@id", quizAttachment.Id);
                }
                if (!String.IsNullOrEmpty(quizAttachment.QuizId))
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
                    cmd.Parameters.AddWithValue("@quizId", quizAttachment.QuizId);
                }
                if (!String.IsNullOrEmpty(quizAttachment.AttachmentUrl))
                {
                    if (addAnd)
                    {
                        sql = String.Concat(sql, ", `attachment_url` ");
                        values = String.Concat(values, ", @attachmentUrl");
                    }
                    else
                    {
                        sql = String.Concat(sql, " `attachment_url` ");
                        values = String.Concat(values, " @attachmentUrl");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@attachmentUrl", quizAttachment.AttachmentUrl);
                }
                if (!String.IsNullOrEmpty(quizAttachment.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", quizAttachment.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(quizAttachment.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", quizAttachment.CreatedAt);
                }
                if (!String.IsNullOrEmpty(quizAttachment.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", quizAttachment.UpdatedAt);
                }
                sql = String.Concat(sql, ")");
                values = String.Concat(values, ")");
                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                quizAttachment.Id = cmd.LastInsertedId.ToString();
                response.Add(quizAttachment);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizAttachmentModel>();
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
        public List<QuizAttachmentModel> Create(string connectionString, List<QuizAttachmentModel> quizAttachmentList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizAttachmentModel> response = new List<QuizAttachmentModel>();
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
                        for (int i = 0; i < quizAttachmentList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO quiz_attachments (";
                                string values = "(";
                                if (!String.IsNullOrEmpty(quizAttachmentList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, quizAttachmentList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(quizAttachmentList[i].QuizId))
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
                                    cmd.Parameters.AddWithValue("@quizId" + i, quizAttachmentList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(quizAttachmentList[i].AttachmentUrl))
                                {
                                    if (addAnd)
                                    {
                                        sql = String.Concat(sql, ", `attachment_url` ");
                                        values = String.Concat(values, ", @attachmentUrl" + i);
                                    }

                                    else
                                    {
                                        sql = String.Concat(sql, " `attachment_url` ");
                                        values = String.Concat(values, "@attachmentUrl" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@attachmentUrl" + i, quizAttachmentList[i].AttachmentUrl);
                                }
                                if (!String.IsNullOrEmpty(quizAttachmentList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizAttachmentList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(quizAttachmentList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizAttachmentList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(quizAttachmentList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizAttachmentList[i].UpdatedAt);
                                }
                                sql = String.Concat(sql, ")");
                                values = String.Concat(values, ")");
                                cmd.CommandText = String.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(quizAttachmentList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizAttachmentModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizAttachmentModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizAttachmentModel>();
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

        public List<QuizAttachmentModel> Update(string connectionString, QuizAttachmentModel quizAttachment, QuizAttachmentModel quizAttachmentWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizAttachmentModel> response = new List<QuizAttachmentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE quiz_attachments SET ";
                if (!String.IsNullOrEmpty(quizAttachment.Id))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `id` = @id");
                    else
                        sql = String.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", quizAttachment.Id);
                }
                if (!String.IsNullOrEmpty(quizAttachment.QuizId))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `quiz_id` = @quizId");
                    else
                        sql = String.Concat(sql, " `quiz_id` = @quizId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@quizId", quizAttachment.QuizId);
                }
                if (!String.IsNullOrEmpty(quizAttachment.AttachmentUrl))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `attachment_url` = @attachmentUrl");
                    else
                        sql = String.Concat(sql, " `attachment_url` = @attachmentUrl");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@attachmentUrl", quizAttachment.AttachmentUrl);
                }
                if (!String.IsNullOrEmpty(quizAttachment.ModifiedBy))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", quizAttachment.ModifiedBy);
                }
                if (!String.IsNullOrEmpty(quizAttachment.CreatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = String.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", quizAttachment.CreatedAt);
                }
                if (!String.IsNullOrEmpty(quizAttachment.UpdatedAt))
                {
                    if (addAnd)
                        sql = String.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = String.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", quizAttachment.UpdatedAt);
                }
                if (quizAttachmentWhere != null)
                {
                    sql = String.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!String.IsNullOrEmpty(quizAttachmentWhere.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = String.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", quizAttachmentWhere.Id);
                    }
                    if (!String.IsNullOrEmpty(quizAttachmentWhere.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizIdWhere", quizAttachmentWhere.QuizId);
                    }
                    if (!String.IsNullOrEmpty(quizAttachmentWhere.AttachmentUrl))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `attachment_url` = @attachmentUrlWhere");
                        else
                            sql = String.Concat(sql, " `attachment_url` = @attachmentUrlWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@attachmentUrlWhere", quizAttachmentWhere.AttachmentUrl);
                    }
                    if (!String.IsNullOrEmpty(quizAttachmentWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", quizAttachmentWhere.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(quizAttachmentWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", quizAttachmentWhere.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(quizAttachmentWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", quizAttachmentWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(quizAttachment);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizAttachmentModel>();
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

        public List<QuizAttachmentModel> Update(string connectionString, List<QuizAttachmentModel> quizAttachmentList, List<QuizAttachmentModel> quizAttachmentWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizAttachmentModel> response = new List<QuizAttachmentModel>();
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
                        for (int i = 0; i < quizAttachmentList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE quiz_attachments SET ";
                                if (!String.IsNullOrEmpty(quizAttachmentList[i].Id))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = String.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, quizAttachmentList[i].Id);
                                }
                                if (!String.IsNullOrEmpty(quizAttachmentList[i].QuizId))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `quiz_id` = @quizId" + i);
                                    else
                                        sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@quizId" + i, quizAttachmentList[i].QuizId);
                                }
                                if (!String.IsNullOrEmpty(quizAttachmentList[i].AttachmentUrl))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `attachment_url` = @attachmentUrl" + i);
                                    else
                                        sql = String.Concat(sql, " `attachment_url` = @attachmentUrl" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@attachmentUrl" + i, quizAttachmentList[i].AttachmentUrl);
                                }
                                if (!String.IsNullOrEmpty(quizAttachmentList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, quizAttachmentList[i].ModifiedBy);
                                }
                                if (!String.IsNullOrEmpty(quizAttachmentList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, quizAttachmentList[i].CreatedAt);
                                }
                                if (!String.IsNullOrEmpty(quizAttachmentList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = String.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, quizAttachmentList[i].UpdatedAt);
                                }
                                if (quizAttachmentWhereList[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!String.IsNullOrEmpty(quizAttachmentWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, quizAttachmentWhereList[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(quizAttachmentWhereList[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizIdWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizIdWhere" + i, quizAttachmentWhereList[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(quizAttachmentWhereList[i].AttachmentUrl))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `attachment_url` = @attachmentUrlWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `attachment_url` = @attachmentUrlWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@attachmentUrlWhere" + i, quizAttachmentWhereList[i].AttachmentUrl);
                                    }
                                    if (!String.IsNullOrEmpty(quizAttachmentWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, quizAttachmentWhereList[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(quizAttachmentWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, quizAttachmentWhereList[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(quizAttachmentWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, quizAttachmentWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizAttachmentList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizAttachmentModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizAttachmentModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizAttachmentModel>();
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

        public List<QuizAttachmentModel> Delete(string connectionString, QuizAttachmentModel quizAttachment)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizAttachmentModel> response = new List<QuizAttachmentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM quiz_attachments  ";
                if (quizAttachment != null)
                {
                    sql += " WHERE ";
                    if (!String.IsNullOrEmpty(quizAttachment.Id))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `id` = @id");
                        else
                            sql = String.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", quizAttachment.Id);
                    }
                    if (!String.IsNullOrEmpty(quizAttachment.QuizId))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `quiz_id` = @quizId");
                        else
                            sql = String.Concat(sql, " `quiz_id` = @quizId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@quizId", quizAttachment.QuizId);
                    }
                    if (!String.IsNullOrEmpty(quizAttachment.AttachmentUrl))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `attachment_url` = @attachmentUrl");
                        else
                            sql = String.Concat(sql, " `attachment_url` = @attachmentUrl");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@attachmentUrl", quizAttachment.AttachmentUrl);
                    }
                    if (!String.IsNullOrEmpty(quizAttachment.ModifiedBy))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = String.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", quizAttachment.ModifiedBy);
                    }
                    if (!String.IsNullOrEmpty(quizAttachment.CreatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = String.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", quizAttachment.CreatedAt);
                    }
                    if (!String.IsNullOrEmpty(quizAttachment.UpdatedAt))
                    {
                        if (addAnd)
                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = String.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", quizAttachment.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(quizAttachment);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<QuizAttachmentModel>();
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

        public List<QuizAttachmentModel> Delete(string connectionString, List<QuizAttachmentModel> quizAttachment)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<QuizAttachmentModel> response = new List<QuizAttachmentModel>();
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
                        for (int i = 0; i < quizAttachment.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM quiz_attachments  ";
                                if (quizAttachment[i] != null)
                                {
                                    sql = String.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!String.IsNullOrEmpty(quizAttachment[i].Id))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = String.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, quizAttachment[i].Id);
                                    }
                                    if (!String.IsNullOrEmpty(quizAttachment[i].QuizId))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `quiz_id` = @quizId" + i);
                                        else
                                            sql = String.Concat(sql, " `quiz_id` = @quizId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@quizId" + i, quizAttachment[i].QuizId);
                                    }
                                    if (!String.IsNullOrEmpty(quizAttachment[i].AttachmentUrl))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `attachment_url` = @attachmentUrl" + i);
                                        else
                                            sql = String.Concat(sql, " `attachment_url` = @attachmentUrl" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@attachmentUrl" + i, quizAttachment[i].AttachmentUrl);
                                    }
                                    if (!String.IsNullOrEmpty(quizAttachment[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = String.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, quizAttachment[i].ModifiedBy);
                                    }
                                    if (!String.IsNullOrEmpty(quizAttachment[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = String.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, quizAttachment[i].CreatedAt);
                                    }
                                    if (!String.IsNullOrEmpty(quizAttachment[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = String.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = String.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, quizAttachment[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(quizAttachment[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<QuizAttachmentModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<QuizAttachmentModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<QuizAttachmentModel>();
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
