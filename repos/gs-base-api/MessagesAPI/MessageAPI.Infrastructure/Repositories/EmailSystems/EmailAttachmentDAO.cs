using System;
using System.Collections.Generic;
using MySqlConnector;
using MessagesAPI.MessageAPI.Domain.Models.EmailSystem;
using MessagesAPI.MessageAPI.Services.EmailSystems.CustomClasses;

namespace MessagesAPI.MessageAPI.Infrastructure.Repositories.EmailSystems
{
    class EmailAttachmentDAO
    {
        private MySqlConnection _connection;
        public Dictionary<string, EmailAttachmentModel> ReadD(string connectionString, EmailAttachmentModel emailAttachment, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailAttachmentModel> response = new Dictionary<string, EmailAttachmentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `message_id`, `attachment_url`, `attachment_name`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_attachments ";
                {
                    if (emailAttachment != null)
                    {
                        if (!string.IsNullOrEmpty(emailAttachment.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailAttachment.Id);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.MessageId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `message_id` = @messageId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `message_id` = @messageId ");
                            }
                            cmd.Parameters.AddWithValue("@messageId", emailAttachment.MessageId);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.AttachmentUrl))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `attachment_url` = @attachmentUrl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `attachment_url` = @attachmentUrl ");
                            }
                            cmd.Parameters.AddWithValue("@attachmentUrl", emailAttachment.AttachmentUrl);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.AttachmentName))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `attachment_name` = @attachmentName ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `attachment_name` = @attachmentName ");
                            }
                            cmd.Parameters.AddWithValue("@attachmentName", emailAttachment.AttachmentName);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailAttachment.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailAttachment.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailAttachment.UpdatedAt);
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
                                    emailAttachment.Id = reader.GetValue(0).ToString();
                                else
                                    emailAttachment.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailAttachment.MessageId = reader.GetValue(1).ToString();
                                else
                                    emailAttachment.MessageId = null;
                                if (!reader.IsDBNull(2))
                                    emailAttachment.AttachmentUrl = reader.GetValue(2).ToString();
                                else
                                    emailAttachment.AttachmentUrl = null;
                                if (!reader.IsDBNull(3))
                                    emailAttachment.AttachmentName = reader.GetValue(3).ToString();
                                else
                                    emailAttachment.AttachmentName = null;
                                if (!reader.IsDBNull(4))
                                    emailAttachment.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    emailAttachment.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    emailAttachment.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    emailAttachment.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    emailAttachment.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    emailAttachment.UpdatedAt = null;
                                response.Add(emailAttachment.Id, emailAttachment);
                                emailAttachment = new EmailAttachmentModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EmailAttachmentModel>();
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
                response = new Dictionary<string, EmailAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EmailAttachmentModel> ReadD(string connectionString, EmailAttachmentModel emailAttachment, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailAttachmentModel> response = new Dictionary<string, EmailAttachmentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `message_id`, `attachment_url`, `attachment_name`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_attachments ";
                {
                    if (emailAttachment != null)
                    {
                        if (!string.IsNullOrEmpty(emailAttachment.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailAttachment.Id);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.MessageId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `message_id` = @messageId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `message_id` = @messageId ");
                            }
                            cmd.Parameters.AddWithValue("@messageId", emailAttachment.MessageId);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.AttachmentUrl))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `attachment_url` = @attachmentUrl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `attachment_url` = @attachmentUrl ");
                            }
                            cmd.Parameters.AddWithValue("@attachmentUrl", emailAttachment.AttachmentUrl);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.AttachmentName))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `attachment_name` = @attachmentName ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `attachment_name` = @attachmentName ");
                            }
                            cmd.Parameters.AddWithValue("@attachmentName", emailAttachment.AttachmentName);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailAttachment.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailAttachment.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailAttachment.UpdatedAt);
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
                                    emailAttachment.Id = reader.GetValue(0).ToString();
                                else
                                    emailAttachment.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailAttachment.MessageId = reader.GetValue(1).ToString();
                                else
                                    emailAttachment.MessageId = null;
                                if (!reader.IsDBNull(2))
                                    emailAttachment.AttachmentUrl = reader.GetValue(2).ToString();
                                else
                                    emailAttachment.AttachmentUrl = null;
                                if (!reader.IsDBNull(3))
                                    emailAttachment.AttachmentName = reader.GetValue(3).ToString();
                                else
                                    emailAttachment.AttachmentName = null;
                                if (!reader.IsDBNull(4))
                                    emailAttachment.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    emailAttachment.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    emailAttachment.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    emailAttachment.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    emailAttachment.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    emailAttachment.UpdatedAt = null;
                                response.Add(emailAttachment.Id, emailAttachment);
                                emailAttachment = new EmailAttachmentModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new Dictionary<string, EmailAttachmentModel>();
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
                response = new Dictionary<string, EmailAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EmailAttachmentModel> ReadD(string connectionString, List<EmailAttachmentModel> emailAttachmentWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailAttachmentModel> response = new Dictionary<string, EmailAttachmentModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `message_id`, `attachment_url`, `attachment_name`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_attachments ";
                if (emailAttachmentWhere != null && emailAttachmentWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailAttachmentWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailAttachmentWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].MessageId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_id` = @messageId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@messageId" + i, emailAttachmentWhere[i].MessageId);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].AttachmentUrl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `attachment_url` = @attachmentUrl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@attachmentUrl" + i, emailAttachmentWhere[i].AttachmentUrl);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].AttachmentName))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `attachment_name` = @attachmentName" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@attachmentName" + i, emailAttachmentWhere[i].AttachmentName);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailAttachmentWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailAttachmentWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailAttachmentWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailAttachmentWhere.Count - 1)
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
                            EmailAttachmentModel emailAttachment = new EmailAttachmentModel(); if (!reader.IsDBNull(0))
                                emailAttachment.Id = reader.GetValue(0).ToString();
                            else
                                emailAttachment.Id = null;
                            if (!reader.IsDBNull(1))
                                emailAttachment.MessageId = reader.GetValue(1).ToString();
                            else
                                emailAttachment.MessageId = null;
                            if (!reader.IsDBNull(2))
                                emailAttachment.AttachmentUrl = reader.GetValue(2).ToString();
                            else
                                emailAttachment.AttachmentUrl = null;
                            if (!reader.IsDBNull(3))
                                emailAttachment.AttachmentName = reader.GetValue(3).ToString();
                            else
                                emailAttachment.AttachmentName = null;
                            if (!reader.IsDBNull(4))
                                emailAttachment.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                emailAttachment.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                emailAttachment.CreatedAt = reader.GetValue(5).ToString();
                            else
                                emailAttachment.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                emailAttachment.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                emailAttachment.UpdatedAt = null;
                            response.Add(emailAttachment.Id, emailAttachment);
                            emailAttachment = new EmailAttachmentModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EmailAttachmentModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EmailAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public Dictionary<string, EmailAttachmentModel> ReadD(string connectionString, List<EmailAttachmentModel> emailAttachmentWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            Dictionary<string, EmailAttachmentModel> response = new Dictionary<string, EmailAttachmentModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `message_id`, `attachment_url`, `attachment_name`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_attachments ";
                if (emailAttachmentWhere != null && emailAttachmentWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailAttachmentWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailAttachmentWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].MessageId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_id` = @messageId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@messageId" + i, emailAttachmentWhere[i].MessageId);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].AttachmentUrl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `attachment_url` = @attachmentUrl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@attachmentUrl" + i, emailAttachmentWhere[i].AttachmentUrl);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].AttachmentName))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `attachment_name` = @attachmentName" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@attachmentName" + i, emailAttachmentWhere[i].AttachmentName);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailAttachmentWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailAttachmentWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailAttachmentWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailAttachmentWhere.Count - 1)
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
                            EmailAttachmentModel emailAttachment = new EmailAttachmentModel(); if (!reader.IsDBNull(0))
                                emailAttachment.Id = reader.GetValue(0).ToString();
                            else
                                emailAttachment.Id = null;
                            if (!reader.IsDBNull(1))
                                emailAttachment.MessageId = reader.GetValue(1).ToString();
                            else
                                emailAttachment.MessageId = null;
                            if (!reader.IsDBNull(2))
                                emailAttachment.AttachmentUrl = reader.GetValue(2).ToString();
                            else
                                emailAttachment.AttachmentUrl = null;
                            if (!reader.IsDBNull(3))
                                emailAttachment.AttachmentName = reader.GetValue(3).ToString();
                            else
                                emailAttachment.AttachmentName = null;
                            if (!reader.IsDBNull(4))
                                emailAttachment.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                emailAttachment.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                emailAttachment.CreatedAt = reader.GetValue(5).ToString();
                            else
                                emailAttachment.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                emailAttachment.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                emailAttachment.UpdatedAt = null;
                            response.Add(emailAttachment.Id, emailAttachment);
                            emailAttachment = new EmailAttachmentModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new Dictionary<string, EmailAttachmentModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new Dictionary<string, EmailAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailAttachmentModel> Read(string connectionString, EmailAttachmentModel emailAttachment, bool firstOne, bool orderByLastDate)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailAttachmentModel> response = new List<EmailAttachmentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `message_id`, `attachment_url`, `attachment_name`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_attachments ";
                {
                    if (emailAttachment != null)
                    {
                        if (!string.IsNullOrEmpty(emailAttachment.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailAttachment.Id);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.MessageId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `message_id` = @messageId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `message_id` = @messageId ");
                            }
                            cmd.Parameters.AddWithValue("@messageId", emailAttachment.MessageId);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.AttachmentUrl))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `attachment_url` = @attachmentUrl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `attachment_url` = @attachmentUrl ");
                            }
                            cmd.Parameters.AddWithValue("@attachmentUrl", emailAttachment.AttachmentUrl);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.AttachmentName))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `attachment_name` = @attachmentName ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `attachment_name` = @attachmentName ");
                            }
                            cmd.Parameters.AddWithValue("@attachmentName", emailAttachment.AttachmentName);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailAttachment.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailAttachment.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailAttachment.UpdatedAt);
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
                                    emailAttachment.Id = reader.GetValue(0).ToString();
                                else
                                    emailAttachment.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailAttachment.MessageId = reader.GetValue(1).ToString();
                                else
                                    emailAttachment.MessageId = null;
                                if (!reader.IsDBNull(2))
                                    emailAttachment.AttachmentUrl = reader.GetValue(2).ToString();
                                else
                                    emailAttachment.AttachmentUrl = null;
                                if (!reader.IsDBNull(3))
                                    emailAttachment.AttachmentName = reader.GetValue(3).ToString();
                                else
                                    emailAttachment.AttachmentName = null;
                                if (!reader.IsDBNull(4))
                                    emailAttachment.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    emailAttachment.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    emailAttachment.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    emailAttachment.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    emailAttachment.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    emailAttachment.UpdatedAt = null;
                                response.Add(emailAttachment);
                                emailAttachment = new EmailAttachmentModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EmailAttachmentModel>();
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
                response = new List<EmailAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailAttachmentModel> Read(string connectionString, EmailAttachmentModel emailAttachment, bool orderByLastDate, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailAttachmentModel> response = new List<EmailAttachmentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "SELECT `id`, `message_id`, `attachment_url`, `attachment_name`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_attachments ";
                {
                    if (emailAttachment != null)
                    {
                        if (!string.IsNullOrEmpty(emailAttachment.Id))
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
                            cmd.Parameters.AddWithValue("@id", emailAttachment.Id);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.MessageId))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `message_id` = @messageId ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `message_id` = @messageId ");
                            }
                            cmd.Parameters.AddWithValue("@messageId", emailAttachment.MessageId);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.AttachmentUrl))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `attachment_url` = @attachmentUrl ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `attachment_url` = @attachmentUrl ");
                            }
                            cmd.Parameters.AddWithValue("@attachmentUrl", emailAttachment.AttachmentUrl);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.AttachmentName))
                        {
                            if (addAnd != true)
                            {
                                sql = string.Concat(sql, " where `attachment_name` = @attachmentName ");
                                addAnd = true;
                            }
                            else
                            {
                                sql = string.Concat(sql, " and `attachment_name` = @attachmentName ");
                            }
                            cmd.Parameters.AddWithValue("@attachmentName", emailAttachment.AttachmentName);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.ModifiedBy))
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
                            cmd.Parameters.AddWithValue("@modifiedBy", emailAttachment.ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.CreatedAt))
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
                            cmd.Parameters.AddWithValue("@createdAt", emailAttachment.CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailAttachment.UpdatedAt))
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
                            cmd.Parameters.AddWithValue("@updatedAt", emailAttachment.UpdatedAt);
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
                                    emailAttachment.Id = reader.GetValue(0).ToString();
                                else
                                    emailAttachment.Id = null;
                                if (!reader.IsDBNull(1))
                                    emailAttachment.MessageId = reader.GetValue(1).ToString();
                                else
                                    emailAttachment.MessageId = null;
                                if (!reader.IsDBNull(2))
                                    emailAttachment.AttachmentUrl = reader.GetValue(2).ToString();
                                else
                                    emailAttachment.AttachmentUrl = null;
                                if (!reader.IsDBNull(3))
                                    emailAttachment.AttachmentName = reader.GetValue(3).ToString();
                                else
                                    emailAttachment.AttachmentName = null;
                                if (!reader.IsDBNull(4))
                                    emailAttachment.ModifiedBy = reader.GetValue(4).ToString();
                                else
                                    emailAttachment.ModifiedBy = null;
                                if (!reader.IsDBNull(5))
                                    emailAttachment.CreatedAt = reader.GetValue(5).ToString();
                                else
                                    emailAttachment.CreatedAt = null;
                                if (!reader.IsDBNull(6))
                                    emailAttachment.UpdatedAt = reader.GetValue(6).ToString();
                                else
                                    emailAttachment.UpdatedAt = null;
                                response.Add(emailAttachment);
                                emailAttachment = new EmailAttachmentModel();
                            }
                    }
                    catch (ArgumentException e)
                    {
                        string MessageString = " Read error occured: " + e.Message;
                        response = new List<EmailAttachmentModel>();
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
                response = new List<EmailAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailAttachmentModel> Read(string connectionString, List<EmailAttachmentModel> emailAttachmentWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailAttachmentModel> response = new List<EmailAttachmentModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `message_id`, `attachment_url`, `attachment_name`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_attachments ";
                if (emailAttachmentWhere != null && emailAttachmentWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailAttachmentWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailAttachmentWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].MessageId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_id` = @messageId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@messageId" + i, emailAttachmentWhere[i].MessageId);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].AttachmentUrl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `attachment_url` = @attachmentUrl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@attachmentUrl" + i, emailAttachmentWhere[i].AttachmentUrl);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].AttachmentName))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `attachment_name` = @attachmentName" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@attachmentName" + i, emailAttachmentWhere[i].AttachmentName);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailAttachmentWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailAttachmentWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailAttachmentWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailAttachmentWhere.Count - 1)
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
                            EmailAttachmentModel emailAttachment = new EmailAttachmentModel(); if (!reader.IsDBNull(0))
                                emailAttachment.Id = reader.GetValue(0).ToString();
                            else
                                emailAttachment.Id = null;
                            if (!reader.IsDBNull(1))
                                emailAttachment.MessageId = reader.GetValue(1).ToString();
                            else
                                emailAttachment.MessageId = null;
                            if (!reader.IsDBNull(2))
                                emailAttachment.AttachmentUrl = reader.GetValue(2).ToString();
                            else
                                emailAttachment.AttachmentUrl = null;
                            if (!reader.IsDBNull(3))
                                emailAttachment.AttachmentName = reader.GetValue(3).ToString();
                            else
                                emailAttachment.AttachmentName = null;
                            if (!reader.IsDBNull(4))
                                emailAttachment.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                emailAttachment.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                emailAttachment.CreatedAt = reader.GetValue(5).ToString();
                            else
                                emailAttachment.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                emailAttachment.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                emailAttachment.UpdatedAt = null;
                            response.Add(emailAttachment);
                            emailAttachment = new EmailAttachmentModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EmailAttachmentModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailAttachmentModel> Read(string connectionString, List<EmailAttachmentModel> emailAttachmentWhere, int fromRow, int limit)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailAttachmentModel> response = new List<EmailAttachmentModel>();
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sqlIn = "";
                string sql = "SELECT `id`, `message_id`, `attachment_url`, `attachment_name`, `modified_by`, `created_at`, `updated_at`  FROM cm_email_attachments ";
                if (emailAttachmentWhere != null && emailAttachmentWhere.Count > 0)
                {
                    sqlIn += " WHERE ";
                    for (int i = 0; i < emailAttachmentWhere.Count; i++)
                    {
                        sqlIn += "(";
                        bool addAnd = false;
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].Id))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `id` = @id" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@id" + i, emailAttachmentWhere[i].Id);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].MessageId))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `message_id` = @messageId" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@messageId" + i, emailAttachmentWhere[i].MessageId);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].AttachmentUrl))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `attachment_url` = @attachmentUrl" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@attachmentUrl" + i, emailAttachmentWhere[i].AttachmentUrl);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].AttachmentName))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `attachment_name` = @attachmentName" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@attachmentName" + i, emailAttachmentWhere[i].AttachmentName);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].ModifiedBy))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `modified_by` = @modifiedBy" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@modifiedBy" + i, emailAttachmentWhere[i].ModifiedBy);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].CreatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `created_at` = @createdAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@createdAt" + i, emailAttachmentWhere[i].CreatedAt);
                        }
                        if (!string.IsNullOrEmpty(emailAttachmentWhere[i].UpdatedAt))
                        {
                            if (addAnd)
                                sqlIn += " AND ";

                            sqlIn += " `updated_at` = @updatedAt" + i;
                            addAnd = true;
                            cmd.Parameters.AddWithValue("@updatedAt" + i, emailAttachmentWhere[i].UpdatedAt);
                        }
                        sqlIn += ")";
                        if (i != emailAttachmentWhere.Count - 1)
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
                            EmailAttachmentModel emailAttachment = new EmailAttachmentModel(); if (!reader.IsDBNull(0))
                                emailAttachment.Id = reader.GetValue(0).ToString();
                            else
                                emailAttachment.Id = null;
                            if (!reader.IsDBNull(1))
                                emailAttachment.MessageId = reader.GetValue(1).ToString();
                            else
                                emailAttachment.MessageId = null;
                            if (!reader.IsDBNull(2))
                                emailAttachment.AttachmentUrl = reader.GetValue(2).ToString();
                            else
                                emailAttachment.AttachmentUrl = null;
                            if (!reader.IsDBNull(3))
                                emailAttachment.AttachmentName = reader.GetValue(3).ToString();
                            else
                                emailAttachment.AttachmentName = null;
                            if (!reader.IsDBNull(4))
                                emailAttachment.ModifiedBy = reader.GetValue(4).ToString();
                            else
                                emailAttachment.ModifiedBy = null;
                            if (!reader.IsDBNull(5))
                                emailAttachment.CreatedAt = reader.GetValue(5).ToString();
                            else
                                emailAttachment.CreatedAt = null;
                            if (!reader.IsDBNull(6))
                                emailAttachment.UpdatedAt = reader.GetValue(6).ToString();
                            else
                                emailAttachment.UpdatedAt = null;
                            response.Add(emailAttachment);
                            emailAttachment = new EmailAttachmentModel();
                        }
                }
                catch (ArgumentException e)
                {
                    string MessageString = " Read error occured: " + e.Message;
                    response = new List<EmailAttachmentModel>();
                    reader.Close();
                }
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailAttachmentModel>();
            }
            finally
            {
                if (_connection != null)
                    dbConn.CloseConnection(_connection);
            }
            return response;
        }
        public List<EmailAttachmentModel> Create(string connectionString, EmailAttachmentModel emailAttachment)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailAttachmentModel> response = new List<EmailAttachmentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "INSERT INTO cm_email_attachments (";
                string values = "(";
                if (!string.IsNullOrEmpty(emailAttachment.Id))
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
                    cmd.Parameters.AddWithValue("@id", emailAttachment.Id);
                }
                if (!string.IsNullOrEmpty(emailAttachment.MessageId))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `message_id` ");
                        values = string.Concat(values, ", @messageId");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `message_id` ");
                        values = string.Concat(values, " @messageId");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@messageId", emailAttachment.MessageId);
                }
                if (!string.IsNullOrEmpty(emailAttachment.AttachmentUrl))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `attachment_url` ");
                        values = string.Concat(values, ", @attachmentUrl");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `attachment_url` ");
                        values = string.Concat(values, " @attachmentUrl");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@attachmentUrl", emailAttachment.AttachmentUrl);
                }
                if (!string.IsNullOrEmpty(emailAttachment.AttachmentName))
                {
                    if (addAnd)
                    {
                        sql = string.Concat(sql, ", `attachment_name` ");
                        values = string.Concat(values, ", @attachmentName");
                    }
                    else
                    {
                        sql = string.Concat(sql, " `attachment_name` ");
                        values = string.Concat(values, " @attachmentName");
                    }
                    addAnd = true;
                    cmd.Parameters.AddWithValue("@attachmentName", emailAttachment.AttachmentName);
                }
                if (!string.IsNullOrEmpty(emailAttachment.ModifiedBy))
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
                    cmd.Parameters.AddWithValue("@modifiedBy", emailAttachment.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(emailAttachment.CreatedAt))
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
                    cmd.Parameters.AddWithValue("@createdAt", emailAttachment.CreatedAt);
                }
                if (!string.IsNullOrEmpty(emailAttachment.UpdatedAt))
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
                    cmd.Parameters.AddWithValue("@updatedAt", emailAttachment.UpdatedAt);
                }
                sql = string.Concat(sql, ")");
                values = string.Concat(values, ")");
                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                cmd.ExecuteNonQuery();
                emailAttachment.Id = cmd.LastInsertedId.ToString();
                response.Add(emailAttachment);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailAttachmentModel>();
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
        public List<EmailAttachmentModel> Create(string connectionString, List<EmailAttachmentModel> emailAttachmentList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailAttachmentModel> response = new List<EmailAttachmentModel>();
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
                        for (int i = 0; i < emailAttachmentList.Count; i++)
                        {
                            bool addAnd = false;
                            try
                            {
                                string sql = "INSERT INTO cm_email_attachments (";
                                string values = "(";
                                if (!string.IsNullOrEmpty(emailAttachmentList[i].Id))
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
                                    cmd.Parameters.AddWithValue("@id" + i, emailAttachmentList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(emailAttachmentList[i].MessageId))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `message_id` ");
                                        values = string.Concat(values, ", @messageId" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `message_id` ");
                                        values = string.Concat(values, "@messageId" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@messageId" + i, emailAttachmentList[i].MessageId);
                                }
                                if (!string.IsNullOrEmpty(emailAttachmentList[i].AttachmentUrl))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `attachment_url` ");
                                        values = string.Concat(values, ", @attachmentUrl" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `attachment_url` ");
                                        values = string.Concat(values, "@attachmentUrl" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@attachmentUrl" + i, emailAttachmentList[i].AttachmentUrl);
                                }
                                if (!string.IsNullOrEmpty(emailAttachmentList[i].AttachmentName))
                                {
                                    if (addAnd)
                                    {
                                        sql = string.Concat(sql, ", `attachment_name` ");
                                        values = string.Concat(values, ", @attachmentName" + i);
                                    }

                                    else
                                    {
                                        sql = string.Concat(sql, " `attachment_name` ");
                                        values = string.Concat(values, "@attachmentName" + i);
                                    }
                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@attachmentName" + i, emailAttachmentList[i].AttachmentName);
                                }
                                if (!string.IsNullOrEmpty(emailAttachmentList[i].ModifiedBy))
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
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailAttachmentList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(emailAttachmentList[i].CreatedAt))
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
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailAttachmentList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(emailAttachmentList[i].UpdatedAt))
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
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailAttachmentList[i].UpdatedAt);
                                }
                                sql = string.Concat(sql, ")");
                                values = string.Concat(values, ")");
                                cmd.CommandText = string.Concat(sql, " VALUES ", values);
                                cmd.ExecuteNonQuery();
                                response.Add(emailAttachmentList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error ocurred while loading the data: " + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailAttachmentModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = " The following error ocurred loading the date: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailAttachmentModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred loading the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailAttachmentModel>();
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

        public List<EmailAttachmentModel> Update(string connectionString, EmailAttachmentModel emailAttachment, EmailAttachmentModel emailAttachmentWhere)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailAttachmentModel> response = new List<EmailAttachmentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "UPDATE cm_email_attachments SET ";
                if (!string.IsNullOrEmpty(emailAttachment.Id))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `id` = @id");
                    else
                        sql = string.Concat(sql, " `id` = @id");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@id", emailAttachment.Id);
                }
                if (!string.IsNullOrEmpty(emailAttachment.MessageId))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `message_id` = @messageId");
                    else
                        sql = string.Concat(sql, " `message_id` = @messageId");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@messageId", emailAttachment.MessageId);
                }
                if (!string.IsNullOrEmpty(emailAttachment.AttachmentUrl))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `attachment_url` = @attachmentUrl");
                    else
                        sql = string.Concat(sql, " `attachment_url` = @attachmentUrl");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@attachmentUrl", emailAttachment.AttachmentUrl);
                }
                if (!string.IsNullOrEmpty(emailAttachment.AttachmentName))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `attachment_name` = @attachmentName");
                    else
                        sql = string.Concat(sql, " `attachment_name` = @attachmentName");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@attachmentName", emailAttachment.AttachmentName);
                }
                if (!string.IsNullOrEmpty(emailAttachment.ModifiedBy))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy");
                    else
                        sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@modifiedBy", emailAttachment.ModifiedBy);
                }
                if (!string.IsNullOrEmpty(emailAttachment.CreatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `created_at` = @createdAt");
                    else
                        sql = string.Concat(sql, " `created_at` = @createdAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@createdAt", emailAttachment.CreatedAt);
                }
                if (!string.IsNullOrEmpty(emailAttachment.UpdatedAt))
                {
                    if (addAnd)
                        sql = string.Concat(sql, ", `updated_at` = @updatedAt");
                    else
                        sql = string.Concat(sql, " `updated_at` = @updatedAt");

                    addAnd = true;
                    cmd.Parameters.AddWithValue("@updatedAt", emailAttachment.UpdatedAt);
                }
                if (emailAttachmentWhere != null)
                {
                    sql = string.Concat(sql, " WHERE ");
                    addAnd = false; //Reset the addAnd for the WHERES
                    if (!string.IsNullOrEmpty(emailAttachmentWhere.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @idWhere");
                        else
                            sql = string.Concat(sql, " `id` = @idWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@idWhere", emailAttachmentWhere.Id);
                    }
                    if (!string.IsNullOrEmpty(emailAttachmentWhere.MessageId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `message_id` = @messageIdWhere");
                        else
                            sql = string.Concat(sql, " `message_id` = @messageIdWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@messageIdWhere", emailAttachmentWhere.MessageId);
                    }
                    if (!string.IsNullOrEmpty(emailAttachmentWhere.AttachmentUrl))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `attachment_url` = @attachmentUrlWhere");
                        else
                            sql = string.Concat(sql, " `attachment_url` = @attachmentUrlWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@attachmentUrlWhere", emailAttachmentWhere.AttachmentUrl);
                    }
                    if (!string.IsNullOrEmpty(emailAttachmentWhere.AttachmentName))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `attachment_name` = @attachmentNameWhere");
                        else
                            sql = string.Concat(sql, " `attachment_name` = @attachmentNameWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@attachmentNameWhere", emailAttachmentWhere.AttachmentName);
                    }
                    if (!string.IsNullOrEmpty(emailAttachmentWhere.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedByWhere", emailAttachmentWhere.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(emailAttachmentWhere.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAtWhere", emailAttachmentWhere.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(emailAttachmentWhere.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAtWhere", emailAttachmentWhere.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                response.Add(emailAttachment);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EmailAttachmentModel>();
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

        public List<EmailAttachmentModel> Update(string connectionString, List<EmailAttachmentModel> emailAttachmentList, List<EmailAttachmentModel> emailAttachmentWhereList)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailAttachmentModel> response = new List<EmailAttachmentModel>();
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
                        for (int i = 0; i < emailAttachmentList.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;

                                string sql = "UPDATE cm_email_attachments SET ";
                                if (!string.IsNullOrEmpty(emailAttachmentList[i].Id))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `id` = @id" + i);
                                    else
                                        sql = string.Concat(sql, " `id` = @id" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@id" + i, emailAttachmentList[i].Id);
                                }
                                if (!string.IsNullOrEmpty(emailAttachmentList[i].MessageId))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `message_id` = @messageId" + i);
                                    else
                                        sql = string.Concat(sql, " `message_id` = @messageId" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@messageId" + i, emailAttachmentList[i].MessageId);
                                }
                                if (!string.IsNullOrEmpty(emailAttachmentList[i].AttachmentUrl))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `attachment_url` = @attachmentUrl" + i);
                                    else
                                        sql = string.Concat(sql, " `attachment_url` = @attachmentUrl" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@attachmentUrl" + i, emailAttachmentList[i].AttachmentUrl);
                                }
                                if (!string.IsNullOrEmpty(emailAttachmentList[i].AttachmentName))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `attachment_name` = @attachmentName" + i);
                                    else
                                        sql = string.Concat(sql, " `attachment_name` = @attachmentName" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@attachmentName" + i, emailAttachmentList[i].AttachmentName);
                                }
                                if (!string.IsNullOrEmpty(emailAttachmentList[i].ModifiedBy))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `modified_by` = @modifiedBy" + i);
                                    else
                                        sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@modifiedBy" + i, emailAttachmentList[i].ModifiedBy);
                                }
                                if (!string.IsNullOrEmpty(emailAttachmentList[i].CreatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `created_at` = @createdAt" + i);
                                    else
                                        sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@createdAt" + i, emailAttachmentList[i].CreatedAt);
                                }
                                if (!string.IsNullOrEmpty(emailAttachmentList[i].UpdatedAt))
                                {
                                    if (addAnd)
                                        sql = string.Concat(sql, ", `updated_at` = @updatedAt" + i);
                                    else
                                        sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                    addAnd = true;
                                    cmd.Parameters.AddWithValue("@updatedAt" + i, emailAttachmentList[i].UpdatedAt);
                                }
                                if (emailAttachmentWhereList[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false; // Reset addAnd for the Where
                                    if (!string.IsNullOrEmpty(emailAttachmentWhereList[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @idWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @idWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@idWhere" + i, emailAttachmentWhereList[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(emailAttachmentWhereList[i].MessageId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `message_id` = @messageIdWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `message_id` = @messageIdWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@messageIdWhere" + i, emailAttachmentWhereList[i].MessageId);
                                    }
                                    if (!string.IsNullOrEmpty(emailAttachmentWhereList[i].AttachmentUrl))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `attachment_url` = @attachmentUrlWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `attachment_url` = @attachmentUrlWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@attachmentUrlWhere" + i, emailAttachmentWhereList[i].AttachmentUrl);
                                    }
                                    if (!string.IsNullOrEmpty(emailAttachmentWhereList[i].AttachmentName))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `attachment_name` = @attachmentNameWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `attachment_name` = @attachmentNameWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@attachmentNameWhere" + i, emailAttachmentWhereList[i].AttachmentName);
                                    }
                                    if (!string.IsNullOrEmpty(emailAttachmentWhereList[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedByWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedByWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedByWhere" + i, emailAttachmentWhereList[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(emailAttachmentWhereList[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAtWhere" + i, emailAttachmentWhereList[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(emailAttachmentWhereList[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAtWhere" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAtWhere" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAtWhere" + i, emailAttachmentWhereList[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(emailAttachmentList[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailAttachmentModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailAttachmentModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailAttachmentModel>();
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

        public List<EmailAttachmentModel> Delete(string connectionString, EmailAttachmentModel emailAttachment)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailAttachmentModel> response = new List<EmailAttachmentModel>();
            bool addAnd = false;
            try
            {
                _connection = dbConn.GetConnection();
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = _connection
                };
                string sql = "DELETE FROM cm_email_attachments  ";
                if (emailAttachment != null)
                {
                    sql += " WHERE ";
                    if (!string.IsNullOrEmpty(emailAttachment.Id))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `id` = @id");
                        else
                            sql = string.Concat(sql, " `id` = @id");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@id", emailAttachment.Id);
                    }
                    if (!string.IsNullOrEmpty(emailAttachment.MessageId))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `message_id` = @messageId");
                        else
                            sql = string.Concat(sql, " `message_id` = @messageId");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@messageId", emailAttachment.MessageId);
                    }
                    if (!string.IsNullOrEmpty(emailAttachment.AttachmentUrl))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `attachment_url` = @attachmentUrl");
                        else
                            sql = string.Concat(sql, " `attachment_url` = @attachmentUrl");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@attachmentUrl", emailAttachment.AttachmentUrl);
                    }
                    if (!string.IsNullOrEmpty(emailAttachment.AttachmentName))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `attachment_name` = @attachmentName");
                        else
                            sql = string.Concat(sql, " `attachment_name` = @attachmentName");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@attachmentName", emailAttachment.AttachmentName);
                    }
                    if (!string.IsNullOrEmpty(emailAttachment.ModifiedBy))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy");
                        else
                            sql = string.Concat(sql, " `modified_by` = @modifiedBy");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@modifiedBy", emailAttachment.ModifiedBy);
                    }
                    if (!string.IsNullOrEmpty(emailAttachment.CreatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `created_at` = @createdAt");
                        else
                            sql = string.Concat(sql, " `created_at` = @createdAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@createdAt", emailAttachment.CreatedAt);
                    }
                    if (!string.IsNullOrEmpty(emailAttachment.UpdatedAt))
                    {
                        if (addAnd)
                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt");
                        else
                            sql = string.Concat(sql, " `updated_at` = @updatedAt");

                        addAnd = true;
                        cmd.Parameters.AddWithValue("@updatedAt", emailAttachment.UpdatedAt);
                    }
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                response.Add(emailAttachment);
            }
            catch (MySqlException e)
            {
                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                response = new List<EmailAttachmentModel>();
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

        public List<EmailAttachmentModel> Delete(string connectionString, List<EmailAttachmentModel> emailAttachment)
        {
            DBConnection dbConn = new DBConnection(connectionString);
            List<EmailAttachmentModel> response = new List<EmailAttachmentModel>();
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
                        for (int i = 0; i < emailAttachment.Count; i++)
                        {
                            try
                            {
                                bool addAnd = false;
                                string sql = "DELETE FROM cm_email_attachments  ";
                                if (emailAttachment[i] != null)
                                {
                                    sql = string.Concat(sql, " WHERE ");
                                    addAnd = false;
                                    if (!string.IsNullOrEmpty(emailAttachment[i].Id))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `id` = @id" + i);
                                        else
                                            sql = string.Concat(sql, " `id` = @id" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@id" + i, emailAttachment[i].Id);
                                    }
                                    if (!string.IsNullOrEmpty(emailAttachment[i].MessageId))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `message_id` = @messageId" + i);
                                        else
                                            sql = string.Concat(sql, " `message_id` = @messageId" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@messageId" + i, emailAttachment[i].MessageId);
                                    }
                                    if (!string.IsNullOrEmpty(emailAttachment[i].AttachmentUrl))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `attachment_url` = @attachmentUrl" + i);
                                        else
                                            sql = string.Concat(sql, " `attachment_url` = @attachmentUrl" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@attachmentUrl" + i, emailAttachment[i].AttachmentUrl);
                                    }
                                    if (!string.IsNullOrEmpty(emailAttachment[i].AttachmentName))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `attachment_name` = @attachmentName" + i);
                                        else
                                            sql = string.Concat(sql, " `attachment_name` = @attachmentName" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@attachmentName" + i, emailAttachment[i].AttachmentName);
                                    }
                                    if (!string.IsNullOrEmpty(emailAttachment[i].ModifiedBy))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `modified_by` = @modifiedBy" + i);
                                        else
                                            sql = string.Concat(sql, " `modified_by` = @modifiedBy" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@modifiedBy" + i, emailAttachment[i].ModifiedBy);
                                    }
                                    if (!string.IsNullOrEmpty(emailAttachment[i].CreatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `created_at` = @createdAt" + i);
                                        else
                                            sql = string.Concat(sql, " `created_at` = @createdAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@createdAt" + i, emailAttachment[i].CreatedAt);
                                    }
                                    if (!string.IsNullOrEmpty(emailAttachment[i].UpdatedAt))
                                    {
                                        if (addAnd)
                                            sql = string.Concat(sql, " AND `updated_at` = @updatedAt" + i);
                                        else
                                            sql = string.Concat(sql, " `updated_at` = @updatedAt" + i);

                                        addAnd = true;
                                        cmd.Parameters.AddWithValue("@updatedAt" + i, emailAttachment[i].UpdatedAt);
                                    }
                                }
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                response.Add(emailAttachment[i]);
                            }
                            catch (MySqlException e)
                            {
                                string MessageString = " The following error occurred while updating data:" + e.ErrorCode + " - " + e.Message;
                                response = new List<EmailAttachmentModel>();
                            }
                        }
                        trans.Commit();
                    }
                    catch (MySqlException e)
                    {
                        string MessageString = "Thee following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                        trans.Rollback();
                        response = new List<EmailAttachmentModel>();
                    }
                }
            }
            catch (MySqlException e)
            {
                string MessageString = "The following error ocurred while updating the data: " + e.ErrorCode + " - " + e.Message;
                response = new List<EmailAttachmentModel>();
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
